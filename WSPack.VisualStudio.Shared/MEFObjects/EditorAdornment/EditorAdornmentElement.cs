using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Outlining;

using WSPack.Lib.CSharp;
using WSPack.Lib.CSharp.Objects;
using WSPack.VisualStudio.Shared.DteProject;
using WSPack.VisualStudio.Shared.Extensions;

namespace WSPack.VisualStudio.Shared.MEFObjects.EditorAdornment
{
  /// <summary>
  /// Prover adereços no editor, como métricas e indicador de final de bloco
  /// </summary>
  sealed class EditorAdornmentElement : TimerBaseAdornment
  {
    internal const string WSEditorAdornmentElementName = "WSEditorAdornmentElementName";

    /// <summary>
    /// Chave para armazenara os métodos da view
    /// </summary>
    internal const string ListMethodInfoKey = "ListMethodInfoKey";

    /// <summary>
    /// Camada de adereço
    /// </summary>
    readonly IAdornmentLayer _layer;

    readonly string _fileName;
    readonly IOutliningManager _outlineManager;
    List<MethodInfo> _lstMethods;
    List<BlockInfo> _lstBlocks;

    #region Construtor
    /// <summary>
    /// Initializes a new instance of the <see cref="EditorAdornmentElement"/> class.
    /// </summary>
    /// <param name="view">Text view to create the adornment for</param>
    /// <param name="outliningManagerService">Prover serviço de Outlighting</param>
    public EditorAdornmentElement(IWpfTextView view, IOutliningManagerService outliningManagerService)
      : base(view, timerAlterouLayout: false, timerAlterouPosicao: false)
    {
      if (view == null)
      {
        throw new ArgumentNullException(nameof(view));
      }
      _layer = view.GetAdornmentLayer(WSEditorAdornmentElementName);
      _outlineManager = outliningManagerService.GetOutliningManager(_view);
      _fileName = view.TextBuffer.GetFilePath();
      _view.Properties.AddProperty(ListMethodInfoKey, new List<MethodInfo>());
    }
    #endregion

    /// <summary>
    /// Indica se o adereço está habilitado
    /// </summary>
    private static bool IsAdornmentEnabled => WSPackPackage.Instance != null &&
      (WSPackPackage.ParametrosMEFObjects.MetricsObj.UseMethodsMetrics ||
      WSPackPackage.ParametrosMEFObjects.BlocksEndObj.UseBlocks);

    protected override void AlterouLayout(object sender, TextViewLayoutChangedEventArgs e)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread($"{nameof(EditorAdornment)}:{nameof(AlterouLayout)}");
      if (IsAdornmentEnabled)
      {
        var isVisibleAreaChanged = IsVisibleAreaChanged(e);
        if (isVisibleAreaChanged && e.OldSnapshot != e.NewSnapshot)
          return;

        PaintAdornment(_view.TextSnapshot);
      }
      else
        _layer.RemoveAllAdornments();
    }

    protected override void OnStartTimer()
    {
      _layer.RemoveAllAdornments();
    }

