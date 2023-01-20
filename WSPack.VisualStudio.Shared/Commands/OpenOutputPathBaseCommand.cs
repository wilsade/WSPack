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
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para exibição do Sobre
  /// </summary>
  internal abstract class OpenOutputPathBaseCommand : BaseCommand
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenOutputPathBaseCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected OpenOutputPathBaseCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      EnvDTE.Project projeto = WSPackPackage.Dte.GetSolutionExplorerActiveProject();
      var menu = (OleMenuCommand)sender;
      menu.Visible = menu.Enabled = false;

      try
      {
        if (projeto?.ConfigurationManager?.ActiveConfiguration?.Properties?.Count > 0)
        {
          menu.ParametersDescription = null;

          string relativeOutput = Convert.ToString(projeto.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value);
          relativeOutput = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(projeto.FullName), relativeOutput);
          System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(relativeOutput);

          string targetName = System.IO.Path.Combine(dInfo.FullName, projeto.Properties.Item("OutputFileName").Value.ToString());
          if (System.IO.File.Exists(targetName))
            menu.ParametersDescription = targetName;
          else
            menu.ParametersDescription = dInfo.FullName;
          menu.Visible = menu.Enabled = System.IO.Directory.Exists(dInfo.FullName);

          menu.Text = $"{ResourcesLib.StrAbrir} Output Path ({projeto.ConfigurationManager.ActiveConfiguration.ConfigurationName})";
        }
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"Erro no ProcessBeforeExecute de OpenOutputPath: {ex.Message}");
      }
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var menu = (OleMenuCommand)sender;
      if (!LocateInWindowsBaseCommand.Locate(menu.ParametersDescription, out var msg))
        Utils.LogOutputMessage(msg);
    }
  }
}