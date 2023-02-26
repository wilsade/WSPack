using System.Collections.Generic;
using System.Linq;

using EnvDTE;

using EnvDTE80;

using WSPack.Lib.Extensions;
using WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper;
using WSPack.VisualStudio.Shared.DocumentationObjects.Macros;
using WSPack.VisualStudio.Shared.Extensions;

namespace WSPack.VisualStudio.Shared.DocumentationObjects
{
  /// <summary>
  /// Objeto para montar o summary em um elemento
  /// </summary>
  public static class DocumentationSummaryObj
  {
    const string AbreTagSummary = "<summary>";
    private const string Str_Bases_Interfaces = "Bases/Interfaces";
    private const string Str_Mesmo_Arquivo = "Mesmo arquivo";

    internal static void IncludeSummaryInElement(IBaseElementEx baseElement, DocumentationParams documentationParams,
      TextSelection selection)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      DocumentationBaseItem summaryfromParams = baseElement.GetSummaryFromParams(documentationParams);
      List<XMLParamNode> arguments = baseElement.GetSummaryForArguments(documentationParams);

      string xmlContent = GetFullSummary(baseElement, summaryfromParams, arguments, documentationParams);
      baseElement.Comment(xmlContent);
      CheckMacroCaretPosition(selection);
    }

    /// <summary>
    /// Montar o nodo summary
    /// </summary>
    /// <param name="baseElement">Elemento que queremos colocar summary</param>
    /// <param name="rule">Regra encontrada</param>
    /// <param name="summaryDocObj">Estrutura XML existente do elemento</param>
    /// <param name="summaryDocObjFromBases">Estrutura XML de classe/interfaces bases</param>
    /// <param name="summaryDocObjSelf">Estrutura XML do arquivo .cs</param>
    static void SetSummaryPart_Summary(IBaseElementEx baseElement, DocumentationBaseItem rule,
      XMLSummaryDocObj summaryDocObj, XMLSummaryDocObj summaryDocObjFromBases, XMLSummaryDocObj summaryDocObjSelf)
    {
      if (summaryDocObj.SummaryNode.IsNullOrWhiteSpaceEx())
      {
        string quemDocumentou = string.Empty;
        string summaryRecuperado = string.Empty;
        if (!summaryDocObjSelf.SummaryNode.IsNullOrWhiteSpaceEx())
        {
          summaryRecuperado = summaryDocObjSelf.SummaryNodeAsXML;
          quemDocumentou = Str_Mesmo_Arquivo;
        }
        else if (!summaryDocObjFromBases.SummaryNode.IsNullOrWhiteSpaceEx())
        {
          summaryRecuperado = summaryDocObjFromBases.SummaryNodeAsXML;
          quemDocumentou = Str_Bases_Interfaces;
        }

        // Self ou Base
        if (!summaryRecuperado.IsNullOrWhiteSpaceEx())
        {
          if (!(baseElement is MethodElementEx method) || !method.IsConstructor)
            summaryDocObj.SummaryNode = summaryRecuperado;
          else
          {
            summaryDocObj.SummaryNode = rule.Summary;
            quemDocumentou = $"{rule.FriendlyName}: {rule.ItemName}";
          }
        }
        else
        {
          summaryDocObj.SummaryNode = rule.Summary;
          quemDocumentou = $"{rule.FriendlyName}: {rule.ItemName}";
        }

        if (!quemDocumentou.IsNullOrEmptyEx())
          BaseElementEx.ShowDocumentationWaring(baseElement.Name, quemDocumentou);
      }
    }

