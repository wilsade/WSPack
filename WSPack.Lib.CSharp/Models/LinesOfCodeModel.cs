namespace WSPack.Lib.CSharp.Models
{
  /// <summary>
  /// Modelo com informações de linhas de código
  /// </summary>
  public class LinesOfCodeModel : BaseMetricModel<MetricsLevels>
  {
    /// <summary>
    /// Inicialização da classe: <see cref="LinesOfCodeModel"/>.
    /// </summary>
    /// <param name="value">Valor da complexidade cognitiva</param>
    public LinesOfCodeModel(double value) : base(value)
    {
    }

    /// <summary>
    /// Nível da complexidade
    /// </summary>
    public override MetricsLevels Level
    {
      get
      {
        if (Value <= 10)
          return MetricsLevels.Bom;
        else if (Value <= 20)
          return MetricsLevels.Moderada;
        else
          return MetricsLevels.Ruim;
      }
    }
  }
}
