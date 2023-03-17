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
  /// Comando para montar itens de menu dos favoritos que já foram adicionados
  /// </summary>
  internal sealed class TFSAddedFavoritesCommand : BaseCommand
  {
    bool _isCommandRead = false;
    OleMenuCommand _menuDinamico;
    readonly List<OleMenuCommand> _lstMenus;
    readonly string _chave = "@";

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.TFSAddedFavorites;

    /// <summary>
    /// Initializes a new instance of the <see cref="TFSAddedFavoritesCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public TFSAddedFavoritesCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _lstMenus = new List<OleMenuCommand>();
      _menu.BeforeQueryStatus += BeforeExecute;
    }

    /// <summary>
    /// Acontece antes da ação do comando. Vamos montar os menus
    /// </summary>
    /// <param name="sender">Objeto que chamou</param>
    /// <param name="e">Argumentos do evento</param>
    protected override void BeforeExecute(object sender, EventArgs e)
    {
      _isCommandRead = true;
      var menu = sender as OleMenuCommand;

      menu.Visible = menu.Enabled = true;

      _menuDinamico = menu;

      var TFSExt = Utils.GetTeamFoundationServerExt();
      if (TFSExt == null)
        return;
      if (TFSExt.ActiveProjectContext.DomainUri == null)
      {
        menu.Visible = false;
        return;
      }

      var lstFavoritos = TFSFavoritesManagerCommand.LoadFromXML();
      MontarFavoritos(lstFavoritos, TFSExt.ActiveProjectContext.DomainUri);

      // Este é o menu que serve de base para os demails.
      // O menu só vai ficar visível se tiver pelo menu um favorito definido no servidor conectado
      var listaFavoritosServidor = TFSFavoritesManagerForm.GetOrderedListFromServer(lstFavoritos, TFSExt.ActiveProjectContext.DomainUri);

      menu.Visible = listaFavoritosServidor.Count > 0;
      if (menu.Visible)
      {
        // O menu base terá o comportamento do primeiro favorito definido
        menu.Text = listaFavoritosServidor[0].Caption;
        menu.ParametersDescription = "Menu dinâmico";
        menu.Properties.Clear();
        menu.Properties.Add(_chave, listaFavoritosServidor[0]);
      }
    }

    private void MontarFavoritos(List<TFSFavoritesParams> lst, string serverUrl)
    {
      OleMenuCommandService mcs = null;
      ThreadHelper.JoinableTaskFactory.Run(async () =>
      {
        mcs = await GetCommandServiceAsync();
      });
      if (mcs == null)
        return;

      foreach (var item in _lstMenus)
      {
        mcs.RemoveCommand(item);
      }
      _lstMenus.Clear();

      var listaFavoritos = TFSFavoritesManagerForm.GetOrderedListFromServer(lst, serverUrl);
      int sum = 1;
      foreach (TFSFavoritesParams row in listaFavoritos)
      {
        int commandId = CommandId + sum;
        CommandID comando = new CommandID(CommandSet, commandId);
        OleMenuCommand menu = new OleMenuCommand(NavegarParaFavorito, comando);
        menu.BeforeQueryStatus += menuAdicionado_BeforeQueryStatus;
        menu.Properties.Add(_chave, row);
        menu.Text = row.Caption;
        mcs.AddCommand(menu);

        _lstMenus.Add(menu);
        sum++;
      }
    }

    /// <summary>
    /// Acontece na verificação dos comandos dos menus de favoritos
    /// </summary>
    /// <param name="sender">Menu</param>
    /// <param name="e">Parâmetros</param>
    void menuAdicionado_BeforeQueryStatus(object sender, EventArgs e)
    {
      OleMenuCommand menu = (OleMenuCommand)sender;

      menu.Visible = menu.Enabled = true;

      // O primeiro menu favorito está "replicado" no menu base. Vamos escondê-lo
      if (menu == _lstMenus[0])
      {
        _menuDinamico.Text = menu.Text;
        menu.Visible = false;
      }
      else
        menu.Visible = true;
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static TFSAddedFavoritesCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new TFSAddedFavoritesCommand(package, commandService);
    }

    /// <summary>
    /// Método para navegar para um favorito
    /// </summary>
    /// <param name="sender">Item de favorito</param>
    /// <param name="e">Argumentos</param>
    void NavegarParaFavorito(object sender, EventArgs e)
    {
      OleMenuCommand menu = (OleMenuCommand)sender;
      TFSFavoritesParams row = (TFSFavoritesParams)menu.Properties[_chave];

      var server = Utils.GetVersionControlServerExt();
      if (server?.Explorer != null)
      {
        LocateInTFSBaseCommand.NavigateToServerItem(server, row.ServerItem);
      }
      else
        Utils.LogOutputMessageForceShow(ResourcesLib.StrSourceControlExplorerNaoConfigurado);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (_isCommandRead)
        NavegarParaFavorito(sender, e);
      else
      {
        Utils.LogOutputMessageForceShow(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
      }
    }
  }
}