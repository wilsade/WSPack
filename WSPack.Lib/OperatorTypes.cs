namespace WSPack.Lib
{
  /// <summary>
  /// Definir operadores
  /// </summary>
  public enum OperatorTypes
  {
    /// <summary>
    /// Igual
    /// </summary>
    Equals = 0,

    /// <summary>
    /// Diferente
    /// </summary>
    NotEquals = 1,

    /// <summary>
    /// Começa com
    /// </summary>
    StartsWith = 2,

    /// <summary>
    /// Contém
    /// </summary>
    Like = 3,

    /// <summary>
    /// Não contém
    /// </summary>
    NotLike = 4,

    /// <summary>
    /// É nulo
    /// </summary>
    IsNull = 5,

    /// <summary>
    /// Não é nulo
    /// </summary>
    IsNotNull = 6
  }
}
