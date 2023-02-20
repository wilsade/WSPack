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
              WSPackPackage.Dte.WriteInOutPutForceShow(tuple.ErrorMessage);
            return false;
          }
        }
        else
        {
          WSPackPackage.Dte.WriteInOutPutForceShow(ResourcesLib.StrObjetoControleVersaoNaoDisponivel);
          return false;
        }
      }
      else
        return false;
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      string serverItem = GetServerItem();
      if (string.IsNullOrEmpty(serverItem))
        Utils.LogOutputMessageForceShow(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
      else
      {
        CopyLocalPathBaseCommand.CopyToClipboard(serverItem);
      }
    }
  }
}
