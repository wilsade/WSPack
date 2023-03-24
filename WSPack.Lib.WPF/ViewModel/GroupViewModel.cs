using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.Win32;

using WSPack.Lib.WPF.Model;
using WSPack.Lib.Extensions;

namespace WSPack.Lib.WPF.ViewModel
{
  /// <summary>
  /// Modelo da visão de Grupos
  /// </summary>
  [DebuggerDisplay("GroupViewModel: {GroupCaption}")]
  [XmlRoot("Group")]
  public partial class GroupViewModel : BaseViewModel
  {
    readonly GroupModel _groupModel;
    ObservableCollection<ProjectViewModel> _lstProjetos;
    ProjectViewModel _selectedProject;
    bool _isFocused, _isSelected;
    private string _groupDefaultPath;

    #region Construtores
    /// <summary>
    /// Inicialização da classe: <see cref="GroupViewModel"/>.
    /// </summary>
    public GroupViewModel()
    {
      _groupModel = new GroupModel();
      _lstProjetos = new ObservableCollection<ProjectViewModel>();
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="GroupViewModel"/>
    /// </summary>
    /// <param name="groupModel">Modelo de grupos</param>
    public GroupViewModel(GroupModel groupModel)
      : this()
    {
      _groupModel = groupModel;
    }
    #endregion

    /// <summary>
    /// Alterar um projeto de lugar, movendo-o para cima ou para baixo
    /// </summary>
    /// <param name="projeto">Grupo</param>
    /// <param name="movingDown">Informe "true" para mover o projeto para baixo</param>
    void ReorderProject(ProjectViewModel projeto, bool movingDown)
    {
      if (movingDown)
      {
        _lstProjetos[projeto.ProjectId].ProjectId--;
        projeto.ProjectId++;
      }

      else
      {
        _lstProjetos[projeto.ProjectId - 2].ProjectId++;
        projeto.ProjectId--;
      }
      RaisePropertyChanged(nameof(ProjectList));
      projeto.IsFocused = true;
      SelectedProject = projeto;
    }

    /// <summary>
    /// Remove um projeto da lista de projeto e ajusta o Id dos demais projetos
    /// </summary>
    /// <param name="projeto">Projeto</param>
    internal void RemoveProjectAndReorder(ProjectViewModel projeto)
    {
      int id = projeto.ProjectId;
      if (id <= 0)
        id = 1;
      _lstProjetos.Remove(projeto);
      for (int i = id - 1; i < _lstProjetos.Count; i++)
      {
        _lstProjetos[i].ProjectId = i + 1;
      }

      RaisePropertyChanged(nameof(HasProjects));
      RaisePropertyChanged(nameof(ProjectList));

      if (_lstProjetos.Any())
      {
        int indiceDesejado = id - 1;
        if (_lstProjetos.Count == indiceDesejado)
          indiceDesejado--;

        SelectedProject = ProjectList[indiceDesejado];
      }
    }

    /// <summary>
    /// Abre um diálogo para escolha de um projeto
    /// </summary>
    /// <param name="groupDefaultPath">Diretório padrão do grupo</param>
    /// <returns>true se escolheu o projeto + projeto escolhido</returns>
    internal static (bool Ok, string FileName) ChooseProjectDialog(string groupDefaultPath)
    {
      var openDlg = new OpenFileDialog()
      {
        DefaultExt = "*.sln",
        Filter = "Solutions e Projetos|*.sln;*.csproj|Solutions (*.sln)|*.sln|Projetos (*.csproj)|*.csproj",
        Title = "Selecione o projeto para inclui-lo ao grupo",
        InitialDirectory = groupDefaultPath
      };
      bool ok = openDlg.ShowDialog() == true;
      return (ok, openDlg.FileName);
    }

    internal string PegarDiretorioPadrao()
    {
      string defaultPath = GroupDefaultPath;
      //if (string.IsNullOrEmpty(defaultPath))
      //{
      //  if (SelectedProject?.ProjectFullPath != null)
      //    defaultPath = Path.GetDirectoryName(SelectedProject.ProjectFullPath);
      //  else if (ProjectList.FirstOrDefault() is ProjectViewModel projectView &&
      //    !string.IsNullOrEmpty(projectView.ProjectFullPath))
      //    defaultPath = Path.GetDirectoryName(projectView.ProjectFullPath);
      //}
      return defaultPath;
    }

    /// <summary>
    /// Verificar se o projeto deverá aparecer no início da lista
    /// </summary>
    /// <param name="projeto">Projeto</param>
    internal void CheckOpenedProjectApearsFirst(ProjectViewModel projeto)
    {
      if (OpenedProjectsFirst)
      {
        projeto.ProjectId = -1;
        _lstProjetos.Sort(x => x.ProjectId, (esteProjeto, indice) => esteProjeto.ProjectId = indice + 1);
        RaisePropertyChanged(nameof(ProjectList));
        Parent?.Save();
      }
    }
  }
}