    /// <summary>
    /// Montar o(s) nodo(s) typeparam
    /// </summary>
    /// <param name="baseElement">Elemento que queremos colocar summary</param>
    /// <param name="summaryDocObj">Estrutura XML existente do elemento</param>
    /// <param name="summaryDocObjFromBases">Estrutura XML de classe/interfaces bases</param>
    /// <param name="lstAllSelfElements">Lista de todos os elementos presentes no mesmo arquivo .cs</param>
    /// <param name="documentationParams">Parâmetros de documentação</param>
    static void SetSummaryPart_GenericArgs(IBaseElementEx baseElement, XMLSummaryDocObj summaryDocObj,
      XMLSummaryDocObj summaryDocObjFromBases, IEnumerable<IBaseElementEx> lstAllSelfElements,
      DocumentationParams documentationParams)
    {
      List<XMLParamNode> lstArgumentosGenericosExistentes = null;
      List<XMLParamNode> lstArgumentosGenericosExistentesBase = null;
      IEnumerable<XMLParamNode> lstArgsGenericosFromSelf = null;

      List<XMLParamNode> lstArgsGenericos = baseElement.GetGenericArgs();
      if (lstArgsGenericos.Any())
      {
        lstArgumentosGenericosExistentes = summaryDocObj.GetGenericArgumentsNodeList();
        lstArgsGenericosFromSelf = lstAllSelfElements.SelectMany(x => x.GetGenericArgs());
        lstArgumentosGenericosExistentesBase = summaryDocObjFromBases.GetGenericArgumentsNodeList();
      }

      var lstQuem = new List<(string Nome, string QuemDocumentou)>();

      foreach (var meuGenerico in lstArgsGenericos)
      {
        // Manter o summary existente
        XMLParamNode achei = lstArgumentosGenericosExistentes.FirstOrDefault(x => x.ParamName.Equals(meuGenerico.ParamName));
        if (achei != null && !achei.ParamSummary.IsNullOrWhiteSpaceEx())
        {
          meuGenerico.ParamSummary = achei.ParamSummary;
          continue;
        }

        // Usar summary do mesmo arquivo
        achei = lstArgsGenericosFromSelf.FirstOrDefault(x => x.ParamName.Equals(meuGenerico.ParamName) && !x.ParamSummary.IsNullOrWhiteSpaceEx());
        if (achei != null)
        {
          meuGenerico.ParamSummary = achei.ParamSummary;
          lstQuem.Add((meuGenerico.ParamName, Str_Mesmo_Arquivo));
          continue;
        }

        // Usar summary do base
        achei = lstArgumentosGenericosExistentesBase.FirstOrDefault(x => x.ParamName.Equals(meuGenerico.ParamName));
        if (achei != null && !achei.ParamSummary.IsNullOrWhiteSpaceEx())
        {
          meuGenerico.ParamSummary = achei.ParamSummary;
          lstQuem.Add((meuGenerico.ParamName, Str_Bases_Interfaces));
          continue;
        }

        // TODO: argumentos genéricos são de que tipo no FileCodeModel? Se for, não precisamos receber 'documentationParams'
        var docFromDic = documentationParams.DictionaryList.FirstOrDefault(x => x.ItemName.Equals(meuGenerico.ParamName));
        if (docFromDic != null)
        {
          meuGenerico.ParamSummary = docFromDic.Summary;
          lstQuem.Add((meuGenerico.ParamName, docFromDic.FriendlyName));
          continue;
        }
      }
      summaryDocObj.SetGenericArgumentsNodeList(lstArgsGenericos);
      if (lstQuem.Any())
      {
        lstQuem.ForEach(x => BaseElementEx.ShowDocumentationWaring(x.Nome, x.QuemDocumentou));
      }
    }

