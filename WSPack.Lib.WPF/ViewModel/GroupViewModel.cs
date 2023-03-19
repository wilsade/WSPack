using System.Diagnostics;
using System.Xml.Serialization;

using WSPack.Lib.WPF.Model;

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
    //ObservableCollection<ProjectViewModel> _lstProjetos;
    //ProjectViewModel _selectedProject;
    bool _isFocused, _isSelected;
    private string _groupDefaultPath;

    #region Construtores
    /// <summary>
    /// Inicialização da classe: <see cref="GroupViewModel"/>.
    /// </summary>
    public GroupViewModel()
    {
      _groupModel = new GroupModel();
      //_lstProjetos = new ObservableCollection<ProjectViewModel>();
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
  }
}
