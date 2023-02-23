namespace WSPack.Lib.CSharp.Models
{
  /// <summary>
  /// Definir tipos genéricos de níveis de métrica
  /// </summary>
  public enum MetricsLevels
  {
    /// <summary>
    /// Bom
    /// </summary>
    Bom = 1,

    /// <summary>
    /// Moderado
    /// </summary>
    Moderada = 2,

    /// <summary>
    /// Ruim
    /// </summary>
    Ruim = 3
  }

  /// <summary>
  /// Definir tipos de nível da complexidade ciclomática
  /// </summary>
  public enum CyclomaticLevels
  {
    /// <summary>
    /// Simples
    /// </summary>
    Simples = 1,

    /// <summary>
    /// Moderada
    /// </summary>
    Moderada = 2,

    /// <summary>
    /// Complexa
    /// </summary>
    Complexa = 3,

    /// <summary>
    /// AltoRisco
    /// </summary>
    AltoRisco = 4
  }
}
