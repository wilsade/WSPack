﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;
using WSPack.Lib.WPF.Model;

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
    /// Indica se o painel esquerdo está expandido
    /// </summary>
    public bool IsLeftPanelExpanded
    {
      get { return _starPageModel.IsLeftPanelExpanded; }
      set
      {
        _starPageModel.IsLeftPanelExpanded = value;
        RaisePropertyChanged(nameof(IsLeftPanelExpanded));
        Save();
      }
    }

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
          if (SelectedGroup != null)
            SelectedGroup.IsSelected = false;
          _selectedGroup = value;
          RaisePropertyChanged(nameof(SelectedGroup));
          SelectedGroup.IsSelected = true;
        }
      }
    }

    /// <summary>
    /// Indica se a StartPage possui algum grupo
    /// </summary>
    public bool HasGroups => _lstGroups != null && _lstGroups.Any();
    /*
    /// <summary>
    /// Lista de Comandos personalizados
    /// </summary>
    [XmlArrayItem("CustomCommand")]
    public ObservableCollection<CustomCommandViewModel> CustomCommandsList
    {
      get
      {
        _lstCustomCommands.Sort(x => x.CustomCommandId);
        return _lstCustomCommands;
      }
      set
      {
        _lstCustomCommands = value;
        RaisePropertyChanged(nameof(CustomCommandsList));
      }
    }

    /// <summary>
    /// Representa o grupo selecionado
    /// </summary>
    [XmlIgnore]
    public CustomCommandViewModel SelectedCustomCommand
    {
      get { return _selectedCustomCommand; }
      set
      {
        if (value != null)
        {
          _selectedCustomCommand = value;
          RaisePropertyChanged(nameof(SelectedCustomCommand));
        }
      }
    }*/

    /// <summary>
    /// Altura máxima do painel de projetos
    /// </summary>
    public double ProjectContainerMaxHeight
    {
      get { return _starPageModel.ProjectContainerMaxHeight; }
      set
      {
        if (value != _starPageModel.ProjectContainerMaxHeight)
        {
          if (value == 0)
            value = double.NaN;
          else if (value < AlturaMinimaGrupoProjetos)
            value = AlturaMinimaGrupoProjetos;
          _starPageModel.ProjectContainerMaxHeight = value;
          RaisePropertyChanged(nameof(ProjectContainerMaxHeight));
        }
      }
    }

    /// <summary>
    /// Comprimento do painel de projetos
    /// </summary>
    public double ProjectContainerWidth
    {
      get { return _starPageModel.ProjectContainerWidth; }
      set
      {
        if (value != _starPageModel.ProjectContainerWidth)
        {
          if (value < StartPageModel.ComprimentoMinimoProjectContainer)
            value = StartPageModel.ComprimentoMinimoProjectContainer;
          _starPageModel.ProjectContainerWidth = value;
          RaisePropertyChanged(nameof(ProjectContainerWidth));
        }
      }
    }

    /// <summary>
    /// Indica se a barra de rolagem horizontal será exibida
    /// </summary>
    public bool ProjectHorizontalScrollVisible
    {
      get => _starPageModel.ProjectHorizontalScrollVisible;
      set
      {
        if (value != _starPageModel.ProjectHorizontalScrollVisible)
        {
          _starPageModel.ProjectHorizontalScrollVisible = value;
          RaisePropertyChanged(nameof(ProjectHorizontalScrollVisible));
        }
      }
    }
  }
}
