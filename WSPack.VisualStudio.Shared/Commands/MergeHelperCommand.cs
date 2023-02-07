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
using Microsoft.VisualStudio.TeamFoundation;

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
  /// Comando para exibição do Sobre
  /// </summary>
  internal sealed class MergeHelperCommand : BaseCommand
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MergeHelperCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected MergeHelperCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      var tfs = Utils.GetTeamFoundationServerExt();
      if (tfs != null)
      {
        tfs.ProjectContextChanged -= AlterouProjetoTFS;
        tfs.ProjectContextChanged += AlterouProjetoTFS;
      }
      else
      {
        Utils.LogDebugMessageForceShow("tfs.ProjectContextChanged: TFS NULO");
      }
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static MergeHelperCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new MergeHelperCommand(package, commandService);
    }

    void AlterouProjetoTFS(object sender, EventArgs e)
    {
      var tfs = sender as TeamFoundationServerExt;
      if (tfs.ActiveProjectContext != null && tfs.ActiveProjectContext.DomainUri != null)
      {
        var vcServer = tfs.GetVersionControlServer();
        if (vcServer != null)
        {
          vcServer.CommitCheckin -= FezCheckIn;
          vcServer.CommitCheckin += FezCheckIn;
        }
        else
        {
          Utils.LogOutputMessageSwitchToMainThread("vcServer.CommitCheckin: vcServer NULO");
        }
      }
      else
      {
        Utils.LogOutputMessageSwitchToMainThread("tfs.ActiveProjectContext.DomainUri NULO");
      }
    }

    static async System.Threading.Tasks.Task WriteInOutputSafeAsync(Action action)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(WSPackPackage.Instance.DisposalToken);
      action.Invoke();
    }

    static void FezCheckIn(object sender, CommitCheckinEventArgs e)
    {
      Trace.WriteLine("Vou entrar no CheckTemplateCheckIn");
      CheckTemplateCheckIn(e);
      Trace.WriteLine("Sai CheckTemplateCheckIn");

      try
      {
        Trace.WriteLine("FezCheckIn: 1");
        if (!WSPackPackage.ParametrosGerais.AbrirTelaMerge)
          return;

        Trace.WriteLine("FezCheckIn: 2");
        IntPtr prt = Process.GetCurrentProcess().MainWindowHandle;
        Trace.WriteLine("FezCheckIn: 3");
        var wrapper = new Win32WindowWrapper(prt);

        Trace.WriteLine("FezCheckIn: 4");
        var form = new MergeHelperForm(e.ChangesetId, false);
        Trace.WriteLine("FezCheckIn: 5");
        if (form.ShowDialog(wrapper) == DialogResult.OK)
        {
          RegistroVisualStudioObj.Instance.Merge(e.ChangesetId, form.BranchOrigemLocalPath, form.BranchDestinoLocalPath,
            (string erro) =>
            {
              Utils.LogOutputMessage(erro);
            }
          );
        }
        WSPackPackage.ParametrosGerais.AbrirTelaMerge = !form.NaoMostrarTelaMerge;
        WSPackPackage.ParametrosGerais.SaveSettingsToStorage();

      }
      catch (Exception ex)
      {
        MessageBoxUtils.ShowError(ex.Message);
      }
    }

    private static void CheckTemplateCheckIn(CommitCheckinEventArgs e)
    {
      try
      {
        string template = WSPackPackage.ParametrosTemplateCheckIn.TemplateCheckIn;
        if (template.IsNullOrWhiteSpaceEx())
        {
          _ = WriteInOutputSafeAsync(() =>
          {
            Utils.LogDebugMessage("FezCheckIn: template NULO");
          });
          return;
        }

        Changeset cs = e.Workspace.VersionControlServer.ChangeSetDetails(e.ChangesetId);
        if (cs != null)
        {
          template = TemplateCheckInCommand.GerarTemplateCheckIn(template, cs);
          //CopyLocalPathBaseCommand.CopyToClipboard(template);
          //Instance._package.MessageBoxShellWarningOk(
          //  ResourcesLib.StrCopiadoAreaTransferencia, "Template de CheckIn");
          Trace.WriteLine("CheckTemplateCheckIn: a");
          _ = WriteInOutputSafeAsync(() =>
          {
            Trace.WriteLine("CheckTemplateCheckIn: b");
            Utils.LogOutputMessage(template);
            Trace.WriteLine("CheckTemplateCheckIn: c");
          });
        }
        else
        {
          _ = WriteInOutputSafeAsync(() =>
          {
            Utils.LogDebugMessage("FezCheckIn: cs NULO");
          });
        }

      }
      catch (Exception ex)
      {
        string msg = $"Erro ao escrever Template com informações do Check In: {ex.GetCompleteMessage()}";
        _ = WriteInOutputSafeAsync(() =>
        {
          Utils.LogDebugError(msg);
        });
        Trace.WriteLine(msg);
      }
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      Trace.WriteLine("Não aplicável");
    }
  }
}