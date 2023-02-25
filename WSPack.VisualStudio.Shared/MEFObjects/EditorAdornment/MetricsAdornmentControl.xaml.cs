using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using EnvDTE;

using Microsoft.VisualStudio.Shell;

using WSPack.Lib;
using WSPack.Lib.CSharp.Models;
using WSPack.Lib.Properties;

namespace WSPack.VisualStudio.Shared.MEFObjects.EditorAdornment
{
  /// <summary>
  /// Interaction logic for MetricsAdornmentControl.xaml
  /// </summary>
  public partial class MetricsAdornmentControl : Border
  {
    Border edtBorda => this;
    MethodModel _method;
    static MenuItem _menuItemExecutarTeste, _menuItemDepurarTeste;
    readonly static Dictionary<string, Image> _dicImages = new Dictionary<string, Image>();

    #region Construtores
    /// <summary>
    /// Inicialização da classe: <see cref="MetricsAdornmentControl"/>.
    /// </summary>
    public MetricsAdornmentControl()
    {
      InitializeComponent();
    }
    #endregion

    static Image GetImage(string imageName)
    {
      Image image;
      if (!_dicImages.ContainsKey(imageName))
      {
        image = new Image
        {
          Source = CreateImageSource(imageName)
        };
        _dicImages.Add(imageName, image);
      }
      else
        image = _dicImages[imageName];
      return image;
    }


    private static BitmapImage CreateImageSource(string imageName)
    {
      try
      {
        return new BitmapImage(new Uri($"pack://application:,,,/WSPack{RegistroVisualStudioObj.Instance.Version};component/Resources/{imageName}"));
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"Erro ao criar imagem {imageName}: {ex.Message}");
        return null;
      }
    }

    static bool IsFindAllReferencesAvailable
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread($"{nameof(IsFindAllReferencesAvailable)}");
        if (_comandoFindAddReferences == null)
          _comandoFindAddReferences = WSPackPackage.Dte.Commands.Item(CommandNames.EditFindAllReferences);

