using System;
using System.Collections.Generic;

using Microsoft.CodeAnalysis;

namespace WSPack.Lib.CSharp.Internals
{
  class CognitiveComplexityWalkerState<TMethodSyntax>
      where TMethodSyntax : SyntaxNode
  {
    public TMethodSyntax CurrentMethod { get; set; }

    public List<SecondaryLocation> IncrementLocations { get; } = new List<SecondaryLocation>();

    // used to track logical operations inside parentheses
    public List<SyntaxNode> LogicalOperationsToIgnore { get; } = new List<SyntaxNode>();

    public bool HasDirectRecursiveCall { get; set; }

    public int NestingLevel { get; set; }

    public int Complexity { get; set; } = 1;

    public void VisitWithNesting<TSyntaxNode>(TSyntaxNode node, Action<TSyntaxNode> visit)
    {
      NestingLevel++;
      visit(node);
      NestingLevel--;
    }

    public void IncreaseComplexityByOne(SyntaxToken token)
    {
      IncreaseComplexity(token, 1, "+1");
    }

    public void IncreaseComplexityByNestingPlusOne(SyntaxToken token)
    {
      var increment = NestingLevel + 1;
      var message = increment == 1
          ? "+1"
          : $"+{increment} (incl {increment - 1} for nesting)";
      IncreaseComplexity(token, increment, message);
    }

    public void IncreaseComplexity(SyntaxToken token, int increment, string message)
    {
      Complexity += increment;
      IncrementLocations.Add(new SecondaryLocation(token.GetLocation(), message));
    }
  }
}
