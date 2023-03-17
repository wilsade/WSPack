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
  /// Comando para Trabalhar offiline do TFS
  /// </summary>
  internal sealed class WorkOfflineCommand : BaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.WorkOffline;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkOfflineCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public WorkOfflineCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      var vcService = (IVersionControlService)Package.GetGlobalService(typeof(IVersionControlService));
      var vcProvider = (IVersionControlProvider)Package.GetGlobalService(typeof(IVersionControlProvider));

      if (vcProvider == null || vcService == null)
        _menu.Enabled = false;

      else
      {
        int errorCode = vcProvider.IsAnySolutionFileControlledByHatteras(out bool isControlled);
        if (Microsoft.VisualStudio.ErrorHandler.Failed(errorCode) || !isControlled)
          _menu.Enabled = false;
        else
        {
          errorCode = vcService.GetSolutionOffline(out bool pOffline);
          _menu.Enabled = (!Microsoft.VisualStudio.ErrorHandler.Failed(errorCode) && !pOffline);
        }
      }
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static WorkOfflineCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new WorkOfflineCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var vcService = (IVersionControlService)Package.GetGlobalService(typeof(IVersionControlService));
      var vcProvider = (IVersionControlProvider)Package.GetGlobalService(typeof(IVersionControlProvider));

      vcService.SetSolutionOffline(true);

      string teamUrl = Utils.GetTeamFoundationServerExt().ActiveProjectContext.DomainUri;

      Microsoft.TeamFoundation.Client.RegisteredProjectCollection projectCollection =
        Microsoft.TeamFoundation.Client.RegisteredTfsConnections.GetProjectCollection(new Uri(teamUrl));
      if (projectCollection != null)
        projectCollection.Offline = true;

      vcProvider.RefreshStatus();
    }
  }
}