namespace WSPack.VisualStudio.Shared.DocumentationObjects
{
  /// <summary>
  /// Definir condições para busca de nomes de Types/Members
  /// </summary>
  public enum SearchConditionsEnum
  {
    /// <summary>
    /// Quaiquer caracteres
    /// </summary>
    Any,

    /// <summary>
    /// Nome exato
    /// </summary>
    Equals,

    /// <summary>
    /// Nome começando com
    /// </summary>
    StartsWith,

    /// <summary>
    /// Nome terminando com
    /// </summary>
    EndsWith,

    /// <summary>
    /// Nome contém
    /// </summary>
    Contains,

    /// <summary>
    /// Nome conforme uma expressão regular
    /// </summary>
    Regex
  }

}
