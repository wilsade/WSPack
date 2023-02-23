using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using WSPack.Lib.CSharp.Internals;

namespace WSPack.Lib.CSharp.Models
{
  /// <summary>
  /// Definir informações de um método
  /// </summary>
  [DebuggerDisplay("{Name}")]
  public class MethodModel
  {
    /// <summary>
    /// Definir a string: TestMethod
    /// </summary>
    internal const string Def_TestMethod = "TestMethod";

    /// <summary>
    /// Definir a string: (get)
    /// </summary>
    internal const string Def_Get = "(get)";

    /// <summary>
    /// Definir a string: (set)
    /// </summary>
    internal const string Def_Set = "(set)";

    /// <summary>
    /// Definir a string: (ctor)
    /// </summary>
    internal const string Def_Ctor = "(ctor)";

    /// <summary>
    /// Definir a string: (dtor)
    /// </summary>
    internal const string Def_Dtor = "(dtor)";

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="MethodModel"/>
    /// </summary>
    public MethodModel()
    {

    }
    #endregion

    /// <summary>
    /// (Gets or sets) Nodo da árvore sintática
    /// </summary>
    internal RoslynMemberNode MemberNode { get; set; }

    /// <summary>
    /// Nome do método
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// Recuperar a localização do final do método
    /// </summary>
    public LocationModel BodyEndLocation { get; internal set; }

    /// <summary>
    /// Recuperar a localização do nodo pai
    /// </summary>
    public LocationModel ClassLocation { get; internal set; }

    /// <summary>
    /// Recuperar o início do método
    /// </summary>
    public LocationModel DeclareLocation { get; internal set; }

    /// <summary>
    /// Recuperar o início do método (incluíndo os comentários)
    /// </summary>
    public LocationModel StartLocation { get; internal set; }

    /// <summary>
    /// Recuperar a localização do nome do método
    /// </summary>
    public LocationModel NameLocation { get; set; }

    /// <summary>
    /// (Gets) Indica se o método é um método Getter ou Setter de uma propriedade
    /// </summary>
    public bool IsGetterOrSetter { get; internal set; }

    /// <summary>
    /// Indica se o método é um método de teste
    /// </summary>
    public bool IsTestMethod { get; internal set; }

    /// <summary>
    /// Indica se o método é um destructor
    /// </summary>
    public bool IsDestructor { get; internal set; }

    /// <summary>
    /// Código fonte do método
    /// </summary>
    public string SourceCode { get; set; }

    /// <summary>
    /// Retornar uma <see cref="string" /> que representa esta instância
    /// </summary>
    /// <returns>
    /// Uma <see cref="string" /> representando esta instância
    /// </returns>
    public override string ToString()
    {
      return Name;
    }

    /// <summary>
    /// Recuperar o summary de um código C#
    /// </summary>
    /// <returns>Summary de um código C#</returns>
    public string GetSummary()
    {
      if (MemberNode.SyntaxNode is CSharpSyntaxNode cSharpSyntaxNode)
      {
        // Recuperar a estrutura do summary
        var xmlTrivia = cSharpSyntaxNode.GetLeadingTrivia()
          .Select(i => i.GetStructure())
          .OfType<DocumentationCommentTriviaSyntax>()
          .FirstOrDefault();

        // Sequer existe '///' definido
        if (xmlTrivia == null)
          return FormatSummary();

        // Não possui '<summary>' definido
        var nodosXmlElementSyntax = xmlTrivia.ChildNodes().OfType<XmlElementSyntax>();
        var hasSummary = nodosXmlElementSyntax.Any(i => i.StartTag.Name.ToString().Equals("summary"));
        if (!hasSummary)
          return FormatSummary();

        if (nodosXmlElementSyntax.FirstOrDefault().Content.ToString().Replace("///", "").Trim().Length == 0)
          return FormatSummary();
      }

      return string.Empty;
    }

    /// <summary>
    /// Formats the summary.
    /// </summary>
    /// <returns></returns>
    string FormatSummary()
    {
      string esqueletoSummary = @"/// <summary>
/// {0}.
/// </summary>
";
      const string patterParam = "/// <param name=\"{0}\"></param>";
      const string patterRetorno = "/// <returns></returns>";

      // Método construtor
      if (MemberNode.Kind == MemberKindEnum.Constructor)
      {
        string nomeTipo = (MemberNode.SyntaxNode.Parent as BaseTypeDeclarationSyntax).Identifier.ToString();
        esqueletoSummary = string.Format(esqueletoSummary, $"Cria um instância da classe <see cref=\"{nomeTipo}\"/>");
      }

      // Outro membro
      else
        esqueletoSummary = string.Format(esqueletoSummary, Name);

      if (MemberNode.SyntaxNode is BaseMethodDeclarationSyntax baseMethod)
      {
        var strParametros_Retorno = new StringBuilder();

        // Parâmetros
        foreach (var esteParametro in baseMethod.ParameterList.Parameters)
        {
          strParametros_Retorno.AppendFormat(patterParam, esteParametro.Identifier.Text).AppendLine();
        }

        // Retorno
        if (baseMethod is MethodDeclarationSyntax method)
        {
          if (method.ReturnType.ToString() != "void")
            strParametros_Retorno.AppendLine(patterRetorno);
        }

        esqueletoSummary += strParametros_Retorno.ToString();
      }

      return esqueletoSummary;
    }

