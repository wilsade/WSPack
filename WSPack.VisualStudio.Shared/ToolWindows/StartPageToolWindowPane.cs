using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using mvs = Microsoft.VisualStudio.Shell;

using WSPack.Lib;
using WSPack.Lib.Properties;
using WSPack.Lib.WPF.Views;

namespace WSPack.VisualStudio.Shared.ToolWindows
{
  /// <summary>
  /// Disponibilizar a janela da página inicial
  /// </summary>
  [Guid(StartPageGuidString)]
  public class StartPageToolWindowPane : mvs.ToolWindowPane
  {
    /// <summary>
    /// StartPageGuidString
    /// </summary>
    public const string StartPageGuidString = "9A72FF2F-4FA5-42B4-A5BA-D83DC0A498A7";

    /// <summary>
    /// Inicialização da classe: <see cref="StartPageToolWindowPane" />.
    /// </summary>
    /// <param name="state">State</param>
    public StartPageToolWindowPane(object state)
          : base()
    {
      mvs.ThreadHelper.ThrowIfNotOnUIThread($"Construtor: {nameof(StartPageToolWindowPane)}");
      Caption = $"{Constantes.WSPackStartPageTitle} [{ResourcesLib.StrIniciando}]";
      var startPageControl = new WSPackStartPage();
      base.Content = startPageControl;

      _ = Task.Run(() =>
      {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
        startPageControl.LoadAsync().GetAwaiter().GetResult();
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
      })
        .ContinueWith(x =>
        {
          startPageControl.Configure();
          Caption = Constantes.WSPackStartPageTitle;
        }, TaskScheduler.FromCurrentSynchronizationContext());

    }
  }
}