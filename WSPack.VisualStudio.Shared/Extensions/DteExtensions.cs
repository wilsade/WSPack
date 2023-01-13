using System;
using System.Diagnostics;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamFoundation;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;

using WSPack.Lib;

namespace WSPack.VisualStudio.Shared.Extensions
{
  static class DteExtensions
  {

    /// <summary>
    /// Escrever na janela: Output
    /// </summary>
    /// <param name="applicationObject">Aplicação DTE2</param>
    /// <param name="forcePanelShow">true para forçar a exbibição do painel</param>
    /// <param name="message">A mensagem a ser escrita</param>
    /// <param name="parameters">Parâmetros da mensagem</param>
    public static void WriteInOutPut(this EnvDTE80.DTE2 applicationObject, bool forcePanelShow,
      string message)
    {
      try
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        EnvDTE.Window window = applicationObject.Windows.Item(EnvDTE.Constants.vsWindowKindOutput);
        EnvDTE.OutputWindow outputWindow = (EnvDTE.OutputWindow)window.Object;
        EnvDTE.OutputWindowPane owp = null;

        for (uint i = 1; i <= outputWindow.OutputWindowPanes.Count; i++)
        {
          string aux = outputWindow.OutputWindowPanes.Item(i).Name;
          if (aux == Constantes.WSPack)
          {
            owp = outputWindow.OutputWindowPanes.Item(i);
            break;
          }
        }

        if (owp == null)
          owp = outputWindow.OutputWindowPanes.Add(Constantes.WSPack);

        if (forcePanelShow)
        {
          window.Activate();
          owp.Activate();
        }
        else
          owp.Activate();

        owp.OutputString(message + Environment.NewLine);
      }
      catch (Exception ex)
      {
        Trace.WriteLine($"WriteInOutPut: {ex.Message}");
      }
    }

    /// <summary>
    /// Recuperar o VersionControlExt da IDE do VS
    /// </summary>
    /// <param name="applicationObject">Aplicação do VS</param>
    /// <param name="versionControl">VersionControlExt</param>
    /// <returns>VersionControlExt da IDE do VS; null, caso contrário</returns>
    public static bool TryGetVersionControlExt(this EnvDTE80.DTE2 applicationObject,
      out VersionControlExt versionControl)
    {
      versionControl = null;
      if (applicationObject != null)
        versionControl = applicationObject.GetObject("Microsoft.VisualStudio.TeamFoundation.VersionControl.VersionControlExt") as VersionControlExt;
      return versionControl != null;
    }

    /// <summary>
    /// Recuperar o controle TeamFoundationServerExt da IDE do VS
    /// </summary>
    /// <param name="applicationObject">Aplicação do VS</param>
    /// <returns>TeamFoundationServerExt da IDE do VS; null, caso contrário</returns>
    public static TeamFoundationServerExt GetTeamFoundationServerExt(this EnvDTE80.DTE2 applicationObject)
    {
      TeamFoundationServerExt tfs = null;
      if (applicationObject != null)
        tfs = applicationObject.GetObject("Microsoft.VisualStudio.TeamFoundation.TeamFoundationServerExt") as TeamFoundationServerExt;
      return tfs;
    }

    /// <summary>
    /// Recuperar o VersionControlExt da IDE do VS
    /// </summary>
    /// <param name="applicationObject">Aplicação do VS</param>
    /// <returns>VersionControlExt da IDE do VS; null, caso contrário</returns>
    public static VersionControlExt GetVersionControlExt(this EnvDTE80.DTE2 applicationObject)
    {
      if (TryGetVersionControlExt(applicationObject, out var vcServer))
        return vcServer;
      return null;
    }



    /// <summary>
    /// Recuperar qual o item que está selecionado no Solution Explorer
    /// </summary>
    /// <param name="applicationObject">Aplicação do VS</param>
    /// <returns>Caminho local do item; string.Empty caso contrário</returns>
    public static string GetSolutionExplorerSelectedItem(this EnvDTE80.DTE2 applicationObject)
    {
      if (applicationObject == null || applicationObject.SelectedItems == null)
        return string.Empty;

      ThreadHelper.ThrowIfNotOnUIThread();

      // Pegar o item selecionado
      string localItem = string.Empty;
      if (applicationObject.SelectedItems.Count > 0)
      {
        EnvDTE.SelectedItem esteItem = applicationObject.SelectedItems.Item(1);

        if (esteItem.ProjectItem == null)
        {
          if (esteItem.Project == null)
          {
            localItem = applicationObject.Solution.FullName;
            return localItem;
          }
          localItem = esteItem.Project.FullName;
          return localItem;
        }

        localItem = esteItem.ProjectItem.get_FileNames(1);
        return localItem;
      }

      return localItem;
    }


    /// <summary>
    /// Recuperar o caminho completo do documento ativo na aba da IDE
    /// </summary>
    /// <param name="dte"></param>
    /// <returns></returns>
    public static string GetDocumentPath(this EnvDTE80.DTE2 dte)
    {
      try
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        var janela = dte.ActiveWindow;
        if (janela?.Document == null)
          return janela.Project?.FileName;
        else
          return janela.Document.FullName;
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
        return string.Empty;
      }
    }
  }
}