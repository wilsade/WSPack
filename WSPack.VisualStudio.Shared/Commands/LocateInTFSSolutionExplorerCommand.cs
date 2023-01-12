using Microsoft.VisualStudio.Shell;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  class LocateInTFSSolutionExplorerCommand : LocateInTFSBaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.LocateInTFSSolutionExplorer;

    #region Construtor
    /// <summary>
    /// Initializes a new instance of the <see cref="LocateInTFSSolutionExplorerCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public LocateInTFSSolutionExplorerCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {

    }
    #endregion

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static LocateInTFSSolutionExplorerCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new LocateInTFSSolutionExplorerCommand(package, commandService);
    }

    /// <summary>
    /// Recuperar o item local conforme tipo de comando
    /// </summary>
    /// <returns>item local conforme tipo de comando</returns>
    protected override string GetLocalItem()
    {
      return CopyLocalPathSolutionExplorerCommand.Instance.GetLocalItem()[0].LocalItem;
    }
  }

}
