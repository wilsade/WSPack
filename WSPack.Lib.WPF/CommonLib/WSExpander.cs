using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WSPack.Lib.WPF.CommonLib
{
  /// <summary>
  /// Expander com estilo
  /// </summary>
  /// <seealso cref="Expander" />
  public class WSExpander : Expander
  {
    /// <summary>
    /// Cria uma instância da classe <see cref="WSExpander"/>
    /// </summary>
    public WSExpander()
      : base()
    {
      if (WSPackFlexSupport.Instance?.VSStyler == null)
        return;

      SetResourceReference(BackgroundProperty, WSPackFlexSupport.Instance.VSStyler
        .ScrollBarBackgroundBrushKeyEx);
    }
  }
}
