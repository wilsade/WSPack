using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using WSPack.Lib.WPF.ViewModel;

namespace WSPack.Lib.WPF.Views
{
  /// <summary>
  /// Interaction logic for StartPageEditWindow.xaml
  /// </summary>
  public partial class StartPageEditWindow : Window
  {
    public StartPageEditWindow()
    {
      InitializeComponent();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.RightCtrl)
        (DataContext as StartPageViewModel).IsExpanded = false;
    }

    private void Window_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.RightCtrl)
        (DataContext as StartPageViewModel).IsExpanded = true;
    }
  }
}
