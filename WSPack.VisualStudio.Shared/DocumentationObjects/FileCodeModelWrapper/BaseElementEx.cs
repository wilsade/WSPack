using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;
using WSPack.Lib.Extensions;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using WSPack.VisualStudio.Shared.DocumentationObjects.Macros;

namespace WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper
{
  /// <summary>
  /// Classe base comum para CodeElement
  /// </summary>
  [DebuggerDisplay("{Name}")]
  public abstract class BaseElementEx : IBaseElementEx
  {
    string _name;

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="BaseElementEx"/>
    /// </summary>
    public BaseElementEx(CodeElement element)
    {
      _name = null;
      Element = element;
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="BaseElementEx"/>
    /// </summary>
    public BaseElementEx(CodeType codeTypeToCastToCodeElement)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      _name = null;
      Element = (CodeElement)codeTypeToCastToCodeElement;
    }
    #endregion

    /// <summary>
    /// Element
    /// </summary>
    public CodeElement Element { get; }

    /// <summary>
    /// Linha onde o elemento se encontra
    /// </summary>
    public int Line
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        return Element.StartPoint.Line;
      }
    }

    /// <summary>
    /// Nome do elemento
    /// </summary>
    public string Name
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        if (_name == null)
        {
          _name = Element.Name;
          int indice = _name.LastIndexOf(".");
          if (indice >= 0)
            _name = _name.Substring(indice + 1);
          if (_name.StartsWith("@"))
            _name = _name.Substring(1);
        }
        return _name;
      }
    }

    /// <summary>
    /// Nome completo do elemento
    /// </summary>
    public string FullName
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        return Element.FullName;
      }
    }

    /// <summary>
    /// Indica se o elemento possui summary
    /// </summary>
    public bool HasSummary
    {
      get
      {
        string summary = GetDocComment();
        if (summary.IsNullOrWhiteSpaceEx())
          return false;
        return !IsXMLEmpty(summary);
      }
    }

    /// <summary>
    /// Indica se o elemento possui a TAG 'return' na estrutura de comentário do summary
    /// </summary>
    public virtual bool HasReturns => false;

    /// <summary>
    /// Indica se o elemento possui a TAG 'value' na estrutura de comentário do summary
    /// </summary>
    public virtual bool HasValue => false;

    /// <summary>
    /// Retornar o tipo que declara o membro
    /// </summary>
    public virtual TypeElementEx DeclaringType => null;

    /// <summary>
    /// Recuperar o comentário do summary presente no elemento.
    /// O comentário possui uma estrutura XML
    /// </summary>
    /// <returns>comentário do summary presente no elemento.</returns>
    public abstract string GetDocComment();

    /// <summary>
    /// Incluir comentário no elemento
    /// </summary>
    /// <param name="xmlContent">Estrutura do comentário</param>
    public abstract void Comment(string xmlContent);

    /// <summary>
    /// Recuperar a regra de documentação (se definido) para este elemento
    /// </summary>
    /// <param name="documentationParams">Parâmetros de documentação</param>
    /// <returns>Documentação para este elemento</returns>
    public abstract DocumentationBaseItem GetSummaryFromParams(DocumentationParams documentationParams);

    /// <summary>
    /// Recuperar o summary dos parâmetros do método
    /// </summary>
    /// <param name="documentationParams">Parâmetros de documentação</param>
    public List<XMLParamNode> GetSummaryForArguments(DocumentationParams documentationParams)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      IEnumerable<MemberElementEx> lstParams = GetParams();
      var lstRetorno = new List<XMLParamNode>();
      foreach (MemberElementEx esteParametro in lstParams)
      {
        DocumentationBaseItem documentationForParam = esteParametro.GetSummaryFromParams(documentationParams);
        if (documentationForParam.Summary == esteParametro.Name)
          documentationForParam.Summary = documentationForParam.Summary.ToFirstCharToUpper();

        var nodo = new XMLParamNode(esteParametro.Name, documentationForParam.Summary)
        {
          QuemDocumentou = $"{documentationForParam.FriendlyName}: {documentationForParam.ItemName}"
        };
        lstRetorno.Add(nodo);
      }
      return lstRetorno;
    }

    /// <summary>
    /// Recuperar os parâmetros do elemento
    /// </summary>
    /// <returns>parâmetros do elemento; lista vazia se o elemento não possui parâmetros</returns>
    public abstract List<MemberElementEx> GetParams();

    /// <summary>
    /// Indica se o Elemento possui argumentos genéricos
    /// </summary>
    /// <returns>true se o Elemento possui argumentos genéricos</returns>
    public bool HasGenericArgs()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      return HasGenericArgs(out _);
    }

    /// <summary>
    /// Indica se o Elemento possui argumentos genéricos
    /// </summary>
    /// <param name="genericParts">Nomes dos argumentos genéricos</param>
    /// <returns>true se o Elemento possui argumentos genéricos</returns>
    public bool HasGenericArgs(out string[] genericParts)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      string fullName = Element.FullName;
      int pos = fullName.LastIndexOf(".");
      if (pos >= 0)
      {
        string nameWithGenericArgs = fullName.Substring(pos + 1);
        nameWithGenericArgs = nameWithGenericArgs.Replace(Name, "");
        genericParts = nameWithGenericArgs.Split(
          new char[] { '<', '>', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
        return genericParts.Any();
      }
      genericParts = new string[] { };
      return false;
    }

    /// <summary>
    /// Recuperar os parâmetros genéricos
    /// </summary>
    /// <returns>Parâmetros genéricos</returns>
    public List<XMLParamNode> GetGenericArgs()
    {
      ThreadHelper.ThrowIfNotOnUIThread();

      // Gerar para Types?
      if (!WSPackPackage.ParametrosDocumentation.GenerateForGenericTypesInTypes &&
        this is TypeElementEx)
        return new List<XMLParamNode>();

      // Gerar para Members?
      if (!WSPackPackage.ParametrosDocumentation.GenerateForGenericTypesInMembers &&
        this is MemberElementEx)
        return new List<XMLParamNode>();

      var lstRetorno = new List<XMLParamNode>();
      if (HasGenericArgs(out var genericParts))
      {
        string xmlComment = GetDocComment();
        XMLSummaryDocObj summaryParentDocObj = XMLSummaryDocObj.LoadOrCreate(xmlComment);
        var lstArgsDoSummary = summaryParentDocObj.GetGenericArgumentsNodeList();

        foreach (var estePart in genericParts)
        {
          XMLParamNode achei = lstArgsDoSummary.FirstOrDefault(x => x.ParamName.Equals(estePart));
          lstRetorno.Add(new XMLParamNode(estePart, achei?.ParamSummary));
        }
      }

      return lstRetorno;
    }

    /// <summary>
    /// Indica a regra de documentação encontrada para este item
    /// </summary>
    public DocumentationBaseItem MatchedDocumentationRule { get; protected set; }

    /// <summary>
    /// Exibir aviso no output ao documentar um elemento
    /// </summary>
    /// <param name="name">Nome do elemento</param>
    /// <param name="quemDocumentou">Quem documentou o elemento</param>
    public static void ShowDocumentationWaring(string name, string quemDocumentou)
    {
      if (WSPackPackage.ParametrosDocumentation.ShowOutputDocumentationWaring)
      {
        string msg = $"Summary para o elemento: {name} - Documentado por: {quemDocumentou}";
        Utils.LogOutputMessage(msg);
      }
    }

    /// <summary>
    /// summary padrão caso o elemento não seja encontrado
    /// </summary>
    protected string DefaultSummary => this is TypeElementEx ?
      WSPackPackage.ParametrosDocumentation.DefaultSummaryForTypes :
      WSPackPackage.ParametrosDocumentation.DefaultSummaryForMembers;

    /// <summary>
    /// Recuperar o summary a partir do Dicionário de documentação
    /// </summary>
    /// <param name="documentation">Documentation</param>
    /// <param name="documentationItem">Documentação do item; DefaultSummary caso o elemento não seja encontrado</param>
    /// <returns>true se o elemento está presente no dicionário</returns>
    protected bool GetSummaryFromDictionary(DocumentationParams documentation, out DocumentationBaseItem documentationItem)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var achei = documentation.DictionaryList.FirstOrDefault(x => x.ItemName.Equals(Name));
      if (achei != null)
      {
        documentationItem = achei;
        return true;
      }
      documentationItem = new DictionaryItem(Name, DefaultSummary, true);
      return false;
    }

    /// <summary>
    /// Verificar se um elemento é encontrado conforme uma <see cref="ConditionItem"/>
    /// </summary>
    /// <param name="condition">Condição a ser analisada</param>
    /// <param name="searchToken">O que vai ser procurado. Ex: Nome do elemento, Tipo ou Tipo de retorno</param>
    /// <returns>true se o elemento foi encontrado na regra</returns>
    protected static bool AnalizeCondition(ConditionItem condition, string searchToken)
    {
      // IgnoreCase?
      StringComparison stringComparison = condition.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

      // Valida qualquer elemento
      if (condition.SearchCondition == SearchConditionsEnum.Any)
      {
        return true;
      }

      // Nome exato
      else if (condition.SearchCondition == SearchConditionsEnum.Equals)
      {
        if (string.Equals(searchToken, condition.NameValue, stringComparison))
        {
          return true;
        }
      }

      // Começa com
      else if (condition.SearchCondition == SearchConditionsEnum.StartsWith)
      {
        if (searchToken.StartsWith(condition.NameValue, stringComparison))
        {
          return true;
        }
      }

      // Termina com
      else if (condition.SearchCondition == SearchConditionsEnum.EndsWith)
      {
        if (searchToken.EndsWith(condition.NameValue, stringComparison))
        {
          return true;
        }
      }

      // Contém
      else if (condition.SearchCondition == SearchConditionsEnum.Contains)
      {
        if (searchToken.IndexOf(condition.NameValue, stringComparison) >= 0)
        {
          return true;
        }
      }

      // Expressão regular
      else
      {
        if (Regex.IsMatch(searchToken, condition.NameValue, condition.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None))
        {
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Verificar se temos macros definidas.
    /// Se tivermos, vamos substituir
    /// </summary>
    /// <param name="documentationParams">Parâmetros de documentação</param>
    /// <param name="documentationItem">Documentação do item; DefaultSummary caso o elemento não seja encontrado</param>
    protected void CheckMacros(DocumentationParams documentationParams, DocumentationBaseItem documentationItem)
    {
      List<MacroBaseItems> lstAllMacros = new List<MacroBaseItems>();

      void GetRecursive(MacroGroupItems grupo)
      {
        lstAllMacros.AddRange(grupo.MacroList);
        foreach (var esteGrupo in grupo.SubGroupsList)
        {
          GetRecursive(esteGrupo);
        }
      }

      // Recuperar todas as macros excetuando a macro $(End)
      GetRecursive(MacroEnvironmentItems.CreateGroup());
      GetRecursive(MacroElementItems.CreateGroup());
      List<MacroGroupItems> lst = MacroMemberItems.CreateGroup();
      lst.ForEach(x => GetRecursive(x));

      // Verificar no summary, returns e remarks se temos alguma macro
      foreach (var esteMacro in lstAllMacros.Where(x => x.Code != MacrosConsts.End))
      {
        bool temMacro = documentationItem.Summary.IndexOf(esteMacro.MacroText) >= 0;
        if (temMacro)
          documentationItem.Summary = documentationItem.Summary.Replace(esteMacro.MacroText, esteMacro.ExecuteMacro(this));

        temMacro = documentationItem.Returns.IndexOf(esteMacro.MacroText) >= 0;
        if (temMacro)
          documentationItem.Returns = documentationItem.Returns.Replace(esteMacro.MacroText, esteMacro.ExecuteMacro(this));

        temMacro = documentationItem.Remarks.IndexOf(esteMacro.MacroText) >= 0;
        if (temMacro)
          documentationItem.Remarks = documentationItem.Remarks.Replace(esteMacro.MacroText, esteMacro.ExecuteMacro(this));
      }

      CheckAbbreviations(documentationParams.AbbreviationList, documentationItem);
    }

    /// <summary>
    /// Verificar se todos os nodos do XML estão vazios
    /// </summary>
    /// <returns>true se todos os nodos do XML estão vazios</returns>
    bool IsXMLEmpty(string xmlContent)
    {
      var doc = new XmlDocument();
      try
      {
        doc.LoadXml(xmlContent);
        XmlElement root = doc["doc"];
        if (root != null)
        {
          foreach (XmlElement esteNodo in root.ChildNodes)
          {
            if (!esteNodo.InnerText.IsNullOrWhiteSpaceEx())
            {
              if (esteNodo.Attributes["name"]?.InnerText != esteNodo.InnerText)
                return false;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Utils.LogDebugMessage($"IsXMLEmpty: {ex.Message}");
      }
      return true;
    }

    private void CheckAbbreviations(List<DictionaryItem> abbreviationList, DocumentationBaseItem documentationItem)
    {
      const string regexPalavraInteira = "\\b{0}\\b";

      foreach (var estaAbreviacao in abbreviationList)
      {
        documentationItem.Summary = Regex.Replace(documentationItem.Summary, regexPalavraInteira.FormatWith(estaAbreviacao.ItemName), estaAbreviacao.Summary);
        documentationItem.Returns = Regex.Replace(documentationItem.Returns, regexPalavraInteira.FormatWith(estaAbreviacao.ItemName), estaAbreviacao.Summary);
        documentationItem.Remarks = Regex.Replace(documentationItem.Remarks, regexPalavraInteira.FormatWith(estaAbreviacao.ItemName), estaAbreviacao.Summary);
      }
    }
  }
}
