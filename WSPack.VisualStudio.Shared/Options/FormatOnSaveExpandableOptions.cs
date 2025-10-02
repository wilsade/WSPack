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

    /// Normalização de quebras de linhas
    /// </summary>
    [DisplayName("Normalização de quebras de linhas")]
    [Description(@"Ao salvar o documento, normalizar as quebras de linhas conforme a opção selecionada:
- CRLF: Carriage Return + Line Feed. Representação: \r\n (Windows)
- LF: Line Feed. Representação: \n (Linux e macOS)
- CR: Carriage Return. Representação: \r (macOS clássico, obsoleto)")]
    [DefaultValue(NormalizeLineEndingsOptions.Default)]
    [PropertyOrder(2)]
    [ReadOnly(false)]
    public NormalizeLineEndingsOptions NormalizeLineEndings { get; set; } = NormalizeLineEndingsOptions.Default;

    /// <summary>
    /// Retornar uma <see cref="string" /> que representa esta instância
    /// </summary>
    /// <returns>
    /// Uma <see cref="string" /> representando esta instância
    /// </returns>
    public override string ToString()
    {
      return $"{UseFormatOnSave}; {ValidExtensions}; {NormalizeLineEndings}";
    }
  }

  /// <summary>
  /// Definir tipos de Normalização de quebras de linha
  /// </summary>
  public enum NormalizeLineEndingsOptions
  {
    Default = 0,
    CRLF = 1,
    LF = 2,
    CR = 3
  }
}
