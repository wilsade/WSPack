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
using WSPack.Lib.Items;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para adicionar um item favorito do TFS
  /// </summary>
  internal sealed class TFSAddFavoriteCommand : BaseCommand
  {
    bool _isCommandRead = false;

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.TFSAddFavorite;

    /// <summary>
    /// Initializes a new instance of the <see cref="TFSAddFavoriteCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected TFSAddFavoriteCommand(AsyncPackage package, OleMenuCommandService commandService)
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
          _menu.Enabled = !TFSFavoritesManagerForm.GetOrderedListFromServer(lst, Utils.GetTeamFoundationServerExt().ActiveProjectContext.DomainUri)
            .Any(x => x.ServerItem.EqualsInsensitive(VCExt.GetSelectedItem().SourceServerPath));
        }
      }
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static TFSAddFavoriteCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new TFSAddFavoriteCommand(package, commandService);
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

      var TFSExt = Utils.GetTeamFoundationServerExt();
      VersionControlExplorerItem item = VCExt.GetSelectedItem();
      string serverItem = item.SourceServerPath;

      var lst = TFSFavoritesManagerCommand.LoadFromXML();
      TFSFavoritesParams row = lst.FirstOrDefault(x => x.TFSUrl.EqualsInsensitive(TFSExt.ActiveProjectContext.DomainUri) && x.ServerItem.EqualsInsensitive(serverItem));
      if (row != null)
        MessageBoxUtils.ShowWarning(ResourcesLib.StrFavoritoExistente, serverItem);
      else
      {
        var info = new FileInfo(item.TargetServerPath);

        if (MessageBoxUtils.InputBox(ResourcesLib.StrInformeNomeFavorito, ResourcesLib.StrCriandoFavorito, out var caption, info.Name))
        {
          TFSFavoritesManagerForm.AdicionarFavorito(lst, TFSExt.ActiveProjectContext.DomainUri, serverItem, caption,
            lstToSave =>
            {
              XmlUtils.SaveXMLParams(lstToSave, WSPackConsts.TFSFavoritesConfigPath);
            });
          Utils.LogOutputMessage(ResourcesLib.StrFavoritoAdicionadoSucesso.FormatWith(serverItem));
        }
      }
    }
  }
}