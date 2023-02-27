using System.Diagnostics;
using System.Xml.Serialization;

namespace WSPack.Lib.DocumentationObjects
{
  /// <summary>
  /// Item base de documentação
  /// </summary>
  [DebuggerDisplay("{ItemName}")]
  public abstract class DocumentationBaseItem
  {
    /// <summary>
    /// Cria uma instância da classe <see cref="DocumentationBaseItem"/>
    /// </summary>
    public DocumentationBaseItem()
    {
      ItemName = Summary = Returns = Remarks = string.Empty;
    }

    /// <summary>
    /// Nome do item
    /// </summary>
    [XmlAttribute]
    public string ItemName { get; set; }

    /// <summary>
    /// Summary que será incluído no item
    /// </summary>
    public string Summary { get; set; }

    /// <summary>
    /// Estrutura de Returns/Value
    /// </summary>
    public string Returns { get; set; }

    /// <summary>
    /// Estrutura de Remarks
    /// </summary>
    public string Remarks { get; set; }

    /// <summary>
    /// Retorna um código hash para esta instância
    /// </summary>
    /// <returns>
    /// Um código hash para esta instância, adequado para uso em algoritmos hash e estrutura de dados como uma tabela hash
    /// </returns>
    public override int GetHashCode()
    {
      return ItemName.GetHashCode();
    }

    /// <summary>
    /// Determina se o <see cref="object" /> especificado é igual ao desta instância
    /// </summary>
    /// <param name="obj">Objeto a ser comparado com o desta instância</param>
    /// <returns>
    /// true se o objeto especificado é igual ao desta instância
    /// </returns>
    public override bool Equals(object obj)
    {
      if (obj is DocumentationBaseItem item)
        return item.ItemName.Equals(ItemName);
      return false;
    }

    /// <summary>
    /// Clonar este objeto
    /// </summary>
    /// <returns>objeto clonado</returns>
    public abstract DocumentationBaseItem Clone();

    /// <summary>
    /// Definir um nome amigável para a classe
    /// </summary>
    public abstract string FriendlyName { get; }
  }

}
