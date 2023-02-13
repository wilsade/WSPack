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

using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para Localizar um item do Source Control Explorer ]no Windows
  /// </summary>
  internal sealed class LocateInWindowsSourceControlExplorerCommand : LocateInWindowsBaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.LocateInWindowsSourceControlExplorer;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocateInWindowsSourceControlExplorerCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected LocateInWindowsSourceControlExplorerCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus; ;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      var vcExt = Utils.GetVersionControlServerExt();
      _menu.Enabled = vcExt?.GetSelectedItems()?.Length > 0;
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static LocateInWindowsSourceControlExplorerCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new LocateInWindowsSourceControlExplorerCommand(package, commandService);
    }

    /// <summary>
    /// Recuperar o item local conforme tipo de comando
    /// </summary>
    /// <returns>Lista contendo informações do Workspace e do LocalItem</returns>
    protected override List<(Workspace Ws, string LocalItem)> GetLocalItem()
    {
      string serverItem = Utils.GetSourceControlExplorerSelectedItem();
      if (!string.IsNullOrEmpty(serverItem))
      {
        var tfs = Utils.GetTeamFoundationServerExt();
        var lstWorkspaceLocalItem = tfs.GetLocalItemForServerItem(serverItem);
        return lstWorkspaceLocalItem;
      }
      return new List<(Workspace Ws, string LocalItem)>();
    }
  }
}