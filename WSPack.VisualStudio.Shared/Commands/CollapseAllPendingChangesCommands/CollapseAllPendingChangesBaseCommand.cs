using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using WSPack.Lib;
using Task = System.Threading.Tasks.Task;
using System.Linq;
using WSPack.Lib.Extensions;
using WSPack.Lib.Items;
using WSPack.Lib.Properties;
using System.IO;
using System.Collections;

using System.Reflection;
using Microsoft.TeamFoundation.VersionControl.Controls.Extensibility;
using Microsoft.TeamFoundation.Controls;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para recolher todos os nodos do pending changes
  /// </summary>
  internal abstract class CollapseAllPendingChangesBaseCommand : BaseCommand
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CollapseAllPendingChangesBaseCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected CollapseAllPendingChangesBaseCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    /// <summary>
    /// Nome da propriedade interna para acessar o Pending Changes
    /// </summary>
    protected abstract string SelectedItemsInternal { get; }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      _menu.Enabled = Utils.GetVersionControlServerExt()?.PendingChanges != null;
    }

    private void Collapse(dynamic root)
    {
      if (root != null)
      {
        root.IsSelected = true;
        root.IsExpanded = false;
        foreach (var item in root.Children)
        {
          Collapse(item);
        }
      }
    }

    private dynamic FindRoot(IEnumerable lstTreeNodeEnum)
    {
      dynamic root = null;
      foreach (dynamic item in lstTreeNodeEnum)
      {
        dynamic paiAtual = item.Parent;
        root = item;
        while (paiAtual != null)
        {
          root = paiAtual;
          paiAtual = paiAtual.Parent;
        }
        break;
      }
      return root;
    }

    static void GetExplorerControls(out IPendingChangesExt changesExt, out ITeamExplorer teamExplorer,
      out ITeamExplorerPage teamExplorerPage)
    {
      changesExt = null;
      teamExplorer = null;
      teamExplorerPage = null;
      ServiceProvider sp = new ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)WSPackPackage.Dte);
      if (sp != null)
      {
        teamExplorer = sp.GetService(typeof(ITeamExplorer)) as ITeamExplorer;
        if (teamExplorer != null)
        {
          if (teamExplorer.CurrentPage == null)
          {
            teamExplorer.NavigateToPage(new Guid(TeamExplorerPageIds.Home), null);
          }
          teamExplorerPage = teamExplorer.CurrentPage;
          if (teamExplorerPage != null)
            changesExt = teamExplorerPage.GetExtensibilityService(typeof(IPendingChangesExt)) as IPendingChangesExt;
        }
      }

      if (changesExt == null)
      {
        var guid = new Guid(TeamExplorerPageIds.PendingChanges);
        teamExplorerPage = teamExplorer.NavigateToPage(guid, null);
        if (teamExplorerPage != null)
          changesExt = teamExplorerPage.GetExtensibilityService(typeof(IPendingChangesExt)) as IPendingChangesExt;
      }
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      try
      {
        bool ok = false;
        GetExplorerControls(out IPendingChangesExt pc, out ITeamExplorer teamExplorer, out ITeamExplorerPage teamExplorerPage);

        if (pc != null)
        {
          PropertyInfo[] lstProps = pc.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);
          if (lstProps != null && lstProps.Length > 0)
          {
            PropertyInfo pProp = lstProps.FirstOrDefault(x => x.Name.Equals(SelectedItemsInternal));
            if (pProp != null)
            {
              object lstTreeNodeObject = pProp.GetValue(pc);
              if (lstTreeNodeObject is IEnumerable)
              {
                IEnumerable lstTreeNodeEnum = lstTreeNodeObject as IEnumerable;

                dynamic root = FindRoot(lstTreeNodeEnum);
                Collapse(root);
                teamExplorerPage.Refresh();
                ok = true;
              }
            }
          }
        }

        if (!ok)
          Utils.LogOutputMessage(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
      }
      catch (Exception ex)
      {
        MessageBoxUtils.ShowError(ex.ToString());
      }
    }
  }
}