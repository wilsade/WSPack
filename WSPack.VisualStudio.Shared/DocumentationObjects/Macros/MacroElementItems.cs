using System.Collections.Generic;

using WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper;

namespace WSPack.VisualStudio.Shared.DocumentationObjects.Macros
{
  /// <summary>
  /// Macros válidas para elementos: Types e Members
  /// </summary>
  public class MacroElementItems : MacroEnvironmentItems
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="MacroElementItems"/>
    /// </summary>
    /// <param name="code">Código da macro como deve ser inserida no texto</param>
    /// <param name="description">Descrição do que a macro faz</param>
    protected MacroElementItems(string code, string description) :
      base(code, description)
    {
    }
    #endregion

    #region Métodos
    /// <summary>
    /// Criar o grupo com as macros válidas para tipos e membros
    /// </summary>
    /// <returns>grupo com as macros válidas para membros</returns>
    public new static MacroGroupItems CreateGroup()
    {
      var grupo = new MacroGroupItems(MacroGroupItems.ElementGroup)
      {
        MacroList = new List<MacroBaseItems>()
        {
          new MacroElementItems(MacrosConsts.ElementName, "Nome do elemento que está sendo documentado"),
          new MacroElementItems(MacrosConsts.ElementFullName, "Nome completo do elemento que está sendo documentado")
        },

        SubGroupsList = new List<MacroGroupItems>()
        {
          new MacroGroupItems(MacroGroupItems.WordsGroup, "Separação das palavras através de espaços")
          {
            MacroList = MacroWordsItems.CreateList(MacroGroupItems.ElementGroup)
          }
        }
      };
      return grupo;
    }

    /// <summary>
    /// Executar a Macro
    /// </summary>
    /// <param name="context">Context</param>
    /// <returns>Texto da macro substituído</returns>
    public override string ExecuteMacro(object context)
    {
      if (context is IBaseElementEx element)
      {
        switch (Code)
        {
          case MacrosConsts.ElementName:
            return element.Name;
          case MacrosConsts.ElementFullName:
            return element.FullName;
          default:
            return base.ExecuteMacro(context);
        }
      }
      else
        return base.ExecuteMacro(context);
    }
    #endregion

  }
}
