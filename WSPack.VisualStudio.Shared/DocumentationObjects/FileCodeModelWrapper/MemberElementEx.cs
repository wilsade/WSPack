using EnvDTE;

using EnvDTE80;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WSPack.Lib.Extensions;

namespace WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper
{
  /// <summary>
  /// Represente um membro do FileCodeModel: propriedade, método/construtor, evento, campo, parâmetro
  /// </summary>
  public abstract class MemberElementEx : BaseElementEx, IBaseElementEx
  {
    TypeElementEx _declaringType;

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="MemberElementEx"/>
    /// </summary>
    /// <param name="codeElement">Code element</param>
    public MemberElementEx(CodeElement2 codeElement)
      : base(codeElement)
    {

    }
    #endregion

    /// <summary>
    /// Criar um membro conforme o escopo
    /// </summary>
    /// <param name="codeElement">Element</param>
    /// <returns>Instância do membro</returns>
    public static MemberElementEx CreateInstance(CodeElement codeElement)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      // Propriedades
      if (codeElement is CodeProperty2 property)
        return new PropertyElementEx(property);

      // Métdoos
      else if (codeElement is CodeFunction2 function)
        return new MethodElementEx(function);

      // Parâmetros
      else if (codeElement is CodeParameter2 parameter)
        return new ParameterElementEx(parameter);

      // Campos
      else if (codeElement is CodeVariable2 codeVariable)
        return new FieldElementEx(codeVariable);

      // Eventos
      else if (codeElement.Kind == vsCMElement.vsCMElementEvent)
        return new EventElementEx((CodeEvent)codeElement);

      else
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retornar o tipo que declara o membro
    /// </summary>
    public override TypeElementEx DeclaringType
    {
      get
      {
        if (_declaringType == null)
          _declaringType = GetDeclaringType();
        return _declaringType;
      }
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
      else if (GetSummaryForMemberFromParams(MemberType, documentationParams, out documentationItem))
      {
        MatchedDocumentationRule = documentationItem;
        CheckMacros(documentationParams, documentationItem);
        return documentationItem;
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
      return new List<MemberElementEx>();
    }

    /// <summary>
    /// Recuperar o tipo do membro
    /// </summary>
    public abstract MemberTypesEnum MemberType { get; }

    /// <summary>
    /// Tipo do elemento
    /// </summary>
    public abstract CodeTypeRef2 ElementType { get; }

    /// <summary>
    /// Recuperar o tipo que declara o membro
    /// </summary>
    /// <returns>Declaring type</returns>
    protected abstract TypeElementEx GetDeclaringType();

    /*
    bool IsProperty(out CodeProperty code)
    {
      if (Element is CodeProperty prop)
      {
        code = prop;
        return true;
      }
      code = null;
      return false;
    }

    bool IsMethod(out CodeFunction code)
    {
      if (Element is CodeFunction function)
      {
        code = function;
        return true;
      }
      code = null;
      return false;
    }*/

    /// <summary>
    /// Recuperar o Summary de um membro com base nos parâmetros de documentação
    /// </summary>
    /// <param name="memberEnum">Tipo de membro para recuperação de summary</param>
    /// <param name="documentationParams">Parâmetros de documentação</param>
    /// <param name="documentationItem">Documentação do membro conforme parametrização; DefaultSummary se não foi encontrada regra para este membro</param>
    /// <returns>true se foi encontrada regra para este membro</returns>
    bool GetSummaryForMemberFromParams(MemberTypesEnum memberEnum, DocumentationParams documentationParams,
      out DocumentationBaseItem documentationItem)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      var lstToCheck = documentationParams.RuleList.OfType<MemberRuleItem>()
        .Where(x => x.MemberType == MemberTypesEnum.All || x.MemberType == memberEnum)
        .OrderBy(x => x.Id);

      foreach (MemberRuleItem esteRegra in lstToCheck)
      {
        if (GetSummary(memberEnum, esteRegra, out documentationItem))
        {
          return true;
        }
      }
      documentationItem = new DictionaryItem(Name, DefaultSummary, true);
      return false;
    }

    bool GetSummary(MemberTypesEnum memberTypes, MemberRuleItem memberRule, out DocumentationBaseItem documentationItem)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      bool achouNome = AnalizeCondition(memberRule.NameCondition, Name);

      if (achouNome)
      {
        // Regra para qualquer membro não pode ter Any no nome e no Type
        if (memberRule.MemberType == MemberTypesEnum.All && memberRule.NameCondition.SearchCondition == SearchConditionsEnum.Any &&
          memberRule.TypeCondition.SearchCondition != SearchConditionsEnum.Any)
          achouNome = false;
      }

      if (achouNome)
      {
        if (memberTypes == MemberTypesEnum.Event || memberTypes == MemberTypesEnum.Field ||
          memberTypes == MemberTypesEnum.Parameters || memberTypes == MemberTypesEnum.Property)
        {
          string nomeTipo = ElementType.AsString;
          achouNome = AnalizeCondition(memberRule.TypeCondition, nomeTipo);
        }
        else if (memberTypes == MemberTypesEnum.Method)
        {
          CodeFunction2 metodo = (CodeFunction2)Element;
          string nomeRetorno = metodo.Type.AsString;
          achouNome = AnalizeCondition(memberRule.TypeCondition, nomeRetorno);
        }
      }

      if (achouNome)
      {
        documentationItem = memberRule.Clone();
        if (documentationItem.Summary.IsNullOrWhiteSpaceEx())
          documentationItem.Summary = DefaultSummary;
      }
      else
        documentationItem = new DictionaryItem(Name, DefaultSummary, true);

      return achouNome;
    }

  }
}
