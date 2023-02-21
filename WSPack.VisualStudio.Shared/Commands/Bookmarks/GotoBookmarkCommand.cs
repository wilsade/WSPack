using System;
using System.Linq;

using Microsoft.VisualStudio.Shell;

using WSPack.VisualStudio.Shared.MEFObjects.Bookmarks;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Classe para os comandos Ir para marcador
  /// </summary>
  internal sealed class GotoBookmarkCommand : BookmarkBaseCommand
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="GotoBookmarkCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected GotoBookmarkCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static GotoBookmarkCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new GotoBookmarkCommand(package, commandService);
    }

    /// <summary>
    /// Recuperar o nome do Comando seguido do atalho
    /// </summary>
    /// <param name="number">Nº 0 a 9 do marcador</param>
    /// <returns>nome do Comando seguido do atalho</returns>
    protected override (string Name, string Shortcut) GetKeyBindingPattern(int number)
    {
      return ($"WSPack.IrParaMarcador{number}", $"Global::Ctrl+{number}");
    }

    protected override int[] GetBookmarksIds()
    {
      return new int[]
      {
        CommandIds.GotoBookmark0,
        CommandIds.GotoBookmark1,
        CommandIds.GotoBookmark2,
        CommandIds.GotoBookmark3,
        CommandIds.GotoBookmark4,
        CommandIds.GotoBookmark5,
        CommandIds.GotoBookmark6,
        CommandIds.GotoBookmark7,
        CommandIds.GotoBookmark8,
        CommandIds.GotoBookmark9
      };
    }

    /// <summary>
    /// Acontece antes da execução do comando
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    protected override void BeforeExecute(BookmarkMenuCommand sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      sender.Enabled = true;
      sender.Visible = BookmarkController.Instance.IsBookmarkBarProvided;
      if (WSPackPackage.Instance == null || !WSPackPackage.ParametrosMEFObjects.UseBookmarks ||
        !BookmarkController.Instance.IsBookmarkBarProvided ||
        !BookmarkController.Instance.Lista.Any(b =>
        {
          ThreadHelper.ThrowIfNotOnUIThread();
          return b.Number == sender.Number &&
            WSPackPackage.Dte.Solution.FindProjectItem(b.FullName) != null;
        }))
      {
        sender.Enabled = false;
        return;
      }
      if (string.IsNullOrEmpty(WSPackPackage.Dte.Solution.FullName))
        sender.Enabled = false;

      else
      {
        var bm = BookmarkController.Instance.Get(sender.Number);
        sender.Enabled = bm != null;
        if (bm != null)
          bm.IsEnabled = sender.Enabled;
      }
    }

    protected override void OnExecute(BookmarkMenuCommand sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      Utils.LogDebugMessage($"Goto marcador: {sender.Number}");
      BookmarkController.Instance.GotoBookmark(sender.Number);
    }
  }
}