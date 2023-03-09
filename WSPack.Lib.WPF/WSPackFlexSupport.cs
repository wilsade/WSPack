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
}
