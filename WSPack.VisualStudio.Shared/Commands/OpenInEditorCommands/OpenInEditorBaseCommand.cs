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

using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;
using WSPack.VisualStudio.Shared.Options;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para abrir um item no editor de texto
  /// </summary>
  internal abstract class OpenInEditorBaseCommand : BaseCommand
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenInEditorBaseCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected OpenInEditorBaseCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Recuperar o item local conforme tipo de comando
    /// </summary>
    /// <returns>Item local conforme tipo de comando</returns>
    public abstract List<(Workspace Ws, string LocalItem)> GetLocalItem();

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var lstWorkspaceLocalItem = GetLocalItem();
      (Workspace Ws, string LocalItem) item = Utils.ChooseItem(lstWorkspaceLocalItem);
      OpenIt(item.LocalItem);
    }


    /// <summary>
    /// Abrir um item no editor de texto definido pelo usuário
    /// </summary>
    /// <param name="fileName">Nome do arquivo</param>
    public static void OpenIt(string fileName)
    {
      if (!string.IsNullOrEmpty(fileName))
      {
        if (File.Exists(fileName))
        {
          PageGeneral pageGeneral = WSPackPackage.GetParametersPage<PageGeneral>();

          string editor = "notepad.exe";
          if (!string.IsNullOrEmpty(pageGeneral.EditorTexto))
            editor = pageGeneral.EditorTexto;

          else if (Utils.GetNotepadPP(out string path))
            editor = path;

          Process.Start(editor, string.Format("\"{0}\"", fileName));
        }
        else
          Utils.LogOutputMessageForceShow(ResourcesLib.StrItemNaoEncontrado.FormatWith(fileName));
      }
      else
        Utils.LogOutputMessage(ResourcesLib.StrItemNaoInformado);
    }
  }
}