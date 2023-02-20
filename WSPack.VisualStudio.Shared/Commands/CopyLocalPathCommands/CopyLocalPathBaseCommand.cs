using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

using WSPack.Lib.Properties;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para copiar o caminho local de um item
  /// </summary>
  abstract class CopyLocalPathBaseCommand : BaseCommand
  {
    #region Construtor
    /// <summary>
    /// Inicialização da classe <see cref="CopyLocalPathBaseCommand"/>
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public CopyLocalPathBaseCommand(AsyncPackage package, OleMenuCommandService commandService) : base(package, commandService)
    {

    }
    #endregion

    /// <summary>
    /// Recuperar o item local conforme tipo de comando
    /// </summary>
    /// <returns>Lista contendo informações do Workspace e do LocalItem</returns>
    public abstract List<(Workspace Ws, string LocalItem)> GetLocalItem();

    /// <summary>
    /// This function is the callback used to execute the command when the menu item is clicked.
    /// See the constructor to see how the menu item is associated with this function using
    /// OleMenuCommandService service and MenuCommand class.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    protected override void DoExecute(object sender, EventArgs e)
    {
      var lstWorkspaceLocalItem = GetLocalItem();
      if (lstWorkspaceLocalItem == null || lstWorkspaceLocalItem.Count == 0)
        Utils.LogOutputMessageForceShow(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
      else
      {
        (Workspace Ws, string LocalItem) item = Utils.ChooseItem(lstWorkspaceLocalItem);
        CopyToClipboard(item.LocalItem);
      }
    }

    /// <summary>
    /// Copiar um conteúdo texto para o Clipboard
    /// </summary>
    /// <param name="textToClipboard">Text to clipboard</param>
    /// <returns>true se copiado com sucesso</returns>
    public static bool CopyToClipboard(string textToClipboard)
    {
      try
      {
        System.Windows.Forms.Clipboard.SetDataObject(textToClipboard, false);
        return true;
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"CopyToClipboard: {ex.Message}");
        return false;
      }
    }
  }
}
