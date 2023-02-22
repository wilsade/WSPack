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
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para Desconectar-se do TFS e sair do Visual Studio
  /// </summary>
  internal sealed class DisconnectAndCloseCommand : BaseCommand
  {
    readonly EnvDTE.Command _command;

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.DisconnectAndClose;

    /// <summary>
    /// Initializes a new instance of the <see cref="DisconnectAndCloseCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected DisconnectAndCloseCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      _command = WSPackPackage.Dte.Commands.Item(CommandNames.TeamDisconnectfromServer);
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      _menu.Enabled = _menu.Visible = _command.IsAvailable;
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static DisconnectAndCloseCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new DisconnectAndCloseCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      WSPackPackage.Dte.ExecuteCommand(CommandNames.TeamDisconnectfromServer);
      WSPackPackage.Dte.ExecuteCommand(CommandNames.FileExit);
    }
  }
}