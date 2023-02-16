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
  /// Comando para adicionar um botão no Solution Explorer para fazer Pull no Git
  /// </summary>
  internal sealed class GitPullToolbarSolutionExplorerCommand : BaseCommand
  {
    readonly EnvDTE.Command _command;
    bool? _canShowIfGit;

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.GitPullToolbarSolutionExplorer;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitPullToolbarSolutionExplorerCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected GitPullToolbarSolutionExplorerCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      _command = WSPackPackage.Dte.Commands.Item(CommandNames.TeamGitPull);
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      _menu.Enabled = _menu.Visible = (_canShowIfGit ?? true) &&
        WSPackPackage.ParametrosGerais.ShowGitPullInSolutionExplorerToolbar &&
        WSPackPackage.Dte?.Solution?.FullName.IsNullOrWhiteSpaceEx() == false &&
        _command != null;
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static GitPullToolbarSolutionExplorerCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new GitPullToolbarSolutionExplorerCommand(package, commandService);
    }

    /// <summary>
    /// Indica se o botão pode ser exibido
    /// </summary>
    public void SetCanShowIsGit(bool canShow)
    {
      _canShowIfGit = canShow;
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      _menu.Enabled = false;
      try
      {
        if (_command.IsAvailable)
          WSPackPackage.Dte.ExecuteCommand(CommandNames.TeamGitPull);
        else
          _ = Utils.TryPullAsync(WSPackPackage.Dte.Solution.FullName);
      }
      finally
      {
        _menu.Enabled = true;
      }
    }
  }
}