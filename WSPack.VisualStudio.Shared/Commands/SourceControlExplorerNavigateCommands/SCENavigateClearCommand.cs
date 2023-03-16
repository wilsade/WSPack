using Microsoft.VisualStudio.Shell;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para limpar o histórico de navegação no SCE
  /// </summary>
  internal sealed class SCENavigateClearCommand : SCENavigateBaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.SCENavigateClear;

    /// <summary>
    /// Initializes a new instance of the <see cref="SCENavigateClearCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected SCENavigateClearCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {

    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static SCENavigateClearCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new SCENavigateClearCommand(package, commandService);
    }

    protected override bool ButtonEnabled => SCENavigationController.Instance.HasNavigation();

    protected override void ProcessExecute()
    {
      SCENavigationController.Instance.ClearNavigation();
    }
  }
}