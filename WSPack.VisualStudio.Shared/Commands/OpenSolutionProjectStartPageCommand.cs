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
using WSPack.Lib.WPF.ViewModel;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para abrir um projeto da StartPage
  /// </summary>
  internal sealed class OpenSolutionProjectStartPageCommand : BaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.OpenSolutionProjectStartPage;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenSolutionProjectStartPageCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public OpenSolutionProjectStartPageCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      CreateKeyBindings("AbrirSolutionProjetoStartPage", "Global::Alt+Shift+O", false);
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static OpenSolutionProjectStartPageCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new OpenSolutionProjectStartPageCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      _package.MessageBoxShellWarningOk(ResourcesLib.StrEmDesenvolvimento);
#warning OpenSolutionProjectStartPage em desenvolvimento
      //int i = 1;
      //while (StartPageViewModel.Instance == null)
      //{
      //  StartPageCommand.ShowStartPage(_package);
      //  System.Threading.Thread.Sleep(500);
      //  Utils.LogDebugMessage($"Esperando StartPage: {i++}");
      //  if (i > 3)
      //    break;
      //}
      //if (StartPageViewModel.Instance != null)
      //  StartPageViewModel.Instance.OpenProjectSolutionCommand.Execute(null);
      //else
      //  Utils.LogOutputMessage(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
    }
  }
}