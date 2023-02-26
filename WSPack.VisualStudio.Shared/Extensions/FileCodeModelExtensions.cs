using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnvDTE;

using WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper;

namespace WSPack.VisualStudio.Shared.Extensions
{
  /// <summary>
  /// Métodos estendidos para FileCodeModel
  /// </summary>
  public static class FileCodeModelExtensions
  {
    static readonly vsCMElement[] _lstScopesForTypes = new vsCMElement[] {
        vsCMElement.vsCMElementEnum,
        vsCMElement.vsCMElementStruct,
        vsCMElement.vsCMElementDelegate,
        vsCMElement.vsCMElementInterface,
        vsCMElement.vsCMElementClass
      };

    static readonly vsCMElement[] _lstScopesForMembers = new vsCMElement[] {
        vsCMElement.vsCMElementFunction,
        vsCMElement.vsCMElementProperty,
        vsCMElement.vsCMElementEvent,
        vsCMElement.vsCMElementVariable
      };

    static void GetAllInheritance(List<CodeType> lstBases, CodeType codeType)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      foreach (CodeType item in codeType.Bases.OfType<CodeType>())
      {
        if (item.FullName != typeof(object).FullName)
          lstBases.Add(item);

        if (item is CodeClass codeClass)
          lstBases.AddRange(codeClass.GetAllInterfaces());

        GetAllInheritance(lstBases, item);
      }
    }

    /// <summary>
    /// Procura por um elemento a partir da linha base ou nas linhas abaixo
    /// </summary>
    /// <param name="basePoint">Base point</param>
    /// <returns>Elemento; null se não encontrado</returns>
    public static IBaseElementEx FindElement(this TextPoint basePoint)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      IBaseElementEx baseElement = null;
      TextSelection selection = basePoint.Parent.Selection;

