using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
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
  /// Comando para exibição do Sobre
  /// </summary>
  internal sealed class CopyServerPathSourceControlExplorerCommand : CopyServerPathBaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.CopyServerPathSourceControlExplorer;

    /// <summary>
    /// Initializes a new instance of the <see cref="CopyServerPathSolutionExplorerCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected CopyServerPathSourceControlExplorerCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      var vcExt = Utils.GetVersionControlServerExt();
      _menu.Enabled = vcExt?.GetSelectedItems()?.Length > 0;
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static CopyServerPathSourceControlExplorerCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new CopyServerPathSourceControlExplorerCommand(package, commandService);
    }

    /// <summary>
    /// Recuperar o item 'Server' conforme tipo de comando
    /// </summary>
    /// <returns>item 'Server' conforme tipo de comando</returns>
    public override string GetServerItem()
    {
      string serverItem = Utils.GetSourceControlExplorerSelectedItem();
      return serverItem;
    }
  }
}
