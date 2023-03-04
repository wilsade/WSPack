using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using WSPack.Lib.WPF.Model;

namespace WSPack.Lib.WPF.ViewModel
{
  /// <summary>
  /// Modelo da visão da StartPage
  /// </summary>
  [XmlRoot("StartPage")]
  public class StartPageViewModel : BaseViewModel
  {
    readonly StartPageModel _starPageModel;

    /// <summary>
    /// Inicialização da classe: <see cref="StartPageViewModel"/>.
    /// </summary>
    public StartPageViewModel()
    {
      _starPageModel = new StartPageModel();
      Instance = this;
    }

    /// <summary>
    /// Recuperar a instãncia de WSStartPageViewModel
    /// </summary>
    internal static StartPageViewModel Instance { get; private set; }
  }
}
