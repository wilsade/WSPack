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

using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.DteProject;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para definição de argumentos de linha de comando do projeto selecionado
  /// </summary>
  internal sealed class ProjectCommandLineArgsCommand : ProjectCommandLineArgsBaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.ProjectCommandLineArgs;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectCommandLineArgsCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public ProjectCommandLineArgsCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static ProjectCommandLineArgsCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new ProjectCommandLineArgsCommand(package, commandService);
    }

    protected override ResponseItem<DteProjectObj> GetProject()
    {
      var projeto = WSPackPackage.Dte.GetSolutionExplorerActiveProject();
      var response = new ResponseItem<DteProjectObj>();
      if (projeto != null && !projeto.IsSolutionFolder())
      {
        response.Success = true;
        response.Item = new DteProjectObj(projeto);
        return response;
      }
      else
        response.ErrorMessage = ResourcesLib.StrNaoFoiPossivelRecuperarProjeto;
      return response;
    }
  }
}