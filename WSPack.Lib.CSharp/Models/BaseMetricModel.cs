using System;

namespace WSPack.Lib.CSharp.Models
{
  /// <summary>
  /// Modelo básico de métrica
  /// </summary>
  public abstract class BaseMetricModel<TEnum> where TEnum : Enum
  {
    #region Construtor
    /// <summary>
    /// Inicialização da classe: <seealso cref="BaseMetricModel{TEnum}"/>.
    /// </summary>
    /// <param name="value">Valor da complexidade cognitiva</param>
    protected BaseMetricModel(double value)
    {
      Value = value;
    }
    #endregion

    /// <summary>
    /// Valor da complexidade cognitiva
    /// </summary>
    public double Value { get; set; }

    /// <summary>
    /// Nível da complexidade
    /// </summary>
    public abstract TEnum Level { get; }
  }
}
