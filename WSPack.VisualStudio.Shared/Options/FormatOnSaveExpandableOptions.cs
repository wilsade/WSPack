using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Opções para o comando FormatOnSave
  /// </summary>
  [TypeConverter(typeof(SortableExpandableObjectConverter))]
  public class FormatOnSaveExpandableOptions
  {
    /// <summary>
    /// Habilitar
    /// </summary>
    [Category(OptionsPageConsts.Editor)]
    [DisplayName("Habilitar")]
    [Description("Habilitar a formatação do documento ao salvar.")]
    [DefaultValue(false)]
    [PropertyOrder(0)]
    [ReadOnly(false)]
    public bool UseFormatOnSave { get; set; }

    /// <summary>
    /// Extensões válidas para formatação
    /// </summary>
    [DisplayName("Extensões válidas")]
    [Description(@"Habilitar a formatação do documento apenas em documentos nestas extensões. 
Separe as extensões desejadas por ; (ponto-e-vírgula).
Ex: cs; xml")]
    [DefaultValue("")]
    [PropertyOrder(1)]
    [ReadOnly(false)]
    public string ValidExtensions { get; set; }

    /// <summary>
    /// Retornar uma <see cref="string" /> que representa esta instância
    /// </summary>
    /// <returns>
    /// Uma <see cref="string" /> representando esta instância
    /// </returns>
    public override string ToString()
    {
      return $"{UseFormatOnSave}; {ValidExtensions}";
    }
  }
}
