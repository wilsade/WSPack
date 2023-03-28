using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using WSPack.Lib.WPF.ViewModel;

namespace WSPack.Lib.WPF.Views
{
  /// <summary>
  /// Interaction logic for WSPackStartPage.xaml
  /// </summary>
  public partial class WSPackStartPage : UserControl
  {
    internal StartPageViewModel _startPageViewModel;
    internal static WSPackStartPage userControlDaStartPage;

    /// <summary>
    /// Inicialização da classe: <see cref="WSPackStartPage"/>.
    /// </summary>
    public WSPackStartPage()
    {
      InitializeComponent();
      if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
      {
        _startPageViewModel = new StartPageViewModel();
        DataContext = _startPageViewModel;
        userControlDaStartPage = this;
      }
    }

    /// <summary>
    /// Carregar a página inicial
    /// </summary>
    /// <returns>Task</returns>
    public async Task LoadAsync()
    {
      _startPageViewModel = await StartPageViewModel.CreateOrLoadFromFileAsync(
        WSPackFlexSupport.Instance.PackSupport.StartPageConfigPath);
    }

    /// <summary>
    /// Configurar
    /// </summary>
    public void Configure()
    {
      DataContext = _startPageViewModel;
      _startPageViewModel.InfoText = "Clique com o botão direito para criar Grupos e Projetos";
      userControlDaStartPage = this;
    }
  }
}
