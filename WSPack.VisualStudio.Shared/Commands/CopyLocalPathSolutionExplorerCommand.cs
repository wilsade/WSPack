using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

using WSPack.VisualStudio.Shared.Commands;
using WSPack.VisualStudio.Shared;
using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  class CopyLocalPathSolutionExplorerCommand : CopyLocalPathBaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.CopyLocalPathSolutionExplorer;

    #region Construtor
    /// <summary>
    /// Inicialização da classe <see cref="CopyLocalPathSolutionExplorerCommand"/>
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public CopyLocalPathSolutionExplorerCommand(AsyncPackage package, OleMenuCommandService commandService) : base(package, commandService)
    {
      Instance = this;
    }
    #endregion

    /// <summary>
    /// Devolve a instãncia da classe: <see cref="CopyLocalPathSolutionExplorerCommand"/>
    /// </summary>
    /// <value>Instância da classe: <see cref="CopyLocalPathSolutionExplorerCommand"/></value>
    public static CopyLocalPathSolutionExplorerCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new CopyLocalPathSolutionExplorerCommand(package, commandService);
    }

    /// <summary>
    /// Recuperar o item local conforme tipo de comando
    /// </summary>
    /// <returns>item local conforme tipo de comando</returns>
    public override List<(Workspace Ws, string LocalItem)> GetLocalItem()
    {
      var localItem = Utils.GetSolutionExplorerSelectedItem();
      return new List<(Workspace Ws, string LocalItem)>
      {
        (null, localItem)
      };
    }
  }
}
