using System;

using Microsoft.VisualStudio.Shell;

using WSPack.VisualStudio.Shared.MEFObjects.Bookmarks;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para excluir todos os marcadores numerados
  /// </summary>
  internal sealed class ClearAllBookmarksCommand : BaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.ClearAllBookmarks;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClearAllBookmarksCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public ClearAllBookmarksCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread($"{nameof(ClearAllBookmarksCommand)}:{nameof(_menu_BeforeQueryStatus)}");
      _menu.Enabled = WSPackPackage.Instance != null &&
        WSPackPackage.ParametrosMEFObjects.UseBookmarks &&
        BookmarkController.Instance.IsBookmarkBarProvided &&
        BookmarkController.Instance.Lista.Count > 0;

      _menu.Visible = BookmarkController.Instance.IsBookmarkBarProvided;
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static ClearAllBookmarksCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new ClearAllBookmarksCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      BookmarkController.Instance.ClearAllBookmarks();
    }
  }
}