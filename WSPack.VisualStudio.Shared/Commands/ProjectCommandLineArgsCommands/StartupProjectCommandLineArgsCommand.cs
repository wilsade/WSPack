using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EnvDTE;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.DteProject;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para definição de argumentos de linha de comando do projeto inicial
  /// </summary>
  internal sealed class StartupProjectCommandLineArgsCommand : ProjectCommandLineArgsBaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.ProjectCommandLineArgsStartupProject;

    /// <summary>
    /// Initializes a new instance of the <see cref="StartupProjectCommandLineArgsCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public StartupProjectCommandLineArgsCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static StartupProjectCommandLineArgsCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new StartupProjectCommandLineArgsCommand(package, commandService);
    }

    /// <summary>
    /// Recuperar o projeto para definição da propriedade de linha de comando
    /// </summary>
    /// <returns>Projeto</returns>
    protected override ResponseItem<DteProjectObj> GetProject()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      ResponseItem<DteProjectObj> projeto = GetProjetoInicial();
      return projeto;
    }

    /// <summary>
    /// Tenta recuperar o projeto definido como projeto inicial da Solution
    /// </summary>
    /// <returns>Resultado da busca de projeto</returns>
    public static ResponseItem<DteProjectObj> GetProjetoInicial()
    {
      ResponseItem<Project> projeto = TryGetProjetoInicial();
      var resposta = new ResponseItem<DteProjectObj>()
      {
        Success = projeto.Success,
        Item = projeto.Success ? new DteProjectObj(projeto.Item) : null,
        ErrorMessage = projeto.ErrorMessage
      };
      return resposta;
    }

    static ResponseItem<Project> TryGetProjetoInicial()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var resposta = new ResponseItem<Project>();
      object objDoServico = Package.GetGlobalService(typeof(SVsSolutionBuildManager));
      if (objDoServico is IVsSolutionBuildManager solutionBuildManager)
      {
        if (ErrorHandler.Failed(solutionBuildManager.get_StartupProject(out IVsHierarchy projectHierarchy)) || projectHierarchy == null)
        {
          Utils.LogOutputMessageForceShow(ResourcesLib.StrNaoFoiPossivelRecuperarProjetoInicial);
          resposta.ErrorMessage = ResourcesLib.StrNaoFoiPossivelRecuperarProjetoInicial;
          return resposta;
        }

        resposta = TryGetProjectFromIVsHierarchy(projectHierarchy);
        return resposta;
      }
      resposta.ErrorMessage = ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao;
      return resposta;
    }

    static ResponseItem<Project> TryGetProjectFromIVsHierarchy(IVsHierarchy hierarchy)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var resposta = new ResponseItem<Project>();
      try
      {
        const uint itemId = (uint)VSConstants.VSITEMID.Root;
        int hr = hierarchy.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_ExtObject, out object obj);
        if (ErrorHandler.Failed(hr))
        {
          resposta.ErrorMessage = ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao;
          return resposta;
        }

        resposta.Item = obj as Project;
        resposta.Success = resposta.Item != null;
        return resposta;
      }
      catch (Exception ex)
      {
        if (ErrorHandler.IsCriticalException(ex))
          throw;
        resposta.ErrorMessage = ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao;
        return resposta;
      }
    }
  }
}