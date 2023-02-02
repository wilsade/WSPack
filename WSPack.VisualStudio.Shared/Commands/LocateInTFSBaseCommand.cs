using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;

using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para exibição do Sobre
  /// </summary>
  internal abstract class LocateInTFSBaseCommand : BaseCommand
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="LocateInTFSBaseCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected LocateInTFSBaseCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }


    static dynamic GetSCEInternalListView(VersionControlExt versionControl)
    {
      var tipo = versionControl.Explorer.GetType();
      var nonPublic = BindingFlags.NonPublic | BindingFlags.Instance;
      var explorerToolWindowProp = tipo.GetProperty("ExplorerToolWindow", nonPublic);
      if (explorerToolWindowProp == null)
      {
        Utils.LogDebugMessageForceShow($"{nameof(explorerToolWindowProp)} nulo");
        return null;
      }

      var explorerToolWindow = explorerToolWindowProp.GetValue(versionControl.Explorer);
      if (explorerToolWindow == null)
      {
        Utils.LogDebugMessageForceShow($"{nameof(explorerToolWindow)} nulo");
        return null;
      }

      var sccExplorerProp = explorerToolWindow.GetType().GetProperty("SccExplorer");
      if (sccExplorerProp == null)
      {
        Utils.LogDebugMessageForceShow($"{nameof(sccExplorerProp)} nulo");
        return null;
      }

      var sccExplorer = sccExplorerProp.GetValue(explorerToolWindow);
      if (sccExplorer == null)
      {
        Utils.LogDebugMessageForceShow($"{nameof(sccExplorer)} nulo");
        return null;
      }

      var listViewExplorerField = sccExplorer.GetType().GetField("listViewExplorer");
      if (listViewExplorerField == null)
      {
        Utils.LogDebugMessageForceShow($"{nameof(listViewExplorerField)} nulo");
        return null;
      }

      dynamic listViewExplorer = listViewExplorerField.GetValue(sccExplorer);
      if (listViewExplorer == null)
      {
        Utils.LogDebugMessageForceShow($"{nameof(listViewExplorer)} nulo");
        return null;
      }
      return listViewExplorer;
    }

    static void TentaSelecionar(VersionControlExt vcExt, dynamic listViewExplorer,
      string serverItem, bool tentandoDeNovo)
    {
      bool condition() => listViewExplorer.SelectedIndices.Count > 0;
      void todo()
      {
        int selectedIndex = listViewExplorer.SelectedIndices[0];
        listViewExplorer.EnsureVisible(selectedIndex);
      }

      var poller = new DispatchedPoller(5, TimeSpan.FromSeconds(0.25), condition,
        todo, () =>
        {
          if (!tentandoDeNovo)
            LastTry(vcExt, listViewExplorer, serverItem);
        });
      poller.Go(tentandoDeNovo);
    }

    static void LastTry(VersionControlExt vcExt, dynamic listViewExplorer, string serverItem)
    {
      vcExt.Explorer.NavigateToRoot();
      vcExt.Explorer.Navigate(serverItem);
      TentaSelecionar(vcExt, listViewExplorer, serverItem, true);
    }

    /// <summary>
    /// Verifica se o item selecionado no SSE representa a mesma pasta do serverItem
    /// </summary>
    /// <param name="serverItem">Server item</param>
    /// <param name="vcExt">VersionControlExt</param>
    private static void CheckSameFolder(string serverItem, VersionControlExt vcExt)
    {
      var explorer = vcExt.Explorer;
      if (string.IsNullOrEmpty(explorer.CurrentFolderItem?.LocalPath))
      {
        var itemSelecionado = Utils.GetSourceControlExplorerSelectedItem();
        if (itemSelecionado.ContainsInsensitive(serverItem) ||
          serverItem.ContainsInsensitive(itemSelecionado))
        {
          explorer.NavigateToRoot();
        }
        else if (!string.IsNullOrWhiteSpace(explorer.CurrentFolderItem?.SourceServerPath))
        {
          if (serverItem.ContainsInsensitive(explorer.CurrentFolderItem?.SourceServerPath))
            explorer.NavigateToRoot();
          else if (Path.GetDirectoryName(serverItem).EndsWithInsensitive(Path.GetDirectoryName(explorer.CurrentFolderItem.SourceServerPath)))
          {
            explorer.NavigateToRoot();
          }
        }
      }

      else
      {
        var vcServer = Utils.GetTeamFoundationServerExt().GetVersionControlServer();
        if (vcServer != null && vcServer.GetWorkspaceForServerItem(serverItem, out var lstWorkspaceLocalItem))
        {
          (Workspace Ws, string LocalItem) item = Utils.ChooseItem(lstWorkspaceLocalItem);
          if (explorer.CurrentFolderItem.LocalPath.Equals(Path.GetDirectoryName(item.LocalItem)))
            explorer.NavigateToRoot();
          else if (Path.GetDirectoryName(explorer.CurrentFolderItem.LocalPath).Equals(Path.GetDirectoryName(item.LocalItem)))
            explorer.NavigateToRoot();
        }
      }
    }

    protected override void BeforeExecute(object sender, EventArgs e)
    {
      _menu.Visible = _menu.Enabled = FlexSourceControlExplorerCommand.IsAvailable;
    }

    /// <summary>
    /// Recuperar o item local conforme tipo de comando
    /// </summary>
    /// <returns>item local conforme tipo de comando</returns>
    protected abstract string GetLocalItem();

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      string localItem = GetLocalItem();

      if (!localItem.IsNullOrEmptyEx())
      {
        if (CopyServerPathBaseCommand.TryGetServerObject(localItem, out var tuple))
        {
          NavigateToServerItem(tuple.VCExt, tuple.ServerItem);
        }
      }
      else
        WSPackPackage.Dte.WriteInOutPutForceShow(ResourcesLib.StrNaoFoiPossivelRecuperarCaminhoLocalItem);
    }

    /// <summary>
    /// Localizar um item no TFS
    /// </summary>
    public static void NavigateToServerItem(VersionControlExt versionControl, string serverItem)
    {
      CheckSameFolder(serverItem, versionControl);
      versionControl.Explorer.Navigate(serverItem);
      var vc = Utils.GetTeamFoundationServerExt().GetVersionControlServer();
      var item = vc?.TryGetItem(serverItem);
      if (item?.ItemType == ItemType.File)
      {
        dynamic listViewExplorer = GetSCEInternalListView(versionControl);
        if (listViewExplorer != null)
          TentaSelecionar(versionControl, listViewExplorer, serverItem, false);
      }
    }
  }
}
