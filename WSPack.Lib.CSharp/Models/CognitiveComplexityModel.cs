namespace WSPack.Lib.CSharp.Models
{
  /// <summary>
  /// Modelo com informações de complexidade cognitiva
  /// </summary>
  public class CognitiveComplexityModel : BaseMetricModel<MetricsLevels>
  {
    readonly bool _isProperty;

    /// <summary>
    /// Inicialização da classe: <see cref="CognitiveComplexityModel" />.
    /// </summary>
    /// <param name="value">Valor da complexidade cognitiva</param>
    /// <param name="isProperty">Indica se a métrica é de uma propriedade</param>
    public CognitiveComplexityModel(double value, bool isProperty)
      : base(value)
    {
      Value = value;
      _isProperty = isProperty;
    }

    /// <summary>
    /// Nível da complexidade conforme <seealso cref="MetricsLevels"/>
    /// </summary>
    public override MetricsLevels Level
    {
      get
      {
        if (_isProperty)
        {
          if (Value <= 2)
            return MetricsLevels.Bom;
          else if (Value == 3)
            return MetricsLevels.Moderada;
          else
            return MetricsLevels.Ruim;
        }
        else
        {
          if (Value <= 10)
            return MetricsLevels.Bom;
          else if (Value <= 15)
            return MetricsLevels.Moderada;
          else
            return MetricsLevels.Ruim;
        }
      }
    }
  }
}
