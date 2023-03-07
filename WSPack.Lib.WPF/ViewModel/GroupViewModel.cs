using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    bool _isFocused;
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
  }
}
