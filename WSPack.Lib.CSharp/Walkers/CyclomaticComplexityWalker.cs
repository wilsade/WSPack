using System.Collections.Generic;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using WSPack.Lib.CSharp.Internals;

namespace WSPack.Lib.CSharp.Walkers
{
  internal sealed class CyclomaticComplexityWalker : CSharpSyntaxWalker
  {
    int _counter;

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="CyclomaticComplexityWalker"/>
    /// </summary>
    private CyclomaticComplexityWalker()
      : base(SyntaxWalkerDepth.Node)
    {
      _counter = 1;
    }
    #endregion

    /// <summary>
    /// Calculatar a complexidade ciclomática
    /// </summary>
    /// <param name="node">Node</param>
    /// <returns>Complexidade ciclomática</returns>
    public static int Calculate(RoslynMemberNode node)
    {
      int counter = 1;
      BlockSyntax body = MemberBodySelector.FindBody(node);
      if (body != null)
      {
        var walker = new CyclomaticComplexityWalker();
        walker.Visit(body);
        counter = walker._counter;
      }

      int num = LogicalComplexityWalker.Calculate(body);
      return checked(counter + num);
    }

    /// <summary>
    /// Called when the visitor visits a IfStatementSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitIfStatement(IfStatementSyntax node)
    {
      base.VisitIfStatement(node);
      checked
      {
        _counter++;
      }
    }

    /// <summary>
    /// Called when the visitor visits a GotoStatementSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitGotoStatement(GotoStatementSyntax node)
    {
      base.VisitGotoStatement(node);
      checked
      {
        _counter++;
      }
    }

    /// <summary>
    /// Called when the visitor visits a ConditionalExpressionSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitConditionalExpression(ConditionalExpressionSyntax node)
    {
      base.VisitConditionalExpression(node);
      checked
      {
        if (node.QuestionToken.Kind() == SyntaxKind.QuestionToken && node.ColonToken.Kind() == SyntaxKind.ColonToken)
        {
          _counter++;
        }
      }
    }

    /// <summary>
    /// Called when the visitor visits a BinaryExpressionSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitBinaryExpression(BinaryExpressionSyntax node)
    {
      base.VisitBinaryExpression(node);
      SyntaxKind syntaxKind = node.OperatorToken.Kind();
      checked
      {
        if (syntaxKind == SyntaxKind.QuestionQuestionToken)
        {
          _counter++;
        }
      }
    }

    /// <summary>
    /// Called when the visitor visits a ElseClauseSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitElseClause(ElseClauseSyntax node)
    {
      base.VisitElseClause(node);
      checked
      {
        _counter++;
      }
    }

    /// <summary>
    /// Called when the visitor visits a InitializerExpressionSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitInitializerExpression(InitializerExpressionSyntax node)
    {
      void CheckParent(SyntaxNode parentNode)
      {
        string chave = parentNode.Parent.ToString();
        if (!_dicInializerDictionary.ContainsKey(chave))
        {
          _dicInializerDictionary.Add(chave, 1);
          checked
          {
            _counter++;
          }
        }
      }

      base.VisitInitializerExpression(node);
      if (node.Kind() == SyntaxKind.CollectionInitializerExpression)
      {
        CheckParent(node.Parent);
      }
      else if (node.Parent.Kind() == SyntaxKind.CollectionInitializerExpression)
      {
        CheckParent(node.Parent.Parent);
      }
      else
      {
        checked
        {
          _counter++;
        }
      }
    }
    readonly Dictionary<string, int> _dicInializerDictionary = new Dictionary<string, int>();

    /// <summary>
    /// Called when the visitor visits a SwitchSectionSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitSwitchSection(SwitchSectionSyntax node)
    {
      base.VisitSwitchSection(node);
      checked
      {
        _counter++;
      }
    }

    /// <summary>
    /// Called when the visitor visits a WhileStatementSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitWhileStatement(WhileStatementSyntax node)
    {
      base.VisitWhileStatement(node);
      checked
      {
        _counter++;
      }
    }

    /// <summary>
    /// Called when the visitor visits a DoStatementSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitDoStatement(DoStatementSyntax node)
    {
      base.VisitDoStatement(node);
      checked
      {
        _counter++;
      }
    }

    /// <summary>
    /// Called when the visitor visits a ForEachStatementSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitForEachStatement(ForEachStatementSyntax node)
    {
      base.VisitForEachStatement(node);
      checked
      {
        _counter++;
      }
    }

    /// <summary>
    /// Called when the visitor visits a ForStatementSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitForStatement(ForStatementSyntax node)
    {
      base.VisitForStatement(node);
      checked
      {
        _counter++;
      }
    }



  }
}
