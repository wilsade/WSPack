namespace WSPack.Lib.DocumentationObjects
{
  /// <summary>
  /// Definir uma regra para membros
  /// </summary>
  public class MemberRuleItem : RuleBaseItem
  {
    #region Construtores
    /// <summary>
    /// Cria uma instância da classe <see cref="MemberRuleItem"/>
    /// </summary>
    public MemberRuleItem()
    {
      MemberType = MemberTypesEnum.All;
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="MemberRuleItem"/>
    /// </summary>
    /// <param name="id">Identificador. Será usado para ordenar e priorizar as regras</param>
    /// <param name="itemName">Nome da regra</param>
    public MemberRuleItem(int id, string itemName)
      : base(id, itemName)
    {
      MemberType = MemberTypesEnum.All;
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="MemberRuleItem"/>
    /// </summary>
    /// <param name="id">Identificador. Será usado para ordenar e priorizar as regras</param>
    /// <param name="itemName">Nome da regra</param>
    /// <param name="nameCondition">Condição para busca de nomes de Types/Members</param>
    public MemberRuleItem(int id, string itemName, ConditionItem nameCondition)
      : base(id, itemName, nameCondition)
    {
      MemberType = MemberTypesEnum.All;
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="MemberRuleItem" />
    /// </summary>
    /// <param name="id">Identificador. Será usado para ordenar e priorizar as regras</param>
    /// <param name="itemName">Nome da regra</param>
    /// <param name="nameCondition">Condição para busca de nomes de Types/Members</param>
    /// <param name="memberType">Tipo de membro onde a regra é válida</param>
    public MemberRuleItem(int id, string itemName, ConditionItem nameCondition, MemberTypesEnum memberType)
      : base(id, itemName, nameCondition)
    {
      MemberType = memberType;
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="MemberRuleItem" />
    /// </summary>
    /// <param name="id">Identificador. Será usado para ordenar e priorizar as regras</param>
    /// <param name="itemName">Nome da regra</param>
    /// <param name="nameCondition">Condição para busca de nomes de Types/Members</param>
    /// <param name="typeCondition">Definir condição para o tipo do membro. Ex: se o membro for do tipo 'string'</param>
    /// <param name="memberType">Tipo de membro onde a regra é válida</param>
    /// <param name="summary">Summary</param>
    public MemberRuleItem(int id, string itemName, ConditionItem nameCondition, ConditionItem typeCondition,
      MemberTypesEnum memberType, string summary)
      : base(id, itemName, nameCondition)
    {
      MemberType = memberType;
      TypeCondition = typeCondition;
      Summary = summary;
    }
    #endregion

    /// <summary>
    /// Tipo de membro onde a regra é válida
    /// </summary>
    public MemberTypesEnum MemberType { get; set; }

    /// <summary>
    /// Definir condição para o tipo do membro. Ex: se o membro for do tipo 'string'
    /// </summary>
    public ConditionItem TypeCondition { get; set; }

    /// <summary>
    /// Retorna uma string representando esta instância
    /// </summary>
    /// <returns>string representando esta instância</returns>
    public override string ToString()
    {
      return $"{NameCondition}\r\n{TypeCondition}\r\n{Summary}";
    }

    /// <summary>
    /// Clonar este objeto
    /// </summary>
    /// <returns>objeto clonado</returns>
    public override DocumentationBaseItem Clone()
    {
      return new MemberRuleItem(Id, ItemName, NameCondition, TypeCondition, MemberType, Summary)
      {
        Remarks = Remarks,
        Returns = Returns
      };
    }

    /// <summary>
    /// Definir um nome amigável para a classe
    /// </summary>
    public override string FriendlyName => "Regra de membros";
  }

}
