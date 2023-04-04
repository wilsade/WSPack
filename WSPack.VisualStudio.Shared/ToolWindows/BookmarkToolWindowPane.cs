using Microsoft.VisualStudio.Shell;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Runtime.InteropServices;
using WSPack.Lib.Properties;
using WSPack.Lib.WPF.Views;

namespace WSPack.VisualStudio.Shared.ToolWindows
{
  /// <summary>
  /// Disponibilizar janela de marcadores
  /// </summary>
  [Guid(WindowGuidString)]
  public class BookmarkToolWindowPane : ToolWindowPane
  {
    /// <summary>
    /// WindowGuidString
    /// </summary>
    public const string WindowGuidString = "823E9387-7D3A-4DBC-84B2-F517FB9B17B0";

    /// <summary>
    /// Inicialização da classe: <see cref="BookmarkToolWindowPane"/>.
    /// </summary>
    public BookmarkToolWindowPane(object state)
          : base(null)
    {
      Caption = ResourcesLib.StrMarcadores;
      var bmUc = new BookmarkWindowControl();
      ThreadHelper.ThrowIfNotOnUIThread($"Construtor: {nameof(BookmarkToolWindowPane)}");
      bmUc.Configure();
      base.Content = bmUc;
    }
  }
}