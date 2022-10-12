using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;

using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

namespace WSPack.VisualStudio.Shared.Commands
{
  abstract class CopyServerPathBaseCommand : BaseCommand
  {
    #region Construtor
    /// <summary>
    /// Inicialização da classe <see cref="CopyServerPathBaseCommand"/>
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public CopyServerPathBaseCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {

    }
    #endregion

    /// <summary>
    /// Recuperar o item 'Server' conforme tipo de comando
    /// </summary>
    /// <returns>item 'Server' conforme tipo de comando</returns>
    public abstract string GetServerItem();

    public static bool TryGetServerObject(string localItem, out (VersionControlExt VCExt, string ServerItem) output)
    {
      output = (null, string.Empty);
      if (!string.IsNullOrEmpty(localItem))
      {
        if (WSPackPackage.Dte.TryGetVersionControlExt(out var versionControl) &&
          versionControl.Explorer != null)
        {
          if (versionControl.TryGetServerItemForLocalItem(localItem, out var tuple))
          {
            output = (versionControl, tuple.ServerItem);
            return true;
          }
          else
          {
            if (FlexSourceControlExplorerCommand.IsAvailable)
              WSPackPackage.Dte.WriteInOutPut(true, tuple.ErrorMessage);
            return false;
          }
        }
        else
        {
          WSPackPackage.Dte.WriteInOutPut(true, ResourcesLib.StrObjetoControleVersaoNaoDisponivel);
          return false;
        }
      }
      else
        return false;
    }

    /// <summary>
    /// This function is the callback used to execute the command when the menu item is clicked.
    /// See the constructor to see how the menu item is associated with this function using
    /// OleMenuCommandService service and MenuCommand class.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    private void Execute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      string serverItem = GetServerItem();
      if (string.IsNullOrEmpty(serverItem))
        Utils.LogOutputMessage(true, ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
      else
      {
        CopyLocalPathBaseCommand.CopyToClipboard(serverItem);
      }
    }
  }
}
