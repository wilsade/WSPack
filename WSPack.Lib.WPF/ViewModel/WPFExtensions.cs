using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

using WSPack.Lib.Properties;

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

    /// <summary>
    /// Mostrar mensagem de aviso
    /// </summary>
    /// <param name="msg">Mensagem a ser exibida</param>
    /// <param name="title">Título do diálogo</param>
    /// <param name="defaultButton">Botão default</param>
    /// <returns>true se a resposta foi 'Yes'</returns>
    public static bool ShowWarningYesNo(string msg, string title = "Aviso",
      MessageBoxResult defaultButton = MessageBoxResult.No)
    {
      return MessageBox.Show(msg, title,
        MessageBoxButton.YesNo,
        MessageBoxImage.Exclamation,
        defaultButton) == MessageBoxResult.Yes;
    }

    /// <summary>
    /// Exibir uma mensagem de confirmação Sim/Não com ícone Question
    /// </summary>
    /// <param name="msg">Mensagem a ser exibida</param>
    /// <param name="defaultButton">Botão default</param>
    /// <returns>true se o usuário escolheu: Sim</returns>
    public static bool ShowConfirmationYesNo(string msg, MessageBoxResult defaultButton = MessageBoxResult.No)
    {
      return MessageBox.Show(msg,
        ResourcesLib.StrConfirmacao, MessageBoxButton.YesNo, MessageBoxImage.Question, defaultButton) == MessageBoxResult.Yes;
    }
  }
}
