using System.Xml.Serialization;

namespace WSPack.VisualStudio.Shared.DocumentationObjects
{
  /// <summary>
  /// Item base para regra de documentação
  /// </summary>
  [XmlInclude(typeof(TypeRuleItem))]
  [XmlInclude(typeof(MemberRuleItem))]
  public abstract class RuleBaseItem : DocumentationBaseItem
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="RuleBaseItem"/>
    /// </summary>
    public RuleBaseItem()
    {
      Id = 0;
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="RuleBaseItem" />
    /// </summary>
    /// <param name="id">Identificador. Será usado para ordenar e priorizar as regras</param>
    /// <param name="itemName">Nome da regra</param>
    public RuleBaseItem(int id, string itemName)
    {
      Id = id;
      ItemName = itemName;
      NameCondition = new ConditionItem();
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="RuleBaseItem"/>
    /// </summary>
    /// <param name="id">Identificador. Será usado para ordenar e priorizar as regras</param>
    /// <param name="itemName">Nome da regra</param>
    /// <param name="nameCondition">Condição para busca de nomes de Types/Members</param>
    public RuleBaseItem(int id, string itemName, ConditionItem nameCondition)
    {
      Id = id;
      ItemName = itemName;
      NameCondition = nameCondition;
    }
    #endregion

    /// <summary>
    /// Identificador. Será usado para ordenar e priorizar as regras
    /// </summary>
    [XmlAttribute]
    public int Id { get; set; }

    /// <summary>
    /// Condição para busca de nomes de Types/Members
    /// </summary>
    public ConditionItem NameCondition { get; set; }

    /// <summary>
    /// Retorna uma string representando esta instância
    /// </summary>
    /// <returns>string representando esta instância</returns>
    public override string ToString()
    {
      return $"{NameCondition}\r\n{Summary}";
    }
  }

}
