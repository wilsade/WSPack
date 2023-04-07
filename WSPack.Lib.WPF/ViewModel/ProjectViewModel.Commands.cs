using System.Windows.Input;

using WSPack.Lib.Properties;
using WSPack.Lib.WPF.SupportLib;

namespace WSPack.Lib.WPF.ViewModel
{
  partial class ProjectViewModel
  {
    /// <summary>
    /// Comando para abrir um projeto
    /// </summary>
    public ICommand OpenProjectCommand
    {
      get
      {
        void onExecute(ProjectViewModel project)
        {
          InternoAbrirProjeto(project);
        }

        bool canExecute(ProjectViewModel projeto)
        {
          return true;
        }

        var comando = new RelayCommand<ProjectViewModel>(onExecute, x => canExecute(x));
        return comando;
      }
    }

    /// <summary>
    /// Comando para abrir o diretório do projeto
    /// </summary>
    public ICommand OpenDirectoryCommand
    {
      get
      {
        void onOpenDirectory(ProjectViewModel projeto)
        {
          if (projeto != null)
            if (projeto.IsDirectory)
              WSPackFlexSupport.Instance.PackSupport.LocateInWindows(projeto.ProjectFullPath);
            else
              WSPackFlexSupport.Instance.PackSupport.LocateInWindows(projeto.ProjectDirectory);
        }

        var comando = new RelayCommand<ProjectViewModel>(onOpenDirectory);
        return comando;
      }
    }

    /// <summary>
    /// Comando para escolher o caminho do projeto
    /// </summary>
    public ICommand ChooseProjectCommand
    {
      get
      {
        void chooseProject(ProjectViewModel projectViewModel)
        {
          var tupla = GroupViewModel.ChooseProjectDialog(projectViewModel.ProjectDirectory);
          if (tupla.Ok)
          {
            ProjectFullPath = tupla.FileName;
            ProjectCaption = System.IO.Path.GetFileNameWithoutExtension(tupla.FileName);
          }
        }

        var comando = new RelayCommand<ProjectViewModel>(chooseProject);
        return comando;
      }
    }

    /// <summary>
    /// Comando para escolher o caminho da pasta
    /// </summary>
    public ICommand ChooseFolderCommand
    {
      get
      {
        void chooseFolder(ProjectViewModel projectViewModel)
        {
          var dlg = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog()
          {
            SelectedPath = projectViewModel.ProjectFullPath + "\\"
          };
          if (dlg.ShowDialog() == true)
          {
            ProjectFullPath = dlg.SelectedPath;
            ProjectCaption = System.IO.Path.GetFileNameWithoutExtension(dlg.SelectedPath);
          }
        }

        var comando = new RelayCommand<ProjectViewModel>(chooseFolder);
        return comando;
      }
    }

    /// <summary>
    /// Comando para localizar o projeto no TFS
    /// </summary>
    public ICommand LocateInTFSCommand
    {
      get
      {
        void locateInTFS(ProjectViewModel projeto)
        {
          _ = Locate_OR_Get(projeto, OperationTFSTypes.Locate);
        }

        bool canLocate(ProjectViewModel projeto)
        {
          return true;
        }

        var comando = new RelayCommand<ProjectViewModel>(locateInTFS, x => canLocate(x));
        return comando;
      }
    }

    /// <summary>
    /// Efetuar um GET Lastest Version e abrir o projeto
    /// </summary>
    public ICommand GetLastestVersionAndOpenCommand
    {
      get
      {
        void getAndOpen(ProjectViewModel projeto)
        {
          bool ok = Locate_OR_Get(projeto, OperationTFSTypes.GetLastestVersion);
          OpenProjectCommand.Execute(projeto);
          if (!ok)
            MessageBoxUtils.ShowWarning(ResourcesLib.StrOperacaoGETNaoRealizada);
          //Src.MessageBoxShell.ShowWarningOK(Properties.Resources.StrOperacaoGETNaoRealizada);
        }

        bool canGetAndOpen(ProjectViewModel projeto)
        {
          return true;
        }

        var comando = new RelayCommand<ProjectViewModel>(getAndOpen, x => canGetAndOpen(x));
        return comando;
      }
    }

    /// <summary>
    /// Efetuar um GET Specific Version e abrir o projeto
    /// </summary>
    public ICommand GetSpecificVersionAndOpenCommand
    {
      get
      {
        void getAndOpen(ProjectViewModel projeto)
        {
          bool ok = Locate_OR_Get(projeto, OperationTFSTypes.GetSpecificVersion);
          OpenProjectCommand.Execute(projeto);
          if (!ok)
            MessageBoxUtils.ShowWarning(ResourcesLib.StrOperacaoGETNaoRealizada);
        }

        bool canGetAndOpen(ProjectViewModel projeto)
        {
          return true;
        }

        var comando = new RelayCommand<ProjectViewModel>(getAndOpen, x => canGetAndOpen(x));
        return comando;
      }
    }
  }
}
