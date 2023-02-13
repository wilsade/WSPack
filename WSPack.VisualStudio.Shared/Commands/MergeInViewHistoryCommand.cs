using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;
using WSPack.VisualStudio.Shared.Forms;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para Auxílio de merge no View History
  /// </summary>
  internal sealed class MergeInViewHistoryCommand : BaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.MergeInViewHistory;

    /// <summary>
    /// Initializes a new instance of the <see cref="MergeInViewHistoryCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected MergeInViewHistoryCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static MergeInViewHistoryCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new MergeInViewHistoryCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var vcExt = WSPackPackage.Dte.GetVersionControlExt();
      if (vcExt == null)
      {
        MessageBoxUtils.ShowWarning(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
        return;
      }

      VersionControlHistoryChangesetItem changeset;
      VersionControlHistory controlHistory = vcExt.History.ActiveWindow;
      if (controlHistory.SelectedChangesets.Length > 1)
      {
        MessageBoxUtils.ShowWarning(ResourcesLib.StrSelecioneApenasUmItem);
        return;
      }

      changeset = controlHistory.SelectedChangesets[0];
      if (changeset != null)
      {
        _ = Task.Run(() =>
        {
          MostrarForm(changeset);
        });
      }
      else
        MessageBoxUtils.ShowInformation(ResourcesLib.StrItemNaoEncontrado.FormatWith("changeset"));
    }

    private void MostrarForm(VersionControlHistoryChangesetItem changeset)
    {
      IntPtr prt = Process.GetCurrentProcess().MainWindowHandle;
      var wrapper = new Win32WindowWrapper(prt);
      var form = new MergeHelperForm(changeset.ChangesetId, !WSPackPackage.ParametrosGerais.AbrirTelaMerge);
      if (form.ShowDialog(wrapper) == DialogResult.OK)
      {
        WSPackPackage.ParametrosGerais.AbrirTelaMerge = !form.NaoMostrarTelaMerge;
        WSPackPackage.ParametrosGerais.SaveSettingsToStorage();
        RegistroVisualStudioObj.Instance.Merge(changeset.ChangesetId, form.BranchOrigemLocalPath, form.BranchDestinoLocalPath,
          (string erro) =>
          {
            Utils.LogOutputMessageSwitchToMainThread(erro);
          }
        );
      }
    }
  }
}