    /// <summary>
    /// Montar o(s) nodo(s) param
    /// </summary>
    /// <param name="arguments">Arguments</param>
    /// <param name="summaryDocObj">Estrutura XML existente do elemento</param>
    /// <param name="summaryDocObjFromBases">Estrutura XML de classe/interfaces bases</param>
    /// <param name="lstAllSelfElements">Lista de todos os elementos presentes no mesmo arquivo .cs</param>
    static void SetSummaryPart_Parameters(List<XMLParamNode> arguments, XMLSummaryDocObj summaryDocObj,
          XMLSummaryDocObj summaryDocObjFromBases, IEnumerable<IBaseElementEx> lstAllSelfElements)
    {
      List<XMLParamNode> lstParametrosExistentes = null;
      List<XMLParamNode> lstParametrosExistentesBase = null;

      if (arguments.Any())
      {
        lstParametrosExistentesBase = summaryDocObjFromBases.GetParamNodeList();
        lstParametrosExistentes = summaryDocObj.GetParamNodeList();
      }

      var lstQuem = new List<(string Nome, string QuemDocumentou)>();
      foreach (var meuParametro in arguments)
      {
        string quemDocumentou = meuParametro.QuemDocumentou;

        // Summary de algum parâmetro do mesmo arquivo
        IBaseElementEx algumParametro = lstAllSelfElements.FirstOrDefault(x => x.Name.EqualsInsensitive(meuParametro.ParamName) && x.HasSummary);
        XMLSummaryDocObj summaryDocForParametro = XMLSummaryDocObj.LoadOrCreate(algumParametro?.GetDocComment());
        List<XMLParamNode> lstParametrosExistentesSelf = summaryDocForParametro.GetParamNodeList();
        XMLParamNode achei;
        if (lstParametrosExistentesSelf.Count == 0 && !summaryDocForParametro.SummaryNode.IsNullOrWhiteSpaceEx())
          achei = new XMLParamNode(meuParametro.ParamName, summaryDocForParametro.SummaryNode.Trim());
        else
          achei = lstParametrosExistentesSelf.FirstOrDefault(x => x.ParamName == meuParametro.ParamName);

        if (achei != null)
          quemDocumentou = Str_Mesmo_Arquivo;

        // Manter summary do parâmetro base
        if (achei == null)
        {
          achei = lstParametrosExistentesBase.FirstOrDefault(x => x.ParamName == meuParametro.ParamName);
          if (achei != null)
            quemDocumentou = Str_Bases_Interfaces;
        }

        // Manter summary do parâmetro existente
        if (achei == null)
          achei = lstParametrosExistentes.FirstOrDefault(x => x.ParamName == meuParametro.ParamName);

        if (achei != null && !achei.ParamSummary.IsNullOrWhiteSpaceEx())
        {
          meuParametro.ParamSummary = achei.ParamSummary;
        }

        if (!quemDocumentou.IsNullOrEmptyEx())
          lstQuem.Add((meuParametro.ParamName, quemDocumentou));
      }
      summaryDocObj.SetParamNodeList(arguments);
      foreach (var itemQuem in lstQuem)
      {
        BaseElementEx.ShowDocumentationWaring(itemQuem.Nome, itemQuem.QuemDocumentou);
      }
    }

    /// <summary>
    /// Montar o nodo returns
    /// </summary>
    /// <param name="baseElement">Elemento que queremos colocar summary</param>
    /// <param name="rule">Regra encontrada</param>
    /// <param name="summaryDocObj">Estrutura XML existente do elemento</param>
    /// <param name="summaryDocObjSelf">Estrutura XML do arquivo .cs</param>
    /// <param name="summaryDocObjFromBases">Estrutura XML de classe/interfaces bases</param>
    static void SetSummaryPart_Return(IBaseElementEx baseElement, DocumentationBaseItem rule,
      XMLSummaryDocObj summaryDocObj, XMLSummaryDocObj summaryDocObjSelf, XMLSummaryDocObj summaryDocObjFromBases)
    {
      // 4. Retorno
      if (baseElement.HasReturns)
      {
        // Self
        if (!summaryDocObjSelf.ReturnsNode.IsNullOrWhiteSpaceEx())
          summaryDocObj.ReturnsNode = summaryDocObjSelf.ReturnsNodeAsXML;

        // Base
        else if (!summaryDocObjFromBases.ReturnsNode.IsNullOrWhiteSpaceEx())
          summaryDocObj.ReturnsNode = summaryDocObjFromBases.ReturnsNodeAsXML;

        else if (summaryDocObj.ReturnsNode.IsNullOrWhiteSpaceEx())
          summaryDocObj.ReturnsNode = rule.Returns;
      }
      else
        summaryDocObj.DeleteNode(SummaryNodesNames.Returns);
    }

    /// <summary>
    /// Montar o nodo value
    /// </summary>
    /// <param name="baseElement">Elemento que queremos colocar summary</param>
    /// <param name="rule">Regra encontrada</param>
    /// <param name="summaryDocObj">Estrutura XML existente do elemento</param>
    /// <param name="summaryDocObjSelf">Estrutura XML do arquivo .cs</param>
    /// <param name="summaryDocObjFromBases">Estrutura XML de classe/interfaces bases</param>
    static void SetSummaryPart_Value(IBaseElementEx baseElement, DocumentationBaseItem rule,
      XMLSummaryDocObj summaryDocObj, XMLSummaryDocObj summaryDocObjSelf, XMLSummaryDocObj summaryDocObjFromBases)
    {
      if (baseElement.HasValue)
      {
        // Value do self
        if (!summaryDocObjSelf.ValueNode.IsNullOrWhiteSpaceEx())
          summaryDocObj.ValueNode = summaryDocObjSelf.ValueNodeAsXML;

        // Value do base
        else if (!summaryDocObjFromBases.ValueNode.IsNullOrWhiteSpaceEx())
          summaryDocObj.ValueNode = summaryDocObjFromBases.ValueNodeAsXML;

        else if (!rule.Returns.IsNullOrWhiteSpaceEx() && summaryDocObj.ValueNode.IsNullOrWhiteSpaceEx())
          summaryDocObj.ValueNode = rule.Returns;
      }
      else
        summaryDocObj.DeleteNode(SummaryNodesNames.Value);
    }

