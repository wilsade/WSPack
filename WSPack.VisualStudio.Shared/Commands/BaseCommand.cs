using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Shell;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Command handler
  /// </summary>
  internal abstract class BaseCommand
  {
    /// <summary>
    /// Gets the service provider from the owner package.
    /// </summary>
    private IAsyncServiceProvider ServiceProvider => _package;

    /// <summary>
    /// VS Package that provides this command, not null.
    /// </summary>
    protected readonly AsyncPackage _package;

    /// <summary>
    /// Menu do comando
    /// </summary>
    protected readonly OleMenuCommand _menu;

    protected abstract void DoExecute(object sender, EventArgs e);

    /// <summary>
    /// Initializes a new instance of the <see cref="AboutCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected BaseCommand(AsyncPackage package, OleMenuCommandService commandService)
    {
      _package = package ?? throw new ArgumentNullException(nameof(package));
      commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

      var menuCommandID = new CommandID(CommandSet, CommandId);
      _menu = new OleMenuCommand(Execute, menuCommandID);
      _menu.BeforeQueryStatus += BeforeExecute;
      commandService.AddCommand(_menu);
    }

    protected virtual void BeforeExecute(object sender, EventArgs e)
    {
      // Implementado nos filhos
    }

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public virtual int CommandId => 0;

    /// <summary>
    /// Command menu group (command set GUID).
    /// </summary>
    public static readonly Guid CommandSet = new Guid("d2aa537b-238c-489b-948c-e89f5badbcae");

    /// <summary>
    /// This function is the callback used to execute the command when the menu item is clicked.
    /// See the constructor to see how the menu item is associated with this function using
    /// OleMenuCommandService service and MenuCommand class.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    private void Execute(object sender, EventArgs e)
    {
      DoExecute(sender, e);
    }
  }
}
