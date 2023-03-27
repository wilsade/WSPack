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

    /// <summary>
    /// Tool window background brush key
    /// </summary>
    object ToolWindowBackgroundBrushKeyEx { get; }

    /// <summary>
    /// Recuperar o background
    /// </summary>
    /// <returns>Background</returns>
    object BackgroundEx { get; }

    /// <summary>
    /// Recuperar o foreground
    /// </summary>
    /// <returns>Foreground</returns>
    object ForegroundEx { get; }

    /// <summary>
    /// Tool window text brush key
    /// </summary>
    object ToolWindowTextBrushKeyEx { get; }

    /// <summary>
    /// Recuperar o start page text control link selected key
    /// </summary>
    /// <returns>Start page text control link selected key</returns>
    object StartPageTextControlLinkSelectedKeyEx { get; }

    /// <summary>
    /// Link style key ex
    /// </summary>
    object LinkStyleKeyEx { get; }

    /// <summary>
    /// Link hover style key ex
    /// </summary>
    object LinkHoverStyleKeyEx { get; }
  }
}