    /*
    /// <summary>
    /// Adds the summary.
    /// </summary>
    string GetSummaryExemplos()
    {
      string esqueletoSummary = @"/// <summary>
/// {0}.
/// </summary>
";
      if (MemberNode.Kind == MemberKindEnum.Constructor)
      {
        string nomeTipo = (MemberNode.SyntaxNode.Parent as BaseTypeDeclarationSyntax).Identifier.ToString();
        esqueletoSummary = string.Format(esqueletoSummary, $"Cria um instância da classe <see cref=\"{nomeTipo}\"/>");
      }
      else
      {
        if (MemberNode.SyntaxNode is MethodDeclarationSyntax method)
        {
          // Recuperar a estrutura do summary
          var xmlTrivia = method.GetLeadingTrivia()
            .Select(i => i.GetStructure())
            .OfType<DocumentationCommentTriviaSyntax>()
            .FirstOrDefault();

          if (xmlTrivia != null)
          {
            var nodosXmlElementSyntax = xmlTrivia.ChildNodes().OfType<XmlElementSyntax>();

            var hasSummary = nodosXmlElementSyntax.Any(i => i.StartTag.Name.ToString().Equals("summary"));

            if (hasSummary)
            {
              string conteudoSummaryOnly = nodosXmlElementSyntax.First().Content.ToFullString();

              // Recuperar os parâmetros que estão presentes no summary
              var allParamNameAttributes = nodosXmlElementSyntax
                        .Where(i => i.StartTag.Name.ToString().Equals("param"))
                        .SelectMany(i => i.StartTag.Attributes.OfType<XmlNameAttributeSyntax>());

              foreach (var esteParametro in method.ParameterList.Parameters)
              {
                var existsInXmlTrivia = allParamNameAttributes
                            .Any(i => i.Identifier.ToString().Equals(esteParametro.Identifier.Text));// ()

                if (!existsInXmlTrivia)
                {
                  // "Parameter Not Documented"
                }
              }

            }
          }
        }
        else
          esqueletoSummary = string.Format(esqueletoSummary, Name);
      }
      Trace.WriteLine("ok");

      var attribute = SyntaxFactory.XmlNameAttribute(SyntaxFactory.XmlName("name"),
            SyntaxFactory.Token(SyntaxKind.DoubleQuoteToken), "nome",
            SyntaxFactory.Token(SyntaxKind.DoubleQuoteToken))
            .WithLeadingTrivia(SyntaxFactory.SyntaxTrivia(SyntaxKind.WhitespaceTrivia, " "));

      var paramNode = SyntaxFactory.XmlElement(
            SyntaxFactory.XmlElementStartTag(SyntaxFactory.XmlName("param"),
                SyntaxFactory.List<XmlAttributeSyntax>()
                    .Add(attribute)), SyntaxFactory.XmlElementEndTag(SyntaxFactory.XmlName("param")));

      const string comment = "THIS IS THE NEW COMMENT";
      var text =
          SyntaxFactory.XmlText(SyntaxFactory.TokenList(
              SyntaxFactory.XmlTextLiteral(
                  SyntaxFactory.TriviaList(),
                  comment,
                  comment,
                  SyntaxFactory.TriviaList())));
      var textList = SyntaxFactory.List<XmlNodeSyntax>(new[] { SyntaxFactory.XmlNewLine("\n"), text, SyntaxFactory.XmlNewLine("\n") });
      var nodoSummary = SyntaxFactory.XmlElement(
                SyntaxFactory.XmlElementStartTag(SyntaxFactory.XmlName("summary")),
                textList,
                SyntaxFactory.XmlElementEndTag(SyntaxFactory.XmlName("summary")));
      string nodoSummarytoSTring = nodoSummary.ToFullString();

      SyntaxTrivia summary = SyntaxFactory.Comment(esqueletoSummary);
      string comentario = summary.ToFullString();
      return comentario;

      //var novo = MemberNode.SyntaxNode.WithoutLeadingTrivia().WithLeadingTrivia(summary);
      //var outro = SyntaxFactory.ParseLeadingTrivia(doc);
      //var novo2 = MemberNode.SyntaxNode.WithLeadingTrivia(outro);
    }
    */
  }
}
