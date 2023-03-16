using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamFoundation;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;

using WSPack.Lib.Extensions;
using WSPack.VisualStudio.Shared.Extensions;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Controlador de navegação do SCE
  /// </summary>
  class SCENavigationController
  {
    static readonly object _locker = new object();
    private static SCENavigationController _instance;
    readonly List<string> _lstServerPaths;
    int _indexAtual = -1;
    bool _navegando = false;
    bool? _oldUsarMenu;

    List<(string Caption, string ToolTip)> _lstControlesMenu;
    CommandBarButton _botao;
    readonly EnvDTE.Events _eventos;

    VersionControlExt _vce;
    VersionControlExt VCE => _vce ?? (_vce = Utils.GetVersionControlServerExt());

    /// <summary>
    /// Inicialização da classe: <see cref="SCENavigationController"/>.
    /// </summary>
    private SCENavigationController()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      _lstServerPaths = new List<string>();
      var tfs = Utils.GetTeamFoundationServerExt();
      if (tfs != null)
      {
        tfs.ProjectContextChanged -= AlterouProjetoTFS;
        tfs.ProjectContextChanged += AlterouProjetoTFS;
      }

      _eventos = WSPackPackage.Dte.Events;
      _eventos.WindowEvents.WindowActivated -= _eventos_WindowActivated;
      _eventos.WindowEvents.WindowActivated += _eventos_WindowActivated;
    }

    static void CreateSingleton()
    {
      lock (_locker)
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        _instance = new SCENavigationController();
      }
    }

    /// <summary>
    /// Devolve a instãncia da classe: <see cref="SCENavigationController"/>
    /// </summary>
    /// <value>Instância da classe: <see cref="SCENavigationController"/></value>
    internal static SCENavigationController Instance
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        if (_instance == null)
          CreateSingleton();
        return _instance;
      }
    }

    private void _eventos_WindowActivated(EnvDTE.Window gotFocus, EnvDTE.Window lostFocus)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      string caption = gotFocus.Caption;
      if (!caption.StartsWith("Source Control Explorer", StringComparison.OrdinalIgnoreCase))
        return;
      PrepareGroup(gotFocus);
    }

    void PegarControlesMenu(CommandBarPopup popup)
    {
      _lstControlesMenu = new List<(string Caption, string ToolTip)>();
      for (int i = 1; i <= popup.Controls.Count; i++)
      {
        if (popup.Controls[i] is CommandBarButton barButton)
          _lstControlesMenu.Add((barButton.Caption, barButton.TooltipText));
      }
    }

    private void PrepareGroup(EnvDTE.Window janela)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      bool okToGo = false;

      try
      {
        if (_botao == null)
          okToGo = true;
        else
          Trace.WriteLine(_botao.Caption);
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
        okToGo = true;
      }

      if (!okToGo)
      {
        okToGo = !_oldUsarMenu.HasValue ||
          _oldUsarMenu.Value != WSPackPackage.ParametrosGerais.UseMenuControllerToolbarSCE;
      }

      if (!okToGo)
        return;

      _oldUsarMenu = WSPackPackage.ParametrosGerais.UseMenuControllerToolbarSCE;
      try
      {
        var prop = janela.GetType().GetProperty("CommandBars");
        var barraTipada = prop.GetValue(janela) as CommandBars;
        var toolbar = barraTipada[1];
        for (int j = 1; j <= toolbar.Controls.Count; j++)
        {
          var controleAtual = toolbar.Controls[j];
          if (controleAtual is CommandBarPopup popup)
          {
            if (popup.Caption == "WSPack.MenuSCE")
            {
              popup.Visible = WSPackPackage.ParametrosGerais.UseMenuControllerToolbarSCE;
              if (popup.Visible)
              {
                _botao = popup.Controls[1] as CommandBarButton;
                PegarControlesMenu(popup);
              }
            }
            continue;
          }

          if (controleAtual is CommandBarComboBox comboBox)
          {
            if (comboBox.Caption == "Favoritos:")
            {
              comboBox.BeginGroup = true;
            }
          }

          if (!(controleAtual is CommandBarButton controle))
          {
            continue;
          }

          controle.Visible = NotInMenuController(controle);
          if (controle.TooltipText == "Navegar para o item anterior no Source Control Explorer")
          {
            controle.BeginGroup = true;
          }
        }
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"Erro ao preparar grupo da Toolbar do SCE: {ex}");
      }
    }

    private bool NotInMenuController(CommandBarButton controle)
    {
      if (controle == null || _lstControlesMenu == null ||
        !WSPackPackage.ParametrosGerais.UseMenuControllerToolbarSCE)
        return true;
      try
      {
        string caption = controle.Caption;
        string tooltip = controle.TooltipText;
        var achei = _lstControlesMenu.Any(x => x.Caption.EqualsInsensitive(caption) ||
          x.ToolTip.EqualsInsensitive(tooltip));
        return !achei;
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
        return true;
      }
    }

    [Conditional("DEBUG")]
    private void Inspecionar(CommandBarButton controle)
    {
      try
      {
        var props = controle.GetType().GetProperties().OrderBy(x => x.Name).ToList();
        var lst = new List<(string Nome, object Valor)>();
        foreach (var item in props)
        {
          try
          {
            var nome = item.Name;
            var valor = item.GetValue(controle);
            lst.Add((nome, valor));
          }
          catch (Exception pEx)
          {
            Debug.WriteLine(pEx);
          }
        }
        foreach (var item in lst)
        {
          Trace.WriteLine($"{item.Nome}: {item.Valor}");
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);
      }
    }

    private void Initialize()
    {
      _lstServerPaths.Clear();
      _indexAtual = 0;
    }

    void AlterouProjetoTFS(object sender, EventArgs e)
    {
      var tfs = sender as TeamFoundationServerExt;
      if (tfs?.ActiveProjectContext?.DomainUri == null)
        return;

      //PrepareGroup();

      VCE.Explorer.SelectionChanged -= Explorer_SelectionChanged;
      VCE.Explorer.SelectionChanged += Explorer_SelectionChanged;
      Initialize();
      var item = VCE.GetSelectedItem()?.SourceServerPath;
      if (item != "$/")
        ProcessarItem(VCE.GetSelectedItem()?.SourceServerPath);
    }


    void ProcessarItem(string serverItem)
    {
      if (serverItem.IsNullOrWhiteSpaceEx())
        return;

      int index = _lstServerPaths.FindIndex(x => x.EqualsInsensitive(serverItem));
      if (index >= 0)
        _lstServerPaths.RemoveAt(index);

      _lstServerPaths.Add(serverItem);
      _indexAtual = _lstServerPaths.Count - 1;
    }

    private void Explorer_SelectionChanged(object sender, EventArgs e)
    {
      if (_navegando)
        return;

      var item = VCE?.GetSelectedItem()?.SourceServerPath;
      ProcessarItem(item);
    }

    internal bool CanNavigateBack() => _indexAtual > 0;
    internal bool CanNavigateForward() => _indexAtual >= 0 &&
      _indexAtual <= _lstServerPaths.Count - 2;
    internal bool CanNavigateUp() => Convert.ToString(VCE?.GetSelectedItem()?.SourceServerPath)?
      .Length > 2;
    internal bool HasNavigation() => _lstServerPaths.Count > 0;

    internal bool CanNavigateSolutionProject()
    {
      var item = VCE?.Explorer?.CurrentFolderItem?.SourceServerPath;
      if (item.IsNullOrWhiteSpaceEx())
        return false;

      if (item.EndsWith("/"))
        item = item.Substring(0, item.Length - 1);
      var split = item.Split('/');

      return split.Length >= 2;
    }

    internal void IniciarItemAtual()
    {
      lock (_locker)
      {
        //PrepareGroup();
        if (_indexAtual == -1)
          Explorer_SelectionChanged(null, EventArgs.Empty);
      }
    }

    internal void NavigateBack()
    {
      _navegando = true;
      try
      {
        var item = _lstServerPaths[_indexAtual - 1];
        LocateInTFSBaseCommand.NavigateToServerItem(VCE, item);
        _indexAtual--;
      }
      finally
      {
        _navegando = false;
      }
    }

    internal void NavigateForward()
    {
      _navegando = true;
      try
      {
        var item = _lstServerPaths[_indexAtual + 1];
        LocateInTFSBaseCommand.NavigateToServerItem(VCE, item);
        _indexAtual++;
      }
      finally
      {
        _navegando = false;
      }
    }

    internal void NavigateUp()
    {
      var item = VCE.GetSelectedItem().SourceServerPath;
      int index = item.LastIndexOf("/");
      string target = item.Substring(0, index);
      if (target == "$")
        target += "/";
      VCE.Explorer.Navigate(target);
    }

    internal void ClearNavigation()
    {
      Initialize();
    }
  }
}