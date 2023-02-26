namespace WSPack.VisualStudio.Shared.DocumentationObjects
{
  /// <summary>
  /// Definir uma condição de procura
  /// </summary>
  public class ConditionItem
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="ConditionItem"/>
    /// </summary>
    public ConditionItem()
    {
      SearchCondition = SearchConditionsEnum.Any;
      IgnoreCase = false;
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="ConditionItem"/>
    /// </summary>
    /// <param name="nameCondition">Nome da condição</param>
    /// <param name="nameValue">Valor definido</param>
    /// <param name="ignoreCase">Informe "true" para igoreCase</param>
    public ConditionItem(SearchConditionsEnum nameCondition, string nameValue, bool ignoreCase)
    {
      SearchCondition = nameCondition;
      NameValue = nameValue;
      IgnoreCase = ignoreCase;
    }
    #endregion

    /// <summary>
    /// Nome da condição
    /// </summary>
    public SearchConditionsEnum SearchCondition { get; set; }

    /// <summary>
    /// Valor definido
    /// </summary>
    public string NameValue { get; set; }

    /// <summary>
    /// Indica se a condição é IgnoreCase
    /// </summary>
    public bool IgnoreCase { get; set; }

    /// <summary>
    /// Retorna uma string representando esta instância
    /// </summary>
    /// <returns>string representando esta instância</returns>
    public override string ToString()
    {
      return $"{SearchCondition} {NameValue}{(SearchCondition != SearchConditionsEnum.Any && IgnoreCase ? " (IgnoreCase)" : "")}";
    }
  }

}
