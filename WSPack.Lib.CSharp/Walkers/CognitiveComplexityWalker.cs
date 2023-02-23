using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using WSPack.Lib.CSharp.Internals;

namespace WSPack.Lib.CSharp.Walkers
{
  internal sealed class CognitiveComplexityWalker : CSharpSyntaxWalker
  {
    /// <summary>
    /// Inicialização da classe: <see cref="CognitiveComplexityWalker"/>.
    /// </summary>
    private CognitiveComplexityWalker()
      : base(SyntaxWalkerDepth.Node)
    {

    }

    public static int Calculate(SyntaxNode node)
    {
      var walker = new CognitiveComplexityWalker();
      walker.SafeVisit(node);

      return walker.State.Complexity;
      //return new CognitiveComplexity(walker.State.Complexity, walker.State.IncrementLocations.ToImmutableArray());
    }

    public CognitiveComplexityWalkerState<MethodDeclarationSyntax> State { get; }
                = new CognitiveComplexityWalkerState<MethodDeclarationSyntax>();

    public override void Visit(SyntaxNode node)
    {
      if (node.IsKind(SyntaxKindEx.LocalFunctionStatement))
      {
        State.VisitWithNesting(node, base.Visit);
      }
      else
      {
        base.Visit(node);
      }
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
      State.CurrentMethod = node;
      base.VisitMethodDeclaration(node);

      if (State.HasDirectRecursiveCall)
      {
        State.HasDirectRecursiveCall = false;
        State.IncreaseComplexity(node.Identifier, 1, "+1 (recursion)");
      }
    }

    public override void VisitIfStatement(IfStatementSyntax node)
    {
      if (node.Parent.IsKind(SyntaxKind.ElseClause))
      {
        base.VisitIfStatement(node);
      }
      else
      {
        State.IncreaseComplexityByNestingPlusOne(node.IfKeyword);
        State.VisitWithNesting(node, base.VisitIfStatement);
      }
    }

    public override void VisitElseClause(ElseClauseSyntax node)
    {
      State.IncreaseComplexityByOne(node.ElseKeyword);
      base.VisitElseClause(node);
    }

    public override void VisitConditionalExpression(ConditionalExpressionSyntax node)
    {
      State.IncreaseComplexityByNestingPlusOne(node.QuestionToken);
      State.VisitWithNesting(node, base.VisitConditionalExpression);
    }

    public override void VisitSwitchStatement(SwitchStatementSyntax node)
    {
      State.IncreaseComplexityByNestingPlusOne(node.SwitchKeyword);
      State.VisitWithNesting(node, base.VisitSwitchStatement);
    }

    public override void VisitForStatement(ForStatementSyntax node)
    {
      State.IncreaseComplexityByNestingPlusOne(node.ForKeyword);
      State.VisitWithNesting(node, base.VisitForStatement);
    }

    public override void VisitWhileStatement(WhileStatementSyntax node)
    {
      State.IncreaseComplexityByNestingPlusOne(node.WhileKeyword);
      State.VisitWithNesting(node, base.VisitWhileStatement);
    }

    public override void VisitDoStatement(DoStatementSyntax node)
    {
      State.IncreaseComplexityByNestingPlusOne(node.DoKeyword);
      State.VisitWithNesting(node, base.VisitDoStatement);
    }

    public override void VisitForEachStatement(ForEachStatementSyntax node)
    {
      State.IncreaseComplexityByNestingPlusOne(node.ForEachKeyword);
      State.VisitWithNesting(node, base.VisitForEachStatement);
    }

    public override void VisitCatchClause(CatchClauseSyntax node)
    {
      State.IncreaseComplexityByNestingPlusOne(node.CatchKeyword);
      State.VisitWithNesting(node, base.VisitCatchClause);
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
      if (State.CurrentMethod != null &&
          node.Expression is IdentifierNameSyntax identifierNameSyntax &&
          node.HasExactlyNArguments(State.CurrentMethod.ParameterList.Parameters.Count) &&
          string.Equals(identifierNameSyntax.Identifier.ValueText,
              State.CurrentMethod.Identifier.ValueText, StringComparison.Ordinal))
      {
        State.HasDirectRecursiveCall = true;
      }

      base.VisitInvocationExpression(node);
    }

    public override void VisitBinaryExpression(BinaryExpressionSyntax node)
    {
      var nodeKind = node.Kind();
      if (!State.LogicalOperationsToIgnore.Contains(node) &&
          (nodeKind == SyntaxKind.LogicalAndExpression ||
           nodeKind == SyntaxKind.LogicalOrExpression))
      {
        var left = node.Left.RemoveParentheses();
        if (!left.IsKind(nodeKind))
        {
          State.IncreaseComplexityByOne(node.OperatorToken);
        }

        var right = node.Right.RemoveParentheses();
        if (right.IsKind(nodeKind))
        {
          State.LogicalOperationsToIgnore.Add(right);
        }
      }

      base.VisitBinaryExpression(node);
    }

    public override void VisitGotoStatement(GotoStatementSyntax node)
    {
      State.IncreaseComplexityByNestingPlusOne(node.GotoKeyword);
      base.VisitGotoStatement(node);
    }

    public override void VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
    {
      State.VisitWithNesting(node, base.VisitSimpleLambdaExpression);
    }

    public override void VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
    {
      State.VisitWithNesting(node, base.VisitParenthesizedLambdaExpression);
    }

  }
}
