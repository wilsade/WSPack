using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;

using WSPack.Lib.WPF;

namespace WSPack.VisualStudio.Shared.WPFs
{
  /// <summary>
  /// Wrapper para cores e estilos do Visual Studio
  /// </summary>
  public class VisualSutioStylerController : IVisualSutioStylerController
  {
    #region Construtor
    /// <summary>
    /// Inicializa a classe <see cref="VisualSutioStylerController"/>
    /// </summary>
    static VisualSutioStylerController()
    {
      Background = VsBrushes.StartPageTabBackgroundKey;
      ToolWindowBackgroundBrushKey = VsBrushes.StartPageTabBackgroundKey;
      Foreground = VsBrushes.ToolWindowTextKey;
      ToolWindowTextBrushKey = VsBrushes.ToolWindowTextKey;
      StartPageTextControlLinkSelectedKey = VsBrushes.StartPageTextControlLinkSelectedKey;
      DirectoryLinkStyleKey = VsBrushes.StartPageTextHeadingKey;
      GroupHeaderStyleKey = VsResourceKeys.TextBlockEnvironment200PercentFontSizeStyleKey;
      GroupHeaderForegroundKey = StartPageColors.StartPageTitleTextBrushKey;
      ScrollBarBackgroundBrushKey = VsBrushes.ScrollBarBackgroundKey;
      StartPageButtonBorderKey = VsBrushes.ButtonShadowKey;
      LinkStyleKey = VsBrushes.StartPageTextControlLinkSelectedKey;
      LinkHoverStyleKey = VsBrushes.StartPageTextControlLinkSelectedHoverKey;
    }
    #endregion

    /// <summary>
    /// Background
    /// </summary>
    public static object Background { get; set; }

    /// <summary>
    /// Foreground
    /// </summary>
    public static object Foreground { get; set; }

    /// <summary>
    /// Tool window background brush key
    /// </summary>
    public static object ToolWindowBackgroundBrushKey { get; set; }

    /// <summary>
    /// Tool window text brush key
    /// </summary>
    public static object ToolWindowTextBrushKey { get; set; }

    /// <summary>
    /// Start page text control link selected key
    /// </summary>
    public static object StartPageTextControlLinkSelectedKey { get; set; }

    /// <summary>
    /// Directory link style key
    /// </summary>
    public static object DirectoryLinkStyleKey { get; set; }

    /// <summary>
    /// Group header style key
    /// </summary>
    public static object GroupHeaderStyleKey { get; set; }

    /// <summary>
    /// Group header foreground key
    /// </summary>
    public static object GroupHeaderForegroundKey { get; set; }

    /// <summary>
    /// ScrollBarBackgroundBrushKey
    /// </summary>
    public static object ScrollBarBackgroundBrushKey { get; set; }

    /// <summary>
    /// StartPageButtonBorderKey
    /// </summary>
    public static object StartPageButtonBorderKey { get; set; }

    /// <summary>
    /// Link style key
    /// </summary>
    public static object LinkStyleKey { get; set; }

    /// <summary>
    /// Link hover style key
    /// </summary>
    public static object LinkHoverStyleKey { get; set; }

    object IVisualSutioStylerController.GetGroupHeaderForegroundKey() => GroupHeaderForegroundKey;
    object IVisualSutioStylerController.GetGroupHeaderStyleKey() => GroupHeaderStyleKey;
    object IVisualSutioStylerController.GetToolWindowBackgroundBrushKey() => ToolWindowBackgroundBrushKey;
  }
}