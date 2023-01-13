using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.TeamFoundation;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;

using WSPack.Lib.Extensions;
using WSPack.VisualStudio.Shared.Extensions;
using WSPack.VisualStudio.Shared.Forms;

namespace WSPack.VisualStudio.Shared
{
  static class Utils
  {
    internal static bool IsDebugOrDebugModeEnabled
    {
      get
      {
#if DEBUG
        return true;
#else
        return WSPackPackage.ParametrosGerais.EnableDebugMode;
#endif
      }
    }

    /// <summary>
    /// Escrever no Output apenas em modo de Debug
    /// </summary>
    /// <param name="message">A mensagem a ser escrita</param>
    /// <param name="forceShow">true para exibir o painel</param>
    public static void LogDebugMessage(string message, bool forceShow = true)
    {
      if (!IsDebugOrDebugModeEnabled)
        return;

      try
      {
        WSPackPackage.Dte.WriteInOutPut(forceShow, message);
        Trace.WriteLine(message);
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
      }
    }

    /// <summary>
    /// Escrever no Output apenas em modo de Debug
    /// </summary>
    /// <param name="message">Message</param>
    public static void LogDebugError(string message)
    {
      if (!IsDebugOrDebugModeEnabled)
        return;

      try
      {
        WSPackPackage.Dte.WriteInOutPut(true, $"*** ERRO ***{Environment.NewLine}{message}");
        Trace.WriteLine(message);
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
      }
    }

    /// <summary>
    /// Escrever na janela: Output
    /// </summary>
    /// <param name="forceShow">true para exibir o painel</param>
    /// <param name="message">A mensagem a ser escrita</param>
    /// <param name="parameters">Parâmetros da mensagem</param>
    public static void LogOutputMessage(bool forceShow, string messages)
    {
      WSPackPackage.Dte.WriteInOutPut(forceShow, messages);
    }

    /// <summary>
    /// Recuperar o item selecionado no SourceControlExplorer
    /// </summary>
    /// <returns></returns>
    public static string GetSourceControlExplorerSelectedItem()
    {
      var vc = GetVersionControlServerExt();
      string serverItem = vc?.GetSelectedItem()?.SourceServerPath;
      return serverItem;
    }


    internal static bool TryGetGitServerItem(string localItem, out string serverItem)
    {
      serverItem = null;
      try
      {
        Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

        var sp = new Microsoft.VisualStudio.Shell.ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)WSPackPackage.Dte);
        if (!(sp.GetService(typeof(IGitExt3)) is IGitExt3 gitExt3 && gitExt3.ActiveRepositories.FirstOrDefault() is IGitRepositoryInfo2 gitRepository))
          return false;
        var replacePath = localItem.ReplaceInsensitive(gitRepository.RepositoryPath, string.Empty)
          .Replace("\\", "/");

        IGitRemoteInfo remoteInfo = gitRepository.Remotes[0];
        serverItem = $"{remoteInfo.FetchUrl}?path={replacePath}";

        return serverItem != null;
      }
      catch (Exception ex)
      {
        LogOutputMessage(false, ex.ToString());
        return false;
      }
    }

    /// <summary>
    /// Recuperar o VersionControlExt da IDE do VS
    /// </summary>
    /// <returns>VersionControlExt da IDE do VS</returns>
    public static VersionControlExt GetVersionControlServerExt()
    {
      return WSPackPackage.Dte.GetVersionControlExt();
    }

    /// <summary>
    /// Recuperar o controle TeamFoundationServerExt da IDE do VS
    /// </summary>
    /// <returns>Controle TeamFoundationServerExt da IDE do VS</returns>
    public static TeamFoundationServerExt GetTeamFoundationServerExt()
    {
      return WSPackPackage.Dte.GetTeamFoundationServerExt();
    }


    /// <summary>
    /// Escolher um item da lista
    /// </summary>
    /// <param name="lstWorkspaceLocalItem">Lista de workspace local item</param>
    /// <returns>Item da lista</returns>
    public static (Workspace Ws, string LocalItem) ChooseItem(List<(Workspace Ws, string LocalItem)> lstWorkspaceLocalItem)
    {
      if (lstWorkspaceLocalItem.Count > 0)
      {
        if (lstWorkspaceLocalItem.Count > 1)
        {
          using (var form = new ChooseWorkspaceLocalItemForm())
          {
            form.BindList(lstWorkspaceLocalItem);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
              return (form.ItemSelecionado.Workspace, form.ItemSelecionado.LocalItem);
            }
            return (null, null);
          }
        }
        return lstWorkspaceLocalItem[0];
      }
      return (null, null);
    }

    /// <summary>
    /// Recuperar qual o item que está selecionado no Solution Explorer
    /// </summary>
    /// <returns></returns>
    public static string GetSolutionExplorerSelectedItem()
    {
      return WSPackPackage.Dte.GetSolutionExplorerSelectedItem();
    }
  }
}