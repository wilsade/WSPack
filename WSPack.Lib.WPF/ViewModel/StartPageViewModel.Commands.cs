using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

using Microsoft.Win32;

using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;
using WSPack.Lib.Items;
using WSPack.Lib.Properties;
using WSPack.Lib.WPF.Model;
using WSPack.Lib.WPF.SupportLib;

using WSPack.Lib.WPF.Views;

namespace WSPack.Lib.WPF.ViewModel
{
  partial class StartPageViewModel
  {
    /// <summary>
    /// Comando para editar a StartPage
    /// </summary>
    public ICommand EditCommand
    {
      get
      {
        void edit(object param)
        {
          var window = new StartPageEditWindow()
          {
            Owner = Application.Current.MainWindow,
            DataContext = this,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
          };

          if (param is GroupViewModel group)
          {
            SelectedGroup = group;
            group.IsFocused = true;
          }

          else if (param is ProjectViewModel projeto)
          {
            SelectedGroup = projeto.Parent;
            projeto.Parent.SelectedProject = projeto;
            projeto.IsFocused = true;
          }

          /*else if (param is CustomCommandViewModel customCommand)
          {
            SelectedCustomCommand = customCommand;
            customCommand.IsFocused = true;
          }

          if (SelectedCustomCommand == null)
          {
            if (_lstCustomCommands.Any())
            {
              SelectedCustomCommand = _lstCustomCommands.First();
              SelectedCustomCommand.IsFocused = true;
            }
          }*/

          if (SelectedGroup == null && _lstGroups.Any())
          {
            SelectedGroup = GroupList?.FirstOrDefault();
            SelectedGroup.IsFocused = true;
          }
          else if (SelectedGroup != null)
          {
            SelectedGroup.IsFocused = true;
            if (SelectedGroup.HasProjects && SelectedGroup.SelectedProject == null)
            {
              SelectedGroup.SelectedProject = SelectedGroup.ProjectList.First();
              SelectedGroup.SelectedProject.IsFocused = true;
            }
          }

          window.ShowDialog();
          Save();
        }
        var comando = new RelayCommand<object>(edit);
        return comando;
      }
    }

    /// <summary>
    /// Comando para adicionar um novo grupo
    /// </summary>
    public ICommand AddGroupCommand
    {
      get
      {
        void addGroup(object c)
        {
          if (MessageBoxUtils.InputBox("Criação de grupo", "Informe o nome do grupo:", out string response))
          {
            GroupViewModel grupo = new GroupViewModel(new GroupModel(_lstGroups.Count + 1, response));
            if (_lstGroups.Any(x => x.GroupCaption.EqualsInsensitive(grupo.GroupCaption)))
            {
              MessageBoxUtils.ShowInformation(string.Format(ResourcesLib.StrGrupoExistente, grupo.GroupCaption));
            }

            else
            {
              grupo.Parent = this;
              grupo.PropertyChanged += (x, y) =>
              {
                if (y.PropertyName == nameof(grupo.GroupId))
                {
                  RaisePropertyChanged(nameof(GroupList));
                }
              };
              _lstGroups.Add(grupo);
              grupo.IsFocused = true;
              SelectedGroup = grupo;
              RaisePropertyChanged(nameof(HasGroups));
              //c?.ScrollIntoView();
            }
          }

        }

        var comando = new RelayCommand<object>(addGroup);
        return comando;
      }
    }

    /// <summary>
    /// Comando para mover um grupo para cima
    /// </summary>
    public ICommand MoveGroupUpCommand
    {
      get
      {
        bool canMove(GroupViewModel grupo)
        {
          return grupo != null && grupo.GroupId > 1;
        }

        var comando = new RelayCommand<GroupViewModel>(x => ReorderGroup(x, false), x => canMove(x));
        return comando;
      }
    }

