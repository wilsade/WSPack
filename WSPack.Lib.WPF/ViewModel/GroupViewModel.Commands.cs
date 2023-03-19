using System.Windows.Input;

using WSPack.Lib.WPF.SupportLib;

namespace WSPack.Lib.WPF.ViewModel
{
  partial class GroupViewModel
  {
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
