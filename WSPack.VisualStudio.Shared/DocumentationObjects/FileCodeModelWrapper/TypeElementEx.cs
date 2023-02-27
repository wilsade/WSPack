using EnvDTE;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WSPack.Lib.DocumentationObjects;
using WSPack.Lib.Extensions;

namespace WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper
{
  /// <summary>
  /// Representa um tipo do FileCodeModel: class, interface, enum, delegate, struct
  /// </summary>
  public class TypeElementEx : BaseElementEx, IBaseElementEx
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="TypeElementEx"/>
    /// </summary>
    /// <param name="codeType">CodeType</param>
    public TypeElementEx(CodeType codeType)
      : base(codeType)
    {
      Element = codeType;
    }
    #endregion

    bool GetSummary(TypeRuleItem typeRule, out DocumentationBaseItem documentationItem)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      bool achou = AnalizeCondition(typeRule.NameCondition, Name);
      if (achou)
      {
        documentationItem = typeRule.Clone();
        if (documentationItem.Summary.IsNullOrWhiteSpaceEx())
          documentationItem.Summary = DefaultSummary;
      }
      else
        documentationItem = new DictionaryItem(Name, "", false);
      return achou;
    }

    /// <summary>
    /// Recuperar a Documentação de um type com base nos parâmetros de documentação
    /// </summary>
    /// <param name="typesEnum">Tipo de type para recuperação de summary</param>
    /// <param name="documentationParams">Parâmetros de documentação</param>
    /// <param name="documentationItem">Documentação do type conforme parametrização; DefaultSummary se não foi encontrado</param>
    /// <returns>true se foi encontrada regra para este type</returns>
    bool GetSummaryForTypeFromParams(TypeTypesEnum typesEnum, DocumentationParams documentationParams,
      out DocumentationBaseItem documentationItem)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      var lstToCheck = documentationParams.RuleList.OfType<TypeRuleItem>()
        .Where(x => x.TypeType == TypeTypesEnum.All || x.TypeType == typesEnum)
        .OrderBy(x => x.Id);

      foreach (TypeRuleItem esteRegra in lstToCheck)
      {
        if (GetSummary(esteRegra, out documentationItem))
        {
          CheckMacros(documentationParams, documentationItem);
          return true;
        }
      }

      documentationItem = new DictionaryItem(Name, DefaultSummary, true);
      return false;
    }

    #region Propriedades
    /// <summary>
    /// Element
    /// </summary>
    public new CodeType Element { get; }

    /// <summary>
    /// Indica se o elemento possui a tag returns
    /// </summary>
    public override bool HasReturns
    {
      get
      {
        Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
        return Element is EnvDTE80.CodeDelegate2 codeDelegate &&
          !(codeDelegate.Type.TypeKind == vsCMTypeRef.vsCMTypeRefVoid);
      }
    }
    #endregion

    #region Métodos
    /// <summary>
    /// Retornar o tipo <see cref="TypeTypesEnum"/> de um Elemento
    /// </summary>
    /// <param name="typeTypes">Tipo</param>
    /// <returns>true se o tipo do Elemento foi encontrado</returns>
    public bool GetTypeType(out TypeTypesEnum typeTypes)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

      // Classe
      if (Element is CodeClass)
      {
        typeTypes = TypeTypesEnum.Classes;
        return true;
      }

      // Interface
      else if (Element is CodeInterface)
      {
        typeTypes = TypeTypesEnum.Interfaces;
        return true;
      }

      // Enum
      else if (Element is CodeEnum)
      {
        typeTypes = TypeTypesEnum.Enums;
        return true;
      }

      // Struct
      else if (Element is CodeStruct)
      {
        typeTypes = TypeTypesEnum.Structs;
        return true;
      }

      // Delegate
      else if (Element.Kind == vsCMElement.vsCMElementDelegate)
      {
        typeTypes = TypeTypesEnum.Delegates;
        return true;
      }

      else
      {
        typeTypes = TypeTypesEnum.All;
        return false;
      }
    }

    /// <summary>
    /// Incluir comentário no elemento
    /// </summary>
    /// <param name="xmlContent">Estrutura do comentário</param>
    public override void Comment(string xmlContent)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      Element.DocComment = xmlContent;
    }

    /// <summary>
    /// Recuperar o comentário do summary presente no elemento.
    /// O comentário possui uma estrutura XML
    /// </summary>
    /// <returns>comentário do summary presente no elemento.</returns>
    public override string GetDocComment()
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      return Element.DocComment;
    }

    /// <summary>
    /// Recuperar a regra de documentação (se definido) para este elemento
    /// </summary>
    /// <param name="documentationParams">Parâmetros de documentação</param>
    /// <returns>Documentação para este elemento</returns>
    public override DocumentationBaseItem GetSummaryFromParams(DocumentationParams documentationParams)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      // Achei o summary no dicionário
      if (GetSummaryFromDictionary(documentationParams, out DocumentationBaseItem documentationItem))
      {
        MatchedDocumentationRule = documentationItem;
        return documentationItem;
      }

      // Procura o summary nas regras
      else if (GetTypeType(out TypeTypesEnum typeTypes))
      {
        if (GetSummaryForTypeFromParams(typeTypes, documentationParams, out documentationItem))
        {
          if (documentationItem.Summary.IsNullOrWhiteSpaceEx())
            documentationItem.Summary = DefaultSummary;
          MatchedDocumentationRule = documentationItem;
          return documentationItem;
        }
      }
      var item = new DictionaryItem(Name, DefaultSummary, true);
      MatchedDocumentationRule = item;
      CheckMacros(documentationParams, item);
      return item;
    }

    /// <summary>
    /// Recuperar os parâmetros do elemento
    /// </summary>
    /// <returns>parâmetros do elemento; lista vazia se o elemento não possui parâmetros</returns>
    public override List<MemberElementEx> GetParams()
    {
      var lst = new List<MemberElementEx>();
      if (Element is EnvDTE80.CodeDelegate2 codeDelegate)
      {
        foreach (CodeElement esteParametro in codeDelegate.Parameters)
        {
          lst.Add(new ParameterElementEx((EnvDTE80.CodeParameter2)esteParametro));
        }
      }
      return lst;
    }
    #endregion
  }
}
