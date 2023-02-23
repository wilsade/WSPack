namespace WSPack.Lib.CSharp.Models
{
  /// <summary>
  /// Definir uma localização básica
  /// </summary>
  public class LocationModel
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="LocationModel"/>
    /// </summary>
    /// <param name="line">Índice (zero based) da linha.</param>
    /// <param name="column">índice (zero based) da coluna.</param>
    public LocationModel(int line, int column)
    {
      Line = line;
      Column = column;
    }
    #endregion

    /// <summary>
    /// Recuperar o índice (zero based) da linha
    /// </summary>
    public int Line { get; private set; }

    /// <summary>
    /// Recuperar o índice (zero based) da coluna
    /// </summary>
    public int Column { get; private set; }

    /// <summary>
    /// Retorna uma string representando esta instância
    /// </summary>
    /// <returns>string representando esta instância</returns>
    public override string ToString()
    {
      return string.Format("Location:{0},{1}", Line, Column);
    }
  }
}
