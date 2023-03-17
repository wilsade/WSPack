using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

using Microsoft.TeamFoundation.MVVM;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para recolher todos os nodos na janela Git Changes
  /// </summary>
  internal sealed class GitChangesCollapseAllCommand : BaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.GitColapseAllChanges;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitChangesCollapseAllCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public GitChangesCollapseAllCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static GitChangesCollapseAllCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new GitChangesCollapseAllCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      try
      {
        ViewModelBase viewModelBase = GetGitViewModelBase();
        var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        PropertyInfo pChangesModel = viewModelBase.GetType().GetProperty("ChangesModel", flags);
        object vChangesModel = pChangesModel.GetValue(viewModelBase);

        PropertyInfo pTree = vChangesModel.GetType().GetProperty("UnstagedChangesNodeTree", flags);
        dynamic vTree = pTree.GetValue(vChangesModel);
        IList lstTree = vTree as IList;
        foreach (dynamic item in lstTree)
        {
          if (item.IsFolder && item.IsExpanded)
            item.IsExpanded = false;
        }
        MethodInfo methodInfo = vChangesModel.GetType().GetMethod("RefreshAsync");
        _ = methodInfo.Invoke(vChangesModel, new object[] { false });
      }
      catch (Exception ex)
      {
        _package.ShowErrorMessage(ex.Message);
      }
    }

    private static ViewModelBase GetGitViewModelBase()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      Guid gitChangesWindowGuid = new Guid("1C64B9C2-E352-428E-A56D-0ACE190B99A6");
      IVsUIShell vsUIShell = (IVsUIShell)Package.GetGlobalService(typeof(SVsUIShell));
      _ = vsUIShell.FindToolWindow((uint)__VSFINDTOOLWIN.FTW_fFrameOnly, ref gitChangesWindowGuid, out IVsWindowFrame vsWindowFrame);

      dynamic windowFrame = vsWindowFrame;
      dynamic toolWindowView = windowFrame.FrameView;
      Grid contentHostingPanel = (Grid)toolWindowView.Content;
      ContentPresenter genericPaneContentPresenter = contentHostingPanel.Children[0] as ContentPresenter;

      ContentPresenter toolWindowBaseProxy = (ContentPresenter)genericPaneContentPresenter.Content;
      UserControl changesView = (UserControl)toolWindowBaseProxy.Content;
      ViewModelBase viewModelBase = (ViewModelBase)changesView.DataContext;
      return viewModelBase;
    }
  }
}