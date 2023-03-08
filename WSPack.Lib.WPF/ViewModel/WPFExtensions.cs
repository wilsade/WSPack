using System.Windows;
using System.Windows.Controls;

namespace WSPack.Lib.WPF.ViewModel
{
  /// <summary>
  /// Métodos estendidos para WPF
  /// </summary>
  public static class WPFExtensions
  {
    /// <summary>
    /// Scrolls the item into view.
    /// </summary>
    /// <param name="control">Control</param>
    /// <param name="item">Item</param>
    public static void ScrollIntoView(this ItemsControl control, object item)
    {
      if (!(control.ItemContainerGenerator.ContainerFromItem(item) is FrameworkElement framework))
        return;
      framework.BringIntoView();
    }

    /// <summary>
    /// Scrolls the last item into view.
    /// </summary>
    /// <param name="control">Control</param>
    public static void ScrollIntoView(this ItemsControl control)
    {
      int count = control.Items.Count;
      if (count == 0)
        return;
      object item = control.Items[count - 1];
      control.ScrollIntoView(item);
    }

    /// <summary>
    /// Scrolls the last item into view.
    /// </summary>
    /// <param name="control">Control</param>
    public static void ScrollIntoView(this ListBox control)
    {
      (control as ItemsControl).ScrollIntoView();
    }

    /// <summary>
    /// Converter booleando em Visibilidade
    /// </summary>
    /// <param name="value">Valor</param>
    /// <returns>booleando em Visibilidade</returns>
    public static Visibility ToVisibility(this bool value)
    {
      if (value)
        return Visibility.Visible;
      return Visibility.Collapsed;
    }
  }
}
