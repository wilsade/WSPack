using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.Lib.WPF
{
  /// <summary>
  /// Prover suporte à funcionalidades de classes do WSPack
  /// </summary>
  public class WSPackFlexSupport
  {
    static WSPackFlexSupport _instance;

    /// <summary>
    /// Inicialização da classe: <see cref="WSPackFlexSupport"/>.
    /// </summary>
    private WSPackFlexSupport()
    {

    }

    public static void Initialize(IVisualSutioStylerController stylerController,
      IWSPackSupport packSupport)
    {
      _instance = new WSPackFlexSupport()
      {
        VSStyler = stylerController,
        PackSupport = packSupport
      };

      XamlSupport.BackgroundEx = stylerController.BackgroundEx;
      XamlSupport.ForegroundEx = stylerController.ForegroundEx;
      XamlSupport.ToolWindowBackgroundBrushKeyEx = stylerController.ToolWindowBackgroundBrushKeyEx;
      XamlSupport.ToolWindowTextBrushKeyEx = stylerController.ToolWindowTextBrushKeyEx;
      XamlSupport.StartPageTextControlLinkSelectedKeyEx = stylerController.StartPageTextControlLinkSelectedKeyEx;
      XamlSupport.LinkStyleKeyEx = stylerController.LinkStyleKeyEx;
      XamlSupport.LinkHoverStyleKeyEx = stylerController.LinkHoverStyleKeyEx;
      XamlSupport.DirectoryLinkStyleKeyEx = stylerController.DirectoryLinkStyleKeyEx;
      XamlSupport.ScrollBarBackgroundBrushKey = stylerController.ScrollBarBackgroundBrushKeyEx;
    }

    /// <summary>
    /// Devolve a instância da classe: <see cref="WSPackFlexSupport"/>
    /// </summary>
    /// <value>Instância da classe: <see cref="WSPackFlexSupport"/></value>
    internal static WSPackFlexSupport Instance => _instance ?? (_instance = new WSPackFlexSupport());

    /// <summary>
    /// Configurador de estilos para componentes do Visual Studio
    /// </summary>
    public IVisualSutioStylerController VSStyler { get; set; }

    /// <summary>
    /// Suporte a funcionalidades dependentes de classes do WSPack
    /// </summary>
    public IWSPackSupport PackSupport { get; set; }
  }

  public class XamlSupport
  {
    public static object BackgroundEx { get; set; } = new object();
    public static object ForegroundEx { get; set; } = new object();
    public static object ToolWindowBackgroundBrushKeyEx { get; set; } = new object();
    public static object ToolWindowTextBrushKeyEx { get; set; } = new object();
    public static object StartPageTextControlLinkSelectedKeyEx { get; set; } = new object();
    public static object LinkStyleKeyEx { get; set; } = new object();
    public static object LinkHoverStyleKeyEx { get; set; } = new object();
    public static object DirectoryLinkStyleKeyEx { get; set; } = new object();
    public static object ScrollBarBackgroundBrushKey { get; set; } = new object();
  }
}
