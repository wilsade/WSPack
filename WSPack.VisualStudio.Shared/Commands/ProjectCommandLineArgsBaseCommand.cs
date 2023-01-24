using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

using WSPack.Lib;
using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.DteProject;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para exibição do Sobre
  /// </summary>
  internal abstract class ProjectCommandLineArgsBaseCommand : BaseCommand
  {
    EnvDTE.Command _commandProjectProperties;
    bool _quebrarLinhas;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectCommandLineArgsBaseCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected ProjectCommandLineArgsBaseCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _quebrarLinhas = true;
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
      //_menu.Supported = false;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      if (_commandProjectProperties == null)
        _commandProjectProperties = WSPackPackage.Dte.Commands.Item("Project.Properties");
      _menu.Enabled = true;
      if (_commandProjectProperties != null)
        _menu.Enabled = _commandProjectProperties.IsAvailable;
    }

    /// <summary>
    /// Recuperar o projeto para definição da propriedade de linha de comando
    /// </summary>
    /// <returns>Projeto</returns>
    protected abstract ResponseItem<DteProjectObj> GetProject();

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      ResponseItem<DteProjectObj> projectItem = GetProject();
      if (projectItem.Success)
      {
        DteProjectObj dteProject = projectItem.Item;
        if (dteProject.Properties != null)
        {
          if (dteProject.Properties.HasCommandLineProperty(out var msgErro))
          {
            using (var argsForm = new ProjectCommandLineArgsForm())
            {
              argsForm.ProjectName = dteProject.UniqueName;
              argsForm.CommandLineArgs = dteProject.Properties.CommandLineArgs;
              argsForm.QuebrarLinhas = _quebrarLinhas;
              if (argsForm.ShowDialog() == DialogResult.OK)
              {
                _quebrarLinhas = argsForm.QuebrarLinhas;
                dteProject.Properties.CommandLineArgs = argsForm.CommandLineArgs;
              }
            }
          }
          else
            MessageBoxUtils.ShowWarning(msgErro);
        }
        else
          Utils.LogOutputMessageForceShow(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
      }
      else
        MessageBoxUtils.ShowWarning(projectItem.ErrorMessage);
    }
  }
}