using System;
using System.Linq;
using System.Windows.Input;

using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;
using WSPack.Lib.WPF.SupportLib;

namespace WSPack.Lib.WPF.ViewModel
{
  partial class GroupViewModel
  {
    /// <summary>
    /// Adicionar uma pasta na lista de projetos do grupo
    /// </summary>
    public ICommand AddFolderCommand
    {
      get
      {
        void AddFolder(GroupViewModel groupViewModel)
        {
          var dlg = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog()
          {
            SelectedPath = groupViewModel.GroupDefaultPath + "\\"
          };
          if (dlg.ShowDialog() == true)
          {
            if (AddingProjectOrFolder(dlg.SelectedPath, out _))
              Parent?.Save();
          }
        }
        var comando = new RelayCommand<GroupViewModel>(AddFolder);
        return comando;
      }
    }

    /// <summary>
    /// Comando para adicionar um projeto
    /// </summary>
    public ICommand AddProjectCommand
    {
      get
      {
        void addProject(object ownerData)
        {
          var tupla = ChooseProjectDialog(PegarDiretorioPadrao());
          if (tupla.Ok)
          {
            if (AddingProjectOrFolder(tupla.FileName, out ProjectViewModel projeto))
            {
              if (ownerData != null)
              {
                Parent?.Save();
                string sender = Convert.ToString(ownerData);
                if (!string.IsNullOrEmpty(sender) || string.Equals(sender, nameof(AddProjectAndOpenCommand)))
                {
                  projeto.OpenProjectCommand.Execute(projeto);
                }
              }
            }
          }

        }

        var comando = new RelayCommand<object>(addProject);
        return comando;
      }
    }

    /// <summary>
    /// Comando para remover um projeto
    /// </summary>
    public ICommand RemoveProjectCommand
    {
      get
      {
        void remove(ProjectViewModel projeto)
        {
          if (WPFExtensions.ShowConfirmationYesNo(ResourcesLib.StrDesejaExcluirProjeto.FormatWith(projeto.ProjectFullPath)))
            RemoveProjectAndReorder(projeto);
        }

        bool canRemove(ProjectViewModel projeto)
        {
          return projeto != null && _lstProjetos.Any();
        }

        var comando = new RelayCommand<ProjectViewModel>(remove, x => canRemove(x));
        return comando;
      }
    }

    /// <summary>
    /// Alterar a ordem de um projeto movendo-o para cima
    /// </summary>
    public ICommand MoveProjectUpCommand
    {
      get
      {
        bool canMove(ProjectViewModel projeto)
        {
          return projeto != null && projeto.ProjectId > 1;
        }

        var comando = new RelayCommand<ProjectViewModel>(x => ReorderProject(x, false), x => canMove(x));
        return comando;
      }
    }

    /// <summary>
    /// Alterar a ordem de um projeto movendo-o para baixo
    /// </summary>
    public ICommand MoveProjectDownCommand
    {
      get
      {
        bool canMove(ProjectViewModel projeto)
        {
          return projeto != null && projeto.ProjectId < _lstProjetos.Count;
        }

        var comando = new RelayCommand<ProjectViewModel>(x => ReorderProject(x, true), x => canMove(x));
        return comando;
      }
    }

    /// <summary>
    /// Comando para abrir e já adicionar um projeto ao grupo
    /// </summary>
    public ICommand AddProjectAndOpenCommand
    {
      get
      {
        void execute()
        {
          AddProjectCommand.Execute(nameof(AddProjectAndOpenCommand));
        }

        var comando = new RelayCommand(execute);
        return comando;
      }
    }

    /// <summary>
    /// Comando apra ordenar os projetos por Nome
    /// </summary>
    public ICommand OrdenarPorNomeCommand
    {
      get
      {
        void Execute()
        {
          // ordenar
          var ordenado = _lstProjetos.OrderBy(x => x.ProjectCaption);
          int id = 1;
          foreach (var esteProjeto in ordenado)
          {
            esteProjeto.ProjectId = id++;
          }
          RaisePropertyChanged(nameof(HasProjects));
          RaisePropertyChanged(nameof(ProjectList));
          Parent?.Save();
        }

        bool PodeExecutar()
        {
          return HasProjects && _lstProjetos.Count > 1;
        }

        var comando = new RelayCommand(Execute, PodeExecutar);
        return comando;
      }
    }

    /// <summary>
    /// Comando para selecionar o diretório padrão
    /// </summary>
    public ICommand SelectDefaultPath
    {
      get
      {
        void Execute()
        {
          var openDlg = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
          string defaultPath = PegarDiretorioPadrao();
          openDlg.SelectedPath = defaultPath;
          if (openDlg.ShowDialog() == true)
          {
            GroupDefaultPath = openDlg.SelectedPath;
          }
        }

        var comando = new RelayCommand(Execute);
        return comando;
      }
    }
  }
}
