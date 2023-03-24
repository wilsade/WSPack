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
    GroupViewModel _selectedGroup;
    ObservableCollection<GroupViewModel> _lstGroups;
    string _infoText;

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

    /// <summary>
    /// Recuperar a instãncia de WSStartPageViewModel
    /// </summary>
    internal static StartPageViewModel Instance { get; private set; }

    /// <summary>
    /// Cria o modelo de visão da StartPage, seja via arquivo de configuração ou modelo em branco
    /// </summary>
    /// <param name="fileName">Nome do arquivo de configuração da StartPage</param>
    /// <param name="onError">Acontece em caso de erro</param>
    /// <returns>Instância de <see cref="StartPageViewModel" /></returns>
    public static async Task<StartPageViewModel> CreateOrLoadFromFileAsync()
    {
      Task<StartPageViewModel> task = Task.Run(() =>
      {
        StartPageViewModel instance = null;

        // Arquivo existe. Vamos ler a configuração existente.
        if (File.Exists(WSPackFlexSupport.Instance.PackSupport.StartPageConfigPath))
        {
          using (var reader = new StreamReader(WSPackFlexSupport.Instance.PackSupport.StartPageConfigPath))
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

              //foreach (var esteProjeto in esteGrupo.ProjectList)
              //{
              //  esteProjeto.Parent = esteGrupo;
              //}
            }

          }
        }

        // Arquivo não existe. Vamos criar uma startpage vazia
        else
        {
          instance = new StartPageViewModel();
          using (var writer = new StreamWriter(WSPackFlexSupport.Instance.PackSupport.StartPageConfigPath, false, Encoding.UTF8))
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
      if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        return;

      //if (_deserializing)
      //  return;

      try
      {
        using (var writer = new StreamWriter(WSPackFlexSupport.Instance.PackSupport.StartPageConfigPath, false, Encoding.UTF8))
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

      string projectNameFromTFS = WSPackFlexSupport.Instance.PackSupport.GetTFSActiveProjectName();

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

  }
}
