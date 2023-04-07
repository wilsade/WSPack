using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;

using WSPack.Lib.WPF.Model;

namespace WSPack.Lib.WPF.ViewModel
{
  /// <summary>
  /// Modelo de projeto da visão
  /// </summary>
  [DebuggerDisplay("ProjectViewModel: {ProjectCaption}")]
  [XmlRoot("Project")]
  public partial class ProjectViewModel : BaseViewModel
  {
    readonly ProjectModel _projectModel;
    bool _isFocused, _isSelected;

    #region Construtores
    /// <summary>
    /// Cria uma instância da classe <see cref="ProjectViewModel"/>
    /// </summary>
    public ProjectViewModel()
    {
      _projectModel = new ProjectModel();
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="ProjectViewModel"/>
    /// </summary>
    /// <param name="projectModel">Modelo de projeto</param>
    public ProjectViewModel(ProjectModel projectModel)
    {
      _projectModel = projectModel;
    }
    #endregion

    /// <summary>
    /// Localizar o projeto no TFS ou efetuar GET
    /// </summary>
    /// <param name="projeto">Projeto</param>
    /// <param name="tipoOperacao">Tipo operacao</param>
    static bool Locate_OR_Get(ProjectViewModel projeto, OperationTFSTypes tipoOperacao)
    {
      string localItem = tipoOperacao == OperationTFSTypes.Locate ? projeto.ProjectFullPath : projeto.ProjectDirectory;
      bool operationOK = WSPackFlexSupport.Instance.PackSupport.Locate_OR_Get(localItem, tipoOperacao);
      return operationOK;
    }

    void DialogoParaExclusao(ProjectViewModel project)
    {
      string msgExclusao = string.Format(ResourcesLib.StrItemNaoEncontrado, project.ProjectFullPath) +
                  Environment.NewLine + Environment.NewLine + ResourcesLib.StrDesejaExcluirItemLista;

      if (WPFExtensions.ShowWarningYesNo(msgExclusao))
      {
        Parent.RemoveProjectAndReorder(project);
        Parent?.Parent?.Save();
      }
    }

    void InternoAbrirProjeto(ProjectViewModel project)
    {
      if (project == null)
        return;

      if (project.ProjectFullPath.EndsWithInsensitive(".sln") || Directory.Exists(project.ProjectFullPath))
      {
        bool abriu = WSPackFlexSupport.Instance.PackSupport.OpenSolutionFile(project.ProjectFullPath);
        if (abriu)
          Parent?.CheckOpenedProjectApearsFirst(project);
        else if (!System.IO.File.Exists(project.ProjectFullPath))
        {
          DialogoParaExclusao(project);
        }
      }
      else
      {
        if (System.IO.File.Exists(project.ProjectFullPath))
        {
          bool ok = WSPackFlexSupport.Instance.PackSupport.OpenProjectFile(project.ProjectFullPath);
          if (ok)
            Parent?.CheckOpenedProjectApearsFirst(project);
        }
        else
          DialogoParaExclusao(project);
      }
    }

  }
}
