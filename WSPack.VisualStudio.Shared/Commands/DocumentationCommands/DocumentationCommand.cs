using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using EnvDTE;

using EnvDTE80;

using Microsoft.VisualStudio.Shell;

using WSPack.Lib.DocumentationObjects;
using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.DocumentationObjects;
using WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para Documentação
  /// </summary>
  internal sealed class DocumentationCommand : BaseKeyPressCommand
  {
    internal const string PatternSee = "<see cref=\"{0}\"/>";

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.Documentation;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentationCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public DocumentationCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (!CreateKeyBindings("GerarDocumentacaoCodeEditor", "Text Editor::Ctrl+Alt+D", false))
        CreateKeyBindings("GerarDocumentacaoCodeEditor", "Editor de Texto::Ctrl+Alt+D", false);
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      string local = WSPackPackage.Dte?.ActiveDocument?.FullName;
      _menu.Enabled = !local.IsNullOrEmptyEx() &&
        File.Exists(local) &&
        Path.GetExtension(local).EqualsInsensitive(".cs");
      _menu.ParametersDescription = local;
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static DocumentationCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new DocumentationCommand(package, commandService);
    }

    /// <summary>
    /// Acontece antes de digitar uma tecla
    /// </summary>
    /// <param name="keypress">Tecla que está para ser pressionada</param>
    /// <param name="selection">Selection</param>
    /// <param name="inStatementCompletion">In statement completion</param>
    /// <param name="cancelKeypress">true para cancelar a digitação da tecla</param>
    protected override void BeforeKeyPress(string keypress, TextSelection selection, bool inStatementCompletion,
          ref bool cancelKeypress)
    {
      ThreadHelper.ThrowIfNotOnUIThread();

      // Usuário não quer usar na digitação de ///
      if (!WSPackPackage.ParametrosDocumentation.GenerateForTripleSlash)
        return;

      // Não tem FileCodeModel
      if (WSPackPackage.Dte?.ActiveDocument?.ProjectItem?.FileCodeModel == null)
        return;

      var editPoint = selection.ActivePoint.CreateEditPoint();
      editPoint.StartOfLine();
      string textoLinha = editPoint.GetText(editPoint.LineLength);

      if (keypress == "/" && textoLinha.Trim() == "//")
      {
        // Se o cursor está em um namespace, sai!
        string proximaLinha = selection.GetNextLine();
        if (proximaLinha.TrimStart().StartsWith("namespace", StringComparison.Ordinal))
        {
          return;
        }

        cancelKeypress = true;
        Utils.LogDebugMessage($"Vou selecionar a linha: {selection.CurrentLine}");
        selection.SelectLine();
        Utils.LogDebugMessage($"Vou fazer Delete(1): {selection.CurrentLine}");
        selection.Delete(1);
        Utils.LogDebugMessage($"Como ficou o Delete: {selection.CurrentLine}");
        IBaseElementEx baseElement = editPoint.FindElement();

        // Se achou um Type, vamos verificar se também acha um membro. Se achar, o membro tem prioridade
        if (baseElement is TypeElementEx)
        {
          Utils.LogDebugMessage($"Achei tipo: {baseElement.Name} ({baseElement.Line})");
          if (WSPackPackage.Dte.ActiveDocument?.ProjectItem?.FileCodeModel is FileCodeModel2 fcm)
          {
            IBaseElementEx baseElementMembro = fcm.GetMemberWithinCursor();
            if (baseElementMembro != null)
            {
              Utils.LogDebugMessage($"Achei membro: {baseElementMembro.Name} ({baseElementMembro.Line})");
              baseElement = baseElementMembro;
            }
          }
        }

        if (baseElement != null)
        {
          WSPackPackage.Dte.SuppressUI = true;
          try
          {
            var docParams = DocumentationUtils.ReadDocumentationParams(WSPackConsts.DocumentationConfigPath);
            DocumentationSummaryObj.IncludeSummaryInElement(baseElement, docParams, selection);
          }
          finally
          {
            WSPackPackage.Dte.SuppressUI = false;
          }
        }
        else
          Utils.LogOutputMessageForceShow(ResourcesLib.StrTipoOuMembroNaoEncontrado);
      }
    }

    /// <summary>
    /// Acontece após digitar uma tecla
    /// </summary>
    /// <param name="keypress">Tecla que está para ser pressionada</param>
    /// <param name="selection">Selection</param>
    /// <param name="inStatementCompletion">In statement completion</param>
    protected override void AfterKeyPress(string keypress, TextSelection selection, bool inStatementCompletion)
    {

    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      DocUsingFileModel();
    }

    private static void DocUsingFileModel()
    {
      //OndeEstou(WSPackPackage.Instance.Dte.ActiveDocument?.ProjectItem?.FileCodeModel as FileCodeModel2);
      ThreadHelper.ThrowIfNotOnUIThread();
      if (WSPackPackage.Dte.ActiveDocument?.ProjectItem?.FileCodeModel is FileCodeModel2 fcm)
      {
        // Type
        TypeElementEx tipo = fcm.GetTypeWithinCursor();
        if (tipo != null)
        {
          var documentationParams = DocumentationUtils.ReadDocumentationParams(WSPackConsts.DocumentationConfigPath);

          // Só vamos documentar o tipo se estivermos com o cursor nele ou na linha logo abaixo
          int linhaElemento = tipo.Line;
          TextSelection selection = WSPackPackage.Dte.ActiveDocument.Selection as TextSelection;
          int linhaSelecao = selection.ActivePoint.Line;
          bool IsCursorReallyOn = (linhaElemento == linhaSelecao) || (linhaElemento + 1 == linhaSelecao);

          if (IsCursorReallyOn)
          {
            DocumentationSummaryObj.IncludeSummaryInElement(tipo, documentationParams, selection);
            return;
          }

          // Member
          IBaseElementEx elemento = fcm.GetMemberWithinCursor();
          if (elemento != null)
          {
            linhaElemento = elemento.Line;
            IsCursorReallyOn = (linhaElemento == linhaSelecao) || (linhaElemento + 1 == linhaSelecao);
          }
          else
            IsCursorReallyOn = false;

          if (IsCursorReallyOn && elemento != null)
          {
            if (elemento is IMethodElementEx code)
            {
              if (code.IsGetOrSet)
                return;
            }

            // Adicionar comentário em algum membro
            DocumentationSummaryObj.IncludeSummaryInElement(elemento, documentationParams, selection);
          }

          else
            Utils.LogOutputMessageForceShow(ResourcesLib.StrTipoOuMembroNaoEncontrado);
        }

        else
          Utils.LogOutputMessageForceShow(ResourcesLib.StrTipoOuMembroNaoEncontrado);
      }
      else
        TestRoslyn();
    }

    static void TestRoslyn()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      string fullName = WSPackPackage.Dte?.ActiveDocument?.FullName;
      var doc = WSPackPackage.Dte?.ActiveDocument;

      if (!string.IsNullOrEmpty(fullName))
      {
        var view = TextEditorExtensions.GetWpfTextView(doc);
        if (view != null && view.Properties.TryGetProperty(MEFObjects.EditorAdornment.EditorAdornmentElement.ListMethodInfoKey, out List<MEFObjects.EditorAdornment.MethodInfo> lstMethods))
        {
          if (lstMethods != null)
          {
            TextSelection selection = (TextSelection)doc.Selection;
            int linhaAtual = selection.CurrentLine - 1;
            int colunaAtual = selection.CurrentColumn - 1;

            var metodoAtual = lstMethods.FirstOrDefault(x => x.Method.DeclareLocation.Line == linhaAtual &&
              x.Method.DeclareLocation.Column == colunaAtual);

            if (metodoAtual == null)
              metodoAtual = lstMethods.FirstOrDefault(x => x.Method.DeclareLocation.Line == linhaAtual);

            if (metodoAtual != null)
            {
              string comentario = metodoAtual.Method.GetSummary();
            }
          }
          else
            Utils.LogOutputMessage(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
        }
        else
          Utils.LogOutputMessage(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);

        //string conteudo = File.ReadAllText(MenuItem.ParametersDescription);
        //var parser = CSharpParserObj.Parse(conteudo, ParseOptions.Create(extractBlocks: false));

        //TextSelection selection = (TextSelection)WSPackPackage.Instance.Dte.ActiveDocument.Selection;
        //int linhaAtual = selection.CurrentLine - 1;
        //int colunaAtual = selection.CurrentColumn - 1;

        //var metodoAtual = parser.MethodsList.FirstOrDefault(x => x.DeclareLocation.Line == linhaAtual &&
        //  x.DeclareLocation.Column == colunaAtual);
        //if (metodoAtual != null)
        //{
        //  string comentario = metodoAtual.GetSummary();
        //}
      }
    }
  }
}