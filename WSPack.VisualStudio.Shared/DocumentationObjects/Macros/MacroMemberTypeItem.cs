using System.Collections.Generic;

using WSPack.Lib.Extensions;
using WSPack.VisualStudio.Shared.Commands;
using WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper;

namespace WSPack.VisualStudio.Shared.DocumentationObjects.Macros
{
  /// <summary>
  /// Macros para o Type de um Member
  /// </summary>
  public class MacroMemberTypeItem : MacroEnvironmentItems
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe: <see cref="MacroMemberTypeItem"/>
    /// </summary>
    /// <param name="code">Código da macro como deve ser inserida no texto</param>
    /// <param name="description">Descrição do que a macro faz</param>
    protected MacroMemberTypeItem(string code, string description) : base(code, description)
    {
    }
    #endregion

    /// <summary>
    /// Criar o grupo com as macros válidas para MemberType
    /// </summary>
    /// <returns></returns>
    public new static MacroGroupItems CreateGroup()
    {
      var grupoMemberType = new MacroGroupItems(MacroGroupItems.ElementTypeGroup)
      {
        ToolTip = "Tipo do elemento. Ex: int, bool, void, etc...",

        MacroList = new List<MacroBaseItems>
        {
          new MacroMemberTypeItem(MacrosConsts.TypeName, "Nome do tipo"),
          new MacroMemberTypeItem(MacrosConsts.TypeNameSee, "Nome do tipo com a tag See"),
          new MacroMemberTypeItem(MacrosConsts.TypeFullName, "Nome completo do tipo"),
          new MacroMemberTypeItem(MacrosConsts.TypeFullNameSee, "Nome completo do tipo com a tag See"),
        }
      };
      return grupoMemberType;
    }

    /// <summary>
    /// Executar a Macro
    /// </summary>
    /// <param name="context">Context</param>
    /// <returns>Texto da macro substituído</returns>
    public override string ExecuteMacro(object context)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      if (context is MemberElementEx memberElement)
      {
        string nome, nomeCompleto;
        if (memberElement.ElementType.TypeKind == EnvDTE.vsCMTypeRef.vsCMTypeRefCodeType)
        {
          nome = memberElement.ElementType.CodeType.Name;
          nomeCompleto = memberElement.ElementType.CodeType.FullName;
        }
        else
        {
          nome = memberElement.ElementType.AsString;
          nomeCompleto = memberElement.ElementType.AsFullName;
        }

        switch (Code)
        {
          case MacrosConsts.TypeName:
            return nome;
          case MacrosConsts.TypeFullName:
            return nomeCompleto;

          case MacrosConsts.TypeNameSee:
            return DocumentationCommand.PatternSee.FormatWith(nome);
          case MacrosConsts.TypeFullNameSee:
            return DocumentationCommand.PatternSee.FormatWith(nomeCompleto);

          default:
            return base.ExecuteMacro(context);
        }
      }
      else
        return base.ExecuteMacro(context);
    }
  }
}
