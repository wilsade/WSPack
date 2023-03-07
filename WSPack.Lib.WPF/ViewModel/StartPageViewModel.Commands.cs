using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
          var window = new StartPageEditWindow();
          if (window.ShowDialog() ?? true)
          {
            MessageBoxUtils.ShowWarning("Atualizar");
          }
        }
        var comando = new RelayCommand(edit);
        return comando;
      }
    }
  }
}
