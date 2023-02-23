using System;

namespace WSPack.Lib.CSharp.Models
{
  /// <summary>
  /// Definir métricas do código C#
  /// </summary>
  public class MetricsModel //: INotifyPropertyChanged
  {
    //void DoPropertyChanged(string propName)
    //{
    //  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    //}

    /// <summary>
    /// Modelo com informações do Índice de manutenção
    /// </summary>
    public MaintainabilityModel Maintainability { get; set; }

    /// <summary>
    /// Complexidade ciclomática
    /// </summary>
    public CyclomaticComplexityModel CyclomaticComplexity { get; set; }

    /// <summary>
    /// Modelo com informações de linhas de código
    /// </summary>
    public LinesOfCodeModel LinesOfCode { get; set; }

    /// <summary>
    /// Número de overloads
    /// </summary>
    public int NumOfOverloads { get; set; }

    /// <summary>
    /// Número de parâmetros
    /// </summary>
    public int NumOfParameters { get; set; }

    /// <summary>
    /// Recuperar o número de variáveis locais
    /// </summary>
    public int NumOfLocalVariables { get; set; }

    /// <summary>
    /// Recuperar a complexidade cognitiva
    /// </summary>
    public CognitiveComplexityModel CognitiveComplexity { get; set; }

    /*
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
    */

    /// <summary>
    /// Retorna uma string representando esta instância
    /// </summary>
    /// <returns>string representando esta instância</returns>
    public override string ToString()
    {
      return $"Complexidade ciclomática: {CyclomaticComplexity.Value} ({CyclomaticComplexity.Level})" + Environment.NewLine +
        $"Complexidade cognitiva: {CognitiveComplexity.Value} ({CognitiveComplexity.Level})" + Environment.NewLine +
        $"Índice de manutenção: {Maintainability.Value} ({Maintainability.Level})" + Environment.NewLine +
        $"Linhas de código: {LinesOfCode.Value} ({LinesOfCode.Level})" + Environment.NewLine + Environment.NewLine +
        $"Número de sobrecargas: {NumOfOverloads}" + Environment.NewLine +
        $"Número de parâmetros: {NumOfParameters}" + Environment.NewLine +
        $"Número de variáveis: {NumOfLocalVariables}";
    }

  }
}