      int needToGo = 0;
      while (baseElement == null && needToGo <= 10)
      {
        selection.EndOfLine();
        selection.CharLeft(Count: 1);
        VirtualPoint vp = selection.ActivePoint;

        baseElement = vp.GetTypeAtLine();
        if (baseElement == null)
          baseElement = vp.GetMemberAtLine();
        if (baseElement != null)
          break;
        selection.LineDown();
        needToGo++;
      }
      return baseElement;
    }

    /// <summary>
    /// Recuperar o type presente na linha exata de uma determinada posição
    /// </summary>
    /// <param name="searchPoint">Start point</param>
    /// <returns>Type na posição; null se não encontrado</returns>
    public static TypeElementEx GetTypeAtLine(this VirtualPoint searchPoint)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      EditPoint startPoint = searchPoint.CreateEditPoint();
      TextSelection selection = searchPoint.Parent.Selection;

      TypeElementEx TryFind(VirtualPoint vp)
      {
        foreach (var esteScope in _lstScopesForTypes)
        {
          try
          {
            CodeElement element = vp.get_CodeElement(esteScope);
            if (element != null && element.StartPoint.Line == vp.Line)
            {
              return new TypeElementEx((CodeType)element);
            }
          }
          catch (Exception ex)
          {
            Utils.LogDebugError($"Erro no TryFind do GetTypeAtLine: {ex.Message}");
          }
        }
        return null;
      }

      try
      {
        // Achou no local do cursor
        TypeElementEx achouType = TryFind(searchPoint);
        if (achouType != null)
          return achouType;

        // Procurar para direita
        while (!searchPoint.AtEndOfLine)
        {
          selection.WordRight(false, 1);
          achouType = TryFind(searchPoint);
          if (achouType != null)
            break;
        }
        if (achouType != null)
          return achouType;

        // Procurar para esquerda
        selection.MoveToPoint(startPoint);
        while (!searchPoint.AtStartOfLine)
        {
          selection.WordLeft(false, 1);
          achouType = TryFind(searchPoint);
          if (achouType != null)
            break;
        }

        return achouType;
      }
      catch (Exception ex)
      {
        System.Diagnostics.Trace.WriteLine("Não foi possível recuperar o Type: " + ex.Message);
        return null;
      }
      finally
      {
        selection.MoveToPoint(startPoint);
      }
    }

    /// <summary>
    /// Recuperar o member presente na linha exata de uma determinada posição
    /// </summary>
    /// <param name="searchPoint">Start point</param>
    /// <param name="includeThisScope">Incluir este elemento na busca</param>
    /// <returns>Member na posição; null se não encontrado</returns>
    public static MemberElementEx GetMemberAtLine(this VirtualPoint searchPoint,
      vsCMElement? includeThisScope = null)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      EditPoint startPoint = searchPoint.CreateEditPoint();
      TextSelection selection = searchPoint.Parent.Selection;

      var lstScopes = _lstScopesForMembers.ToList();
      if (includeThisScope != null)
        lstScopes.Insert(0, includeThisScope.Value);

      MemberElementEx TryFind(VirtualPoint vp)
      {
        foreach (var esteScope in lstScopes)
        {
          try
          {
            CodeElement element = vp.get_CodeElement(esteScope);

            int acheiLinha = -1;
            if (element != null)
            {
              // Em princípio, esta será a linha do elemento, a menos que tenhamos um campo com atributo.
              acheiLinha = element.StartPoint.Line;
              if (esteScope == vsCMElement.vsCMElementVariable && AchaLinhaCampo(element, out var linhaAux))
                acheiLinha = linhaAux;
            }

            if (element != null && acheiLinha == vp.Line)
            {
              if (esteScope == vsCMElement.vsCMElementFunction)
              {
                var temp = new MethodElementEx((EnvDTE80.CodeFunction2)element);
                if (!temp.IsGetOrSet)
                  return temp;
              }
              else
                return MemberElementEx.CreateInstance(element);
            }
          }
          catch (Exception ex)
          {
            Utils.LogDebugError($"Erro no TryFind do GetMemberAtLine: {ex.Message}");
          }
        }
        return null;
      }

      try
      {
        // Achou no local do cursor
        MemberElementEx achouMember = TryFind(searchPoint);
        if (achouMember != null)
          return achouMember;

        // Procurar para direita
        while (!searchPoint.AtEndOfLine)
        {
          selection.WordRight(false, 1);
          achouMember = TryFind(searchPoint);
          if (achouMember != null)
            break;
        }
        if (achouMember != null)
          return achouMember;

        // Procurar para esquerda
        selection.MoveToPoint(startPoint);
        while (!searchPoint.AtStartOfLine)
        {
          selection.WordLeft(false, 1);
          achouMember = TryFind(searchPoint);
          if (achouMember != null)
            break;
        }

        return achouMember;
      }
      catch (Exception ex)
      {
        System.Diagnostics.Trace.WriteLine("Não foi possível recuperar o Member: " + ex.Message);
        return null;
      }
      finally
      {
        selection.MoveToPoint(startPoint);
      }
    }

    private static bool AchaLinhaCampo(CodeElement element, out int linha)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      //var scopes = new vsCMPart[] { vsCMPart.vsCMPartWholeWithAttributes, vsCMPart.vsCMPartNavigate, vsCMPart.vsCMPartAttributesWithDelimiter };
      var scopes = new vsCMPart[] { vsCMPart.vsCMPartNavigate };
      linha = -1;
      foreach (vsCMPart esteScope in scopes)
      {
        System.Diagnostics.Trace.WriteLine(esteScope.ToString());
        try
        {
          var start = element.GetStartPoint(esteScope);
          if (start != null)
          {
            linha = start.Line;
            System.Diagnostics.Trace.WriteLine(linha);
            return true;
          }
        }
        catch (Exception ex)
        {
          System.Diagnostics.Trace.WriteLine(ex.Message);
        }
      }
      return false;
    }

    /// <summary>
    /// Recuperar o type onde o cursor se encontra
    /// </summary>
    /// <param name="fileCodeModel">FileCodeModel.</param>
    /// <returns>CodeElement presentando uma classe/interface/enum/struct/delegate; null, se o cursor não estiver em um type válido</returns>
    public static TypeElementEx GetTypeWithinCursor(this EnvDTE80.FileCodeModel2 fileCodeModel)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      TypeElementEx Procura(VirtualPoint editPoint)
      {
        try
        {
          foreach (var esteScope in _lstScopesForTypes)
          {
            CodeElement element = editPoint.get_CodeElement(esteScope);
            if (element != null)
            {
              return new TypeElementEx((CodeType)element);
            }
          }
        }
        catch (Exception ex)
        {
          System.Diagnostics.Trace.WriteLine("Não foi possível recuperar o Type: " + ex.Message);
        }
        return null;
      }

      var selecao = fileCodeModel.Parent.Document.Selection as TextSelection;
      var point = selecao.ActivePoint;
      var achou = Procura(point);
      EditPoint oldEditPoint = null;
      if (achou == null)
      {
        oldEditPoint = selecao.ActivePoint.CreateEditPoint();
        selecao.EndOfLine(false);

        try
        {
          int needToGo = 0;
          while (achou == null && (needToGo <= 50))
          {
            selecao.WordLeft(false, 1);
            point = selecao.ActivePoint;
            if (point.AtStartOfLine)
              break;
            achou = Procura(point);
            needToGo++;
          }
        }
        catch (Exception ex2)
        {
          Utils.LogDebugError($"Erro ao tentar recuperar o membro à esquerda: {ex2.Message}");
        }
      }
      if (oldEditPoint != null)
      {
        try
        {
          //selecao.MoveToPoint(oldEditPoint);
        }
        catch (Exception ex3)
        {
          Utils.LogDebugError($"Não foi possível voltar com a posição do cursor: {ex3.Message}");
        }
      }
      return achou;

    }

    /// <summary>
    /// Recuperar o membro onde o cursor se encontra
    /// </summary>
    /// <param name="fileCodeModel">FileCodeModel.</param>
    /// <returns>CodeProperty ou CodeMethod ou CodeEvent; null, se o cursor estiver em outro membro</returns>
    public static IBaseElementEx GetMemberWithinCursor(this EnvDTE80.FileCodeModel2 fileCodeModel)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      IBaseElementEx Procura(VirtualPoint editPoint)
      {
        try
        {
          foreach (var esteScope in _lstScopesForMembers)
          {
            CodeElement element = editPoint.get_CodeElement(esteScope);
            if (element != null)
            {
              return MemberElementEx.CreateInstance(element);
            }
          }
        }
        catch (Exception ex)
        {
          Utils.LogDebugError("Não foi possível recuperar o Member: " + ex.Message);
        }
        return null;
      }

      var selecao = fileCodeModel.Parent.Document.Selection as TextSelection;
      var point = selecao.ActivePoint;
      var achou = Procura(point);
      EditPoint oldEditPoint = null;
      if (achou == null)
      {
        oldEditPoint = selecao.ActivePoint.CreateEditPoint();
        selecao.EndOfLine(false);

        try
        {
          int needToGo = 0;
          while (achou == null && (needToGo <= 50))
          {
            selecao.WordLeft(false, 1);
            point = selecao.ActivePoint;
            if (point.AtStartOfLine)
              break;
            achou = Procura(point);
            needToGo++;
          }
        }
        catch (Exception ex2)
        {
          Utils.LogDebugError($"Erro ao tentar recuperar o membro à esquerda: {ex2.Message}");
        }
      }
      if (oldEditPoint != null)
      {
        try
        {
          selecao.MoveToPoint(oldEditPoint);
        }
        catch (Exception ex3)
        {
          Utils.LogDebugError($"Não foi possível voltar com a posição do cursor: {ex3.Message}");
        }
      }
      return achou;
    }

    /// <summary>
    /// Recuperar recursivamente todas as classes bases
    /// </summary>
    /// <param name="codeType">Tipo</param>
    /// <returns>todas as classes bases</returns>
    public static List<CodeType> GetAllBases(this CodeType codeType)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      var lstBases = new List<CodeType>();
      GetAllInheritance(lstBases, codeType);
      return lstBases;
    }

    /// <summary>
    /// Recuperar recursivamente todas as interfaces
    /// </summary>
    /// <param name="codeClass">Classe</param>
    /// <returns>todas as interfaces</returns>
    public static List<CodeType> GetAllInterfaces(this CodeClass codeClass)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      List<CodeType> lstInterfaces = codeClass.ImplementedInterfaces.OfType<CodeType>().ToList();
      var lstRecursive = new List<CodeType>();
      foreach (var item in lstInterfaces)
      {
        GetAllInheritance(lstRecursive, item);
      }
      lstInterfaces.AddRange(lstRecursive);
      return lstInterfaces;
    }

    /// <summary>
    /// Indica se o Elemento é um membro
    /// </summary>
    /// <param name="codeElement">Elemento a ser analisado</param>
    /// <returns>true se o elemento é um membro</returns>
    public static bool IsCodeMember(this CodeElement codeElement)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      return codeElement is CodeProperty ||
        codeElement is CodeFunction ||
        codeElement is CodeVariable ||
        codeElement.Kind == vsCMElement.vsCMElementEvent ||
        codeElement is CodeParameter;
    }

    /// <summary>
    /// Recuperar o nome da maneira segura
    /// </summary>
    /// <param name="codeFunction">Code function</param>
    /// <returns>Nome; ou string.Empty em caso de erro</returns>
    public static string GetNameSafe(this CodeFunction codeFunction)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      try
      {
        return codeFunction.Name;
      }
      catch (Exception ex)
      {
        System.Diagnostics.Trace.WriteLine(ex.Message);
        return string.Empty;
      }
    }

    /// <summary>
    /// Recuperar o tipo do método
    /// </summary>
    /// <param name="codeFunction">Code function</param>
    /// <returns>Tipo do método; vsCMFunction.vsCMFunctionOther em caso de erro</returns>
    public static vsCMFunction GetFunctionKindSafe(this CodeFunction codeFunction)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      try
      {
        return codeFunction.FunctionKind;
      }
      catch (Exception ex)
      {
        System.Diagnostics.Trace.WriteLine($"Erro ao recuperar FunctionKind: {ex.Message}");
        return vsCMFunction.vsCMFunctionOther;
      }
    }
  }
}
