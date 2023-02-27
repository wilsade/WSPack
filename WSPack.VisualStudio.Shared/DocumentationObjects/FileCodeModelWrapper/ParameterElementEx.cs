using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE80;
using EnvDTE;
using WSPack.Lib.DocumentationObjects;

namespace WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper
{
  /// <summary>
  /// Representa um parâmetro do FileCodeModel
  /// </summary>
  public class ParameterElementEx : MemberElementEx
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe: <see cref="ParameterElementEx"/>
    /// </summary>
    /// <param name="codeParameter">Parâmetro</param>
    public ParameterElementEx(CodeParameter2 codeParameter)
      : base((CodeElement2)codeParameter)
    {
      Element = codeParameter;
    }
    #endregion

    /// <summary>
    /// Parâmetro
    /// </summary>
    public new CodeParameter2 Element { get; }

    /// <summary>
    /// Recuperar o tipo do membro
    /// </summary>
    public override MemberTypesEnum MemberType => MemberTypesEnum.Parameters;

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
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      string docForParam;
      if (Element.Parent is CodeFunction2 codeFunction)
        docForParam = codeFunction.DocComment;
      else if (Element.Parent is CodeDelegate2 codeDelegate)
        docForParam = codeDelegate.DocComment;
      else
        docForParam = string.Empty;

      XMLSummaryDocObj summaryParentDocObj = XMLSummaryDocObj.LoadOrCreate(docForParam);
      XMLParamNode nodo = summaryParentDocObj.GetParamNodeList().FirstOrDefault(x => x.ParamName == Name);
      XMLSummaryDocObj summaryDocObj = XMLSummaryDocObj.LoadOrCreate("");
      summaryDocObj.SummaryNode = nodo?.ParamSummaryAsXML;
      return summaryDocObj.XMLContent;
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
    /// Recuperar o tipo que declara o parâmetro
    /// </summary>
    /// <returns>Declaring type</returns>
    protected override TypeElementEx GetDeclaringType()
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      return new TypeElementEx((CodeType)((CodeFunction2)Element.Parent).Parent);
    }
  }
}
