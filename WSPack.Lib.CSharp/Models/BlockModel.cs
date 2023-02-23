using System.Diagnostics;

namespace WSPack.Lib.CSharp.Models
{
  /// <summary>
  /// Definir um bloco (qualquer código entre { } )
  /// </summary>
  [DebuggerDisplay("{Name}")]
  public class BlockModel
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <seealso cref="BlockModel"/>
    /// </summary>
    /// <param name="name">Recuperar o Nome do bloco</param>
    /// <param name="start">Recuperar a localização inicial</param>
    /// <param name="end">Recuperar a localização final</param>
    public BlockModel(string name, LocationModel start, LocationModel end)
    {
      Name = name;
      Start = start;
      End = end;
    }
    #endregion

    /// <summary>
    /// Recuperar o Nome do bloco
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Recuperar a localização inicial
    /// </summary>
    public LocationModel Start { get; set; }

    /// <summary>
    /// Recuperar a localização final
    /// </summary>
    public LocationModel End { get; set; }

  }
}
