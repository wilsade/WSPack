using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

using WSPack.Lib.Properties;
using WSPack.Lib.WPF.Model;
using WSPack.Lib.Extensions;
using WSPack.Lib.WPF.Views;

namespace WSPack.Lib.WPF.ViewModel
{
  /// <summary>
  /// Modelo da visão da StartPage
  /// </summary>
  [XmlRoot("StartPage")]
  public partial class StartPageViewModel : BaseViewModel
  {
    readonly StartPageModel _starPageModel;
    ObservableCollection<GroupViewModel> _lstGroups;
    //ObservableCollection<CustomCommandViewModel> _lstCustomCommands;
    GroupViewModel _selectedGroup;
    //CustomCommandViewModel _selectedCustomCommand;
    readonly Dictionary<string, List<TFSProjectModel>> _dicTFSProjects;
    string _infoText;
    const int AlturaMinimaGrupoProjetos = 150;
    double _heightBeforeCollapse;
    double? _actualHeight;

#warning Rever os custom commands
    readonly List<CustomCommandModel> _lstComandosDefaults = new List<CustomCommandModel>
    {
      new CustomCommandModel(0, "New project", "File.NewProject", ""),
      new CustomCommandModel(0, "Open project", "File.OpenProject", ""),
      new CustomCommandModel(0, "Open from Source Control", "File.OpenfromSourceControl", "")
    };

