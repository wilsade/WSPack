using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.Lib.WPF
{
  /// <summary>
  /// Controlar os estilos do Visual Studio
  /// </summary>
  public interface IVisualSutioStylerController
  {
    /// <summary>
    /// Recuperar o group header foreground key
    /// </summary>
    /// <returns>Group header foreground key</returns>
    object GetGroupHeaderForegroundKey();

    /// <summary>
    /// Recuperar o group header style key
    /// </summary>
    /// <returns>Group header style key</returns>
    object GetGroupHeaderStyleKey();
  }
}