    protected override void OnTimer(ITextSnapshot textSnapshot)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      if (IsAdornmentEnabled)
        ParseDocument(_view.TextSnapshot);
      else
        _layer.RemoveAllAdornments();
    }

    private void ParseDocument(ITextSnapshot textSnapshot)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      _ = DteProjectObj.TryCreateFromActiveDocument(out var dteProject);

      CSharpParserObj parser = CSharpParserObj.Parse(textSnapshot.GetText(),
        ParseOptions.Create(WSPackPackage.ParametrosMEFObjects.MetricsObj.UseMethodsMetrics,
          WSPackPackage.ParametrosMEFObjects.BlocksEndObj.UseBlocks,
          dteProject?.Properties?.ActiveConfiguration?.DefineConstants));

      var lstTasks = new List<Task>
      {
        Task.Run(() =>
        {
          _lstMethods = parser.MethodsList.Select(x => new MethodInfo()
          {
            Method = x,
            Position = GetPosition(textSnapshot, x.DeclareLocation.Line, x.DeclareLocation.Column),
            Metric = null
          }).ToList();
          _view.Properties[ListMethodInfoKey] = _lstMethods;
        }),

        Task.Run(() =>
        {
          _lstBlocks = parser.BlocksList.Select(x => new BlockInfo()
          {
            Block = x,
            StartPosition = GetPosition(textSnapshot, x.Start.Line, x.Start.Column),
            EndPosition = GetPosition(textSnapshot, x.End.Line, x.End.Column)
          }).ToList();
        })
      };

      Task.WaitAll(lstTasks.ToArray());
      if (!_view.IsClosed)
      {
        // pintar
        PaintAdornment(textSnapshot);
      }
    }

    /// <summary>
    /// Executar o método necessário para pintar os adereços
    /// </summary>
    /// <param name="textSnapshot">Text snapshot</param>
    void PaintAdornment(ITextSnapshot textSnapshot)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread($"{nameof(EditorAdornment)}:{nameof(PaintAdornment)}");
      _layer.RemoveAllAdornments();
      AderecoFinalBloco(textSnapshot);
      AderecoMetricas(textSnapshot);
    }

    private void AderecoFinalBloco(ITextSnapshot textSnapshot)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      if (_view.IsClosed)
        return;

      // Usuário não quer usar
      if (!WSPackPackage.ParametrosMEFObjects.BlocksEndObj.UseBlocks)
        return;

      if (_lstBlocks == null)
        OnTimer(textSnapshot);
      else if (_timer == null || _timer.IsEnabled)
        return;

      foreach (BlockInfo esteBloco in _lstBlocks)
      {
        // Posição do bloco é maior do que o texto
        if (esteBloco.EndPosition > _view.TextSnapshot.Length)
          continue;

        if (!GetSnapshotSpan(esteBloco, out SnapshotSpan sssStart, out SnapshotSpan sssEnd))
          continue;

        // Se o inicio do bloco está visível, continua
        if (WSPackPackage.ParametrosMEFObjects.BlocksEndObj.ShowOnlyIfStartIsNotVisivel)
        {
          if (GetGeometrySafe(sssStart, out _))
            continue;
        }

        // Se o fim não está visível, continua
        if (!GetGeometrySafe(sssEnd, out Geometry geometryEnd))
          continue;

        // Nâo exibir se estiver Collapsed
        if (_outlineManager == null)
          continue;

        if (!GetCollapsedRegionsSafe(_outlineManager, sssEnd, out var regioes))
          continue;
        if (regioes.Any())
          continue;

        // Número mínimo de linhas para que o bloco seja exibido
        int numLinhasBloco = esteBloco.Block.End.Line - esteBloco.Block.Start.Line;
        if (numLinhasBloco < WSPackPackage.ParametrosMEFObjects.BlocksEndObj.ShowOnlyIfLineNumberGreaterOrEqual)
          continue;

        const int paddingHorizontal = 5;
        const int paddingVertical = 3;
        int paddingLeftTextoLinha = 0;

        // Verificar se existe texto digitado na linha que vamos decorar
        var linha = textSnapshot.GetLineFromPosition(sssEnd.Start);
        var sssLinha = new SnapshotSpan(linha.Start, linha.Extent.End);
        if (!GetGeometrySafe(sssLinha, out var geometryLinha))
          continue;

        string texto = null;
        try
        {
          // coloquei -1: estava dando erro de IndexOutOfRange
          // coloquei -2: estava dando erro de IndexOutOfRange
          int tamanho = linha.Length;
          if (tamanho >= 3)
            tamanho -= 2;
          texto = textSnapshot.GetText(sssEnd.Start, tamanho);
        }
        catch (Exception ex)
        {
          Utils.LogDebugError($"AderecoFinalBloco - textSnapshot.GetText: {ex.Message}");
        }

        if (!string.IsNullOrEmpty(texto) && texto.Length > 1)
        {
          sssEnd = new SnapshotSpan(textSnapshot, sssLinha.End.Position - 1, 1);
          if (!GetGeometrySafe(sssEnd, out geometryEnd))
            continue;
          //geometryEnd = _view.TextViewLines.GetMarkerGeometry(sssEnd);
          paddingLeftTextoLinha = paddingHorizontal;
        }

        if (geometryEnd == null)
          continue;

        try
        {
          UIElement label = BlockFlexControl.CreateBlock(esteBloco.Block);
          if (label == null)
            return;
          Canvas.SetLeft(label, geometryEnd.Bounds.Left + paddingHorizontal + paddingLeftTextoLinha);
          Canvas.SetTop(label, geometryEnd.Bounds.Top - paddingVertical);
          _layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, sssEnd, null, label, null);
        }
        catch (Exception ex)
        {
          Utils.LogDebugError($"AderecoFinalBloco - _layer.AddAdornment: {ex.Message}");
        }
      }
    }

    bool GetSnapshotSpan(BlockInfo bloco, out SnapshotSpan start, out SnapshotSpan end)
    {
      start = new SnapshotSpan();
      end = new SnapshotSpan();

      int startPos = bloco.StartPosition;
      if (startPos > 0)
        startPos -= 1;
      try
      {
        start = new SnapshotSpan(_view.TextSnapshot, startPos, 1);
        end = new SnapshotSpan(_view.TextSnapshot, bloco.EndPosition - 1, 1);
        return true;
      }
      catch (Exception ex)
      {
        Trace.WriteLine("GetSnapshotSpan: " + ex.Message);
        return false;
      }
    }

    private void AderecoMetricas(ITextSnapshot textSnapshot)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      if (_view.IsClosed)
        return;

      if (!CanShowMetrics())
        return;

      if (_lstMethods == null)
        OnTimer(textSnapshot);
      else if (_timer == null || _timer.IsEnabled)
        return;

      int total = _lstMethods.Count;
      for (int i = 0; i < total; i++)
      {
        MethodInfo methodInfo = _lstMethods[i];

        // Nâo exibir para get/set
        if (methodInfo.Method.IsGetterOrSetter && !WSPackPackage.ParametrosMEFObjects.MetricsObj.ShowGetSetMetrics)
          continue;

        if (methodInfo.Position > _view.TextSnapshot.Length)
          continue;

        // Método possui apens uma linha
        if (methodInfo.Method.DeclareLocation.Line == methodInfo.Method.BodyEndLocation.Line)
          continue;

        SnapshotSpan sss;
        try
        {
          sss = new SnapshotSpan(_view.TextSnapshot, methodInfo.Position, 1);
        }
        catch (Exception ex)
        {
          Trace.WriteLine("RepaintComplexity: " + ex.Message);
          continue;
        }

        // Nâo exibir se estiver Collapsed
        if (_outlineManager == null)
          continue;

        if (!GetCollapsedRegionsSafe(_outlineManager, sss, out var regioes))
          continue;
        if (regioes.Any())
          continue;

        // Não exibir se não estiver visível
        if (!GetGeometrySafe(sss, out var geometry))
          continue;
        var isMethodVisible = geometry != null;
        if (!isMethodVisible)
          continue;

        if (methodInfo.Metric == null)
          methodInfo.Metric = MethodMetricsObj.Calculate(methodInfo.Method);

        // Se é método de teste; Se nº linhas >= a um valor
        if ((methodInfo.Method.IsTestMethod && WSPackPackage.ParametrosMEFObjects.MetricsObj.AlwaysShowForUnitTest) ||
          (methodInfo.Metric.CyclomaticComplexity.Value >= WSPackPackage.ParametrosMEFObjects.MetricsObj.MinValueToShowMetrics))
        {
          string tooltip = string.Format("Informações do método {0}:\r\n\r\n{1}", methodInfo.Method.Name, methodInfo.Metric.ToString());
          FrameworkElement bloco = MetricsAdornmentControl.CreateBlock(methodInfo.Method, methodInfo.Metric, tooltip);
          if (bloco == null)
            return;

          int ajusteLeftDestrutores = methodInfo.Method.IsDestructor ? 5 : 0;

          // Get/Set está na mesma linha. Vamos chegar o adereço um pouco para a direita e diminuir o tamanho do bloco
          if (methodInfo.Method.NameLocation.Line.Equals(methodInfo.Method.BodyEndLocation.Line) && methodInfo.Method.IsGetterOrSetter)
          {
            ajusteLeftDestrutores += -2;
            bloco.Width -= 5;
          }

          Canvas.SetLeft(bloco, geometry.Bounds.Left - bloco.Width - (3 + ajusteLeftDestrutores));
          Canvas.SetTop(bloco, geometry.Bounds.Top + 1);
          _layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, sss, null, bloco, null);
        }
      }
    }

    bool CanShowMetrics()
    {
      if (WSPackPackage.Instance == null)
        return false;

      var options = WSPackPackage.ParametrosMEFObjects.MetricsObj;

      // Usuário não quer usar
      if (!options.UseMethodsMetrics)
        return false;

      // Não exibir em arquivos .designer
      if (string.IsNullOrEmpty(_fileName) || (!options.ShowDesignerMetrics &&
        _fileName.EndsWith(".designer.cs", StringComparison.OrdinalIgnoreCase)))
        return false;

      // Não exibir durante o Debug
      if (!options.ShowDesignTimeMetrics &&
        WSPackPackage.Dte?.Debugger?.CurrentMode != EnvDTE.dbgDebugMode.dbgDesignMode)
        return false;

      // Não exibir no Metadata
      if (WSPackPackage.Dte.ActiveWindow?.Caption?.Contains("[from metadata]") == true)
        return false;

      return true;
    }
  }
}