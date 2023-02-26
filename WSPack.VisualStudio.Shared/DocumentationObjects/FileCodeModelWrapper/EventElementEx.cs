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
  /// Representa um evento do FileCodeModel
  /// </summary>
  public class EventElementEx : MemberElementEx
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe: <see cref="EventElementEx"/>
    /// </summary>
    /// <param name="code">Evento</param>
    public EventElementEx(CodeEvent code) : base((CodeElement2)code)
    {
      Element = code;
    }
    #endregion

    /// <summary>
    /// Evento
    /// </summary>
    public new CodeEvent Element { get; }

    /// <summary>
    /// Recuperar o tipo do membro
    /// </summary>
    public override MemberTypesEnum MemberType => MemberTypesEnum.Event;

    /// <summary>
    /// Tipo do elemento
    /// </summary>
    public override CodeTypeRef2 ElementType => Element.Type as CodeTypeRef2;

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
    /// Recuperar o tipo que declara o membro
    /// </summary>
    /// <returns>Declaring type</returns>
    protected override TypeElementEx GetDeclaringType()
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      return new TypeElementEx((CodeType)Element.Parent);
    }
  }
}
