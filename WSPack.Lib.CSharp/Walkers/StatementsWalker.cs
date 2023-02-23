using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using WSPack.Lib.CSharp.Internals;

namespace WSPack.Lib.CSharp.Walkers
{
  /// <summary>
  /// Objeto para analisar comandos
  /// </summary>
  /// <seealso cref="CSharpSyntaxWalker" />
  internal sealed class StatementsWalker : CSharpSyntaxWalker
  {
    int _counter;

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="StatementsWalker"/>
    /// </summary>
    private StatementsWalker()
      : base(SyntaxWalkerDepth.Node)
    {

    }
    #endregion

    public static int CalculateLineNumbers(RoslynMemberNode node)
    {
      var walker = new StatementsWalker();

      BlockSyntax body = MemberBodySelector.FindBody(node);
      if (body != null)
        walker.Visit(body);

      walker.CalculateConstructorStatements(node);
      walker.CalculateCompilerGeneratedPropertyStatements(node);
      walker.CalculateMethodExpression(node);
      return walker._counter;
    }

    private void CalculateConstructorStatements(RoslynMemberNode node)
    {
      checked
      {
        ConstructorDeclarationSyntax constructorDeclarationSyntax;
        if (node.Kind == MemberKindEnum.Constructor && (constructorDeclarationSyntax = node.SyntaxNode as ConstructorDeclarationSyntax) != null && constructorDeclarationSyntax.Initializer != null)
        {
          Visit(constructorDeclarationSyntax.Initializer);
          _counter++;
        }
      }
    }

    private void CalculateCompilerGeneratedPropertyStatements(RoslynMemberNode node)
    {
      checked
      {
        switch (node.Kind)
        {
          case MemberKindEnum.GetProperty:
          case MemberKindEnum.SetProperty:
            if (MemberBodySelector.FindBody(node) == null)
            {
              _counter++;
            }
            return;
          default:
            return;
        }
      }
    }

    private void CalculateMethodExpression(RoslynMemberNode node)
    {
      checked
      {
        if (node.Kind == MemberKindEnum.Method)
        {
          MethodDeclarationSyntax method = node.SyntaxNode as MethodDeclarationSyntax;
          if (method.ExpressionBody != null)
          {
            _counter++;
          }
        }
      }
    }

    public override void VisitInitializerExpression(InitializerExpressionSyntax node)
    {
      base.VisitInitializerExpression(node);
      checked
      {
        _counter += node.Expressions.Count;
      }
    }

    public override void VisitUsingDirective(UsingDirectiveSyntax node)
    {
      base.VisitUsingDirective(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitBreakStatement(BreakStatementSyntax node)
    {
      base.VisitBreakStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitCheckedStatement(CheckedStatementSyntax node)
    {
      base.VisitCheckedStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitContinueStatement(ContinueStatementSyntax node)
    {
      base.VisitContinueStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitDoStatement(DoStatementSyntax node)
    {
      base.VisitDoStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitEmptyStatement(EmptyStatementSyntax node)
    {
      base.VisitEmptyStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitExpressionStatement(ExpressionStatementSyntax node)
    {
      base.VisitExpressionStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitFixedStatement(FixedStatementSyntax node)
    {
      base.VisitFixedStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitForEachStatement(ForEachStatementSyntax node)
    {
      base.VisitForEachStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitForStatement(ForStatementSyntax node)
    {
      base.VisitForStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitGlobalStatement(GlobalStatementSyntax node)
    {
      base.VisitGlobalStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitGotoStatement(GotoStatementSyntax node)
    {
      base.VisitGotoStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitIfStatement(IfStatementSyntax node)
    {
      base.VisitIfStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitLabeledStatement(LabeledStatementSyntax node)
    {
      base.VisitLabeledStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
    {
      base.VisitLocalDeclarationStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitLockStatement(LockStatementSyntax node)
    {
      base.VisitLockStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitReturnStatement(ReturnStatementSyntax node)
    {
      base.VisitReturnStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitSwitchStatement(SwitchStatementSyntax node)
    {
      base.VisitSwitchStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitThrowStatement(ThrowStatementSyntax node)
    {
      base.VisitThrowStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitUnsafeStatement(UnsafeStatementSyntax node)
    {
      base.VisitUnsafeStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitUsingStatement(UsingStatementSyntax node)
    {
      base.VisitUsingStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitWhileStatement(WhileStatementSyntax node)
    {
      base.VisitWhileStatement(node);
      checked
      {
        _counter++;
      }
    }

    public override void VisitYieldStatement(YieldStatementSyntax node)
    {
      base.VisitYieldStatement(node);
      checked
      {
        _counter++;
      }
    }

  }
}
