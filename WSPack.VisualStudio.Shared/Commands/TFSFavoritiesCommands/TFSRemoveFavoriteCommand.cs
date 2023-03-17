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
using Microsoft.VisualStudio.TeamFoundation.VersionControl;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para remover um itens dos Favoritos do TFS
  /// </summary>
  internal sealed class TFSRemoveFavoriteCommand : BaseCommand
  {
    bool _isCommandRead = false;

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.TFSRemoveFavorite;

    /// <summary>
    /// Initializes a new instance of the <see cref="TFSRemoveFavoriteCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public TFSRemoveFavoriteCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      _isCommandRead = true;
      VersionControlExt VCExt = Utils.GetVersionControlServerExt();
      if (VCExt != null && VCExt.Explorer != null && VCExt.Explorer.SelectedItems != null)
      {
        // Apenas um item selecionado
        if (VCExt.Explorer.SelectedItems.Length != 1)
          _menu.Enabled = false;

        // Se já adicionou o favorito, desabilita
        else
        {
          var lst = TFSFavoritesManagerCommand.LoadFromXML();
          _menu.Enabled = TFSFavoritesManagerForm.GetOrderedListFromServer(lst, Utils.GetTeamFoundationServerExt().ActiveProjectContext.DomainUri)
            .Any(x => x.ServerItem.EqualsInsensitive(VCExt.GetSelectedItem().SourceServerPath));
        }
      }
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static TFSRemoveFavoriteCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new TFSRemoveFavoriteCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (!_isCommandRead)
      {
        Utils.LogOutputMessageForceShow(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
        return;
      }

      VersionControlExt VCExt = Utils.GetVersionControlServerExt();
      if (VCExt == null || VCExt.GetSelectedItem() == null)
        return;

      string serverItem = VCExt.GetSelectedItem().SourceServerPath;
      if (MessageBoxUtils.ShowWarningYesNo(ResourcesLib.StrConfirmaExclusaoFavorito, serverItem))
      {
        var TFSExt = Utils.GetTeamFoundationServerExt();
        var lst = TFSFavoritesManagerCommand.LoadFromXML();
        var achei = lst.FirstOrDefault(x => x.TFSUrl.EqualsInsensitive(TFSExt.ActiveProjectContext.DomainUri) && x.ServerItem.EqualsInsensitive(serverItem));
        if (achei != null)
        {
          lst.Remove(achei);
          XmlUtils.SaveXMLParams(lst, WSPackConsts.TFSFavoritesConfigPath);
        }
        else
          WSPackPackage.Dte.WriteInOutPutForceShow(ResourcesLib.StrItemNaoEncontrado.FormatWith(serverItem));
      }
    }
  }
}