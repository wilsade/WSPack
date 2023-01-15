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

using WSPack.Lib.Forms;
using WSPack.Lib.Properties;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para exibição do Sobre
  /// </summary>
  internal abstract class LocateInWindowsBaseCommand : BaseCommand
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="LocateInWindowsBaseCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected LocateInWindowsBaseCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Recuperar o item local conforme tipo de comando
    /// </summary>
    /// <returns>Lista contendo informações do Workspace e do LocalItem</returns>
    protected abstract List<(Workspace Ws, string LocalItem)> GetLocalItem();

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var lstWorkspaceLocalItem = GetLocalItem();
      (Workspace Ws, string LocalItem) item = Utils.ChooseItem(lstWorkspaceLocalItem);

      if (!string.IsNullOrEmpty(item.LocalItem))
      {
        if (!Locate(item.LocalItem, out string msg))
          Utils.LogOutputMessageForceShow(msg);
      }
      else
        Utils.LogOutputMessage(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
    }

    /// <summary>
    /// Localizar uma pasta/arquivo no Windows
    /// </summary>
    /// <param name="localPath">Caminho da pasta ou arquivo</param>
    /// <param name="msg">Mensagem em caso de falha</param>
    /// <returns>true se o item foi localizado</returns>
    public static bool Locate(string localPath, out string msg)
    {
      if (!string.IsNullOrEmpty(localPath))
      {
        if (!File.Exists(localPath) && !Directory.Exists(localPath))
        {
          msg = string.Format(ResourcesLib.StrItemNaoEncontrado, localPath);
          return false;
        }
        else
        {
          msg = string.Empty;
          System.Diagnostics.Process.Start("explorer.exe", "/select,\"" + localPath + "\"");
          return true;
        }
      }

      else
      {
        msg = ResourcesLib.StrItemNaoInformado;
        return false;
      }
    }
  }
}