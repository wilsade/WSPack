/*
 * SonarAnalyzer for .NET
 * Copyright (C) 2015-2019 SonarSource SA
 * mailto: contact AT sonarsource DOT com
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WSPack.Lib.CSharp.Internals
{
  /// <summary>
  /// Auxiliar para Walkers
  /// </summary>
  static class CSharpSyntaxWalkerHelper
  {
    /// <summary>
    /// SafeVisit
    /// </summary>
    /// <param name="syntaxWalker">Syntax walker</param>
    /// <param name="syntaxNode">Syntax node</param>
    /// <returns></returns>
    public static bool SafeVisit(this CSharpSyntaxWalker syntaxWalker, SyntaxNode syntaxNode)
    {
      try
      {
        syntaxWalker.Visit(syntaxNode);
        return true;
      }
      catch (InsufficientExecutionStackException)
      {
        // Roslyn walker overflows the stack when the depth of the call is around 2050.
        // See https://github.com/SonarSource/sonar-dotnet/issues/2115
        return false;
      }
    }

    /// <summary>
    /// HasExactlyNArguments
    /// </summary>
    /// <param name="invocation">Invocation</param>
    /// <param name="count">Count</param>
    /// <returns></returns>
    public static bool HasExactlyNArguments(this InvocationExpressionSyntax invocation, int count)
    {
      return invocation != null &&
          invocation.ArgumentList != null &&
          invocation.ArgumentList.Arguments.Count == count;
    }

    /// <summary>
    /// RemoveParentheses
    /// </summary>
    /// <param name="expression">Expression</param>
    /// <returns></returns>
    public static SyntaxNode RemoveParentheses(this SyntaxNode expression)
    {
      var currentExpression = expression;
      while (currentExpression?.IsKind(SyntaxKind.ParenthesizedExpression) ?? false)
      {
        currentExpression = ((ParenthesizedExpressionSyntax)currentExpression).Expression;
      }
      return currentExpression;
    }

    /// <summary>
    /// RemoveParentheses
    /// </summary>
    /// <param name="expression">Expression</param>
    /// <returns></returns>
    public static ExpressionSyntax RemoveParentheses(this ExpressionSyntax expression) =>
          (ExpressionSyntax)RemoveParentheses((SyntaxNode)expression);

  }
}
