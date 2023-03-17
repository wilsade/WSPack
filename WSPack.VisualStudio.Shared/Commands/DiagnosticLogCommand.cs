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
  /// Comando para exibição do Registro de diagnóstico
  /// </summary>
  internal sealed class DiagnosticLogCommand : BaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.DiagnosticLog;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiagnosticLogCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public DiagnosticLogCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      _menu.Enabled = false;
      try
      {
        var path = Path.GetTempPath();
        var di = new DirectoryInfo(path);
        var files = di?.EnumerateFiles("*.failure.txt");
        FileInfo primeiro = files?.OrderByDescending(x => x.CreationTime)
          .FirstOrDefault();
        _menu.ParametersDescription = primeiro?.FullName;
        _menu.Enabled = primeiro?.Exists == true;
      }
      catch (Exception ex)
      {
        Utils.LogDebugError(ex.Message);
      }
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static DiagnosticLogCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new DiagnosticLogCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      try
      {
        if (File.Exists(_menu.ParametersDescription))
          WSPackPackage.Dte.ItemOperations.OpenFile(_menu.ParametersDescription,
          EnvDTE.Constants.vsViewKindTextView);
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