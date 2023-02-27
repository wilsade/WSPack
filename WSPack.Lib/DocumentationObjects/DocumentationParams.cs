using System.Collections.Generic;

namespace WSPack.Lib.DocumentationObjects
{
  /// <summary>
  /// Definição de parâmetros de documentação
  /// </summary>
  public class DocumentationParams
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="DocumentationParams"/>
    /// </summary>
    public DocumentationParams()
    {
      DictionaryList = new List<DictionaryItem>();
      AbbreviationList = new List<DictionaryItem>();
      RuleList = new List<RuleBaseItem>();
    }
    #endregion

    /// <summary>
    /// Dicionário global.
    /// Será utilizado para documentar qualquer item: tipos ou membros ou parâmetros
    /// </summary>
    public List<DictionaryItem> DictionaryList { get; set; }

    /// <summary>
    /// Lista de regras. Podem ser regras de Types ou Members
    /// </summary>
    public List<RuleBaseItem> RuleList { get; set; }

    /// <summary>
    /// Definição de abreviações. 
    /// </summary>
    public List<DictionaryItem> AbbreviationList { get; set; }
  }

}
