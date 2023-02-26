using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE80;
using EnvDTE;

namespace WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper
{
  /// <summary>
  /// Representa uma propriedade do FileCodeModel
  /// </summary>
  public class PropertyElementEx : MemberElementEx
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe: <see cref="PropertyElementEx"/>
    /// </summary>
    /// <param name="codeProperty">Propriedade do FileCodeModel</param>
    public PropertyElementEx(CodeProperty2 codeProperty)
          : base((CodeElement2)codeProperty)
    {
      Element = codeProperty;
    }
    #endregion

    /// <summary>
    /// Propriedade
    /// </summary>
    public new CodeProperty2 Element { get; }

    /// <summary>
    /// Recuperar o tipo do membro
    /// </summary>
    public override MemberTypesEnum MemberType => MemberTypesEnum.Property;

    /// <summary>
    /// Tipo do elemento
    /// </summary>
    public override CodeTypeRef2 ElementType => Element.Type as CodeTypeRef2;

    /// <summary>
    /// Indica se o elemento possui a TAG 'value' na estrutura de comentário do summary
    /// </summary>
    public override bool HasValue
    {
      get
      {
        Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
        return Element.Type.TypeKind != vsCMTypeRef.vsCMTypeRefVoid;
      }
    }

    /// <summary>
    /// Recuperar o comentário do summary presente no elemento.
    /// O comentário possui uma estrutura XML
    /// </summary>
    /// <returns>comentário do summary presente no elemento.</returns>
    public override string GetDocComment()
    {
      return Element.DocComment;
    }

    /// <summary>
    /// Incluir comentário no elemento
    /// </summary>
    /// <param name="xmlContent">Estrutura do comentário</param>
    public override void Comment(string xmlContent)
    {
      Element.DocComment = xmlContent;
    }

    /// <summary>
    /// Recuperar o tipo que declara a propriedade
    /// </summary>
    /// <returns>Declaring type</returns>
    protected override TypeElementEx GetDeclaringType()
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      return new TypeElementEx((CodeType)Element.Parent2);
    }
  }
}
