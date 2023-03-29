using System.Windows.Controls;

namespace WSPack.Lib.WPF.CommonLib
{
  /// <summary>
  /// TextBox com estilo e cores
  /// </summary>
  public class WSTextBox : TextBox
  {
    /// <summary>
    /// Cria uma instância da classe <see cref="WSTextBox"/>
    /// </summary>
    public WSTextBox()
      : base()
    {
      if (WSPackFlexSupport.Instance?.VSStyler == null)
        return;

      SetResourceReference(ForegroundProperty, 
        WSPackFlexSupport.Instance.VSStyler.ForegroundEx);
      SetResourceReference(BackgroundProperty, 
        WSPackFlexSupport.Instance.VSStyler.BackgroundEx);
      SetResourceReference(CaretBrushProperty, 
        WSPackFlexSupport.Instance.VSStyler.StartPageTextControlLinkSelectedKeyEx);
    }
  }
}
