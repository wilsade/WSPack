namespace WSPack.Lib.WPF.CommonLib
{
  /// <summary>
  /// TextBox que aceita somente números
  /// </summary>
  public class WSNumberTextBox : WSTextBox
  {
    /// <summary>
    /// Cria uma instância da classe <see cref="WSNumberTextBox"/>
    /// </summary>
    public WSNumberTextBox()
      : base()
    {
      PreviewTextInput += WSNumberTextBox_PreviewTextInput;
    }

    private void WSNumberTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
      var regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
      e.Handled = regex.IsMatch(e.Text);
    }
  }
}
