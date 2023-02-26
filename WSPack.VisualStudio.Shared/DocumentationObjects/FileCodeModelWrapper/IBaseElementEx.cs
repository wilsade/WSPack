using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper
{
  /// <summary>
  /// Definir características comuns para elementos do FileCodeModel
  /// </summary>
  public interface IBaseElementEx
  {
    /// <summary>
    /// Nome do elemento
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Nome completo do elemento
    /// </summary>
    string FullName { get; }

    /// <summary>
    /// Linha onde o elemento se encontra
    /// </summary>
    int Line { get; }

    /// <summary>
    /// Indica se o elemento possui summary
    /// </summary>
    bool HasSummary { get; }

    /// <summary>
    /// Indica se o elemento possui a TAG 'return' na estrutura de comentário do summary
    /// </summary>
    bool HasReturns { get; }

    /// <summary>
    /// Indica se o elemento possui a TAG 'value' na estrutura de comentário do summary
    /// </summary>
    bool HasValue { get; }

    /// <summary>
    /// Recuperar o comentário do summary presente no elemento.
    /// O comentário possui uma estrutura XML
    /// </summary>
    /// <returns>comentário do summary presente no elemento.</returns>
    string GetDocComment();

    /// <summary>
    /// Incluir comentário no elemento
    /// </summary>
    /// <param name="xmlContent">Estrutura do comentário</param>
    void Comment(string xmlContent);

    /// <summary>
    /// Recuperar a regra de documentação (se definido) para este elemento
    /// </summary>
    /// <param name="documentationParams">Parâmetros de documentação</param>
    /// <returns>Documentação para este elemento</returns>
    DocumentationBaseItem GetSummaryFromParams(DocumentationParams documentationParams);

    /// <summary>
    /// Recuperar o summary dos parâmetros do método
    /// </summary>
    /// <param name="documentationParams">Parâmetros de documentação</param>
    List<XMLParamNode> GetSummaryForArguments(DocumentationParams documentationParams);

    /// <summary>
    /// Retornar o tipo que declara o membro
    /// </summary>
    TypeElementEx DeclaringType { get; }

    /// <summary>
    /// Recuperar os parâmetros do elemento
    /// </summary>
    /// <returns>parâmetros do elemento; lista vazia se o elemento não possui parâmetros</returns>
    List<MemberElementEx> GetParams();

    /// <summary>
    /// Recuperar os parâmetros genéricos
    /// </summary>
    /// <returns>Parâmetros genéricos</returns>
    List<XMLParamNode> GetGenericArgs();
  }

}
