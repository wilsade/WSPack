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
      if (VisualSutioFlexStylerController.Instance?.Controller == null)
        return;

      SetResourceReference(Control.ForegroundProperty,
        VisualSutioFlexStylerController.Instance.Controller.GetGroupHeaderForegroundKey());
      SetResourceReference(StyleProperty,
        VisualSutioFlexStylerController.Instance.Controller.GetGroupHeaderStyleKey());
    }
  }
}
