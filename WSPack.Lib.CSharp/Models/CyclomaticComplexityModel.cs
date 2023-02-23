namespace WSPack.Lib.CSharp.Models
{
  /// <summary>
  /// Modelo com informações de complexidade ciclomática
  /// </summary>
  public class CyclomaticComplexityModel : BaseMetricModel<CyclomaticLevels>
  {
    /// <summary>
    /// Inicialização da classe: <see cref="CyclomaticComplexityModel"/>.
    /// </summary>
    /// <param name="value">Valor da complexidade cognitiva</param>
    public CyclomaticComplexityModel(double value)
      : base(value)
    {
    }

    /// <summary>
    /// Nível da complexidade conforme
    /// </summary>
    public override CyclomaticLevels Level
    {
      get
      {
        if (Value <= 10)
          return CyclomaticLevels.Simples;
        else if (Value <= 20)
          return CyclomaticLevels.Moderada;
        else if (Value <= 50)
          return CyclomaticLevels.Complexa;
        else
          return CyclomaticLevels.AltoRisco;
      }
    }
  }
}
