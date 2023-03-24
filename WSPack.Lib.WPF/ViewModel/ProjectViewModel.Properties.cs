using System.Linq;
using System.Xml.Serialization;

namespace WSPack.Lib.WPF.ViewModel
{
  partial class ProjectViewModel
  {
    /// <summary>
    /// Grupo que contém este projeto
    /// </summary>
    [XmlIgnore]
    public GroupViewModel Parent { get; set; }

    /// <summary>
    /// Diretório do projeto
    /// </summary>
    public string ProjectDirectory => _projectModel.DirectoryName;

    /// <summary>
    /// Indica se o projeto existe
    /// </summary>
    public bool ProjectExists => System.IO.File.Exists(ProjectFullPath);

    /// <summary>
    /// Indica se o projeto está no TFS
    /// </summary>
    public bool IsProjectInTFS => Parent.Parent.IsProjectInTFS(ProjectFullPath);

    /// <summary>
    /// Identificador do projeto
    /// </summary>
    [XmlElement("Id")]
    public int ProjectId
    {
      get => _projectModel.Id;
      set
      {
        _projectModel.Id = value;
        RaisePropertyChanged(nameof(ProjectId));
      }
    }

    /// <summary>
    /// Título do projeto
    /// </summary>
    [XmlElement("Caption")]
    public string ProjectCaption
    {
      get => _projectModel.Caption;
      set
      {
        if (!string.IsNullOrWhiteSpace(value))
        {
          if (Parent?.ProjectList == null || !Parent.ProjectList.Any(x => x.ProjectCaption.EqualsInsensitive(value)))
          {
            _projectModel.Caption = value;
            RaisePropertyChanged(nameof(ProjectCaption));
          }
        }
      }
    }

    /// <summary>
    /// Caminho do projeto
    /// </summary>
    [XmlElement("FullPath")]
    public string ProjectFullPath
    {
      get => _projectModel.FullPath;
      set
      {
        if (!string.IsNullOrWhiteSpace(value))
        {
          _projectModel.FullPath = value;
          RaisePropertyChanged(nameof(ProjectFullPath));
          RaisePropertyChanged(nameof(ProjectDirectory));
          RaisePropertyChanged(nameof(ProjectExists));
        }
      }
    }

    /// <summary>
    /// Indica se o controle do projeto está com o foco
    /// </summary>
    [XmlIgnore]
    public bool IsFocused
    {
      get => _isFocused;
      set
      {
        _isFocused = value;
        RaisePropertyChanged(nameof(IsFocused));
      }
    }
  }
}
