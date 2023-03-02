using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Shell;

using WSPack.Lib;
using WSPack.Lib.Properties;
using WSPack.Lib.WPF.Views;

namespace WSPack.VisualStudio.Shared.ToolWindows
{
  /// <summary>
  /// Disponibilizar a janela da página inicial
  /// </summary>
  [Guid(StartPageGuidString)]
  public class StartPageToolWindowPane : ToolWindowPane
  {
    /// <summary>
    /// StartPageGuidString
    /// </summary>
    public const string StartPageGuidString = "9A72FF2F-4FA5-42B4-A5BA-D83DC0A498A7";

    /// <summary>
    /// Inicialização da classe: <see cref="BookmarkToolWindowPane" />.
    /// </summary>
    /// <param name="state">State</param>
    public StartPageToolWindowPane(object state)
          : base()
    {
      ThreadHelper.ThrowIfNotOnUIThread($"Construtor: {nameof(StartPageToolWindowPane)}");
      Caption = $"{Constantes.WSPackStartPageTitle} [{ResourcesLib.StrIniciando}]";
      var startPageControl = new WSPackStartPage();
      base.Content = startPageControl;

      _ = System.Threading.Tasks.Task.Run(() =>
      {
        //_ = startPageControl.LoadAsync();
      })
        .ContinueWith(x =>
        {
          //startPageControl.Configure();
          Caption = Constantes.WSPackStartPageTitle;
        }, TaskScheduler.FromCurrentSynchronizationContext());

    }
  }
}