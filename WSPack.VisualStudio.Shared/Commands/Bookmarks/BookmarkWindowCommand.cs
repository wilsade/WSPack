﻿using System;

using Microsoft.VisualStudio.Shell;

using WSPack.VisualStudio.Shared.ToolWindows;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para exibição da janela de marcadores
  /// </summary>
  internal sealed class BookmarkWindowCommand : BaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.BookmarkWindow;

    /// <summary>
    /// Initializes a new instance of the <see cref="BookmarkWindowCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public BookmarkWindowCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      _menu.Enabled = _menu.Visible = WSPackPackage.Instance != null &&
        WSPackPackage.ParametrosMEFObjects.UseBookmarks;
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static BookmarkWindowCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new BookmarkWindowCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      Utils.LogDebugMessage("BookmarkToolWindowPane");
      _ = _package.JoinableTaskFactory.RunAsync(async () =>
      {
        ToolWindowPane window = await _package.ShowToolWindowAsync(
          typeof(BookmarkToolWindowPane),
          0,
          create: true,
          cancellationToken: _package.DisposalToken);
      });
    }
  }
}