using Microsoft.CodeAnalysis;

namespace WSPack.Lib.CSharp.Internals
{
  /// <summary>
  /// Definir um membro e seu tipo
  /// </summary>
  internal class RoslynMemberNode
  {
    #region Construtores
    /// <summary>
    /// Cria uma instância da classe <seealso cref="RoslynMemberNode"/>
    /// </summary>
    public RoslynMemberNode()
    {
    }

    /// <summary>
    /// Cria uma instância da classe <seealso cref="RoslynMemberNode"/>
    /// </summary>
    /// <param name="syntaxNode">(Gets or sets) Nodo da árvore sintática</param>
    /// <param name="kind">(Gets or sets) Tipo de membro</param>
    public RoslynMemberNode(SyntaxNode syntaxNode, MemberKindEnum kind)
    {
      SyntaxNode = syntaxNode;
      Kind = kind;
    }
    #endregion

    /// <summary>
    /// (Gets or sets) Nodo da árvore sintática
    /// </summary>
    public SyntaxNode SyntaxNode { get; set; }

    /// <summary>
    /// (Gets or sets) Tipo de membro
    /// </summary>
    public MemberKindEnum Kind { get; set; }
  }
}
