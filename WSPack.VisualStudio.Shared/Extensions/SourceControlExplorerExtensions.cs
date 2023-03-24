using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.TeamFoundation;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Items;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Commands;

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
                string.Join(Environment.NewLine, lstWorkspaceLocalItem.Select(x => $"{x.Ws.Name} - {x.LocalItem}")));
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

    /// <summary>
    /// Recuperar o LocalItem com base em um serverItem
    /// </summary>
    /// <param name="tfsExt">TFS ext</param>
    /// <param name="serverItem">Server item</param>
    /// <returns>Lista contendo informações do workspace e do item local; ou lista vazia</returns>
    public static List<(Workspace Ws, string LocalItem)> GetLocalItemForServerItem(this TeamFoundationServerExt tfsExt, string serverItem)
    {
      VersionControlServer vcServer = tfsExt.GetVersionControlServer();
      if (vcServer != null && vcServer.GetWorkspaceForServerItem(serverItem, out var lstWorkspaceLocalItem))
        return lstWorkspaceLocalItem;
      return null;
    }

    /// <summary>
    /// Verifica se existe workspace para um ServerItem
    /// </summary>
    /// <param name="vcServer">Controle de fontes (VersionControlServer)</param>
    /// <param name="serverItem">Server item</param>
    /// <returns>true se existe workspace para um ServerItem</returns>
    public static bool HasWorkspaceForServerItem(this VersionControlServer vcServer, string serverItem)
    {
      return GetWorkspaceForServerItem(vcServer, serverItem, out _);
    }

    /// <summary>
    /// Recuperar os Check In Notes definidos em um determinado caminho do servidor
    /// </summary>
    /// <param name="vcServer">VersionControlServer</param>
    /// <param name="serverPath">Caminho de um item do TFS</param>
    /// <returns>Lista de CheckInNotes definida no serverPath; todos os CheckInNotes caso contrário</returns>
    public static IEnumerable<string> GetCheckInNotes(this VersionControlServer vcServer, string serverPath)
    {
      CheckinNoteFieldDefinition[] lstNotas = vcServer.GetCheckinNoteDefinitionsForServerPaths(
        new string[] { serverPath });
      if (lstNotas.Length > 0)
      {
        IEnumerable<string> lstAux = lstNotas.Select(x => x.Name);
        return lstAux.ToArray();
      }

      else
      {
        string[] notas = vcServer.GetAllCheckinNoteFieldNames();
        return notas;
      }
    }


    /// <summary>
    /// Recuperar o Workspace e o ServerItem com base no LocalItem
    /// </summary>
    /// <param name="vcServer">Controle de fontes (VersionControlServer)</param>
    /// <param name="localItem">Local item</param>
    /// <param name="workspace">Workspace</param>
    /// <param name="serverItem">true se existe workspace para o LocalItem</param>
    /// <returns></returns>
    public static bool GetWorkspaceForLocalItem(this VersionControlServer vcServer, string localItem, out Workspace workspace, out string serverItem)
    {
      serverItem = null;
      workspace = null;
      if (vcServer == null)
        return false;

      workspace = vcServer.TryGetWorkspace(localItem);
      if (workspace != null)
      {
        serverItem = workspace.TryGetServerItemForLocalItem(localItem);
        return true;
      }
      return false;
    }

    /// <summary>
    /// Verifica se o item existe no TFS
    /// </summary>
    /// <param name="teamFoundationServer">teamFoundationServer</param>
    /// <param name="serverItem">Server item</param>
    /// <param name="msg">Msg</param>
    /// <returns>true se o item existe</returns>
    public static bool ItemExists(this TeamFoundationServerExt teamFoundationServer, string serverItem, out string msg)
    {
      return ItemExists(teamFoundationServer?.GetVersionControlServer(), serverItem, out msg);
    }

    /// <summary>
    /// Verifica se o item existe no TFS
    /// </summary>
    /// <param name="vcServer">vcServer</param>
    /// <param name="serverItem">Server item</param>
    /// <param name="msg">Msg</param>
    /// <returns>true se o item existe</returns>
    public static bool ItemExists(this VersionControlServer vcServer, string serverItem, out string msg)
    {
      if (vcServer != null)
      {
        msg = "";
        var achei = vcServer.TryGetItem(serverItem);
        if (achei == null)
          msg = ResourcesLib.StrItemNaoEncontrado.FormatWith(serverItem);
        return achei != null;
      }
      else
      {
        msg = ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao;
        return false;
      }
    }


    /// <summary>
    /// Abre um diálogo para escolher um item do TFS
    /// </summary>
    /// <param name="vcServer">Version Control Server</param>
    /// <param name="preSelectItem">Pré-selecionar um item (pode ser null)</param>
    /// <returns>Item escolhido; null, se nenhum item foi escolhido</returns>
    internal static string ShowChooseTFSItemDialog(this VersionControlServer vcServer, string preSelectItem)
    {
      try
      {
        Assembly controlsAssembly = Assembly.Load(@"Microsoft.TeamFoundation.VersionControl.Controls");
        Type vcChooseItemDialogType = controlsAssembly.GetType("Microsoft.TeamFoundation.VersionControl.Controls.DialogChooseItem");

        ConstructorInfo ci = vcChooseItemDialogType.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new Type[] { typeof(VersionControlServer), typeof(string), typeof(string) },
                null);

        Form _chooseItemDialog = null;
        PropertyInfo _selectItemProperty = null;
        DialogResult dialogResult;

        _chooseItemDialog = (Form)ci.Invoke(new object[] { vcServer, string.IsNullOrEmpty(preSelectItem) ||
          preSelectItem.Length <= 2 ? "$/" : Path.GetDirectoryName(preSelectItem), preSelectItem });
        _chooseItemDialog.StartPosition = FormStartPosition.CenterScreen;
        _selectItemProperty = vcChooseItemDialogType.GetProperty("SelectedItem", BindingFlags.Instance | BindingFlags.NonPublic);

        _chooseItemDialog.StartPosition = FormStartPosition.CenterScreen;
        if (_chooseItemDialog.ShowDialog() == DialogResult.OK)
        {
          dialogResult = _chooseItemDialog.DialogResult;
          Item selectedItem = (Item)_selectItemProperty.GetValue(_chooseItemDialog, null);
          string itemEscolhido = selectedItem.ServerItem;
          return itemEscolhido;
        }
      }
      catch (Exception ex)
      {
        MessageBoxUtils.ShowError(ex.Message);
      }

      return null;
    }

    public static bool LocateOrGet(string localItemFullPath, OperationTFSTypes tipoOperacao)
    {
      bool operationOK = false;

      // TFS Explorer OK?
      var vcExt = WSPackPackage.Dte.GetVersionControlExt();
      bool ok = vcExt != null && vcExt.Explorer != null;
      if (ok)
        ok = WSPackPackage.Dte.GetTeamFoundationServerExt().IsSourceControlExplorerActive();

      if (ok)
      {
        var versionControlServer = Utils.GetTeamFoundationServerExt().GetVersionControlServer();

        // TFS OK? Item existe?
        if (versionControlServer != null && versionControlServer.GetWorkspaceForLocalItem(localItemFullPath, out var ws, out var serverItem))
        {
          // O projeto é um item do TFS?
          if (!string.IsNullOrEmpty(serverItem))
          {
            // Localizar
            if (tipoOperacao == OperationTFSTypes.Locate)
            {
              var server = Utils.GetVersionControlServerExt();
              if (server?.Explorer != null)
              {
                LocateInTFSBaseCommand.NavigateToServerItem(server, serverItem);
              }
              else
                Utils.LogOutputMessageForceShow(ResourcesLib.StrSourceControlExplorerNaoConfigurado);
            }

            // GET
            else
            {
              Utils.LogOutputMessage("\r\nEfetuando GET" + $": {localItemFullPath}");
              GetOptions opcoes = GetOptions.None;
              if (tipoOperacao == OperationTFSTypes.GetSpecificVersion)
                opcoes = GetOptions.GetAll | GetOptions.Overwrite;

              GetStatus status = ws.Get(new[] { localItemFullPath }, VersionSpec.Latest, RecursionType.Full, opcoes);
              Utils.LogOutputMessage(string.Format(ResourcesLib.StrTotalItensRecuperados, status.NumFiles));
              operationOK = true;
            }
          }
        }
        else
        {
          Utils.ShowWindowConnectToTFS();
          Utils.LogOutputMessageForceShow(Environment.NewLine + ResourcesLib.StrItemNaoEncontradoTFS + $": {localItemFullPath}");
        }
      }
      else
      {
        if (tipoOperacao != OperationTFSTypes.Locate)
        {
          Utils.LogDebugMessage("SSE não configurado. Vamos tentar fazer o GET via tf.exe");
          Utils.LogOutputMessage("\r\nEfetuando GET" + $": {localItemFullPath}");

          var info = RegistroVisualStudioObj.Instance.TFProcessStartInfo();
          info.Arguments = $"get \"{localItemFullPath}\" /recursive /overwrite";
          if (tipoOperacao == OperationTFSTypes.GetSpecificVersion)
            info.Arguments += " /all /force";

          int result = UtilsLib.ExecuteCommand(info.FileName, info.Arguments, out string output,
            out string outerros);
          Utils.LogOutputMessage(output);
          Trace.WriteLine(outerros);
          operationOK = (result == 1) || (result == 0);
        }
        else
          Utils.ShowWindowConnectToTFS();
      }
      return operationOK;
    }
  }
}