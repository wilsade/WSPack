using System.Collections.Generic;
using System.Linq;

using WSPack.Lib.Extensions;
using WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper;

namespace WSPack.VisualStudio.Shared.DocumentationObjects.Macros
{
  /// <summary>
  /// Definição de macros para manipulação de palavras
  /// </summary>
  public class MacroWordsItems : MacroBaseItems
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância desta classe
    /// </summary>
    /// <param name="code">Código da macro como deve ser inserida no texto</param>
    /// <param name="description">Descrição do que a macro faz</param>
    public MacroWordsItems(string code, string description)
    {
      Code = code;
      Description = description;
    }
    #endregion

    /// <summary>
    /// Executar a Macro
    /// </summary>
    /// <param name="macroCode">Código da macro, conforme <see cref="MacrosConsts"/></param>
    /// <param name="typeOrMemberName">Nome de um type ou member</param>
    /// <returns>Texto da macro substituído</returns>
    static string ProcessName(string macroCode, string typeOrMemberName)
    {
      string[] lstWords = typeOrMemberName.PascalCaseSplit();

      switch (macroCode)
      {
        case MacrosConsts.AllSpaced:
          return string.Join(" ", lstWords);
        case MacrosConsts.AllLowerCase:
          return string.Join(" ", lstWords).ToLower();
        case MacrosConsts.AllFirstUpperCase:
          return string.Join(" ", lstWords).ToLower().ToFirstCharToUpper();
        case MacrosConsts.AllExceptFirst:
          return string.Join(" ", lstWords.ExceptFirst()).ToLower().ToFirstCharToUpper();
        case MacrosConsts.AllExceptFirstLowerCase:
          return string.Join(" ", lstWords.ExceptFirst()).ToLower();
        case MacrosConsts.AllExceptLast:
          return string.Join(" ", lstWords.ExceptLast()).ToLower().ToFirstCharToUpper();

        case MacrosConsts.FirstUpperCase:
          return string.Join(" ", lstWords.FirstOrDefault()).ToFirstCharToUpper();
        case MacrosConsts.FirstLowerCase:
          return string.Join(" ", lstWords.FirstOrDefault()).ToLower();

        case MacrosConsts.SecondUpperCase:
          return lstWords.SecondOrFirst().ToFirstCharToUpper();
        case MacrosConsts.SecondLowerCase:
          return lstWords.SecondOrFirst().ToLower();

        case MacrosConsts.BetweenFirstAndLastUpperCase:
          return string.Join(" ", lstWords.BetweenFirstAndLast().Select(x => x.ToFirstCharToUpper()));
        case MacrosConsts.BetweenFirstAndLastLowerCase:
          return string.Join(" ", lstWords.BetweenFirstAndLast()).ToLower();

        case MacrosConsts.LastUpperCase:
          return string.Join(" ", lstWords.LastOrDefault()).ToFirstCharToUpper();
        case MacrosConsts.LastLowerCase:
          return string.Join(" ", lstWords.LastOrDefault()).ToLower();

        default:
          return typeOrMemberName;
      }
    }

    #region Métodos
    /// <summary>
    /// Executar a Macro
    /// </summary>
    /// <param name="context">Context</param>
    /// <returns>Texto da macro substituído</returns>
    public override string ExecuteMacro(object context)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      if (context is IBaseElementEx element)
      {
        string macroCode = null, anyElement = null;

        // DeclaringTypeName
        if (Code.StartsWith(MacroGroupItems.DeclaringTypeGroup))
        {
          anyElement = element.DeclaringType?.Name;
          macroCode = Code.Replace(MacroGroupItems.DeclaringTypeGroup + ".", "");
        }

        // ElementName
        else if (Code.StartsWith(MacroGroupItems.ElementGroup))
        {
          anyElement = element.Name;
          macroCode = Code.Replace(MacroGroupItems.ElementGroup + ".", "");
        }

        return ProcessName(macroCode, anyElement);
      }
      else
        return base.ExecuteMacro(context);
    }

    /// <summary>
    /// Criar uma lista com as macros 'Word'
    /// </summary>
    /// <param name="parentGroupName">Nome do grupo pai, conforme <see cref="MacroGroupItems"/></param>
    /// <returns>lista com as macros 'Word'</returns>
    public static List<MacroBaseItems> CreateList(string parentGroupName)
    {
      return new List<MacroBaseItems>
      {
        // All
        new MacroWordsItems($"{parentGroupName}.{MacrosConsts.AllSpaced}",
          "Cada palavra do nome"),
        new MacroWordsItems($"{parentGroupName}.{MacrosConsts.AllLowerCase}",
          "Cada palavra do nome: inicial minúscula"),
        new MacroWordsItems($"{parentGroupName}.{MacrosConsts.AllFirstUpperCase}",
          "Cada palavra do nome: primeira palavra com inicial maiúscula"),
        new MacroWordsItems($"{parentGroupName}.{MacrosConsts.AllExceptFirst}",
          "Cada palavra do nome a partir da segunda palavra"),
        new MacroWordsItems($"{parentGroupName}.{MacrosConsts.AllExceptFirstLowerCase}",
          "Cada palavra do nome a partir da segunda palavra: inicial minúscula"),
        new MacroWordsItems($"{parentGroupName}.{MacrosConsts.AllExceptLast}",
          "Cada palavra do nome excetuando a última"),

        // First
        new MacroWordsItems($"{parentGroupName}.{MacrosConsts.FirstUpperCase}",
          "Primeira palavra: inicial maiúscula"),
        new MacroWordsItems($"{parentGroupName}.{MacrosConsts.FirstLowerCase}",
          "Primeira palavra: inicial minúscula"),

        // Second
        new MacroWordsItems($"{parentGroupName}.{MacrosConsts.SecondUpperCase}",
          "Segunda palavra: inicial maiúscula"),
        new MacroWordsItems($"{parentGroupName}.{MacrosConsts.SecondLowerCase}",
          "Segunda palavra: inicial minúscula"),

        // Last
        new MacroWordsItems($"{parentGroupName}.{MacrosConsts.LastUpperCase}",
          "Última palavra: inicial maiúscula"),
        new MacroWordsItems($"{parentGroupName}.{MacrosConsts.LastLowerCase}",
          "Última palavra: inicial minúscula"),

        // Between
        new MacroWordsItems($"{parentGroupName}.{MacrosConsts.BetweenFirstAndLastUpperCase}",
          "Entre a primeira e última palavra: inicial maiúscula"),
        new MacroWordsItems($"{parentGroupName}.{MacrosConsts.BetweenFirstAndLastLowerCase}",
          "Entre a primeira e última palavra: inicial minúscula")
      };
    }
    #endregion
  }
}
