using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
  /// Comando para alterar o controle de fonte entre TFS e GIT
  /// </summary>
  internal sealed class ChangeSourceControlCommand : BaseCommand
  {
    private const string CHANGE_PATTERN = "Change Source Control PlugIn to: {0}";
    private const string GUID_GIT = "11B8E6D7-C08B-4385-B321-321078CDD1F8";
    private const string GUID_TFS = "4CA58AB2-18FA-4F8D-95D4-32DDF27D184C";
    private const string TFS = "TFS";
    private const string GIT = "GIT";

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.ChangeSourceControl;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeSourceControlCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public ChangeSourceControlCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      try
      {
        bool tfsActive = IsTFS();
        _menu.Text = tfsActive ?
          CHANGE_PATTERN.FormatWith(GIT) : CHANGE_PATTERN.FormatWith(TFS);
      }
      catch (Exception ex)
      {
        Utils.LogDebugError(ex.ToString());
      }
    }

    public bool IsGit()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      WSPackPackage.Instance.ScciProviderInterface.GetSourceControlProviderID(out Guid guid);
      bool isgit = guid.ToString().EqualsInsensitive(GUID_GIT);

      try
      {
        var msg = $"Current Source Control Provider ID: {guid}";
        Utils.LogDebugMessage($"{msg}. IsGit? {isgit}");
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
      }

      return isgit;
    }

    public bool IsTFS()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      WSPackPackage.Instance.ScciProviderInterface.GetSourceControlProviderID(out Guid guid);
      return guid.ToString().EqualsInsensitive(GUID_TFS);
    }

    public void RegisterNone()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      Guid guid = typeof(IVsSccProvider).GUID;
      WSPackPackage.Instance.ScciProviderInterface.GetSourceControlProviderInterface(ref guid, out IntPtr pvv);
      if (pvv != IntPtr.Zero)
      {
        IVsSccProvider provider = Marshal.GetObjectForIUnknown(pvv) as IVsSccProvider;
        System.Diagnostics.Trace.WriteLine(provider.GetType().Namespace);
        var status = provider.SetInactive();
        Utils.LogDebugMessage("Inativado");
        System.Diagnostics.Trace.WriteLine(status);
      }
      _ = WSPackPackage.Instance.ScciProvider.RegisterSourceControlProvider(new Guid("{00000000-0000-0000-0000-000000000000}"));
    }

    public void RegisterGit()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      int hr = WSPackPackage.Instance.ScciProvider.RegisterSourceControlProvider(new Guid("{" + GUID_GIT + "}"));
      Marshal.ThrowExceptionForHR(hr);
    }

    public void RegisterTFS()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      int hr = WSPackPackage.Instance.ScciProvider.RegisterSourceControlProvider(new Guid("{" + GUID_TFS + "}"));
      Marshal.ThrowExceptionForHR(hr);
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static ChangeSourceControlCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new ChangeSourceControlCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      try
      {
        if (FlexSourceControlExplorerCommand.IsAvailable)
        {
          RegisterGit();
          _menu.Text = CHANGE_PATTERN.FormatWith(TFS);
          Utils.LogOutputMessage("Alterado para GIT");
        }
        else
        {
          RegisterTFS();
          _menu.Text = CHANGE_PATTERN.FormatWith(GIT);
          Utils.LogOutputMessage("Alterado para TFS");
        }
      }
      catch (Exception ex)
      {
        _package.ShowErrorMessage(ex.Message);
      }
    }
  }
}