using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;

namespace WSPack.Lib.WPF.ViewModel
{
  partial class StartPageViewModel
  {
    /// <summary>
    /// Indica se a StarPage está carregando
    /// </summary>
    [XmlIgnore]
    public string InfoText
    {
      get => _infoText;
      set
      {
        if (value != null)
        {
          _infoText = value;
          RaisePropertyChanged(nameof(InfoText));
        }
      }
    }

    /// <summary>
    /// Indica se a StartPage possui algum grupo
    /// </summary>
    public bool HasGroups => _lstGroups != null && _lstGroups.Any();

    /// <summary>
    /// Lista de grupos
    /// </summary>
    [XmlArrayItem("Group")]
    public ObservableCollection<GroupViewModel> GroupList
    {
      get
      {
        _lstGroups.Sort(x => x.GroupId);
        return _lstGroups;
      }
      set
      {
        _lstGroups = value;
        RaisePropertyChanged(nameof(GroupList));
      }
    }

    /// <summary>
    /// Representa o grupo selecionado
    /// </summary>
    [XmlIgnore]
    public GroupViewModel SelectedGroup
    {
      get { return _selectedGroup; }
      set
      {
        if (value != null)
        {
          _selectedGroup = value;
          RaisePropertyChanged(nameof(SelectedGroup));
        }
      }
    }
  }
}
