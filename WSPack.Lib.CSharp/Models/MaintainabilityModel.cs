namespace WSPack.Lib.CSharp.Models
{
  /// <summary>
  /// Modelo com informações do índice de manutenção
  /// </summary>
  public class MaintainabilityModel : BaseMetricModel<MetricsLevels>
  {
    /// <summary>
    /// Inicialização da classe: <see cref="MaintainabilityModel"/>.
    /// </summary>
    /// <param name="value">Valor da complexidade cognitiva</param>
    public MaintainabilityModel(double value) : base(value)
    {
    }

    /// <summary>
    /// Nível da complexidade
    /// </summary>
    public override MetricsLevels Level
    {
      get
      {
        if (Value < 10)
          return MetricsLevels.Ruim;
        else if (Value <= 20)
          return MetricsLevels.Moderada;
        else
          return MetricsLevels.Bom;
      }
    }
  }
}
