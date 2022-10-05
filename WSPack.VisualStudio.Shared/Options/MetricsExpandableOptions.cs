using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Opção para um adereço
  /// </summary>
  [TypeConverter(typeof(SortableExpandableObjectConverter))]
  public class MetricsExpandableOptions
  {
    int _minValueToShowMetrics;

    /// <summary>
    /// Habilitar: Usar métricas em métodos
    /// </summary>
    [Category(OptionsPageConsts.Metricas)]
    [DisplayName("Habilitar")]
    [Description("Habilitar o cálculo e exibição das métricas de cada método.")]
    [DefaultValue(true)]
    [PropertyOrder(0)]
    public bool UseMethodsMetrics { get; set; }

    /// <summary>
    /// Exibir métricas em Tempo de execução (Run-Time)
    /// </summary>
    [Category(OptionsPageConsts.Metricas)]
    [DisplayName("  Exibir em Tempo de execução (Run-Time)")]
    [Description("Habilite esta opção para que as métricas também sejam exibidas em Run-Time.")]
    [DefaultValue(false)]
    [ReadOnly(false)]
    [PropertyOrder(1)]
    public bool ShowDesignTimeMetrics { get; set; }

    /// <summary>
    /// Exibir para métodos Get/Set de propriedades
    /// </summary>
    [Category(OptionsPageConsts.Metricas)]
    [DisplayName("  Exibir para métodos Get/Set de propriedades")]
    [Description("Exibir para métodos Get/Set de propriedades.")]
    [DefaultValue(true)]
    [PropertyOrder(2)]
    [ReadOnly(false)]
    public bool ShowGetSetMetrics { get; set; }

    /// <summary>
    /// Sempre exibir para métodos de Teste unitário
    /// </summary>
    [Category(OptionsPageConsts.Metricas)]
    [DisplayName("  Sempore exibir para métodos de Teste unitário")]
    [Description("Sempre exibir para métodos de Teste unitário (métod que possuem o atributo [TestMethod].")]
    [DefaultValue(true)]
    [PropertyOrder(3)]
    [ReadOnly(false)]
    public bool AlwaysShowForUnitTest { get; set; }

    /// <summary>
    /// Exibir para métodos definidos em arquivos '.designer.cs'
    /// </summary>
    [Category(OptionsPageConsts.Metricas)]
    [DisplayName("  Exibir para métodos definidos em arquivos '.designer.cs'")]
    [Description("Habilite esta opção para que as métricas também sejam calculadas em arquivos terminados em '.Designer'.")]
    [DefaultValue(false)]
    [PropertyOrder(4)]
    [ReadOnly(false)]
    public bool ShowDesignerMetrics { get; set; }

    /// <summary>
    /// Exibir se a complexidade ciclomática for maior ou igual a
    /// </summary>
    [Category(OptionsPageConsts.Metricas)]
    [DisplayName("  Valor mínimo para exibição")]
    [Description(@"Valor mínimo para que a complexidade ciclomática seja exibida.

Os valores possuem a seguinte classificação:
De 1 a 10: Simples
De 11 a 20: Moderada
De 21 a 50: Complexa
51 ou mais: Alto risco

Complexidades maiores do que 10 sempre serão exibidas.")]
    [DefaultValue(1)]
    [PropertyOrder(5)]
    [ReadOnly(false)]
    public int MinValueToShowMetrics
    {
      get { return _minValueToShowMetrics; }
      set
      {
        if (value < 1)
          value = 1;
        else if (value > 10)
          value = 10;
        _minValueToShowMetrics = value;
      }
    }

    /// <summary>
    /// Retornar uma <see cref="string" /> que representa esta instância
    /// </summary>
    /// <returns>
    /// Uma <see cref="string" /> representando esta instância
    /// </returns>
    public override string ToString()
    {
      //return string.Empty;
      return $"{UseMethodsMetrics}; {ShowDesignTimeMetrics}; {ShowGetSetMetrics}; {AlwaysShowForUnitTest}; {ShowDesignerMetrics}; {MinValueToShowMetrics}";
    }
  }
}
