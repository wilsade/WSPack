using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using WSPack.Lib.CSharp.Internals;
using WSPack.Lib.CSharp.Models;

namespace WSPack.Lib.CSharp.Walkers
{
  /// <summary>
  /// Classe para extrair métodos da árvore sintática
  /// </summary>
  /// <seealso cref="CSharpSyntaxWalker" />
  internal sealed class MethodsWalker : CSharpSyntaxWalker
  {
    readonly List<MethodModel> _lstMethods = new List<MethodModel>();

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="MethodsWalker"/>
    /// </summary>
    private MethodsWalker()
    {

    }
    #endregion

    /// <summary>
    /// Extrair os métodos da árvore sintática
    /// </summary>
    /// <param name="root">Raiz da árvore</param>
    /// <returns>Lista de métodos</returns>
    public static List<MethodModel> ExtractMethods(SyntaxNode root)
    {
      var methodsWalker = new MethodsWalker();
      methodsWalker.Visit(root);
      return methodsWalker._lstMethods;
    }

    void AddMethod(MethodDeclarationSyntax node)
    {
      AddMethod(
        node.Identifier.Text, node, MemberKindEnum.Method,
        node.Parent as TypeDeclarationSyntax,
        node.Identifier,
        node.GetLocation(),
        node.Identifier.GetLocation(),
        node.Modifiers,
        node.ToString(),
        false,
        false,
        node.ReturnType,
        node.AttributeLists);
    }

    void AddMethod(PropertyDeclarationSyntax propertyNode, AccessorDeclarationSyntax getterOrSetter)
    {
      MemberKindEnum tipo;
      string strGetOuSet;
      if (getterOrSetter.Kind() == SyntaxKind.GetAccessorDeclaration)
      {
        tipo = MemberKindEnum.GetProperty;
        strGetOuSet = MethodModel.Def_Get;
      }
      else
      {
        tipo = MemberKindEnum.SetProperty;
        strGetOuSet = MethodModel.Def_Set;
      }

      AddMethod(
        propertyNode.Identifier.Text + strGetOuSet, propertyNode, tipo,
        propertyNode.Parent as TypeDeclarationSyntax,
        getterOrSetter.Keyword,
        getterOrSetter.GetLocation(),
        propertyNode.Identifier.GetLocation(),
        getterOrSetter.Modifiers,
        getterOrSetter.ToString(),
        true,
        false,
        null,
        getterOrSetter.AttributeLists);
    }

    /// <summary>
    /// Adds the method.
    /// </summary>
    /// <param name="methodName">Nome do método.</param>
    /// <param name="node">Nodo da árvore sintática.</param>
    /// <param name="tipoMembro">Tipo de membro</param>
    /// <param name="parent">Classe que contém o método.</param>
    /// <param name="identifier">Token que representa o identificador do método.</param>
    /// <param name="nodeLocation">Localização do nodo do método.</param>
    /// <param name="nameLocation">Localização do nome do método.</param>
    /// <param name="modifiers">Modificadores do métodos.</param>
    /// <param name="sourceCode">Código fonte do método.</param>
    /// <param name="isGetterSetter">"true" para indicar que o método é Getter/Setter.</param>
    /// <param name="isDestructor">"true" para indicar que o método é um destructor</param>
    /// <param name="returnType">Tipo de retorno do método (pode ser null).</param>
    /// <param name="attributeList">Lista de atributos.</param>
    void AddMethod(string methodName, SyntaxNode node, MemberKindEnum tipoMembro, TypeDeclarationSyntax parent,
      SyntaxToken identifier, Location nodeLocation, Location nameLocation, SyntaxTokenList modifiers,
      string sourceCode, bool isGetterSetter, bool isDestructor, TypeSyntax returnType, SyntaxList<AttributeListSyntax> attributeList)
    {
      if (parent == null)
        return;

      LinePosition bodyEndPosition = nodeLocation.GetLineSpan().EndLinePosition;
      LinePosition classLocation = AchaMenor(parent.Identifier, parent.Modifiers, null);
      LinePosition declareLocation = AchaMenor(identifier, modifiers, returnType);
      LinePosition nameLocationX = nameLocation.GetLineSpan().StartLinePosition;
      bool isTestMethod = false;
      if (!isGetterSetter)
        isTestMethod = attributeList.Any(x => x.ToString().Contains(MethodModel.Def_TestMethod));

      LinePosition startPosition = declareLocation;
      int ajustePosicaoLineDocumentation = 0;
      if (node.HasLeadingTrivia)
      {
        SyntaxTriviaList leadingTrivia = node.GetLeadingTrivia();

        // As #regions também são identificados como LeadingTrivia
        var tupla = CheckIfDirective(leadingTrivia);
        sourceCode = tupla.TriviaCode + sourceCode;
        startPosition = leadingTrivia.ElementAt(tupla.LineIndex).GetLocation().GetLineSpan().EndLinePosition;

        // Pegar o elemento que contém uma linha com documentação do tipo /// <summary>
        var trivia = leadingTrivia.FirstOrDefault(x => x.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));

        // Verifica se o método está comentado com barra-barra //
        if (trivia != null && trivia.IsKind(SyntaxKind.None))
          trivia = leadingTrivia.FirstOrDefault(x => x.IsKind(SyntaxKind.SingleLineCommentTrivia));

        // SingleLineDocumentationCommentTrivia tem o início do caracter sem contar o ///.
        // Ou seja, /// <summary>: a coluna posição não vai pegar a primeira barra
        else
          ajustePosicaoLineDocumentation = 3;

        if (trivia != null && !trivia.IsKind(SyntaxKind.None))
        {
          var posicaoCandidato = trivia.GetLocation().GetLineSpan().StartLinePosition;
          startPosition = posicaoCandidato;
        }
      }

