using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;
using WSPack.Lib.WPF.Model;

namespace WSPack.Lib.WPF.Views
{
  /// <summary>
  /// Interaction logic for BookmarkWindowControl.xaml
  /// </summary>
  public partial class BookmarkWindowControl : UserControl
  {
    /// <summary>
    /// Inicialização da classe: <see cref="BookmarkWindowControl"/>.
    /// </summary>
    public BookmarkWindowControl()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Configurar
    /// </summary>
    public void Configure()
    {
      lstMarcadores.ItemsSource = WSPackFlexSupport.Instance.PackSupport.GetBookMarkGetBindingList();
      WSPackFlexSupport.Instance.PackSupport.BookmarkBindingChanged += Instance_BindingChanged;
      WSPackFlexSupport.Instance.PackSupport.BookmarksChanged += Instance_BookmarksChanged;
    }

    void Instance_BookmarksChanged(object sender, EventArgs e)
    {
      btnLimparBookmark.IsEnabled = lstMarcadores.Items.Count > 0;
    }

    void Instance_BindingChanged(object sender, EventArgs e)
    {
      lstMarcadores.ItemsSource = WSPackFlexSupport.Instance.PackSupport.GetBookMarkGetBindingList();
      btnLimparBookmark.IsEnabled = lstMarcadores.Items.Count > 0;
    }

    void itemKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete)
        btnRemoveBookmark_Click(sender, e);
      else if (e.Key == Key.F2)
        RenomearMarcador(lstMarcadores.SelectedItems[0] as Bookmark);
    }

    private void RenomearMarcador(Bookmark bookmark)
    {
      string oldValue = bookmark.Name;
      if (MessageBoxUtils.InputBox(ResourcesLib.StrRenomearMarcador,
        ResourcesLib.StrInformeNomeMarcador.FormatWith(bookmark.Number),
        out string response, oldValue))
      {
        if (response != oldValue)
        {
          bookmark.Name = response;
          gridMarcadores.AutoSizeColumns();
        }
      }
    }

    void itemDoubleClick(object sender, MouseButtonEventArgs e)
    {
      WSPackFlexSupport.Instance.PackSupport.GotoBookmark(lstMarcadores.SelectedItem as Bookmark);
    }

    private void btnGotoBookmark_Click(object sender, RoutedEventArgs e)
    {
      WSPackFlexSupport.Instance.PackSupport.GotoBookmark(lstMarcadores.SelectedItem as Bookmark);
    }

    private void btnRemoveBookmark_Click(object sender, RoutedEventArgs e)
    {
      WSPackFlexSupport.Instance.PackSupport.RemoveBookmark(lstMarcadores.SelectedItem as Bookmark);
    }

    private void lstMarcadores_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      btnRemoveBookmark.IsEnabled = btnGotoBookmark.IsEnabled = btnRenameBookmark.IsEnabled =
        lstMarcadores.SelectedIndex != -1;
    }

    private void lstMarcadores_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
      e.Handled = lstMarcadores.Items.Count == 0 ||
        lstMarcadores.SelectedItems.Count != 1;
    }

    private void menuItemRenomeaMarcador_Click(object sender, RoutedEventArgs e)
    {
      RenomearMarcador(lstMarcadores.SelectedItems[0] as Bookmark);
    }

    private void btnRenameBookmark_Click(object sender, RoutedEventArgs e)
    {
      RenomearMarcador(lstMarcadores.SelectedItems[0] as Bookmark);
    }

    private void btnLimparBookmark_Click(object sender, RoutedEventArgs e)
    {
      WSPackFlexSupport.Instance.PackSupport.ClearAllBookmarks();
    }

    private void ToolBar_Loaded(object sender, RoutedEventArgs e)
    {
      ToolBar toolBar = sender as ToolBar;
      if (toolBar.Template.FindName("OverflowGrid", toolBar) is FrameworkElement overflowGrid)
      {
        overflowGrid.Visibility = Visibility.Collapsed;
      }

      if (toolBar.Template.FindName("MainPanelBorder", toolBar) is FrameworkElement mainPanelBorder)
      {
        mainPanelBorder.Margin = new Thickness(0);
      }
    }
  }
}
