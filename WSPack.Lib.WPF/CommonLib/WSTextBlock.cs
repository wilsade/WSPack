using System.Windows.Controls;

namespace WSPack.Lib.WPF.CommonLib
{
  /// <summary>
  /// TextBlock com estivo
  /// </summary>
  /// <seealso cref="TextBlock" />
  public class WSTextBlock : TextBlock
  {
    /// <summary>
    /// Cria uma instância da classe <see cref="WSTextBlock"/>
    /// </summary>
    public WSTextBlock()
      : base()
    {
      if (WSPackFlexSupport.Instance?.VSStyler == null)
        return;

      SetResourceReference(Control.ForegroundProperty,
        WSPackFlexSupport.Instance.VSStyler.GetGroupHeaderForegroundKey());
      SetResourceReference(StyleProperty,
        WSPackFlexSupport.Instance.VSStyler.GetGroupHeaderStyleKey());
    }
  }

}
