using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

using WSPack.Lib.Extensions;
namespace WSPack.Lib.WPF.ViewModel
{
  partial class GroupViewModel
  {
    /// <summary>
    /// Identificador do projeto
    /// </summary>
    [XmlElement("Id")]
    public int GroupId
    {
      get { return _groupModel.Id; }
      set
      {
        _groupModel.Id = value;
        RaisePropertyChanged(nameof(GroupId));
      }
    }

    /// <summary>
    /// Nome do grupo
    /// </summary>
    [XmlElement("Caption")]
    public string GroupCaption
    {
      get { return _groupModel.Caption; }
      set
      {
        if (!string.IsNullOrWhiteSpace(value))
        {
          if (Parent?.GroupList == null || !Parent.GroupList.Any(x => x.GroupCaption.EqualsInsensitive(value)))
          {
            _groupModel.Caption = value;
            RaisePropertyChanged(nameof(GroupCaption));
          }
        }
      }
    }

    /// <summary>
    /// Lista de projetos do grupo
    /// </summary>
    [XmlArrayItem("Project")]
    public ObservableCollection<ProjectViewModel> ProjectList
    {
      get
      {
        _lstProjetos.Sort(x => x.ProjectId);
        return _lstProjetos;
      }
      set { _lstProjetos = value; }
    }

    /// <summary>
    /// Indica se o grupo possui algum projeto
    /// </summary>
    public bool HasProjects => _lstProjetos != null && _lstProjetos.Any();

    /// <summary>
    /// Representa o projeto que está selecionado
    /// </summary>
    [XmlIgnore]
    public ProjectViewModel SelectedProject
    {
      get { return _selectedProject; }
      set
      {
        if (value != null)
        {
          if (SelectedProject != null)
            SelectedProject.IsSelected = false;

          _selectedProject = value;
          RaisePropertyChanged(nameof(SelectedProject));
          SelectedProject.IsSelected = true;
        }
      }
    }

    /// <summary>
    /// StartPage que contém o grupo
    /// </summary>
    [XmlIgnore]
    public StartPageViewModel Parent { get; set; }

    /// <summary>
    /// Indica se o controle do grupo está com o foco
    /// </summary>
    [XmlIgnore]
    public bool IsFocused
    {
      get { return _isFocused; }
      set
      {
        _isFocused = value;
        RaisePropertyChanged(nameof(IsFocused));
      }
    }

    [XmlIgnore]
    public bool IsSelected
    {
      get => _isSelected;
      set
      {
        _isSelected = value;
        RaisePropertyChanged(nameof(IsSelected));
      }
    }

    /// <summary>
    /// Indica se projetos recém abertos irão aparecer no início da lista
    /// </summary>
    public bool OpenedProjectsFirst
    {
      get => _openedProjectsFirst;
      set
      {
        _openedProjectsFirst = value;
        RaisePropertyChanged(nameof(OpenedProjectsFirst));
      }
    }

    /// <summary>
    /// Diretório padrão ao abrir o diálogo para escolher um projeto
    /// </summary>
    public string GroupDefaultPath
    {
      get => _groupDefaultPath;
      set
      {
        _groupDefaultPath = value;
        RaisePropertyChanged(nameof(GroupDefaultPath));
      }
    }
  }
}
