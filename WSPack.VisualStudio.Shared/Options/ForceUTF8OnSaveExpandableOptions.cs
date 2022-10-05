using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Opções para forçar codificação UTF8 ao salvar
  /// </summary>
  [TypeConverter(typeof(SortableExpandableObjectConverter))]
  public class ForceUTF8OnSaveExpandableOptions
  {
    /// <summary>
    /// Habilitar
    /// </summary>
    [Category(OptionsPageConsts.Editor)]
    [DisplayName("Habilitar")]
    [Description("Habilitar a conversão para UTF-8 ao salvar o documento.")]
    [DefaultValue(false)]
    [PropertyOrder(0)]
    [ReadOnly(false)]
    public bool ForceUTF8OnSave { get; set; } = true;

    /// <summary>
    /// Extensões válidas para formatação
    /// </summary>
    [DisplayName("Extensões válidas")]
    [Description(@"Forçar codificação UTF-8 apenas em documentos nestas extensões. 
Separe as extensões desejadas por ; (ponto-e-vírgula).
Ex: cs; xml")]
    [DefaultValue("")]
    [PropertyOrder(1)]
    [ReadOnly(false)]
    public string ValidExtensions { get; set; } = "cs";

    /// <summary>
    /// Retornar uma <see cref="string" /> que representa esta instância
    /// </summary>
    /// <returns>
    /// Uma <see cref="string" /> representando esta instância
    /// </returns>
    public override string ToString()
    {
      return $"{ForceUTF8OnSave}; {ValidExtensions}";
    }

  }
}