        return _comandoFindAddReferences != null && _comandoFindAddReferences.IsAvailable;
      }
    }
    static Command _comandoFindAddReferences = null;

    /// <summary>
    /// Calcular elementos com base na complexidade
    /// </summary>
    /// <param name="metrics">Modelo que contém as métricas.</param>
    /// <param name="corTexto">The cor texto.</param>
    /// <param name="tamanhoBorda">The tamanho borda.</param>
    /// <param name="backgroundBorda">The background borda.</param>
    /// <param name="brushBorda">The brush borda.</param>
    static void CalculateUIElements(MetricsModel metrics, out Color corTexto, out double tamanhoBorda,
      out Brush backgroundBorda, out Brush brushBorda)
    {
      if (metrics.CyclomaticComplexity.Level == CyclomaticLevels.Simples)
      {
        corTexto = Colors.White;
        tamanhoBorda = 12;
        backgroundBorda = Brushes.Green;
        brushBorda = Brushes.DarkGreen;
        if (metrics.CyclomaticComplexity.Value == 10)
          tamanhoBorda = 18;
      }

      else if (metrics.CyclomaticComplexity.Level == CyclomaticLevels.Moderada)
      {
        corTexto = Colors.Black;
        tamanhoBorda = 18;
        backgroundBorda = Brushes.Yellow;
        brushBorda = Brushes.DarkBlue;
      }

      else if (metrics.CyclomaticComplexity.Level == CyclomaticLevels.Complexa)
      {
        corTexto = Colors.Black;
        backgroundBorda = Brushes.DarkSalmon;
        brushBorda = Brushes.DarkRed;
        tamanhoBorda = 21;
      }

      else
      {
        corTexto = Colors.Black;
        backgroundBorda = Brushes.OrangeRed;
        brushBorda = Brushes.DarkRed;
        if (metrics.CyclomaticComplexity.Value <= 99)
          tamanhoBorda = 21;
        else
          tamanhoBorda = 25;
      }
    }

    /// <summary>
    /// Creates the block.
    /// </summary>
    /// <param name="method">Method</param>
    /// <param name="metrics">Modelo que contém as métricas.</param>
    /// <param name="toolTip">Tool tip</param>
    /// <returns></returns>
    public static FrameworkElement CreateBlock(MethodModel method, MetricsModel metrics, string toolTip)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      CalculateUIElements(metrics, out Color corTexto, out double tamanhoBorda, out Brush backgroundBorda, out Brush brushBorda);
      try
      {
        TextBlock textBlock = CreateTextBlock(metrics, toolTip, corTexto);
        ContextMenu menu = CreateContextMenu(method);
        Border borda = CreateBorder(backgroundBorda, brushBorda, tamanhoBorda);
        borda.ToolTip = toolTip;
        borda.ContextMenu = menu;
        borda.Child = textBlock;
        if (borda != null)
          return borda;
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"Erro ao criar borda de metrics: {ex}");
      }

      MetricsAdornmentControl control = null;
      try
      {
        control = new MetricsAdornmentControl
        {
          _method = method
        };
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"Erro ao criar MetricsAdornmentControl: {ex.Message}");
      }

      if (control != null)
      {
        control.SetBloco(metrics, corTexto, toolTip);
        control.SetBorda(backgroundBorda, brushBorda, tamanhoBorda, toolTip);
      }
      return control;
    }

    static TextBlock CreateTextBlock(MetricsModel metrics, string toolTipText, Color corTexto)
    {
      string numero = metrics.CyclomaticComplexity.Value.ToString();
      var textBlock = new TextBlock()
      {
        Text = numero,
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center,
        FontFamily = new FontFamily("Consolas"),
        FontSize = 9.0,
        Foreground = new SolidColorBrush(corTexto),
        ToolTip = toolTipText,
        Focusable = false
      };
      return textBlock;
    }

    static ContextMenu CreateContextMenu(MethodModel method)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var menu = new ContextMenu();

      #region Recortar o método
      var menuItem = new MenuItem
      {
        Header = ResourcesLib.strRecortarMetodo,
        ToolTip = ResourcesLib.strHintRecortarMetodo,
        Icon = GetImage("recortar.png")
      };
      menuItem.Click += (sender, e) => MenuItemRecortarClick(method, e);
      menu.Items.Add(menuItem);
      #endregion

      #region Copiar o método
      menuItem = new MenuItem
      {
        Header = ResourcesLib.strCopiarMetodo,
        ToolTip = ResourcesLib.strHintCopiarMetodo,
        Icon = GetImage("copiarMetodo.png")
      };
      menuItem.Click += (sender, e) => MenuItemCopiarClick(method, e);
      menu.Items.Add(menuItem);
      #endregion

      menu.Items.Add(new Separator());

      #region Ir para o final do método
      menuItem = new MenuItem
      {
        Header = ResourcesLib.strIrParaFinalMetodo,
        ToolTip = ResourcesLib.strHintIrParaFinalMetodo,
        Icon = GetImage("Arrow-Down.png")
      };
      menuItem.Click += (sender, e) => MenuItemIrFinalMetodoClick(method, e);
      menu.Items.Add(menuItem);
      #endregion

      #region Ir para o type
      menuItem = new MenuItem
      {
        Header = ResourcesLib.strIrParaType,
        ToolTip = ResourcesLib.strHintIrParaType,
        Icon = GetImage("Class.png")
      };
      menuItem.Click += (sender, e) => MenuItemIrTypeClick(method, e);
      menu.Items.Add(menuItem);
      #endregion

      menu.Items.Add(new Separator());

      #region Encontrar referências
      var menuItemEncontrarReferencias = new MenuItem
      {
        Header = ResourcesLib.StrEncontrarReferencias,
        ToolTip = ResourcesLib.strHintEncontrarReferencias
      };
      menuItemEncontrarReferencias.Click += (sender, e) =>
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        try
        {
          TextSelection selection = (TextSelection)WSPackPackage.Dte.ActiveDocument.Selection;
          selection.MoveToLineAndOffset(method.NameLocation.Line + 1, method.NameLocation.Column + 1);
          WSPackPackage.Dte.ExecuteCommand("Edit.FindAllReferences");
        }
        catch (Exception ex)
        {
          MessageBoxUtils.ShowError(ex.Message);
        }
      };
      menu.Items.Add(menuItemEncontrarReferencias);
      #endregion

      #region Método de teste
      if (method.IsTestMethod)
      {
        bool criar = true;
        if (WSPackPackage.Dte.Debugger != null && WSPackPackage.Dte.Debugger.CurrentMode != dbgDebugMode.dbgDesignMode)
          criar = false;

        if (criar)
        {
          menu.Items.Add(new Separator());

          #region Executar teste
          _menuItemExecutarTeste = new MenuItem
          {
            Header = ResourcesLib.strExecutarTeste,
            ToolTip = ResourcesLib.strHintExecutarTesteUnitario,
            Icon = GetImage("Test.png")
          };
          _menuItemExecutarTeste.Click += (sender, e) => ExecuteTest(method, "TestExplorer.RunAllTestsInContext");
          menu.Items.Add(_menuItemExecutarTeste);
          #endregion

          #region Depurar teste
          _menuItemDepurarTeste = new MenuItem
          {
            Header = ResourcesLib.strDepurarTeste,
            ToolTip = ResourcesLib.strHintDepurarTesteUnitario
          };
          _menuItemDepurarTeste.Click += (sender, e) => ExecuteTest(method, "TestExplorer.DebugAllTestsInContext");
          menu.Items.Add(_menuItemDepurarTeste);
          #endregion
        }
      }

      #endregion

      menu.Opened += (x, y) =>
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        if (_menuItemDepurarTeste != null && _menuItemExecutarTeste != null)
        {
          if (WSPackPackage.Dte?.Solution?.SolutionBuild is SolutionBuild sb)
          {
            vsBuildState vsBS = sb.BuildState;
            _menuItemDepurarTeste.IsEnabled = _menuItemExecutarTeste.IsEnabled = vsBS != vsBuildState.vsBuildStateInProgress;
          }
          else
            _menuItemDepurarTeste.IsEnabled = _menuItemExecutarTeste.IsEnabled = true;
        }

        menuItemEncontrarReferencias.IsEnabled = IsFindAllReferencesAvailable;
      };

      return menu;
    }

    static Border CreateBorder(Brush backgroundBorda, Brush brushBorda, double tamanhoBorda)
    {
      var border = new Border
      {
        Background = backgroundBorda,
        CornerRadius = new CornerRadius(3.0),
        BorderThickness = new Thickness(1),
        BorderBrush = brushBorda,
        VerticalAlignment = VerticalAlignment.Center,
        HorizontalAlignment = HorizontalAlignment.Center,
        Width = tamanhoBorda,
        Height = 13.0,
        Focusable = false
      };

      // Abrir menu
      border.PreviewMouseLeftButtonUp += (x, y) =>
      {
        y.Handled = true;
        Border element = (Border)x;
        element.ContextMenu.IsOpen = true;
      };

      border.MouseEnter += (a, b) =>
      {
        border.Cursor = Cursors.Hand;
      };

      return border;
    }

    private void SetBorda(Brush backgroundBorda, Brush brushBorda, double tamanhoBorda, string tooltip)
    {
      edtBorda.Background = backgroundBorda;
      edtBorda.CornerRadius = new CornerRadius(3.0);
      edtBorda.BorderThickness = new Thickness(1);
      edtBorda.BorderBrush = brushBorda;
      edtBorda.VerticalAlignment = VerticalAlignment.Center;
      edtBorda.HorizontalAlignment = HorizontalAlignment.Center;
      edtBorda.Width = tamanhoBorda;
      edtBorda.Height = 13.0;
      edtBorda.ToolTip = tooltip;

      // Abrir menu
      edtBorda.PreviewMouseLeftButtonUp += (x, y) =>
      {
        y.Handled = true;
        Border element = (Border)x;
        element.ContextMenu.IsOpen = true;
      };

      edtBorda.MouseEnter += (a, b) =>
      {
        edtBorda.Cursor = Cursors.Hand;
      };
    }

    private void SetBloco(MetricsModel metrics, Color corTexto, string tooltip)
    {
      string numero = metrics.CyclomaticComplexity.Value.ToString();
      edtBloco.Text = numero;
      edtBloco.HorizontalAlignment = HorizontalAlignment.Center;
      edtBloco.VerticalAlignment = VerticalAlignment.Center;
      edtBloco.FontFamily = new FontFamily("Consolas");
      edtBloco.FontSize = 9.0;
      edtBloco.Foreground = new SolidColorBrush(corTexto);
      edtBloco.ToolTip = tooltip;
    }

    private void ContextMenu_Opened(object sender, RoutedEventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      menuItemEncontrarReferencias.IsEnabled = IsFindAllReferencesAvailable;

      separaTeste.Visibility = menuItemExecutarTeste.Visibility = menuItemDepurarTeste.Visibility = Visibility.Collapsed;
      #region Método de teste
      if (_method.IsTestMethod)
      {
        bool exibir = true;
        if (WSPackPackage.Dte?.Debugger?.CurrentMode != EnvDTE.dbgDebugMode.dbgDesignMode)
          exibir = false;

        if (exibir)
        {
          separaTeste.Visibility = menuItemExecutarTeste.Visibility = menuItemDepurarTeste.Visibility = Visibility.Visible;
        }
      }
      #endregion
    }

    void MenuItemRecortar_Click(object sender, RoutedEventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      MenuItemRecortarClick(_method, e);
    }

    static void MenuItemRecortarClick(object methodSender, RoutedEventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (methodSender is MethodModel method)
      {
        try
        {
          TextSelection selection = (TextSelection)WSPackPackage.Dte.ActiveDocument.Selection;
          selection.MoveToLineAndOffset(method.BodyEndLocation.Line + 1, method.BodyEndLocation.Column + 1, false);
          selection.MoveToLineAndOffset(method.StartLocation.Line + 1, method.StartLocation.Column + 1, true);
          selection.Cut();
          selection.ActivePoint.TryToShow();
        }
        catch (Exception ex)
        {
          MessageBoxUtils.ShowError(ex.Message);
        }
      }
    }

    private void MenuItemCopiar_Click(object sender, RoutedEventArgs e)
    {
      MenuItemCopiarClick(_method, e);
    }

    static void MenuItemCopiarClick(object methodSender, RoutedEventArgs e)
    {
      if (methodSender is MethodModel method)
      {
        try
        {
          System.Windows.Forms.Clipboard.SetDataObject(method.SourceCode, true);
        }
        catch (Exception ex)
        {
          MessageBoxUtils.ShowError(ex.Message);
        }
      }
    }

    private void MenuItemIrFinalMetodo_Click(object sender, RoutedEventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      MenuItemIrFinalMetodoClick(_method, e);
    }

    static void MenuItemIrFinalMetodoClick(object methodSender, RoutedEventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (methodSender is MethodModel method)
      {
        try
        {
          TextSelection selection = (TextSelection)WSPackPackage.Dte.ActiveDocument.Selection;
          selection.MoveToLineAndOffset(method.BodyEndLocation.Line + 1, method.BodyEndLocation.Column + 1);
        }
        catch (Exception ex)
        {
          MessageBoxUtils.ShowError(ex.Message);
        }
      }
    }

    private void MenuItemIrType_Click(object sender, RoutedEventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      MenuItemIrFinalMetodoClick(_method, e);
    }

    static void MenuItemIrTypeClick(object methodSender, RoutedEventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (methodSender is MethodModel method)
      {
        try
        {
          TextSelection selection = (TextSelection)WSPackPackage.Dte.ActiveDocument.Selection;
          selection.MoveToLineAndOffset(method.ClassLocation.Line + 1, method.ClassLocation.Column + 1);
        }
        catch (Exception ex)
        {
          MessageBoxUtils.ShowError(ex.Message);
        }
      }
    }

    static void ExecuteTest(MethodModel method, string commandName)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      try
      {
        TextSelection selection = (TextSelection)WSPackPackage.Dte.ActiveDocument.Selection;
        selection.MoveToLineAndOffset(method.NameLocation.Line, method.NameLocation.Column);

        WSPackPackage.Dte.ExecuteCommand(commandName);
      }
      catch (Exception ex)
      {
        MessageBoxUtils.ShowError(ex.Message);
      }
    }

    private void menuItemExecutarTeste_Click(object sender, RoutedEventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      ExecuteTest(_method, "TestExplorer.RunAllTestsInContext");
    }

    private void menuItemDepurarTeste_Click(object sender, RoutedEventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      ExecuteTest(_method, "TestExplorer.DebugAllTestsInContext");
    }
  }
}
