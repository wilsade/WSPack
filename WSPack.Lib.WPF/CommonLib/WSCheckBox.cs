using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WSPack.Lib.WPF.CommonLib
{
  /// <summary>
  /// CheckBox com estilo
  /// </summary>
  /// <seealso cref="CheckBox" />
  public class WSCheckBox : CheckBox
  {
    /// <summary>
    /// Cria uma instância da classe <see cref="WSCheckBox"/>
    /// </summary>
    public WSCheckBox()
      : base()
    {
      if (WSPackFlexSupport.Instance?.VSStyler == null)
        return;

      SetResourceReference(ForegroundProperty, 
        WSPackFlexSupport.Instance.VSStyler.ForegroundEx);
    }
  }
}