    /// <summary>
    /// Comando para mover um grupo para baixo
    /// </summary>
    public ICommand MoveGroupDownCommand
    {
      get
      {
        bool canMove(GroupViewModel grupo)
        {
          return grupo != null && grupo.GroupId < _lstGroups.Count;
        }

        var comando = new RelayCommand<GroupViewModel>(x => ReorderGroup(x, true), x => canMove(x));
        return comando;
      }
    }

    /// <summary>
    /// Remover um grupo da lista de grupos
    /// </summary>
    public ICommand RemoveGroupCommand
    {
      get
      {
        void remove(GroupViewModel grupo)
        {
          string aux = grupo.ProjectList.Any() ? " e todos os seus projetos" : "";
          if (WPFExtensions.ShowConfirmationYesNo($"Deseja excluir o grupo {grupo.GroupCaption}{aux}?"))
            RemoveGroupAndReorder(grupo);
        }

        bool canRemove(GroupViewModel grupo)
        {
          return grupo != null && _lstGroups.Any();
        }

        var comando = new RelayCommand<GroupViewModel>(remove, x => canRemove(x));
        return comando;
      }
    }

    /// <summary>
    /// Comando para exportar a configuração
    /// </summary>
    public ICommand ExportCommand
    {
      get
      {
        void export()
        {
          var dlg = new SaveFileDialog()
          {
            DefaultExt = "*.cfg",
            Filter = "StartPage|*.cfg",
            Title = "Informe o nome do arquivo"
          };

          if (dlg.ShowDialog() == true)
          {
            Save(dlg.FileName);
          }
        }

        bool canExport() => File.Exists(WSPackFlexSupport.Instance.PackSupport.StartPageConfigPath) && _lstGroups.Any();

        var comando = new RelayCommand(export, canExport);
        return comando;
      }
    }

    /// <summary>
    /// Comando para importar a configuração
    /// </summary>
    public ICommand ImportCommand
    {
      get
      {
        void Import()
        {
          var dlg = new OpenFileDialog()
          {
            DefaultExt = "*.cfg",
            Filter = "StartPage|*.cfg",
            Title = "Selecione o arquivo de importação"
          };

          if (dlg.ShowDialog() == true)
          {
            _ = RefreshDataContextAsync(dlg.FileName);
          }
        }

        var comando = new RelayCommand(Import);
        return comando;
      }
    }

    /// <summary>
    /// Atualizar o modelo conforme arquivo XML
    /// </summary>
    public ICommand UpdateModelCommand
    {
      get
      {
        void updateModel()
        {
          _ = RefreshDataContextAsync(WSPackFlexSupport.Instance.PackSupport.StartPageConfigPath);
        }

        bool canUpdate()
        {
          return File.Exists(WSPackFlexSupport.Instance.PackSupport.StartPageConfigPath);
        }

        var comando = new RelayCommand(updateModel, canUpdate);
        return comando;
      }
    }

    /// <summary>
    /// Comando para exibir ou não os diretórios de cada projeto
    /// </summary>
    public ICommand ShowProjectsDirectoryCommand
    {
      get
      {
        void execute()
        {
          ShowProjectsDirectory = !ShowProjectsDirectory;
        }

        var comando = new RelayCommand(execute);
        return comando;
      }
    }

    /// <summary>
    /// Comando para trocar a visibilidade da barra de rolagem horizontal dos projetos
    /// </summary>
    public ICommand ProjectHorizontalScrollCommand
    {
      get
      {
        void Execute()
        {
          ProjectHorizontalScrollVisible = !ProjectHorizontalScrollVisible;
        }

        var comando = new RelayCommand(Execute);
        return comando;
      }
    }

