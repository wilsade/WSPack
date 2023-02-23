using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using WSPack.Lib.CSharp.Internals;

namespace WSPack.Lib.CSharp.Walkers
{
  /// <summary>
  /// Objeto para percorrer nodos de declaração de variáveis
  /// </summary>
  /// <seealso cref="CSharpSyntaxWalker" />
  internal sealed class MethodLocalVariablesWalker : CSharpSyntaxWalker
  {
    int _numLocalVariables;

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="MethodLocalVariablesWalker"/>
    /// </summary>
    private MethodLocalVariablesWalker()
      : base(SyntaxWalkerDepth.Node)
    { }
    #endregion

    public static int Calculate(RoslynMemberNode memberNode)
    {
      //SyntaxNode memberNode2 = memberNode.SyntaxNode;
      BlockSyntax body = MemberBodySelector.FindBody(memberNode);
      if (body != null)
      {
        var walker = new MethodLocalVariablesWalker();
        walker.Visit(body);
        return walker._numLocalVariables;
      }
      return 0;
    }

    /// <summary>
    /// Called when the visitor visits a VariableDeclarationSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
    {
      base.VisitVariableDeclaration(node);
      for (int i = 0; i < node.Variables.Count; i++)
      {
        //VariableDeclaratorSyntax estaVariavel = node.Variables[i];
        checked
        {
          _numLocalVariables++;
        }
      }
    }
  }
}
