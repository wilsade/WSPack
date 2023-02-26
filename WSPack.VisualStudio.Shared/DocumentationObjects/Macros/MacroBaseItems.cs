using System.Diagnostics;

namespace WSPack.VisualStudio.Shared.DocumentationObjects.Macros
{
  /// <summary>
  /// Definição de macros para substituição de texto
  /// </summary>
  [DebuggerDisplay("{Code}")]
  public abstract class MacroBaseItems
  {
    #region Propriedades
    /// <summary>
    /// Código da macro
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Código da macro como deve ser inserida no texto
    /// </summary>
    public string MacroText => GetMacroText(Code);

    /// <summary>
    /// Descrição do que a macro faz
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Grupo pai
    /// </summary>
    public MacroGroupItems Parent { get; protected set; }
    #endregion

    #region Métodos
    /// <summary>
    /// Recuperar o texto completo da macro do jeito que ela deve estar inserida no texto
    /// </summary>
    /// <param name="macroConst">MacroConst</param>
    /// <returns>texto completo da macro do jeito que ela deve estar inserida no texto</returns>
    public static string GetMacroText(string macroConst) => $"$({macroConst})";

    /// <summary>
    /// Executar a Macro
    /// </summary>
    /// <param name="context">Context</param>
    /// <returns>Texto da macro substituído</returns>
    public virtual string ExecuteMacro(object context)
    {
      return MacroText;
    }
    #endregion

  }
}
