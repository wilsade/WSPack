
using EnvDTE;

using EnvDTE80;

using Microsoft.VisualStudio.Shell;

using WSPack.Lib.Extensions;
using WSPack.VisualStudio.Shared.Extensions;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para manipulação de KeyPress
  /// </summary>
  internal abstract class BaseKeyPressCommand : BaseCommand
  {
    readonly TextDocumentKeyPressEvents _textDocKeyEvents;
    readonly DocumentEvents _documentEvents;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseKeyPressCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected BaseKeyPressCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      DTE2 _dte = WSPackPackage.Dte;

      var _events2 = (Events2)_dte.Events;
      _documentEvents = _events2.get_DocumentEvents(null);
      _textDocKeyEvents = _events2.get_TextDocumentKeyPressEvents(null);

      _documentEvents.DocumentOpened += DocumentEvents_DocumentOpened;
      _documentEvents.DocumentClosing += DocumentEvents_DocumentClosing;
    }

    /// <summary>
    /// Acontece após digitar uma tecla
    /// </summary>
    /// <param name="keypress">Tecla pressionada</param>
    /// <param name="selection">Selection</param>
    /// <param name="inStatementCompletion">In statement completion</param>
    protected abstract void AfterKeyPress(string keypress, TextSelection selection, bool inStatementCompletion);

    /// <summary>
    /// Acontece antes de digitar uma tecla
    /// </summary>
    /// <param name="keypress">Tecla que está para ser pressionada</param>
    /// <param name="selection">Selection</param>
    /// <param name="inStatementCompletion">In statement completion</param>
    /// <param name="cancelKeypress">true para cancelar a digitação da tecla</param>
    protected abstract void BeforeKeyPress(string keypress, TextSelection selection, bool inStatementCompletion,
          ref bool cancelKeypress);

    private void DocumentEvents_DocumentClosing(Document document)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (document.IsCSharpFile())
      {
        if (_textDocKeyEvents != null)
        {
          if (GetActiveDocumentPath(out var local) && local.Equals(document.FullName))
          {
            _textDocKeyEvents.BeforeKeyPress -= new _dispTextDocumentKeyPressEvents_BeforeKeyPressEventHandler(BeforeKeyPress);
            _textDocKeyEvents.AfterKeyPress -= new _dispTextDocumentKeyPressEvents_AfterKeyPressEventHandler(AfterKeyPress);
          }
        }
      }
    }

    private void DocumentEvents_DocumentOpened(Document document)
    {
      if (document.IsCSharpFile())
      {
        if (_textDocKeyEvents != null)
        {
          _textDocKeyEvents.BeforeKeyPress += new _dispTextDocumentKeyPressEvents_BeforeKeyPressEventHandler(BeforeKeyPress);
          _textDocKeyEvents.AfterKeyPress += new _dispTextDocumentKeyPressEvents_AfterKeyPressEventHandler(AfterKeyPress);
        }
      }
    }
  }
}