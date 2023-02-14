using EnvDTE;

using Microsoft.VisualStudio.Shell;

namespace WSPack.VisualStudio.Shared.Commands
{
  class SelectionReg
  {
    readonly TextDocument _textDocument;

    /// <summary>
    /// Inicialização da classe: <see cref="SelectionReg"/>.
    /// </summary>
    /// <param name="textDocument">Text document</param>
    public SelectionReg(TextDocument textDocument)
    {
      ThreadHelper.ThrowIfNotOnUIThread($"{nameof(SelectionReg)}:ctor");
      _textDocument = textDocument;
      var topPoint = textDocument.Selection.TopPoint;
      Line = textDocument.CreateEditPoint().GetLines(topPoint.Line, topPoint.Line + 1);
    }

    /// <summary>
    /// Texto da linha
    /// </summary>
    internal string Line { get; }

    /// <summary>
    /// Begin
    /// </summary>
    internal VirtualPoint Begin
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread($"{nameof(SelectionReg)}:{nameof(Begin)}");
        return _textDocument.Selection.TopPoint;
      }
    }

    public void MoveTo(int startColumn, int endColumn)
    {
      ThreadHelper.ThrowIfNotOnUIThread($"{nameof(SelectionReg)}:{nameof(MoveTo)}");
      var selection = _textDocument.Selection;
      if (selection == null)
        return;

      selection.MoveToLineAndOffset(Begin.Line, startColumn);
      selection.MoveToLineAndOffset(Begin.Line, endColumn, true);
    }
  }
}