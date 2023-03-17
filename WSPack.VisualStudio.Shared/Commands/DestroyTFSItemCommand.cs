using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using swf = System.Windows.Forms;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;
using Microsoft.TeamFoundation.VersionControl.Common;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para destruir um item do TFS
  /// </summary>
  internal sealed class DestroyTFSItemCommand : BaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.DestroyTFSItem;

    /// <summary>
    /// Initializes a new instance of the <see cref="DestroyTFSItemCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public DestroyTFSItemCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      CheckMenu();
    }

    private void CheckMenu()
    {
      var tfs = Utils.GetTeamFoundationServerExt();
      var vcExt = Utils.GetVersionControlServerExt();

      _menu.Enabled = tfs?.ActiveProjectContext?.DomainUri != null &&
        tfs.GetVersionControlServer() != null &&
        vcExt?.GetSelectedItems()?.Length == 1;

      _menu.ParametersDescription = null;
      if (_menu.Enabled)
      {
        VersionControlExplorerItem item = vcExt.GetSelectedItem();
        if (item.ItemId > 0)
          _menu.ParametersDescription = item.SourceServerPath;
        else
          _menu.Enabled = false;
      }
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static DestroyTFSItemCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new DestroyTFSItemCommand(package, commandService);
    }

    static bool ConfirmouExclusao(string item)
    {
      return MessageBoxUtils.ShowWarningYesNo(ResourcesLib.StrDestruirItemTFS.FormatWith(item),
        swf.MessageBoxDefaultButton.Button2) &&
        MessageBoxUtils.ShowWarningYesNo(ResourcesLib.StrTemCerteza, swf.MessageBoxDefaultButton.Button2);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      VersionControlExplorerItem item = null;
      if (string.IsNullOrEmpty(_menu.ParametersDescription))
        CheckMenu();

      if (_menu.ParametersDescription.IsNullOrEmptyEx())
      {
        Utils.LogOutputMessageForceShow(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
        return;
      }

      if (!ConfirmouExclusao(_menu.ParametersDescription))
        return;

      try
      {
        var vcServer = Utils.GetTeamFoundationServerExt().GetVersionControlServer();
        if (vcServer != null)
        {
          vcServer.Destroy(new ItemSpec(_menu.ParametersDescription, RecursionType.Full),
            VersionSpec.Latest, null, DestroyFlags.None);
          WSPackPackage.Dte.ExecuteCommand("View.Refresh");
        }
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"Erro ao destruir item: {ex.Message}");
      }
    }
  }
}