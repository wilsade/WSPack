namespace WSPack.VisualStudio.Shared.DocumentationObjects
{
  /// <summary>
  /// Item do dicionário
  /// </summary>
  public class DictionaryItem : DocumentationBaseItem
  {
    readonly bool _isDefaultRule;

    #region Construtores
    /// <summary>
    /// Cria uma instância da classe <see cref="DictionaryItem" />.
    /// </summary>
    public DictionaryItem()
    {
      _isDefaultRule = false;
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="DictionaryItem" />.
    /// </summary>
    /// <param name="itemName">Nome do item</param>
    /// <param name="itemSummary">Summary do item</param>
    /// <param name="isDefaultRule">Indica se este item representa uma regra Default</param>
    public DictionaryItem(string itemName, string itemSummary, bool isDefaultRule)
          : base()
    {
      ItemName = itemName;
      Summary = itemSummary;
      _isDefaultRule = isDefaultRule;
    }
    #endregion

    /// <summary>
    /// Clonar este objeto
    /// </summary>
    /// <returns>objeto clonado</returns>
    public override DocumentationBaseItem Clone()
    {
      return new DictionaryItem(ItemName, Summary, _isDefaultRule)
      {
        Remarks = Remarks,
        Returns = Returns,
      };
    }

    /// <summary>
    /// Definir um nome amigável para a classe
    /// </summary>
    public override string FriendlyName => _isDefaultRule ? "Regra padrão" : "Regra de dicionário";
  }

}
