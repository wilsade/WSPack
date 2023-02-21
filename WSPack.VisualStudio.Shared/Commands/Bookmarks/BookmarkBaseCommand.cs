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

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para para Toogle e Goto Bookmark
  /// </summary>
  internal abstract class BookmarkBaseCommand : BaseCommand
  {
    /// <summary>
    /// Menu para marcador
    /// </summary>
    protected class BookmarkMenuCommand : OleMenuCommand
    {
      /// <summary>
      /// Inicialização da classe: <see cref="BookmarkMenuCommand"/>.
      /// </summary>
      /// <param name="number">Nº 0 a 9 do marcador</param>
      /// <param name="invokeHandler">Execute command</param>
      /// <param name="id">Identificador do comando, conforme vsct</param>
      public BookmarkMenuCommand(int number, EventHandler invokeHandler, CommandID id)
              : base(invokeHandler, id)
      {
        Number = number;
      }

      /// <summary>
      /// Nº 0 a 9 do marcador
      /// </summary>
      public int Number { get; set; }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BookmarkBaseCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected BookmarkBaseCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var lst = GetBookmarksIds();

      for (int i = 1; i < lst.Length; i++)
      {
        int number = i;
        BookmarkMenuCommand menu = CreateMenuInternal(number, lst[i]);

        var tupla = GetKeyBindingPattern(number);
        CreateKeyBindings(tupla.Name, tupla.Shortcut, true);
        commandService.AddCommand(menu);
      }

      var menu0 = CreateMenuInternal(0, lst[0]);
      commandService.AddCommand(menu0);
    }

    protected BookmarkMenuCommand CreateMenuInternal(int number, int commandId)
    {
      CommandID comando = new CommandID(CommandSet, commandId);
      var menuItem = new BookmarkMenuCommand(number, DoExecute, comando);
      menuItem.BeforeQueryStatus += MenuItem_BeforeQueryStatus;
      return menuItem;
    }

    /// <summary>
    /// Recuperar o nome do Comando seguido do atalho
    /// </summary>
    /// <param name="number">Nº 0 a 9 do marcador</param>
    /// <returns>nome do Comando seguido do atalho</returns>
    protected abstract (string Name, string Shortcut) GetKeyBindingPattern(int number);

    /// <summary>
    /// Recuperar os Ids dos marcadores, de 0 a  9
    /// </summary>
    /// <returns>Bookmarks ids</returns>
    protected abstract int[] GetBookmarksIds();

    /// <summary>
    /// Acontece antes da execução do comando
    /// </summary>
    /// <param name="sender">Objeto que chamou</param>
    /// <param name="e">Argumentos do evento</param>
    protected abstract void BeforeExecute(BookmarkMenuCommand sender, EventArgs e);

    /// <summary>
    /// Ação do comando
    /// </summary>
    /// <param name="sender">Objeto que chamou</param>
    /// <param name="e">Argumentos do evento</param>
    protected abstract void OnExecute(BookmarkMenuCommand sender, EventArgs e);

    /// <summary>
    /// Acontece antes da ação do comando
    /// </summary>
    /// <param name="sender">Objeto que chamou</param>
    /// <param name="e">Argumentos do evento</param>
    private void MenuItem_BeforeQueryStatus(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      BeforeExecute((BookmarkMenuCommand)sender, e);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      OnExecute((BookmarkMenuCommand)sender, e);
    }
  }
}