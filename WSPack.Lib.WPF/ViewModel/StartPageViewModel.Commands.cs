﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using WSPack.Lib.Extensions;
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
        void edit()
        {
          var window = new StartPageEditWindow()
          {
            DataContext = this
          };
          window.ShowDialog();
          Save();
        }
        var comando = new RelayCommand(edit);
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
  }
}
