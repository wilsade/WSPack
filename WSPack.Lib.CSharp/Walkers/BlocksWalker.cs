using System;
using System.Collections.Generic;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using WSPack.Lib.CSharp.Models;

namespace WSPack.Lib.CSharp.Walkers
{
  /// <summary>
  /// Objeto para percorrer a árvore sintática e extrair blocos de código (qualquer código entre { } )
  /// </summary>
  /// <seealso cref="CSharpSyntaxWalker" />
  internal sealed class BlocksWalker : CSharpSyntaxWalker
  {
    const string AbreChave = "{";
    const int TamanhoMaximoNome = 50;
    readonly List<BlockModel> _lstBlocks;

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="BlocksWalker"/>
    /// </summary>
    private BlocksWalker()
      : base(SyntaxWalkerDepth.Token)
    {
      _lstBlocks = new List<BlockModel>();
    }
    #endregion

    /// <summary>
    /// Calcular total de blocos
    /// </summary>
    /// <param name="root">Root</param>
    /// <returns>Total de blocos</returns>
    public static List<BlockModel> Calculate(SyntaxNode root)
    {
      var walker = new BlocksWalker();
      walker.Visit(root);
      return walker._lstBlocks;
    }

    /// <summary>
    /// Visitar um Token
    /// </summary>
    /// <param name="token"></param>
    public override void VisitToken(SyntaxToken token)
    {
#if DEBUG
      string tipo = token.Kind().ToString();
#endif
      if (token.IsKind(SyntaxKind.OpenBraceToken))
      {
        if (token.Parent is BlockSyntax)
        {
          if (token.Parent.Parent != null)
            AddBlock(token.Parent.Parent);
        }

        else
        {
          AddBlock(token.Parent);
        }
      }
      base.VisitToken(token);
    }

    /// <summary>
    /// Visitar um nodo
    /// </summary>
    /// <param name="node"></param>
    public override void Visit(SyntaxNode node)
    {
#if DEBUG
      string texto = node.ToString();
#endif
      base.Visit(node);
    }

    /// <summary>
    /// Visitar um bloco
    /// </summary>
    /// <param name="node"></param>
    public override void VisitBlock(BlockSyntax node)
    {
#if DEBUG
      string texto = node.ToString();
#endif
      base.VisitBlock(node);
    }

    private void AddBlock(SyntaxNode syntaxNode)
    {
      string Nome = ParseName(syntaxNode);
      var inicio = syntaxNode.GetLocation().GetLineSpan().StartLinePosition;
      var fim = syntaxNode.GetLocation().GetLineSpan().EndLinePosition;

      if (syntaxNode is TryStatementSyntax tryNode)
        fim = tryNode.Block.GetLocation().GetLineSpan().EndLinePosition;
      else
      {
        if (syntaxNode is IfStatementSyntax ifNode)
          fim = ifNode.Statement.GetLocation().GetLineSpan().EndLinePosition;
      }

      _lstBlocks.Add(new BlockModel(
        Nome,
        new LocationModel(inicio.Line, inicio.Character),
        new LocationModel(fim.Line, fim.Character))
      );
    }

    private string ParseName(SyntaxNode syntaxNode)
    {
      string toString;

      // Tratar Initializer de construtores
      if (syntaxNode is InitializerExpressionSyntax ctrInitializer && ctrInitializer.Parent is ObjectCreationExpressionSyntax)
        toString = ctrInitializer.Parent.ToString();
      else
        toString = syntaxNode.ToString();
      string semQuebras = toString.Replace("\r", "").Replace("\n", " ");

      if (syntaxNode is BaseMethodDeclarationSyntax baseMethod)
      {
        return ParseNameFromMethod(baseMethod);

        //var regex = GetRegexTiraAtributo();
        //semQuebras = regex.Replace(semQuebras, "");
      }

      if (syntaxNode is BaseTypeDeclarationSyntax baseType)
      {
        return ParseNameFromType(baseType);
      }

      var splitted = semQuebras.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
      string nome = string.Empty;
      foreach (var esteSplit in splitted)
      {
        // Chegamos no início de bloco.
        if (esteSplit.Equals(AbreChave))
          break;

        nome += esteSplit + " ";
        if (nome.Length > TamanhoMaximoNome)
          break;
      }
      nome = ParseNameFromProperty(nome, syntaxNode);

      return nome;
    }

