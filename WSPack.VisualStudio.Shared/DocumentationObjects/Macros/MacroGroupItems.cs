using System.Collections.Generic;

namespace WSPack.VisualStudio.Shared.DocumentationObjects.Macros
{
  /// <summary>
  /// Definir um conjunto de macros válidas em um determinado grupo
  /// </summary>
  public class MacroGroupItems
  {
    internal const string DefaultGroup = "Default";
    internal const string DeclaringTypeGroup = "DeclaringTypeName";
    internal const string ElementGroup = "ElementName";
    internal const string WordsGroup = "Words";
    internal const string ElementTypeGroup = "ElementType";

    #region Construtor
    /// <summary>
    /// Cria uma instância desta classe
    /// </summary>
    /// <param name="groupName">Nome do grupo ao qual um conjunto de macros pertence</param>
    /// <param name="tooltip">Descrição do grupo</param>
    public MacroGroupItems(string groupName, string tooltip = "")
    {
      GroupName = groupName;
      ToolTip = tooltip;
      MacroList = new List<MacroBaseItems>();
      SubGroupsList = new List<MacroGroupItems>();
    }
    #endregion

    /// <summary>
    /// Nome do grupo ao qual um conjunto de macros pertence
    /// </summary>
    public string GroupName { get; set; }

    /// <summary>
    /// Descrição do grupo
    /// </summary>
    public string ToolTip { get; set; }

    /// <summary>
    /// Lista de macros deste grupo
    /// </summary>
    public List<MacroBaseItems> MacroList { get; set; }

    /// <summary>
    /// Lista de subgrupos
    /// </summary>
    public List<MacroGroupItems> SubGroupsList { get; set; }
  }
}
