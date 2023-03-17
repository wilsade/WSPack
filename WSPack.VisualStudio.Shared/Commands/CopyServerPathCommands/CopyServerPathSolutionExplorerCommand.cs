using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

using WSPack.Lib.Forms;
using WSPack.Lib.Properties;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para copiar o caminho server de um item do solution explorer
  /// </summary>
  internal sealed class CopyServerPathSolutionExplorerCommand : CopyServerPathBaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.CopyServerPathSolutionExplorer;

    /// <summary>
    /// Initializes a new instance of the <see cref="CopyServerPathSolutionExplorerCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public CopyServerPathSolutionExplorerCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static CopyServerPathSolutionExplorerCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new CopyServerPathSolutionExplorerCommand(package, commandService);
    }

    /// <summary>
    /// Recuperar o item 'Server' conforme tipo de comando
    /// </summary>
    /// <returns>item 'Server' conforme tipo de comando</returns>
    public override string GetServerItem()
    {
      var lstWorkspaceLocalItem = CopyLocalPathSolutionExplorerCommand.Instance.GetLocalItem();
      (Workspace Ws, string LocalItem) item = Utils.ChooseItem(lstWorkspaceLocalItem);
      if (string.IsNullOrEmpty(item.LocalItem))
        Utils.LogOutputMessageForceShow(ResourcesLib.StrNaoFoiPossivelRecuperarCaminhoLocalItem);
      else
      {
        if (TryGetServerObject(item.LocalItem, out var tuple))
        {
          return tuple.ServerItem;
        }
        else if (Utils.TryGetGitServerItem(item.LocalItem, out string serverItem))
          return serverItem;
      }
      return string.Empty;
    }
  }
}
