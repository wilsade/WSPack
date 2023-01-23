using System;
using System.Diagnostics;

using EnvDTE;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;

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


    /// <summary>
    /// Recuperar o projeto selecionado no Solution Explorer
    /// </summary>
    /// <param name="dte">Aplicação do VS</param>
    /// <returns>Projeto selecionado; null, caso contrário</returns>
    public static EnvDTE.Project GetSolutionExplorerActiveProject(this EnvDTE80.DTE2 dte)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      try
      {
        if (dte != null && dte.Solution != null && dte.Solution.Count > 0 && !string.IsNullOrEmpty(dte.Solution.FullName) &&
            dte.Solution.Projects.Count > 0)
        {
          if (dte.ActiveSolutionProjects != null)
          {
            if (dte.ActiveSolutionProjects is Array array && array.Length > 0)
              return array.GetValue(0) as EnvDTE.Project;
          }
        }
      }
      catch (Exception ex)
      {
        Utils.LogDebugError("Erro do VS ao dte.ActiveSolutionProjects != null: " + ex.Message);
      }

      return null;
    }

    /// <summary>
    /// Recuperar a primeira propriedade que tenha um dos nomes informados
    /// </summary>
    /// <param name="properties">Enumerado de propriedades do objeto</param>
    /// <param name="propertyNames">Lista de nomes para procurar</param>
    /// <returns>primeira propriedade que tenha um dos nomes informados</returns>
    public static ResponseItem<Property> GetProperty(this EnvDTE.Properties properties, params string[] propertyNames)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (properties != null)
      {
        foreach (Property item in properties)
        {
          foreach (var propertyName in propertyNames)
          {
            if (propertyName.EqualsInsensitive(item.Name))
            {
              return new ResponseItem<Property>()
              {
                Item = item,
                Success = true
              };
            }
          }
        }
      }
      return new ResponseItem<Property>()
      {
        Success = false,
        ErrorMessage = ResourcesLib.StrPropriedadeNaoEncontrada
      };
    }

    /// <summary>
    /// Indica se o projeto é .Core / Standard ou Shared
    /// </summary>
    /// <param name="hierarchy">Hierarchy</param>
    /// <returns>true se o projeto é .NetCore ou Standard</returns>
    public static bool IsDotNetCoreStandardShared(this IVsHierarchy hierarchy)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      return hierarchy.IsCapabilityMatch("CPS");
    }

    /// <summary>
    /// Indica se o projeto é um Solution folder
    /// </summary>
    /// <param name="project">Project</param>
    /// <returns>true se o projeto é um Solution folder</returns>
    public static bool IsSolutionFolder(this Project project)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      return project.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";
    }
  }
}