    /// <summary>
    /// Comando para buscar uma Solution / Projeto previamente cadastrado
    /// </summary>
    public ICommand OpenProjectSolutionCommand
    {
      get
      {
        void Execute()
        {
          using (var form = new LookupListBaseForm()
          {
            Caption = ResourcesLib.StrAbrirSolutionProjetoPaginaInicial,
            Label = ResourcesLib.StrLocalizarSolutionProjetoASerAberto
          })
          {
            var projetos = GroupList.SelectMany(x => x.ProjectList)
              .Where(x => File.Exists(x.ProjectFullPath))
              .Select(p => new LookupGridItem(p.ProjectCaption, p.ProjectFullPath) { OwnerData = p })
              .OrderBy(x => x.Nome).ToList();
            form.Bind(projetos);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
              var projeto = (ProjectViewModel)form.ItemSelecionado.OwnerData;
              projeto.OpenProjectCommand.Execute(projeto);
            }
          }
        }

        bool CanExecute()
        {
          return HasGroups && GroupList.SelectMany(x => x.ProjectList)
            .Any(x => File.Exists(x.ProjectFullPath));
        }

        var comando = new RelayCommand(Execute, CanExecute);
        return comando;
      }
    }

    /*
    /// <summary>
    /// Comando para adicionar um comando personalizado
    /// </summary>
    public ICommand AddCustomCommand
    {
      get
      {
        void Execute()
        {
          var janela = new CustomCommandWindow();
          if (janela.ShowDialog() == true)
            AddCustomCommandInList(janela.CommandName, janela.CommandDef, janela.CommandArguments); ;
        }

        var comando = new RelayCommand(Execute);
        return comando;
      }
    }

    /// <summary>
    /// Remover um comando personalizado
    /// </summary>
    public ICommand RemoveCustomCommand
    {
      get
      {
        void Execute(CustomCommandViewModel customCommand)
        {
          if (MessageBoxUtils.ShowWarningYesNo(string.Format(ResourcesLib.StrDesejaExcluirEsteComando, customCommand.CustomCommandCaption)))
          {
            RemoveCustomCommandAndReorder(customCommand);
            Save();
          }
        }

        bool CanExecute(CustomCommandViewModel customCommand)
        {
          return customCommand != null && _lstCustomCommands.Any();
        }

        var comando = new RelayCommand<CustomCommandViewModel>(Execute, x => CanExecute(x));
        return comando;
      }
    }

    /// <summary>
    /// Alterar a ordem de um CustomCommand movendo-o para cima
    /// </summary>
    public ICommand MoveCustomCommandUpCommand
    {
      get
      {
        bool canMove(CustomCommandViewModel customCommand)
        {
          return customCommand != null && customCommand.CustomCommandId > 1;
        }

        var comando = new RelayCommand<CustomCommandViewModel>(x => ReorderCustomCommand(x, false), x => canMove(x));
        return comando;
      }
    }

    /// <summary>
    /// Comando para mover um CustomCommand para baixo
    /// </summary>
    public ICommand MoveCustomCommandDownCommand
    {
      get
      {
        bool canMove(CustomCommandViewModel customCommand)
        {
          return customCommand != null && customCommand.CustomCommandId < _lstCustomCommands.Count;
        }

        var comando = new RelayCommand<CustomCommandViewModel>(x => ReorderCustomCommand(x, true), x => canMove(x));
        return comando;
      }
    }

    /// <summary>
    /// Adicionar os comandos personalizados padrões
    /// </summary>
    public ICommand AddDefaultCustomCommands
    {
      get
      {
        void Execute()
        {
          foreach (var esteComandoDefault in _lstComandosDefaults)
          {
            if (!CustomCommandsList.Any(x => x.CustomCommandDef.Equals(esteComandoDefault.CommandDef)))
            {
              AddCustomCommandInList(esteComandoDefault.Caption, esteComandoDefault.CommandDef, "");
            }
          }
        }

        bool CanExecute()
        {
          return _lstComandosDefaults.Any(d => !CustomCommandsList.Any(c => c.CustomCommandDef.Equals(d.CommandDef)));
        }

        var comando = new RelayCommand(Execute, CanExecute);
        return comando;
      }
    }*/
  }
}
