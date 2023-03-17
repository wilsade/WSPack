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

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;
using WSPack.VisualStudio.Shared.MEFObjects.Bookmarks;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para criar/excluir um marcador numerado
  /// </summary>
  internal sealed class ToggleBookmarkBaseCommand : BookmarkBaseCommand
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ToggleBookmarkBaseCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public ToggleBookmarkBaseCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static ToggleBookmarkBaseCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new ToggleBookmarkBaseCommand(package, commandService);
    }

    /// <summary>
    /// Recuperar os Ids dos marcadores, de 0 a  9
    /// </summary>
    /// <returns>Bookmarks ids</returns>
    protected override int[] GetBookmarksIds()
    {
      return new int[] {
        CommandIds.ToggleBookmark0,
        CommandIds.ToggleBookmark1,
        CommandIds.ToggleBookmark2,
        CommandIds.ToggleBookmark3,
        CommandIds.ToggleBookmark4,
        CommandIds.ToggleBookmark5,
        CommandIds.ToggleBookmark6,
        CommandIds.ToggleBookmark7,
        CommandIds.ToggleBookmark8,
        CommandIds.ToggleBookmark9
     };
    }

    /// <summary>
    /// Recuperar o nome do Comando seguido do atalho
    /// </summary>
    /// <param name="number">Nº 0 a 9 do marcador</param>
    /// <returns>nome do Comando seguido do atalho</returns>
    protected override (string Name, string Shortcut) GetKeyBindingPattern(int number)
    {
      return ($"WSPack.AlternarMarcador{number}", $"Global::Ctrl+Shift+{number}");
    }

    /// <summary>
    /// Acontece antes da execução do comando
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    protected override void BeforeExecute(BookmarkMenuCommand sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();

      sender.Enabled = WSPackPackage.Instance != null &&
        WSPackPackage.ParametrosMEFObjects.UseBookmarks &&
        WSPackPackage.Dte.ActiveDocument.GetWpfTextView() != null;
      sender.Visible = WSPackPackage.Instance != null &&
        WSPackPackage.ParametrosMEFObjects.UseBookmarks;

      if (!string.IsNullOrEmpty(WSPackPackage.Dte.Solution.FullName) &&
        BookmarkController.Instance.Lista.FirstOrDefault(b =>
        {
          ThreadHelper.ThrowIfNotOnUIThread();
          return b.Number == sender.Number &&
            WSPackPackage.Dte.Solution.FindProjectItem(b.FullName) != null;
        }) != null)
        sender.Checked = true;
    }

    /// <summary>
    /// Ação do comando
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    protected override void OnExecute(BookmarkMenuCommand sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      Utils.LogDebugMessage($"Toggle marcador: {sender.Number}");
      BookmarkController.Instance.ToggleBookmark(sender.Number);
    }
  }
}