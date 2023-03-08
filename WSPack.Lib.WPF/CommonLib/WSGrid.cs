using System.Windows.Controls;

namespace WSPack.Lib.WPF.CommonLib
{
  /// <summary>
  /// Grid com estilo
  /// </summary>
  /// <seealso cref="Grid" />
  public class WSGrid : Grid
  {
    /// <summary>
    /// Cria uma instância da classe <see cref="WSGrid"/>
    /// </summary>
    public WSGrid() : base()
    {
      if (VisualSutioFlexStylerController.Instance?.Controller == null)
        return;
      SetResourceReference(BackgroundProperty,
        VisualSutioFlexStylerController.Instance.Controller.GetToolWindowBackgroundBrushKey());
    }
  }
}
