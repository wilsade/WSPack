using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.TeamFoundation;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;
using Microsoft.Win32;

using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Commands;
using WSPack.VisualStudio.Shared.Extensions;
using WSPack.VisualStudio.Shared.Forms;

namespace WSPack.VisualStudio.Shared
{
  static class Utils
  {
    static (bool Achou, string FullPath) GetNotepadPPFromRegistry()
    {
      try
      {
        var chave = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Notepad++", false);
        if (chave != null)
        {
          try
          {
            var valor = Convert.ToString(chave.GetValue(null));
            if (!string.IsNullOrEmpty(valor))
            {
              string notepadPath = Path.Combine(valor, "notepad++.exe");
              return (File.Exists(notepadPath), notepadPath);
            }
          }
          finally
          {
            chave.Close();
          }
        }
      }
      catch (Exception ex)
      {
        Trace.WriteLine("Erro ao tentar recuperar notepad++ (registro): " + ex.Message);
      }
      return (false, null);
    }

    static (bool Achou, string FullPath) GetNotepadPPFromProgramFiles()
    {
      try
      {
        var paths = new string[] { Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%"),
          Environment.ExpandEnvironmentVariables("%ProgramW6432%")};
        foreach (var estePath in paths)
        {
          string notepadPath = Path.Combine(estePath, "Notepad++", "Notepad++.exe");
          if (File.Exists(notepadPath))
          {
            return (File.Exists(notepadPath), notepadPath);
          }
        }
      }
      catch (Exception ex)
      {
        Trace.WriteLine("Erro ao tentar recuperar notepad++ (path): " + ex.Message);
      }
      return (false, null);
    }

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
    private static void LogDebugMessage(string message, bool forceShow)
    {
      if (!IsDebugOrDebugModeEnabled)
        return;

      try
      {
        if (forceShow)
          WSPackPackage.Dte.WriteInOutPutForceShow(message);
        else
          WSPackPackage.Dte.WriteInOutPut(message);
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
    /// <param name="message">A mensagem a ser escrita</param>
    public static void LogDebugMessage(string message)
    {
      LogDebugMessage(message, false);
    }

    /// <summary>
    /// Escrever no Output apenas em modo de Debug
    /// </summary>
    /// <param name="message">A mensagem a ser escrita</param>
    public static void LogDebugMessageForceShow(string message)
    {
      LogDebugMessage(message, true);
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
        WSPackPackage.Dte.WriteInOutPutForceShow($"*** ERRO ***{Environment.NewLine}{message}");
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
    public static void LogOutputMessage(string messages)
    {
      WSPackPackage.Dte.WriteInOutPut(messages);
    }

    /// <summary>
    /// Escrever na janela: Output
    /// </summary>
    /// <param name="forceShow">true para exibir o painel</param>
    /// <param name="message">A mensagem a ser escrita</param>
    /// <param name="parameters">Parâmetros da mensagem</param>
    public static void LogOutputMessageForceShow(string messages)
    {
      WSPackPackage.Dte.WriteInOutPutForceShow(messages);
    }

    /// <summary>
    /// Verifica se o Notepad++ existe e retorna o caminho
    /// </summary>
    /// <param name="path">Caminho do notepad++.exe</param>
    /// <returns>true se o notepad foi encontrado</returns>
    internal static bool GetNotepadPP(out string path)
    {
      var tupla = GetNotepadPPFromRegistry();
      if (!tupla.Achou)
        tupla = GetNotepadPPFromProgramFiles();
      path = tupla.FullPath;
      return tupla.Achou;
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
        LogOutputMessage(ex.ToString());
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

    public static void LaunchBrowser(string fileName)
    {
      try
      {
        string browserName = "msedge.exe";
        using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice"))
        {
          if (userChoiceKey != null)
          {
            object progIdValue = userChoiceKey.GetValue("Progid");
            if (progIdValue != null)
            {
              if (progIdValue.ToString().ContainsInsensitive("chrome"))
                browserName = "chrome.exe";
              else if (progIdValue.ToString().ContainsInsensitive("firefox"))
                browserName = "firefox.exe";
              else if (progIdValue.ToString().ContainsInsensitive("safari"))
                browserName = "safari.exe";
              else if (progIdValue.ToString().ContainsInsensitive("opera"))
                browserName = "opera.exe";
            }
          }
        }

        _ = Process.Start(new ProcessStartInfo(browserName, fileName));
        LogOutputMessageForceShow("Caso o arquivo não seja exibido corretamente:" + Environment.NewLine +
          "- Desabilite a segurança do Browser; ou" + Environment.NewLine +
          "- Tente abrir no editor de texto; ou" + Environment.NewLine +
          "- Tente abrir no Excel");
      }
      catch (Exception ex)
      {
        LogDebugError($"Erro ao abrir Browser no arquivo [{fileName}]: {ex.Message}");
        OpenInEditorBaseCommand.OpenIt(fileName);
      }
    }

    /// <summary>
    /// Executar uma ação na Thread principal
    /// </summary>
    /// <param name="action">Action</param>
    /// <returns>Task</returns>
    public static async Task ExecuteInMainThreadAsync(Action action)
    {
      await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(WSPackPackage.Instance.DisposalToken);
      action.Invoke();
    }

    /// <summary>
    /// Exibir a janela para conectar-se ao TFS
    /// </summary>
    public static void ShowWindowConnectToTFS()
    {
      string command = "Team.ManageConnections";

#if DEBUG
      var lst = WSPackPackage.Dte.ListAllCommands();
      Trace.WriteLine(string.Join(Environment.NewLine, lst));
#endif

      try
      {
        WSPackPackage.Dte.ExecuteCommand(command);
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
      }
      WSPackPackage.Dte.WriteInOutPutForceShow(ResourcesLib.StrSourceControlExplorerNaoConfigurado);
    }
  }
}