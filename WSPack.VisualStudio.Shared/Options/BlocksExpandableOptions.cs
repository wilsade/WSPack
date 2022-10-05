using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Opções para adereço de final de bloco
  /// </summary>
  [TypeConverter(typeof(SortableExpandableObjectConverter))]
  public class BlocksExpandableOptions
  {
    private int _showOnlyIfLineNumberGreaterOrEqual;

    /// <summary>
    /// Habilitar
    /// </summary>
    [PropertyOrder(0)]
    [DefaultValue(true)]
    [DisplayName("Habilitar")]
    [Description(@"Habilitar a exibição de um identificador no final de cada bloco. 
O identificador será composto por um símbolo '<<' seguido do nome do bloco.")]
    public bool UseBlocks { get; set; }

    /// <summary>
    /// Exibir apenas se o início do bloco não estiver visível (recomendado)
    /// </summary>
    [PropertyOrder(1)]
    [ReadOnly(false)]
    [DefaultValue(true)]
    [DisplayName("  Exibir apenas se o início do bloco não estiver visível")]
    [Description(@"Marque, para que seja exibido apenas quando o início do bloco não estiver visível (recomendado). 
Desmarque para que seja sempre exibido.")]
    public bool ShowOnlyIfStartIsNotVisivel { get; set; }

    /// <summary>
    /// Habilitar navegação
    /// </summary>
    [PropertyOrder(2)]
    [ReadOnly(false)]
    [DefaultValue(true)]
    [DisplayName("  Habilitar navegação")]
    [Description(@"Habilitar navegação para o início do block através do Click do Mouse. 
O 'Click' irá posicionar o Cursor no início do bloco.")]
    public bool EnableNavigation { get; set; }

    /// <summary>
    /// Destacar o identificador ao passar o Mouse por cima.
    /// </summary>
    [PropertyOrder(3)]
    [ReadOnly(false)]
    [DefaultValue(true)]
    [DisplayName("  Destacar ao movimentar o mouse")]
    [Description(@"Destacar o identificador ao passar o Mouse por cima.")]
    public bool MouseHoverEffect { get; set; }

    /// <summary>
    /// Número mínimo de linhas para exibição de blocos
    /// </summary>
    [PropertyOrder(4)]
    [ReadOnly(false)]
    [DefaultValue(5)]
    [DisplayName("  Exibir se o total de linhas do bloco for maior ou igual a")]
    [Description(@"Exibir o identificador apenas se o total de linhas do bloco for maior do que um determinado valor.")]
    public int ShowOnlyIfLineNumberGreaterOrEqual
    {
      get => _showOnlyIfLineNumberGreaterOrEqual;
      set
      {
        if (value < 1)
          value = 1;
        _showOnlyIfLineNumberGreaterOrEqual = value;
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
      return $"{UseBlocks}; {ShowOnlyIfStartIsNotVisivel}; {EnableNavigation}; {MouseHoverEffect}; {ShowOnlyIfLineNumberGreaterOrEqual}";
    }
  }
}
