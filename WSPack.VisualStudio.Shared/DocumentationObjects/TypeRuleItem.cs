namespace WSPack.VisualStudio.Shared.DocumentationObjects
{
  /// <summary>
  /// Definir regras para Types
  /// </summary>
  public class TypeRuleItem : RuleBaseItem
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="TypeRuleItem"/>
    /// </summary>
    public TypeRuleItem()
    {

    }

    /// <summary>
    /// Cria uma instância da classe <see cref="TypeRuleItem"/>
    /// </summary>
    /// <param name="id">Identificador. Será usado para ordenar e priorizar as regras</param>
    /// <param name="ruleName">Nome da regra</param>
    public TypeRuleItem(int id, string ruleName)
      : base(id, ruleName)
    {
      TypeType = TypeTypesEnum.Classes;
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="TypeRuleItem"/>
    /// </summary>
    /// <param name="id">Identificador. Será usado para ordenar e priorizar as regras</param>
    /// <param name="ruleName">Nome da regra</param>
    /// <param name="nameCondition">Condição para busca de nomes de Types/Members</param>
    public TypeRuleItem(int id, string ruleName, ConditionItem nameCondition)
      : base(id, ruleName, nameCondition)
    {
      TypeType = TypeTypesEnum.Classes;
    }
    #endregion

    /// <summary>
    /// Definir o tipo de Type: Classe, Inteface, etc...
    /// </summary>
    public TypeTypesEnum TypeType { get; set; }

    /// <summary>
    /// Clonar este objeto
    /// </summary>
    /// <returns>objeto clonado</returns>
    public override DocumentationBaseItem Clone()
    {
      return new TypeRuleItem(Id, ItemName, NameCondition)
      {
        Remarks = Remarks,
        Returns = Returns,
        Summary = Summary,
        TypeType = TypeType,
      };
    }

    /// <summary>
    /// Definir um nome amigável para a classe
    /// </summary>
    public override string FriendlyName => "Regra de Tipos";
  }
}
