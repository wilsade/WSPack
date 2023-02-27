using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE80;
using WSPack.Lib.DocumentationObjects;

namespace WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper
{
  /// <summary>
  /// Representa um método
  /// </summary>
  public class MethodElementEx : MemberElementEx, IMethodElementEx
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="MethodElementEx"/>
    /// </summary>
    /// <param name="codeFunction">Code function</param>
    public MethodElementEx(CodeFunction2 codeFunction) : base((CodeElement2)codeFunction)
    {
      Element = codeFunction;
    }
    #endregion

    /// <summary>
    /// Método
    /// </summary>
    public new CodeFunction2 Element { get; }

    /// <summary>
    /// Recuperar o tipo do membro
    /// </summary>
    public override MemberTypesEnum MemberType => IsConstructor ?
      MemberTypesEnum.Constructor : MemberTypesEnum.Method;

    /// <summary>
    /// Tipo do elemento
    /// </summary>
    public override CodeTypeRef2 ElementType => Element.Type as CodeTypeRef2;

    /// <summary>
    /// Indica se a função é Get ou Set de alguma propriedade
    /// </summary>
    public bool IsGetOrSet => Element.FunctionKind == vsCMFunction.vsCMFunctionPropertyGet ||
      Element.FunctionKind == vsCMFunction.vsCMFunctionPropertySet;

    /// <summary>
    /// Indica se a função é 'void'
    /// </summary>
    public bool IsVoid
    {
      get
      {
        Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
        return Element.Type.TypeKind == vsCMTypeRef.vsCMTypeRefVoid;
      }
    }

    /// <summary>
    /// Indica se o elemento possui a TAG 'return' na estrutura de comentário do summary
    /// </summary>
    public override bool HasReturns
    {
      get
      {
        Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
        return !IsVoid;
      }
    }

    /// <summary>
    /// Indica se é o método construtor
    /// </summary>
    public bool IsConstructor => Element.FunctionKind == vsCMFunction.vsCMFunctionConstructor;

    /// <summary>
    /// Recuperar o tipo que declara o membro
    /// </summary>
    /// <returns>Declaring type</returns>
    protected override TypeElementEx GetDeclaringType()
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      return new TypeElementEx((CodeType)Element.Parent);
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
    /// Recuperar os parâmetros do elemento
    /// </summary>
    /// <returns>parâmetros do elemento; lista vazia se o elemento não possui parâmetros</returns>
    public override List<MemberElementEx> GetParams()
    {
      var lst = new List<MemberElementEx>();
      foreach (CodeParameter esteParametro in Element.Parameters)
      {
        lst.Add(new ParameterElementEx((CodeParameter2)esteParametro));
      }
      return lst;
    }

  }
}
