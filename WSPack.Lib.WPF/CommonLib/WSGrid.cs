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
      if (WSPackFlexSupport.Instance?.VSStyler == null)
        return;
      SetResourceReference(BackgroundProperty,
        WSPackFlexSupport.Instance.VSStyler.ToolWindowBackgroundBrushKeyEx);
    }
  }
}