    /// <summary>
    /// Montar o nodo remarks
    /// </summary>
    /// <param name="rule">Regra encontrada</param>
    /// <param name="summaryDocObj">Estrutura XML existente do elemento</param>
    /// <param name="summaryDocObjSelf">Estrutura XML do arquivo .cs</param>
    /// <param name="summaryDocObjFromBases">Estrutura XML de classe/interfaces bases</param>
    static void SetSummaryPart_Remarks(DocumentationBaseItem rule, XMLSummaryDocObj summaryDocObj,
      XMLSummaryDocObj summaryDocObjSelf, XMLSummaryDocObj summaryDocObjFromBases)
    {
      string remarksRecuperado = string.Empty;
      if (!summaryDocObjSelf.RemarksNode.IsNullOrWhiteSpaceEx())
        remarksRecuperado = summaryDocObjSelf.RemarksNodeAsXML;
      else if (!summaryDocObjFromBases.RemarksNode.IsNullOrWhiteSpaceEx())
        remarksRecuperado = summaryDocObjFromBases.RemarksNodeAsXML;

      if (!remarksRecuperado.IsNullOrWhiteSpaceEx())
        summaryDocObj.RemarksNode = remarksRecuperado;
      else if (!rule.Remarks.IsNullOrWhiteSpaceEx())
        summaryDocObj.RemarksNode = rule.Remarks;
    }

    static string GetFullSummary(IBaseElementEx baseElement, DocumentationBaseItem rule,
      List<XMLParamNode> arguments, DocumentationParams documentationParams)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      /*
       * Recuperar o comentário para podermos atualizá-lo, se ele existir:
       * 1. Summary
       * 2. Argumentos genéricos
       * 3. Parâmetros
       * 4. Retorno
       *   4.1 value
       * 5. Remarks
       */
      XMLSummaryDocObj summaryDocObj = XMLSummaryDocObj.LoadOrCreate(baseElement.GetDocComment());
      XMLSummaryDocObj summaryDocObjFromBases = GetSummaryDocFromBases(baseElement);

      // Self
      IEnumerable<IBaseElementEx> lstAllSelfElements;

      // Usuário não quer utilizar; ou
      // Estamos documentando um tipo. Neste caso, não existirá outro tipo com mesmo nome no arquivo
      if (!WSPackPackage.ParametrosDocumentation.SearchSelfDocumentation || (baseElement is TypeElementEx ele && !ele.HasGenericArgs()))
      {
        lstAllSelfElements = new List<IBaseElementEx>();
      }
      else
      {
        var fcm = (FileCodeModel2)WSPackPackage.Dte.ActiveDocument.ProjectItem.FileCodeModel;
        lstAllSelfElements = GetSelfAllElements(fcm.CodeElements);
      }

      IBaseElementEx self = lstAllSelfElements.FirstOrDefault(x => x.Name.EqualsInsensitive(baseElement.Name) && x.HasSummary);

      // Estamos documentando um tipo e já temos construtor com summary.
      // Não queremos colocar o summary do construtor na documentacao da tipo
      if (self is MethodElementEx method && method.IsConstructor && baseElement is TypeElementEx)
        self = null;
      XMLSummaryDocObj summaryDocObjSelf = XMLSummaryDocObj.LoadOrCreate(self?.GetDocComment());

      // 1. Summary
      SetSummaryPart_Summary(baseElement, rule, summaryDocObj, summaryDocObjFromBases, summaryDocObjSelf);

      // 2. Argumentos genéricos
      SetSummaryPart_GenericArgs(baseElement, summaryDocObj, summaryDocObjFromBases, lstAllSelfElements, documentationParams);

