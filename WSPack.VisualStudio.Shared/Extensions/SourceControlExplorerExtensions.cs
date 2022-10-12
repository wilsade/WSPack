using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.TeamFoundation;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;

using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;

namespace WSPack.VisualStudio.Shared.Extensions
{
  /// <summary>
  /// Métodos estendidos para o Source Control Explorer / Team Explorer
  /// </summary>
  public static class SourceControlExplorerExtensions
  {

    /// <summary>
    /// Tentar recuperar o ServerItem com base em um LocalItem
    /// </summary>
    /// <param name="vcExt">VersionControlExt</param>
    /// <param name="localItem">Local item</param>
    /// <param name="output">ServerItem e mensagem de erro em caso de falha</param>
    /// <returns>true se existe serverItem para localItem</returns>
    public static bool TryGetServerItemForLocalItem(this VersionControlExt vcExt, string localItem,
      out (string ServerItem, string ErrorMessage) output)
    {
      string serverItem = null;
      if (vcExt != null && WSPackPackage.Dte.GetTeamFoundationServerExt().IsSourceControlExplorerActive())
      {
        if (vcExt.SolutionWorkspace != null)
          serverItem = vcExt.SolutionWorkspace.TryGetServerItemForLocalItem(localItem);

        else if (vcExt != null && vcExt.Explorer != null && vcExt.Explorer.Workspace != null)
          serverItem = vcExt.Explorer.Workspace.TryGetServerItemForLocalItem(localItem);

        if (!string.IsNullOrEmpty(serverItem))
        {
          output = (serverItem, null);
          return true;
        }
        else
        {
          output = (null, $"{ResourcesLib.StrItemNaoEncontradoTFS} - {localItem}");
          return false;
        }
      }
      else
      {
        output = (null, ResourcesLib.StrSourceControlExplorerNaoConfigurado);
        return false;
      }
    }

    /// <summary>
    /// Verifica se o source control explorer está ativo
    /// </summary>
    /// <param name="tfsExt">TFS ext</param>
    /// <returns>true se source control explorer está ativo</returns>
    public static bool IsSourceControlExplorerActive(this TeamFoundationServerExt tfsExt)
    {
      bool desconectado = tfsExt == null ||
        tfsExt.ActiveProjectContext == null ||
        string.IsNullOrEmpty(tfsExt.ActiveProjectContext.DomainUri) ||
        tfsExt.GetVersionControlServer() == null;
      return !desconectado;
    }

    /// <summary>
    /// Recuperar o VersionControlServer com base no TFS Server
    /// </summary>
    /// <param name="tfs">TFS server</param>
    /// <returns>VersionControlServer; null, caso contrário</returns>
    public static VersionControlServer GetVersionControlServer(this TeamFoundationServerExt tfs)
    {
      VersionControlServer vcServer = null;
      if (tfs?.ActiveProjectContext?.DomainUri == null)
        return vcServer;

      TfsTeamProjectCollection tfsObj = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(tfs.ActiveProjectContext.DomainUri));
      if (tfsObj != null)
      {
        vcServer = tfsObj.GetService<VersionControlServer>();
      }

      return vcServer;
    }

    /// <summary>
    /// Retornar o item selecionado no Source Control Explorer
    /// </summary>
    /// <param name="vcExt">Objeto VersionControlExplorer da IDE</param>
    /// <returns>Item selecionado; null, caso contrário</returns>
    public static VersionControlExplorerItem GetSelectedItem(this VersionControlExt vcExt)
    {
      var itens = GetSelectedItems(vcExt);
      if (itens?.Length > 0)
        return itens[0];
      return null;
    }

    /// <summary>
    /// Retornar os itens selecionados no Source Control Explorer
    /// </summary>
    /// <param name="vcExt">Objeto VersionControlExplorer da IDE</param>
    /// <returns>Itens selecionados; null, caso contrário</returns>
    public static VersionControlExplorerItem[] GetSelectedItems(this VersionControlExt vcExt)
    {
      if (vcExt?.Explorer?.SelectedItems != null)
        return vcExt.Explorer.SelectedItems;
      return null;
    }

    /// <summary>
    /// Navegar para a raiz do TFS
    /// </summary>
    /// <param name="vcExt">VersionControlExt</param>
    public static void NavigateToRoot(this VersionControlExplorerExt vcExt)
    {
      vcExt.Navigate("$/");
    }



    /// <summary>
    /// Recuperar o Workspace e o localItem com base em um serverItem
    /// </summary>
    /// <param name="vcServer">Controle de fontes (VersionControlServer)</param>
    /// <param name="serverItem">Server item</param>
    /// <param name="lstWorkspaceLocalItem">Lista de Workspaces/localItem encontrados</param>
    /// <returns>true se o workspace e o local item foram recuperados</returns>
    public static bool GetWorkspaceForServerItem(this VersionControlServer vcServer, string serverItem,
      out List<(Workspace Ws, string LocalItem)> lstWorkspaceLocalItem)
    {
      List<Func<Workspace[]>> lstQueryWorkspaces = new List<Func<Workspace[]>>()
      {
        () => new Workspace[]{ Utils.GetVersionControlServerExt()?.Explorer?.Workspace },
        () => vcServer.QueryWorkspaces(null, vcServer.AuthorizedUser, Environment.MachineName),
        () => vcServer.QueryWorkspaces(null, null, Environment.MachineName, WorkspacePermissions.Use)
      };

      lstWorkspaceLocalItem = new List<(Workspace Ws, string LocalItem)>();
      foreach (var esteHandler in lstQueryWorkspaces)
      {
        Workspace[] lstWorkSpace = esteHandler.Invoke();
        if (lstWorkSpace != null)
        {
          foreach (var esteWs in lstWorkSpace)
          {
            string tempLocalItem = esteWs?.TryGetLocalItemForServerItem(serverItem);
            if (!tempLocalItem.IsNullOrWhiteSpaceEx())
            {
              lstWorkspaceLocalItem.Add((esteWs, tempLocalItem));
            }
          }
          if (lstWorkspaceLocalItem.Count > 0)
          {
            if (lstWorkspaceLocalItem.Count > 1)
            {
              Utils.LogDebugMessage($"Mais de um localItem para o serverItem: {serverItem}" + Environment.NewLine +
                string.Join(Environment.NewLine, lstWorkspaceLocalItem.Select(x => $"{x.Ws.Name} - {x.LocalItem}")), false);
            }
            return true;
          }
        }
      }

      return false;
    }


    /// <summary>
    /// Recuperar um item do TFS
    /// </summary>
    /// <param name="vcServer">VersionControlServer</param>
    /// <param name="serverItem">ServerItem</param>
    /// <returns>item retornado; null, caso o item não tenha sido encontrado</returns>
    public static Item TryGetItem(this VersionControlServer vcServer, string serverItem)
    {
      ItemSet lstItems = vcServer.GetItems(
        serverItem,
        VersionSpec.Latest,
        RecursionType.None,
        DeletedState.Any,
        ItemType.Any,
        false);

      if (lstItems.Items.Length == 0)
        return null;
      else
        return lstItems.Items[0];
    }
  }
}