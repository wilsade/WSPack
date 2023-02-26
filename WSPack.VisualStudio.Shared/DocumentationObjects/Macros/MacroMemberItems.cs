using System.Collections.Generic;

using WSPack.Lib.Extensions;
using WSPack.VisualStudio.Shared.Commands;
using WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper;

namespace WSPack.VisualStudio.Shared.DocumentationObjects.Macros
{
  /// <summary>
  /// Macros válidas para membros
  /// </summary>
  public class MacroMemberItems : MacroEnvironmentItems
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="MacroMemberItems"/>
    /// </summary>
    /// <param name="code">Código da macro como deve ser inserida no texto</param>
    /// <param name="description">Descrição do que a macro faz</param>
    protected MacroMemberItems(string code, string description)
      : base(code, description)
    {
    }
    #endregion

    #region Métodos
    /// <summary>
    /// Criar o grupo com as macros válidas para membros
    /// </summary>
    /// <returns>grupo com as macros válidas para membros</returns>
    public new static List<MacroGroupItems> CreateGroup()
    {
      var grupoDeclaringType = new MacroGroupItems(MacroGroupItems.DeclaringTypeGroup)
      {
        MacroList = new List<MacroBaseItems>()
        {
          new MacroMemberItems(MacrosConsts.DeclaringTypeName, "Nome do tipo que declara o membro"),
          new MacroMemberItems(MacrosConsts.DeclaringTypeNameSee, "Nome do tipo que declara o membro com a tag See"),
          new MacroMemberItems(MacrosConsts.DeclaringTypeFullName, "Nome completo do tipo que declara o membro"),
          new MacroMemberItems(MacrosConsts.DeclaringTypeFullNameSee, "Nome completo do tipo que declara o membro com a tag See")
        },

        SubGroupsList = new List<MacroGroupItems>()
        {
          new MacroGroupItems(MacroGroupItems.WordsGroup, "Separação das palavras através de espaços")
          {
            MacroList = MacroWordsItems.CreateList(MacroGroupItems.DeclaringTypeGroup)
          }
        }
      };

      MacroGroupItems grupoMemberType = MacroMemberTypeItem.CreateGroup();
      return new List<MacroGroupItems> { grupoDeclaringType, grupoMemberType };
    }

    /// <summary>
    /// Executar a Macro
    /// </summary>
    /// <param name="context">Context</param>
    /// <returns>Texto da macro substituído</returns>
    public override string ExecuteMacro(object context)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      if (context is MemberElementEx member && member.DeclaringType is TypeElementEx tipo)
      {
        string nome = tipo.Name;
        string fullName = tipo.FullName;
        var split = fullName.Split('<', '>');
        if (split.Length == 3)
        {
          nome += "{" + split[1] + "}";
          fullName = fullName.Replace("<", "{").Replace(">", "}");
        }

        switch (Code)
        {
          case MacrosConsts.DeclaringTypeName:
            return nome;
          case MacrosConsts.DeclaringTypeNameSee:
            return DocumentationCommand.PatternSee.FormatWith(nome);
          case MacrosConsts.DeclaringTypeFullName:
            return fullName;
          case MacrosConsts.DeclaringTypeFullNameSee:
            return DocumentationCommand.PatternSee.FormatWith(fullName);

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
