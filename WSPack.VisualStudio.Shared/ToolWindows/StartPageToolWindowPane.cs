using Microsoft.VisualStudio.Shell;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using WSPack.Lib;
using WSPack.Lib.Properties;

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
      //var startPageControl = new WSPackStartPage();
      //base.Content = startPageControl;

      base.Content = new Label
      {
        Content = "Em desenvolvimento"
      };

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