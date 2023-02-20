using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

using WSPack.Lib.Extensions;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;
namespace WSPack.VisualStudio.Shared.Commands
{
  class CopyLocalPathSourceControlExplorerCommand : CopyLocalPathBaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.CopyLocalPathSourceControlExplorer;

    #region Construtor
    /// <summary>
    /// Inicialização da classe <see cref="CopyLocalPathSourceControlExplorerCommand"/>
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public CopyLocalPathSourceControlExplorerCommand(AsyncPackage package, OleMenuCommandService commandService) : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }
    #endregion
    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      _menu.Enabled = !Utils.GetSourceControlExplorerSelectedItem().IsNullOrEmptyEx();
    }

    /// <summary>
    /// Devolve a instãncia da classe: <see cref="CopyLocalPathSourceControlExplorerCommand"/>
    /// </summary>
    /// <value>Instância da classe: <see cref="CopyLocalPathSourceControlExplorerCommand"/></value>
    public static CopyLocalPathSourceControlExplorerCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new CopyLocalPathSourceControlExplorerCommand(package, commandService);
    }

    /// <summary>
    /// Recuperar o item local conforme tipo de comando
    /// </summary>
    /// <returns>item local conforme tipo de comando</returns>
    public override List<(Workspace Ws, string LocalItem)> GetLocalItem()
    {
      string serverItem = Utils.GetSourceControlExplorerSelectedItem();
      if (!serverItem.IsNullOrEmptyEx())
      {
        var lst = SourceControlExplorerExtensions.GetLocalItemForServerItem(Utils.GetTeamFoundationServerExt(), serverItem);
        return lst;
      }
      return new List<(Workspace Ws, string LocalItem)>();
    }
  }
}
