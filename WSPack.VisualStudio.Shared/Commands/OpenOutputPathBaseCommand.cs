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

using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.DteProject;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para abrir o output path
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
      var menu = (OleMenuCommand)sender;
      menu.Visible = menu.Enabled = false;

      try
      {
        if (string.IsNullOrWhiteSpace(WSPackPackage.Dte.Solution.FullName))
          return;

        DteProjectObj dteProjectObj = GetProject();
        var target = dteProjectObj?.Properties.ActiveConfiguration.TargetFullPathName;
        if (target.IsNullOrWhiteSpaceEx())
          return;

        var dirName = Path.GetDirectoryName(target);
        if (File.Exists(target))
          menu.ParametersDescription = target;
        else
          menu.ParametersDescription = Path.GetDirectoryName(target);
        menu.Visible = menu.Enabled = System.IO.Directory.Exists(dirName);
        menu.Text = $"{ResourcesLib.StrAbrir} Output Path ({dteProjectObj.Properties.ActiveConfiguration.Name})";
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"Erro no _menu_BeforeQueryStatus de OpenOutputPath: {ex.Message}");
      }
    }

    private static DteProjectObj GetProject()
    {
      EnvDTE.Project projeto = WSPackPackage.Dte.GetSolutionExplorerActiveProject();
      var dteProjectObj = DteProjectObj.Create(projeto);
      if (dteProjectObj == null)
        return null;
      if (dteProjectObj.IsSharedProject || dteProjectObj?.Properties.ActiveConfiguration == null)
      {
        ResponseItem<DteProjectObj> responseItem = StartupProjectCommandLineArgsCommand.GetProjetoInicial();
        if (responseItem.Success)
          dteProjectObj = responseItem.Item;
      }

      return dteProjectObj;
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