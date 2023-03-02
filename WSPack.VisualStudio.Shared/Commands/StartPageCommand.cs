using System;

using EnvDTE;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using WSPack.VisualStudio.Shared.ToolWindows;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para exibição da Página inicial
  /// </summary>
  internal sealed class StartPageCommand : BaseCommand
  {
    readonly SolutionEvents _solutionEvents;

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.WSPackStartPage;

    /// <summary>
    /// Initializes a new instance of the <see cref="StartPageCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected StartPageCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      _solutionEvents = WSPackPackage.Dte.Events.SolutionEvents;
      _solutionEvents.AfterClosing += FechouSolution;
      _solutionEvents.Opened += AbriuSolution;
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static StartPageCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new StartPageCommand(package, commandService);
    }

    async Task HideStartPageAsync()
    {
      var toolWindow = await WSPackPackage.Instance.FindToolWindowAsync(
        typeof(StartPageToolWindowPane), 0, false, WSPackPackage.Instance.DisposalToken);
      _ = ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
      {
        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
        if (toolWindow != null)
        {
          var windowPane = toolWindow.GetIVsWindowPane() as StartPageToolWindowPane;
          if (windowPane?.Frame is IVsWindowFrame frame)
            frame.Hide();
        }
      });
    }

    void AbriuSolution()
    {
      if (WSPackPackage.ParametrosGerais.AutoShowHideStartPageOnSolutionCloseOpen)
      {
        _ = HideStartPageAsync();
      }
    }

    private void FechouSolution()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (WSPackPackage.ParametrosGerais.AutoShowHideStartPageOnSolutionCloseOpen)
      {
        DoExecute(this, EventArgs.Empty);
      }
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      Utils.LogDebugMessage(nameof(StartPageCommand));
      ShowStartPage(_package);
    }

    /// <summary>
    /// Exibir a página inicial
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static void ShowStartPage(AsyncPackage package)
    {
      package.JoinableTaskFactory.RunAsync(async () =>
      {
        ToolWindowPane window = await package.ShowToolWindowAsync(
          typeof(StartPageToolWindowPane),
          0,
          create: true,
          cancellationToken: package.DisposalToken);
      });
    }
  }
}