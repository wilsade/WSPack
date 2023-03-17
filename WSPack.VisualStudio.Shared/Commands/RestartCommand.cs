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
using Microsoft.VisualStudio.Shell.Interop;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para reiniciar o VS
  /// </summary>
  internal sealed class RestartCommand : BaseCommand
  {
    readonly OleMenuCommand _menuAsAdm;

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.RestartVisualStudio;

    /// <summary>
    /// Initializes a new instance of the <see cref="RestartCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public RestartCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      var comandoIdAsAdm = new CommandID(CommandSet, CommandIds.RestartVisualStudioAsADM);
      _menuAsAdm = new OleMenuCommand(DoExecute, comandoIdAsAdm);
      commandService.AddCommand(_menuAsAdm);
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static RestartCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new RestartCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var ivsShell = _package.GetService<SVsShell, IVsShell4>();
      if (sender == _menuAsAdm)
        ivsShell.Restart((uint)__VSRESTARTTYPE.RESTART_Elevated);
      else
        ivsShell.Restart((uint)__VSRESTARTTYPE.RESTART_Normal);
    }
  }
}