    private string ParseNameFromProperty(string name, SyntaxNode syntaxNode)
    {
      if (syntaxNode.Parent != null)
      {
        if (syntaxNode.Parent is PropertyDeclarationSyntax propDecl)
        {
          // Definição da propriedade
          name = propDecl.Identifier.Text;
          return name;
        }

        // Get/Set
        else if (syntaxNode is AccessorDeclarationSyntax && syntaxNode.Parent.Parent is PropertyDeclarationSyntax)
        {
          AccessorDeclarationSyntax accessor = (AccessorDeclarationSyntax)syntaxNode;

          // Nome da propriedade
          string propName = ((PropertyDeclarationSyntax)syntaxNode.Parent.Parent).Identifier.Text;

          // Get
          if (accessor.Kind() == SyntaxKind.GetAccessorDeclaration)
            name = string.Format("{0} ({1})", "get", propName);

          // Set
          else if (accessor.Kind() == SyntaxKind.SetAccessorDeclaration)
            name = string.Format("{0} ({1})", "set", propName);

        }
      }

      return name;
    }

    string GetModificadorStaticOverride(ISymbol symbol)
    {
      string modificador2 = "";
      if (symbol.IsStatic)
        modificador2 = "static ";
      else if (symbol.IsOverride)
        modificador2 = "override ";
      else if (symbol.IsAbstract)
        modificador2 = "abstract ";
      return modificador2;
    }

    private string GetModificadorAccessibility(Accessibility acesso, string nodeToString)
    {
      string modificador = string.Empty;
      if (acesso == Accessibility.Public)
        modificador = "public ";
      else if (acesso == Accessibility.Internal)
        modificador = "internal ";
      else if (acesso == Accessibility.Protected)
        modificador = "protected ";
      else if (acesso == Accessibility.Private && nodeToString.Contains("private "))
        modificador = "private ";
      return modificador;
    }

    #region Compilação para extrair informações do type/método
    Compilation GetCompilation(SyntaxTree tree)
    {
      if (_compilation == null)
      {
        PortableExecutableReference mscorlib = MetadataReference.CreateFromFile(
          typeof(object).Assembly.Location);

        _compilation = CSharpCompilation.Create("xpto.dll", new[] { tree }, new[] { mscorlib });
      }

      return _compilation;
    }
    Compilation _compilation;

    private string ParseNameFromMethod(BaseMethodDeclarationSyntax method)
    {
      var compilacao = GetCompilation(method.SyntaxTree);
      var model = compilacao.GetSemanticModel(method.SyntaxTree);
      IMethodSymbol symbol = model.GetDeclaredSymbol(method);

      string nomeDesejado = symbol.ToDisplayString();

      string patterSubstituir = "";
      if (symbol.ContainingType != null)
        patterSubstituir = symbol.ContainingType.ToString() + ".";

      //if (symbol.ContainingNamespace != null)
      //  patterSubstituir = symbol.ContainingNamespace.ToString() + ".";

      if (!string.IsNullOrEmpty(patterSubstituir))
        nomeDesejado = nomeDesejado.Replace(patterSubstituir, "");

      string methodToString = method.ToString();

      string modificador = GetModificadorAccessibility(symbol.DeclaredAccessibility, methodToString);
      string modificador2 = GetModificadorStaticOverride(symbol);

      string retorno;
      if (symbol.MethodKind == MethodKind.Constructor)
        retorno = "";
      else if (symbol.MethodKind == MethodKind.Destructor)
      {
        modificador = "";
        retorno = "";
      }
      else
        retorno = symbol.ReturnType.ToDisplayString() + " ";

      if (!string.IsNullOrEmpty(modificador))
      {
        if (methodToString.Contains("new " + modificador))
          modificador = "new " + modificador;
        else if (methodToString.Contains(modificador + "new"))
          modificador += "new ";
      }

      return modificador + modificador2 + retorno + nomeDesejado;
    }

    private string ParseNameFromType(BaseTypeDeclarationSyntax typeDeclarationSyntax)
    {
      var compilacao = GetCompilation(typeDeclarationSyntax.SyntaxTree);
      var model = compilacao.GetSemanticModel(typeDeclarationSyntax.SyntaxTree);
      INamedTypeSymbol symbol = model.GetDeclaredSymbol(typeDeclarationSyntax);

      string nomeDesejado = symbol.Name;

      string modificador = GetModificadorAccessibility(symbol.DeclaredAccessibility, typeDeclarationSyntax.ToString());
      string modificador2 = GetModificadorStaticOverride(symbol);

      string baseType = "";
      if (symbol.BaseType != null && !symbol.BaseType.ToDisplayString().StartsWith("System") &&
        !symbol.BaseType.ToDisplayString().StartsWith("object"))
        baseType = " : " + symbol.BaseType.Name;

      return modificador + modificador2 + nomeDesejado + baseType;
    }

    #endregion
  }
}
