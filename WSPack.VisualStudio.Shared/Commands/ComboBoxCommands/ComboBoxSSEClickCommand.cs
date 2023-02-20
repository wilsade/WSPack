using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;
using WSPack.Lib.Items;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para: Adicionar um comboBox na ToolBar do Source Control Explorer e 
  /// adicionar uma ação ao selecionar um item
  /// </summary>
  internal sealed class ComboBoxSSEClickCommand : BaseDropDownComboCommand
  {
    List<TFSFavoritesParams> _listaFavoritosServidor;

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.SSEComboBoxFavoritos;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComboBoxSSEClickCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected ComboBoxSSEClickCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      var TFSExt = Utils.GetTeamFoundationServerExt();
      OleMenuCommand menu = sender as OleMenuCommand;
      menu.Visible = menu.Enabled = true;

      menu.Enabled = TFSExt != null && TFSExt.ActiveProjectContext.DomainUri != null;
      if (menu.Enabled)
        _listaFavoritosServidor = TFSFavoritesManagerForm.GetOrderedListFromServer(TFSFavoritesManagerCommand.LoadFromXML(),
          TFSExt.ActiveProjectContext.DomainUri);
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static ComboBoxSSEClickCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new ComboBoxSSEClickCommand(package, commandService);
    }

    /// <summary>
    /// Item selecionado no combo
    /// </summary>
    public string FavoritoSelecionado
    {
      get
      {
        if (_listaFavoritosServidor == null ||
          string.IsNullOrWhiteSpace(ItemSelecionadoCombo) ||
          ItemSelecionadoCombo.EqualsInsensitive(Constantes.GerenciadorFavoritos))
          return null;
        string tfsPath = _listaFavoritosServidor.FirstOrDefault(x => x.Caption == ItemSelecionadoCombo)?.ServerItem;
        return tfsPath;
      }
    }

    string[] ListaItems
    {
      get
      {
        var TFSExt = Utils.GetTeamFoundationServerExt();
        _listaFavoritosServidor = TFSFavoritesManagerForm.GetOrderedListFromServer(TFSFavoritesManagerCommand.LoadFromXML(),
          TFSExt.ActiveProjectContext.DomainUri);
        return _listaFavoritosServidor.Select(x => x.Caption).Union(new string[] { Constantes.GerenciadorFavoritos }).ToArray();
      }
    }

    /// <summary>
    /// Recuperar a lista de itens que será preenchida no ComboBox
    /// </summary>
    /// <returns>Lista de itens que será preenchida no ComboBox</returns>
    protected override string[] GetItems()
    {
      return ListaItems;
    }

    /// <summary>
    /// Método a ser disparado quando o item for selecionado
    /// </summary>
    /// <param name="item">Item selecionado</param>
    protected override void ItemSelected(string item)
    {
      // Exibir o gerenciador de favoritos
      if (item == Constantes.GerenciadorFavoritos)
        ExibirGerenciadorFavoritos();

      // Navegar para favoritos
      else
      {
        var server = Utils.GetVersionControlServerExt();
        if (server?.Explorer != null)
        {
          string tfsPath = _listaFavoritosServidor.FirstOrDefault(x => x.Caption == item).ServerItem;
          LocateInTFSBaseCommand.NavigateToServerItem(server, tfsPath);
        }
        else
          Utils.LogOutputMessageForceShow(ResourcesLib.StrSourceControlExplorerNaoConfigurado);
      }
    }

    private static void ExibirGerenciadorFavoritos()
    {
      try
      {
        WSPackPackage.Dte.ExecuteCommand("WSPack.GerenciadorFavoritosTFS");
      }
      catch (Exception ex)
      {
        Utils.LogDebugError(ex.Message);
      }
    }
  }
}