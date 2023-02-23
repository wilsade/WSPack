using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WSPack.Lib.CSharp.Internals
{
  /// <summary>
  /// Objeto para selecionar o Body de um membro
  /// </summary>
  internal static class MemberBodySelector
  {
    public static BlockSyntax FindBody(RoslynMemberNode node)
    {
      Dictionary<MemberKindEnum, Func<SyntaxNode, BlockSyntax>> dictionary = new Dictionary<MemberKindEnum, Func<SyntaxNode, BlockSyntax>>
      {
        { MemberKindEnum.Method, (SyntaxNode x) => ((MethodDeclarationSyntax)x).Body },
        { MemberKindEnum.Constructor, (SyntaxNode x) => ((ConstructorDeclarationSyntax)x).Body },
        { MemberKindEnum.Destructor, (SyntaxNode x) => ((DestructorDeclarationSyntax)x).Body },
        { MemberKindEnum.GetProperty, (SyntaxNode x) => GetPropertyAccessorBody((PropertyDeclarationSyntax)x, SyntaxKind.GetAccessorDeclaration) },
        { MemberKindEnum.SetProperty, (SyntaxNode x) => GetPropertyAccessorBody((PropertyDeclarationSyntax)x, SyntaxKind.SetAccessorDeclaration) },
        { MemberKindEnum.AddEventHandler, (SyntaxNode x) => GetEventAccessorBody((EventDeclarationSyntax)x, SyntaxKind.AddAccessorDeclaration) },
        { MemberKindEnum.RemoveEventHandler, (SyntaxNode x) => GetEventAccessorBody((EventDeclarationSyntax)x, SyntaxKind.RemoveAccessorDeclaration) }
      };
      Dictionary<MemberKindEnum, Func<SyntaxNode, BlockSyntax>> dictionary2 = dictionary;
      BlockSyntax result;
      if (dictionary2.TryGetValue(node.Kind, out Func<SyntaxNode, BlockSyntax> func) && (result = func(node.SyntaxNode)) != null)
      {
        return result;
      }
      return null;
    }

    /*
    private static BlockSyntax GetAccessorBody(AccessorListSyntax syntax, SyntaxKind kind)
    {
      AccessorDeclarationSyntax accessorDeclarationSyntax = syntax.Accessors.SingleOrDefault((AccessorDeclarationSyntax a) => a.Kind() == kind);
      if (accessorDeclarationSyntax != null)
      {
        return accessorDeclarationSyntax.Body;
      }
      return null;
    }*/

    static BlockSyntax GetBody(BasePropertyDeclarationSyntax syntax, SyntaxKind kind)
    {
      AccessorDeclarationSyntax accessorDeclarationSyntax = syntax.AccessorList.Accessors.SingleOrDefault((AccessorDeclarationSyntax a) => a.Kind() == kind);
      if (accessorDeclarationSyntax != null)
      {
        return accessorDeclarationSyntax.Body;
      }
      return null;
    }

    private static BlockSyntax GetPropertyAccessorBody(PropertyDeclarationSyntax syntax, SyntaxKind kind)
    {
      return GetBody(syntax, kind);
    }

    private static BlockSyntax GetEventAccessorBody(EventDeclarationSyntax syntax, SyntaxKind kind)
    {
      return GetBody(syntax, kind);
    }

  }
}
