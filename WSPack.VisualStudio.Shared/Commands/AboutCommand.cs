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
  /// Comando para exibição do Sobre
  /// </summary>
  internal sealed class AboutCommand : BaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.About;

    /// <summary>
    /// Initializes a new instance of the <see cref="AboutCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected AboutCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static AboutCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new AboutCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();

      string fileName = Process.GetCurrentProcess().MainModule.FileName;
      string versao = fileName.Contains("2022") ? "2022" : "2019";

      string message = $"Inside {GetType().FullName}.MenuItemCallback()" + Environment.NewLine +
        fileName;
      string title = $"AboutCommand {versao}";

      // Show a message box to prove we were here
      VsShellUtilities.ShowMessageBox(
          _package,
          message,
          title,
          Microsoft.VisualStudio.Shell.Interop.OLEMSGICON.OLEMSGICON_INFO,
          Microsoft.VisualStudio.Shell.Interop.OLEMSGBUTTON.OLEMSGBUTTON_OK,
          Microsoft.VisualStudio.Shell.Interop.OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
    }
  }
}
