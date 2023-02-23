using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WSPack.Lib.CSharp.Walkers
{
  /// <summary>
  /// Objeto para calcular a complexidade lógica de um membro
  /// </summary>
  /// <seealso cref="CSharpSyntaxWalker" />
  internal sealed class LogicalComplexityWalker : CSharpSyntaxWalker
  {
    internal int _counter;

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="LogicalComplexityWalker"/>
    /// </summary>
    private LogicalComplexityWalker()
      : base(SyntaxWalkerDepth.Node)
    {

    }
    #endregion

    public static int Calculate(BlockSyntax body)
    {
      int counter = 0;
      if (body != null)
      {
        var walker = new LogicalComplexityWalker();
        walker.Visit(body);
        counter = walker._counter;
      }
      return counter;
    }

    /// <summary>
    /// Called when the visitor visits a BinaryExpressionSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitBinaryExpression(BinaryExpressionSyntax node)
    {
      base.VisitBinaryExpression(node);
      SyntaxKind syntaxKind = node.Kind();
      SyntaxKind syntaxKind2 = syntaxKind;
      switch (syntaxKind2)
      {
        case SyntaxKind.LogicalOrExpression:
        case SyntaxKind.LogicalAndExpression:
          break;
        case SyntaxKind.BitwiseOrExpression:
        case SyntaxKind.BitwiseAndExpression:
        case SyntaxKind.ExclusiveOrExpression:
        case SyntaxKind.EqualsExpression:
        case SyntaxKind.NotEqualsExpression:
        case SyntaxKind.LessThanExpression:
        case SyntaxKind.LessThanOrEqualExpression:
        case SyntaxKind.GreaterThanExpression:
        case SyntaxKind.GreaterThanOrEqualExpression:
          return;
        default:
          if (syntaxKind2 != SyntaxKind.LogicalNotExpression)
          {
            return;
          }
          break;
      }
      checked
      {
        _counter++;
      }
    }

  }
}
