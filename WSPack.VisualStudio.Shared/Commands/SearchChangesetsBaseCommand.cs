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
using WSPack.VisualStudio.Shared.Forms;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para exibição do Sobre
  /// </summary>
  internal abstract class SearchChangesetsBaseCommand : BaseCommand
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchChangesetsBaseCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected SearchChangesetsBaseCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      _menu.Enabled = WSPackPackage.Instance != null &&
        WSPackPackage.Dte != null;
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (WSPackPackage.Dte.GetTeamFoundationServerExt().IsSourceControlExplorerActive())
      {
        var VCExt = WSPackPackage.Dte.GetVersionControlExt();
        if (VCExt?.Explorer == null)
        {
          MessageBoxUtils.ShowWarning(ResourcesLib.StrSourceControlExplorerNaoConfigurado);
          return;
        }
        var resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchChangesetsForm));
        string caption = resources.GetString("$this.Text");

        // Form já está aberto?
        IntPtr achou = NativeMethods.FindWindow(null, caption);
        if (achou != IntPtr.Zero)
        {
          NativeMethods.ShowWindow(achou, SW_Modos.SW_RESTORE.GetHashCode());
          NativeMethods.SetForegroundWindow(achou);
        }
        else
        {
          _ = Utils.GetVersionControlServerExt().Explorer;

          IntPtr prt = Process.GetCurrentProcess().MainWindowHandle;
          Win32WindowWrapper wrapper = new Win32WindowWrapper(prt);

          // Descobrir o sender no SourceControlExplorer
          SenderTypes senderType = SenderTypes.MenuWSPack;
          if (sender is OleMenuCommand menu)
          {
            senderType = menu.CommandID.ID == CommandIds.SearchChangesets.GetHashCode() ?
             SenderTypes.MenuWSPack : SenderTypes.SourceControlExplorer;
          }

          SearchChangesetsForm formBusca = new SearchChangesetsForm(senderType, WSPackConsts.SearchChangesetsConfigPath);
          formBusca.Show(wrapper);
        }
      }

      // Não conectado. Mostrar tela para conectar-se ao TFS
      else
        Utils.ShowWindowConnectToTFS();
    }
  }
}