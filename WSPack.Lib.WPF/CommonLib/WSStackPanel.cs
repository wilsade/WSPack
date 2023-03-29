using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WSPack.Lib.WPF.CommonLib
{
  /// <summary>
  /// StackPanel com estilo
  /// </summary>
  /// <seealso cref="System.Windows.Controls.StackPanel" />
  public class WSStackPanel : StackPanel
  {
    /// <summary>
    /// Cria uma instância da classe <see cref="WSStackPanel"/>
    /// </summary>
    public WSStackPanel()
      : base()
    {
      if (WSPackFlexSupport.Instance?.VSStyler == null)
        return;

      SetResourceReference(BackgroundProperty, WSPackFlexSupport.Instance?.VSStyler
        .ScrollBarBackgroundBrushKeyEx);
    }
  }
}
