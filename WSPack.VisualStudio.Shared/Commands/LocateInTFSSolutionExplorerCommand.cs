using Microsoft.VisualStudio.Shell;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// Recuperar o item local conforme tipo de comando
    /// </summary>
    /// <returns>item local conforme tipo de comando</returns>
    protected override string GetLocalItem()
    {
      return CopyLocalPathSolutionExplorerCommand.Instance.GetLocalItem()[0].LocalItem;
    }
  }

}
