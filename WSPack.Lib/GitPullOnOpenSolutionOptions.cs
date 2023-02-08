namespace WSPack.Lib
{
  /// <summary>
  /// Definir tipos de opção para fazer Pull em uma solution do Git
  /// </summary>
  public enum GitPullOnOpenSolutionOptions
  {
    /// <summary>
    /// Nao
    /// </summary>
    Nao = 0,

    /// <summary>
    /// A cada hora
    /// </summary>
    ACadaHora = 1,

    /// <summary>
    /// A cada 2 horas
    /// </summary>
    ACada2Horas = 2,

    /// <summary>
    /// A cada 3 horas
    /// </summary>
    ACada3Horas = 3,

    /// <summary>
    /// A cada 4 horas
    /// </summary>
    ACada4Horas = 4,

    /// <summary>
    /// Uma vez ao dia
    /// </summary>
    UmaVezAoDia = 5,

    /// <summary>
    /// Sempre
    /// </summary>
    Sempre = 6
  }
}
