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
  /// Comando para exibição do Log Registro de atividade 
  /// </summary>
  internal sealed class ActivityLogCommand : BaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.ActivityLog;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivityLogCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected ActivityLogCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += MenuItem_BeforeQueryStatus;
    }

    private void MenuItem_BeforeQueryStatus(object sender, EventArgs e)
    {
      _menu.ParametersDescription = Path.Combine(_package.UserDataPath, "ActivityLog.xml");
      _menu.Enabled = File.Exists(_menu.ParametersDescription);
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static ActivityLogCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new ActivityLogCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      try
      {
        if (File.Exists(_menu.ParametersDescription))
          Utils.LaunchBrowser(_menu.ParametersDescription);
        else
          Utils.LogOutputMessageForceShow(string.Format(ResourcesLib.StrItemNaoEncontrado, _menu.ParametersDescription));
      }
      catch (Exception ex)
      {
        Utils.LogOutputMessageForceShow(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao +
          Environment.NewLine + ex.Message);
      }
    }
  }
}