    #region Construtores
    /// <summary>
    /// Inicialização da classe: <see cref="StartPageViewModel" />.
    /// </summary>
    public StartPageViewModel()
    {
      _starPageModel = new StartPageModel();
      Instance = this;
      InfoText = ResourcesLib.StrIniciando;
      _lstGroups = new ObservableCollection<GroupViewModel>();
      //_lstCustomCommands = new ObservableCollection<CustomCommandViewModel>();
      _dicTFSProjects = new Dictionary<string, List<TFSProjectModel>>(StringComparer.OrdinalIgnoreCase);

      if (_lstGroups.Count == -1)
      {
        _lstGroups.Add(new GroupViewModel(new GroupModel(1, "Estudos"))
        {
          ProjectList = new ObservableCollection<ProjectViewModel>()
        {
          new ProjectViewModel(new ProjectModel(1, "mvvm", @"C:\_Wilsade\Projetos\CSharp\Estudos\WPFStuding\MVVM\MvvmExample\MvvmExample.sln")),
          new ProjectViewModel(new ProjectModel(2, "WinFormApp1", @"C:\Users\William\Documents\Visual Studio 2015\Projects\WindowsFormsApplication1\WindowsFormsApplication1.sln"))
        },
        });

        _lstGroups.Add(new GroupViewModel(new GroupModel(3, "Particular"))
        {
          ProjectList = new ObservableCollection<ProjectViewModel>()
        {
          new ProjectViewModel(new ProjectModel(1, "ConsoleApp1", @"C:\Users\William\Documents\Visual Studio 2017\Projects\ConsoleApp1\ConsoleApp1.sln"))
        }
        });

        _lstGroups.Add(new GroupViewModel(new GroupModel(2, "Ferramentas"))
        {
          ProjectList = new ObservableCollection<ProjectViewModel>()
        {
          new ProjectViewModel(new ProjectModel(1, "WindowsProjTestarPowerShell", @"C:\Users\William\Documents\Visual Studio 2015\Projects\WindowsAppTestarPowerShell\WindowsAppTestarPowerShell\WindowsAppTestarPowerShell.csproj"))
        }
        });

        _lstGroups.Add(new GroupViewModel(new GroupModel(4, "Outros"))
        {
          ProjectList = new ObservableCollection<ProjectViewModel>()
        {
          new ProjectViewModel(new ProjectModel(1, "Outro projeto1", "não sei o caminho1")),
          new ProjectViewModel(new ProjectModel(2, "Outro projeto2", "não sei o caminho2")),
          new ProjectViewModel(new ProjectModel(3, "Outro projeto3", "não sei o caminho3")),
          new ProjectViewModel(new ProjectModel(4, "Outro projeto4", "não sei o caminho4")),
          new ProjectViewModel(new ProjectModel(5, "Outro projeto5", "não sei o caminho5")),
          new ProjectViewModel(new ProjectModel(6, "Outro projeto6", "não sei o caminho6"))
        }
        });

        _lstGroups.Add(new GroupViewModel(new GroupModel(5, "Outros5")));
        _lstGroups.Add(new GroupViewModel(new GroupModel(6, "Outros6")));
        _lstGroups.Add(new GroupViewModel(new GroupModel(7, "Outros7")));
        _lstGroups.Add(new GroupViewModel(new GroupModel(8, "Outros8")));
        _lstGroups.Add(new GroupViewModel(new GroupModel(9, "Outros9")));
        _lstGroups.Add(new GroupViewModel(new GroupModel(10, "Outros10")));

        foreach (var item in _lstGroups)
        {
          item.SelectedProject = item.ProjectList.FirstOrDefault();
          item.PropertyChanged += (x, y) =>
          {
            if (y.PropertyName == "GroupId")
            {
              RaisePropertyChanged(nameof(GroupList));
            }
          };
        }
      }

      //if (_lstCustomCommands.Count == -1)
      //{
      //  _lstCustomCommands.Add(new CustomCommandViewModel(new CustomCommandModel(1, "SSC", "View.TfsSourceControlExplorer")));
      //  _lstCustomCommands.Add(new CustomCommandViewModel(new CustomCommandModel(2, "WS Parâmetros", "WSPack.Parametros")));
      //  RaisePropertyChanged(nameof(CustomCommandsList));
      //}
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="StartPageViewModel"/>
    /// </summary>
    /// <param name="startPageItem">Item de StartPage</param>
    public StartPageViewModel(StartPageModel startPageItem)
      : this()
    {
      _starPageModel = startPageItem;
    }
    #endregion

    /*
    /// <summary>
    /// Alterar um CustomCommand de lugar, movendo-o para cima ou para baixo
    /// </summary>
    /// <param name="customCommand">CustomCommand</param>
    /// <param name="movingDown">Informe "true" para mover o comando para baixo</param>
    void ReorderCustomCommand(CustomCommandViewModel customCommand, bool movingDown)
    {
      if (movingDown)
      {
        _lstCustomCommands[customCommand.CustomCommandId].CustomCommandId--;
        customCommand.CustomCommandId++;
      }

      else
      {
        _lstCustomCommands[customCommand.CustomCommandId - 2].CustomCommandId++;
        customCommand.CustomCommandId--;
      }

      RaisePropertyChanged(nameof(CustomCommandsList));
      customCommand.IsFocused = true;
      SelectedCustomCommand = customCommand;
    }*/

    /// <summary>
    /// Alterar um grupo de lugar, movendo-o para cima ou para baixo
    /// </summary>
    /// <param name="grupo">Grupo</param>
    /// <param name="movingDown">Informe "true" para mover o grupo para baixo</param>
    void ReorderGroup(GroupViewModel grupo, bool movingDown)
    {
      if (movingDown)
      {
        _lstGroups[grupo.GroupId].GroupId--;
        grupo.GroupId++;
      }

      else
      {
        _lstGroups[grupo.GroupId - 2].GroupId++;
        grupo.GroupId--;
      }

      RaisePropertyChanged(nameof(GroupList));
      grupo.IsFocused = true;
      SelectedGroup = grupo;
    }

    void RemoveGroupAndReorder(GroupViewModel groupToDelete)
    {
      int id = groupToDelete.GroupId;
      _lstGroups.Remove(groupToDelete);
      for (int i = id - 1; i < _lstGroups.Count; i++)
      {
        _lstGroups[i].GroupId = i + 1;
      }
      RaisePropertyChanged(nameof(GroupList));
      RaisePropertyChanged(nameof(HasGroups));

      if (_lstGroups.Any())
      {
        int indiceDesejado = id - 1;
        if (_lstGroups.Count == indiceDesejado)
          indiceDesejado--;

        SelectedGroup = _lstGroups[indiceDesejado];
      }
    }
    /*
    void RemoveCustomCommandAndReorder(CustomCommandViewModel customCommandToDelete)
    {
      int id = customCommandToDelete.CustomCommandId;
      _lstCustomCommands.Remove(customCommandToDelete);
      for (int i = id - 1; i < _lstCustomCommands.Count; i++)
      {
        _lstCustomCommands[i].CustomCommandId = i + 1;
      }
      RaisePropertyChanged(nameof(CustomCommandsList));

      if (_lstCustomCommands.Any())
      {
        int indiceDesejado = id - 1;
        if (_lstCustomCommands.Count == indiceDesejado)
          indiceDesejado--;

        SelectedCustomCommand = _lstCustomCommands[indiceDesejado];
        SelectedCustomCommand.IsFocused = true;
      }
    }*/

    private async Task RefreshDataContextAsync(string fileName)
    {
      var importado = await CreateOrLoadFromFileAsync(fileName);
      Instance = importado;
      WSPackStartPage.userControlDaStartPage._startPageViewModel = importado;
      WSPackStartPage.userControlDaStartPage.DataContext = importado;
      Save(WSPackFlexSupport.Instance.PackSupport.StartPageConfigPath);
    }

    /*
    void AddCustomCommandInList(string caption, string def, string args)
    {
      int id = _lstCustomCommands.Count + 1;
      _lstCustomCommands.Add(new CustomCommandViewModel(new CustomCommandModel(id, caption, def, args)) { Parent = this });
      RaisePropertyChanged(nameof(CustomCommandsList));
    }*/

    /// <summary>
    /// Recuperar a instãncia de WSStartPageViewModel
    /// </summary>
    public static StartPageViewModel Instance { get; private set; }

    /// <summary>
    /// Cria o modelo de visão da StartPage, seja via arquivo de configuração ou modelo em branco
    /// </summary>
    /// <param name="fileName">Nome do arquivo de configuração da StartPage</param>
    /// <param name="onError">Acontece em caso de erro</param>
    /// <returns>Instância de <see cref="StartPageViewModel" /></returns>
    public static async Task<StartPageViewModel> CreateOrLoadFromFileAsync(string fileName)
    {
      Task<StartPageViewModel> task = Task.Run(() =>
      {
        StartPageViewModel instance = null;

        // Arquivo existe. Vamos ler a configuração existente.
        if (File.Exists(fileName))
        {
          using (var reader = new StreamReader(fileName))
          {
            var serializer = new XmlSerializer(typeof(StartPageViewModel));
            //_deserializing = true;
            try
            {
              object obj = serializer.Deserialize(reader);
              instance = (StartPageViewModel)obj;
            }
            finally
            {
              //_deserializing = false;
            }

            //foreach (var esteCustomCommand in instance._lstCustomCommands)
            //{
            //  esteCustomCommand.Parent = instance;
            //}

            foreach (var esteGrupo in instance._lstGroups)
            {
              esteGrupo.Parent = instance;

              esteGrupo.PropertyChanged += (x, y) =>
              {
                if (y.PropertyName == nameof(esteGrupo.GroupId))
                {
                  instance.RaisePropertyChanged(nameof(GroupList));
                }
              };

              foreach (var esteProjeto in esteGrupo.ProjectList)
              {
                esteProjeto.Parent = esteGrupo;
              }
            }

          }
        }

        // Arquivo não existe. Vamos criar uma startpage vazia
        else
        {
          instance = new StartPageViewModel();
          using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
          {
            XmlSerializer serializer = new XmlSerializer(typeof(StartPageViewModel));
            serializer.Serialize(writer, instance);
          }
        }

        return instance;
      });

      return await task;
    }

    /// <summary>
    /// Salva o modelo no arquivo de configuração
    /// </summary>
    public void Save()
    {
      Save(WSPackFlexSupport.Instance.PackSupport.StartPageConfigPath);
    }

    /// <summary>
    /// Salva o modelo no arquivo de configuração
    /// </summary>
    /// <param name="fileName">Nome do arquivo</param>
    public void Save(string fileName)
    {
      if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        return;

      //if (_deserializing)
      //  return;

      try
      {
        using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
        {
          XmlSerializer serializer = new XmlSerializer(typeof(StartPageViewModel));
          serializer.Serialize(writer, Instance);
        }
      }
      catch (Exception ex)
      {
        WSPackFlexSupport.Instance.PackSupport.LogError($"Não foi possível salvar as configurações do arquivo da StartPage: {ex.Message}");
      }
    }

    /// <summary>
    /// Insica se um projeto está no TFS
    /// </summary>
    /// <param name="projectFullPath">Caminho local completo do projeto no Windows</param>
    /// <returns>true se está</returns>
    public bool IsProjectInTFS(string projectFullPath)
    {
      if (projectFullPath.IsNullOrWhiteSpaceEx())
        return false;

      if (WSPackFlexSupport.Instance.PackSupport.IsGit(projectFullPath))
        return false;

      string teamProjectName = WSPackFlexSupport.Instance.PackSupport.GetTFSActiveProjectName();
      if (string.IsNullOrWhiteSpace(teamProjectName))
        return false;

      List<TFSProjectModel> lstProjetos;
      if (!_dicTFSProjects.ContainsKey(teamProjectName))
      {
        lstProjetos = new List<TFSProjectModel>();
        _dicTFSProjects.Add(teamProjectName, lstProjetos);
      }
      else
        lstProjetos = _dicTFSProjects[teamProjectName];

      var achei = lstProjetos.FirstOrDefault(x => x.ProjectFullLocalPath.EqualsInsensitive(projectFullPath));
      if (achei == null)
      {
        achei = new TFSProjectModel
        {
          ProjectFullLocalPath = projectFullPath,
          TeamProjectUrl = teamProjectName
        };

        (bool OK, string WsName, string ServerItem) = WSPackFlexSupport.Instance.PackSupport.GetWorkspaceForLocalItem(projectFullPath);
        achei.IsInTFS = OK;
        lstProjetos.Add(achei);
      }
      return achei.IsInTFS;
    }

    /// <summary>
    /// Indica se a página está em modo de edição
    /// </summary>
    public bool ShowProjectsDirectory
    {
      get { return _starPageModel.ShowProjectsDirectory; }
      set
      {
        if (_starPageModel.ShowProjectsDirectory != value)
        {
          _starPageModel.ShowProjectsDirectory = value;
          RaisePropertyChanged(nameof(ShowProjectsDirectory));
          Save();
        }
      }
    }
  }
}