      // 3. Parâmetros
      SetSummaryPart_Parameters(arguments, summaryDocObj, summaryDocObjFromBases, lstAllSelfElements);

      // 4. Retorno
      SetSummaryPart_Return(baseElement, rule, summaryDocObj, summaryDocObjSelf, summaryDocObjFromBases);

      // 4.1. Value
      SetSummaryPart_Value(baseElement, rule, summaryDocObj, summaryDocObjSelf, summaryDocObjFromBases);

      // 5. Remarks
      SetSummaryPart_Remarks(rule, summaryDocObj, summaryDocObjSelf, summaryDocObjFromBases);

      string montado = summaryDocObj.XMLContent;
      return montado;
    }

    private static void CheckMacroCaretPosition(TextSelection selection)
    {
      /*
       * 1. Se informamos a macro $(End)
       * 2. Descobrir a linha de início do <summary>
       * 3. Substituir a macro por string.Empty
       * 4. Posicionar o cursor na primeira ocorrência da macro
       */
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      WSPackPackage.Dte.SuppressUI = true;
      try
      {
        string MacroEnd = MacroBaseItems.GetMacroText(MacrosConsts.End);
        int linhaInicial = 0;
        var lstMacro = new List<(int Linha, int Posicao)>();
        selection.LineDown();
        string textoLinha;
        do
        {
          // Texto da linha atual
          selection.LineUp();

          selection.EndOfLine();
          EditPoint endPoint = selection.ActivePoint.CreateEditPoint();

          selection.StartOfLine(vsStartOfLineOptions.vsStartOfLineOptionsFirstColumn);
          EditPoint editPoint = selection.ActivePoint.CreateEditPoint();
          textoLinha = editPoint.GetText(selection.ActivePoint.LineLength);

          ReplaceXMLChars(editPoint, endPoint);

          // Tem macro?
          int posicao = textoLinha.IndexOf(MacroEnd);
          if (posicao > 0)
            lstMacro.Add((selection.CurrentLine, posicao));

          // Achou início do summary. Podemos sair
          if (textoLinha.Contains(AbreTagSummary))
          {
            linhaInicial = selection.CurrentLine + 1;
            break;
          }
        } while (!string.IsNullOrEmpty(textoLinha));

        // Limpar as macros
        if (lstMacro.Count > 0)
        {
          foreach (var (Linha, Posicao) in lstMacro)
          {
            selection.GotoLine(Linha);
            selection.StartOfLine(vsStartOfLineOptions.vsStartOfLineOptionsFirstColumn);
            EditPoint startPoint = selection.ActivePoint.CreateEditPoint();

            selection.EndOfLine();
            EditPoint endPoint = selection.ActivePoint.CreateEditPoint();

            startPoint.ReplacePattern(endPoint, MacroEnd, "");
          }
          selection.MoveToLineAndOffset(lstMacro[0].Linha, lstMacro[0].Posicao + 1);
        }
        else if (linhaInicial > 0)
        {
          selection.MoveToLineAndOffset(linhaInicial, 1);
          selection.EndOfLine();
        }
      }
      finally
      {
        WSPackPackage.Dte.SuppressUI = false;
      }
    }

    static void ReplaceXMLChars(EditPoint startPoint, EditPoint endPoint)
    {
      List<(string Code, string Meaning)> lst = new List<(string Code, string Meaning)>
      {
        ("&lt;", "<"),
        ("&gt;", ">"),
        ("&amp;", "&")
      };
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      foreach (var (Code, Meaning) in lst)
      {
        startPoint.ReplacePattern(endPoint, Code, Meaning);
        startPoint.StartOfLine();
      }
    }

    private static XMLSummaryDocObj GetSummaryDocFromBases(IBaseElementEx baseElement)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      if (!WSPackPackage.ParametrosDocumentation.SearchBaseDocumentation)
        return XMLSummaryDocObj.LoadOrCreate("");

      string baseComment = string.Empty;

      // Se estamos documentando um membro, vamos tentar recuperar o summary definido nos ancrestrais
      if (baseElement is MemberElementEx member)
      {
        MemberTypesEnum memberType = member.MemberType;

        // Propriedades, métodos e eventos
        if (memberType == MemberTypesEnum.Property || memberType == MemberTypesEnum.Event ||
          memberType == MemberTypesEnum.Method || memberType == MemberTypesEnum.Constructor)
        {
          // Toda hierarquia de herança
          List<CodeType> lstBases = member.DeclaringType.Element.GetAllBases();
          if (member.DeclaringType.Element is CodeClass codeClass)
            lstBases.AddRange(codeClass.GetAllInterfaces());

          // Apenas types que tenham o mesmo membro
          IEnumerable<MemberElementEx> lstMesmoMembro;
          switch (memberType)
          {
            case MemberTypesEnum.Property:
              lstMesmoMembro = lstBases.SelectMany(x =>
                    {
                      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
                      return x.Members.OfType<CodeProperty2>();
                    })
                .Where(m => m.Name == baseElement.Name)
                .Select(p => new PropertyElementEx(p));
              baseComment = lstMesmoMembro.FirstOrDefault(x => x.HasSummary)?.GetDocComment();
              break;
            case MemberTypesEnum.Method:

              lstMesmoMembro = lstBases.SelectMany(x =>
                    {
                      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
                      return x.Members.OfType<CodeFunction2>();
                    })
                .Where(m =>
                {
                  Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
                  return m.GetFunctionKindSafe() != vsCMFunction.vsCMFunctionConstructor &&
                    m.GetNameSafe() == baseElement.Name && m.Parameters.Count == baseElement.GetParams().Count;
                })
                .Select(p => new MethodElementEx(p)).Where(x => x.HasSummary);
              baseComment = CheckSameParams(baseElement, lstMesmoMembro);
              break;
            case MemberTypesEnum.Constructor:
              lstMesmoMembro = lstBases.SelectMany(x =>
                    {
                      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
                      return x.Members.OfType<CodeFunction2>();
                    })
                .Where(m =>
                {
                  Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
                  return m.GetFunctionKindSafe() == vsCMFunction.vsCMFunctionConstructor &&
                    m.Parameters.Count == baseElement.GetParams().Count;
                })
                .Select(p => new MethodElementEx(p)).Where(x => x.HasSummary);
              baseComment = CheckSameParams(baseElement, lstMesmoMembro);
              break;
            case MemberTypesEnum.Event:
              string tipoEventoMember = (member.Element as CodeEvent).Type.AsString;
              lstMesmoMembro = lstBases.SelectMany(x =>
                    {
                      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
                      return x.Members.OfType<CodeEvent>();
                    })
                .Where(m =>
                    {
                      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
                      return m.Name == baseElement.Name && m.Type.AsString == tipoEventoMember;
                    })
                .Select(p => new EventElementEx(p));
              baseComment = lstMesmoMembro.FirstOrDefault(x => x.HasSummary)?.GetDocComment();
              break;
            default:
              baseComment = string.Empty;
              break;
          }
        }
      }
      return XMLSummaryDocObj.LoadOrCreate(baseComment);
    }

    static IEnumerable<IBaseElementEx> GetSelfAllElements(CodeElements elements)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      foreach (CodeElement esteElemento in elements)
      {
        if (esteElemento is CodeType codeType)
        {
          yield return new TypeElementEx(codeType);

          // Parâmetros de delegate não são 'Children'
          if (esteElemento is CodeDelegate codeDelegate)
          {
            foreach (var esteItem in GetSelfAllElements(codeDelegate.Parameters))
            {
              yield return esteItem;
            }
          }
        }

        else if (esteElemento.IsCodeMember())
          yield return MemberElementEx.CreateInstance(esteElemento);
        foreach (var esteItem in GetSelfAllElements(esteElemento.Children))
        {
          yield return esteItem;
        }
      }
    }

    static string CheckSameParams(IBaseElementEx baseElement, IEnumerable<MemberElementEx> lstMesmoMembro)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      string baseComment = null;
      var lstParametrosBase = baseElement.GetParams().Select(x =>
          {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            return (CodeParameter)x.Element;
          }).ToList();
      foreach (var item in lstMesmoMembro)
      {
        bool igual = true;
        var lstParametros = item.GetParams().Select(x =>
            {
              Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
              return (CodeParameter)x.Element;
            }).ToList();
        for (int i = 0; i < lstParametros.Count; i++)
        {
          if (lstParametros[i].Type.AsString != lstParametrosBase[i].Type.AsString)
          {
            igual = false;
            break;
          }
        }
        if (igual)
        {
          baseComment = item.GetDocComment();
          break;
        }
      }
      return baseComment;
    }
  }
}