      var metodo = new MethodModel()
      {
        Name = methodName,
        BodyEndLocation = new LocationModel(bodyEndPosition.Line, bodyEndPosition.Character),
        ClassLocation = new LocationModel(classLocation.Line, classLocation.Character),
        DeclareLocation = new LocationModel(declareLocation.Line, declareLocation.Character),
        StartLocation = new LocationModel(startPosition.Line, startPosition.Character - ajustePosicaoLineDocumentation),
        IsGetterOrSetter = isGetterSetter,
        IsTestMethod = isTestMethod,
        IsDestructor = isDestructor,
        NameLocation = new LocationModel(nameLocationX.Line, nameLocationX.Character),
        SourceCode = sourceCode,
        MemberNode = new RoslynMemberNode(node, tipoMembro)
      };
      _lstMethods.Add(metodo);
    }

    static (string TriviaCode, int LineIndex) CheckIfDirective(SyntaxTriviaList leadingTrivia)
    {
      string code = leadingTrivia.ToString();
      int line = 0;

#if DEBUG
      SyntaxTrivia triviaDebug = leadingTrivia.ElementAt(0);
      Location triviaDebugLoc = triviaDebug.GetLocation();
#endif
      // Se o método não tem comentário e tem uma region acima dele, o "#if region NOME" estava sendo o start do método
      SyntaxTrivia ifTrivia = leadingTrivia.FirstOrDefault(x => x.IsKind(SyntaxKind.IfDirectiveTrivia));
      if (!ifTrivia.IsKind(SyntaxKind.None))
      {
        // Se tem #if, geralmente vai ser:
        // 0: EndOfLineTrivia
        // 1: IfDirectiveTrivia
        // 2: EndOfLineTrivia
        // 3: WhitespaceTrivia
        // Não vamos considerar as posições 0, 1 e 2
        int posWhiteSpace = leadingTrivia.IndexOf(SyntaxKind.WhitespaceTrivia);
        if (posWhiteSpace > 0)
        {
          line = posWhiteSpace;
          code = leadingTrivia.Skip(posWhiteSpace).ToSyntaxTriviaList().ToString();
        }
      }

      return (code, line);
    }

    LinePosition AchaMenor(SyntaxToken identifier, SyntaxTokenList modifiers, TypeSyntax returnType)
    {
      LinePosition menor = identifier.GetLocation().GetLineSpan().StartLinePosition;
      if (modifiers.Count > 0)
      {
#if DEBUG
        SyntaxToken element = modifiers.ElementAt(0);
        Location eleLoc = element.GetLocation();
#endif

        LinePosition token = modifiers.ElementAt(0).GetLocation().GetLineSpan().StartLinePosition;
        if (token.Line <= menor.Line && token.Character < menor.Character)
          menor = token;
      }

      if (returnType != null)
      {
        LinePosition token = returnType.GetLocation().GetLineSpan().StartLinePosition;
        if (token.Line <= menor.Line && token.Character < menor.Character)
          menor = token;
      }

      return menor;
    }

    /// <summary>
    /// Called when the visitor visits a MethodDeclarationSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
      base.VisitMethodDeclaration(node);
      AddMethod(node);
    }

    /// <summary>
    /// Called when the visitor visits a ConstructorDeclarationSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
    {
      base.VisitConstructorDeclaration(node);
      AddMethod(
        node.Identifier.Text + MethodModel.Def_Ctor, node, MemberKindEnum.Constructor,
        node.Parent as TypeDeclarationSyntax,
        node.Identifier,
        node.GetLocation(),
        node.Identifier.GetLocation(),
        node.Modifiers,
        node.ToString(),
        false,
        false,
        null,
        node.AttributeLists);
    }

    /// <summary>
    /// Called when the visitor visits a DestructorDeclarationSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitDestructorDeclaration(DestructorDeclarationSyntax node)
    {
      base.VisitDestructorDeclaration(node);
      AddMethod(
        node.Identifier.Text + MethodModel.Def_Dtor, node, MemberKindEnum.Destructor,
        (TypeDeclarationSyntax)node.Parent,
        node.Identifier,
        node.GetLocation(),
        node.Identifier.GetLocation(),
        node.Modifiers,
        node.ToString(),
        false, true,
        null,
        node.AttributeLists);
    }

    /// <summary>
    /// Called when the visitor visits a PropertyDeclarationSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
      base.VisitPropertyDeclaration(node);

      // Get
      if (node.AccessorList != null)
      {
        AccessorDeclarationSyntax accessor = node.AccessorList.Accessors.FirstOrDefault(x => x.Kind() == SyntaxKind.GetAccessorDeclaration);
        if (accessor != null && accessor.Body != null)
        {
          AddMethod(node, accessor);
        }

        // Set
        accessor = node.AccessorList.Accessors.FirstOrDefault(x => x.Kind() == SyntaxKind.SetAccessorDeclaration);
        if (accessor != null && accessor.Body != null)
        {
          AddMethod(node, accessor);
        }
      }
    }


  }
}
