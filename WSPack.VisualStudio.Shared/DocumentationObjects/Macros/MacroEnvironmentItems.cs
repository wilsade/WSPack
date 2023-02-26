using System;
using System.Collections.Generic;

using WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper;

namespace WSPack.VisualStudio.Shared.DocumentationObjects.Macros
{
  /// <summary>
  /// Definição de macros para geração de summary
  /// </summary>
  public class MacroEnvironmentItems : MacroBaseItems
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância desta classe
    /// </summary>
    /// <param name="code">Código da macro como deve ser inserida no texto</param>
    /// <param name="description">Descrição do que a macro faz</param>
    protected MacroEnvironmentItems(string code, string description)
    {
      Code = code;
      Description = description;
    }
    #endregion

    #region Métodos
    /// <summary>
    /// Criar o grupo padrão com as macros de ambientes
    /// </summary>
    /// <returns>grupo padrão com as macros de ambientes</returns>
    public static MacroGroupItems CreateGroup()
    {
      return new MacroGroupItems(MacroGroupItems.DefaultGroup)
      {
        MacroList = new List<MacroBaseItems>()
        {
          new MacroEnvironmentItems(MacrosConsts.End, "Local onde o cursor ficará"),
          new MacroEnvironmentItems(MacrosConsts.RuleName, "Nome da regra de documentação"),
          new MacroEnvironmentItems(MacrosConsts.EnvironmentUserName, "Nome do usuário"),
          new MacroEnvironmentItems(MacrosConsts.EnvironmentMachineName, "Nome do computador"),
          new MacroEnvironmentItems(MacrosConsts.DateTimeNow, "Data / Hora"),
          new MacroEnvironmentItems(MacrosConsts.DateTimeNowShortDate, "Data"),
          new MacroEnvironmentItems(MacrosConsts.DateTimeNowShortTime, "Hora")
        }
      };
    }

    /// <summary>
    /// Executar a Macro
    /// </summary>
    /// <param name="context">Context</param>
    /// <returns>Texto da macro substituído</returns>
    public override string ExecuteMacro(object context)
    {
      switch (Code)
      {
        case MacrosConsts.RuleName:
          if (context is BaseElementEx baseElement && baseElement.MatchedDocumentationRule != null)
            return baseElement.MatchedDocumentationRule.ItemName;
          else
            return base.ExecuteMacro(context);
        case MacrosConsts.EnvironmentUserName:
          return Environment.UserName;
        case MacrosConsts.EnvironmentMachineName:
          return Environment.MachineName;
        case MacrosConsts.DateTimeNow:
          return DateTime.Now.ToString();
        case MacrosConsts.DateTimeNowShortDate:
          return DateTime.Now.ToShortDateString();
        case MacrosConsts.DateTimeNowShortTime:
          return DateTime.Now.ToShortTimeString();

        default:
          return base.ExecuteMacro(context);
      }
    }
    #endregion
  }
}
