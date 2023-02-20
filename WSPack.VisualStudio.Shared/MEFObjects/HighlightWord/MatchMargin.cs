using System;
using System.Windows;

using Microsoft.VisualStudio.Text.Editor;

namespace WSPack.VisualStudio.Shared.MEFObjects.HighlightWord
{
  /// <summary>
  /// Margem vertical para exibição das palavras encontradas no editor
  /// </summary>
  public sealed class MatchMargin : IWpfTextViewMargin, ITextViewMargin
  {
    /// <summary>
    /// Name of this margin.
    /// </summary>
    public const string Name = "WSPackEditorMatchMargin";
    internal const string MatchMarginAdornmentLayerName = "WSPackMatchMarginAdornmentLayer";

    #region Private Members
    private MatchMarginElement _matchMarginElement;
    private bool _isDisposed = false;
    #endregion

    /// <summary>
    /// Constructor for the MatchMargin.
    /// </summary>
    /// <param name="textViewHost">The IWpfTextViewHost in which this margin will be displayed.</param>
    /// <param name="scrollBar">ScrollBar</param>
    /// <param name="path">Caminho do documento</param>
    /// <param name="factory">Provedor da margem</param>
    public MatchMargin(IWpfTextViewHost textViewHost, IVerticalScrollBar scrollBar, string path, MatchMarginProvider factory)
    {
      // Validate
      if (textViewHost == null)
        throw new ArgumentNullException("textViewHost");

      _matchMarginElement = new MatchMarginElement(textViewHost.TextView, factory, scrollBar, path);
    }

    #region IWpfTextViewMargin Members
    /// <summary>
    /// The FrameworkElement that renders the margin.
    /// </summary>
    public FrameworkElement VisualElement
    {
      get
      {
        ThrowIfDisposed();
        return _matchMarginElement;
      }
    }
    #endregion

    #region ITextViewMargin Members
    /// <summary>
    /// For a horizontal margin, this is the height of the margin (since the width will be determined by the ITextView. For a vertical margin, this is the width of the margin (since the height will be determined by the ITextView.
    /// </summary>
    public double MarginSize
    {
      get
      {
        ThrowIfDisposed();
        return _matchMarginElement.ActualWidth;
      }
    }

    /// <summary>
    /// The visible property, true if the margin is visible, false otherwise.
    /// </summary>
    public bool Enabled
    {
      get
      {
        ThrowIfDisposed();
        return _matchMarginElement.Enabled;
      }
    }

    /// <summary>
    /// Gets the <see cref="T:Microsoft.VisualStudio.Text.Editor.ITextViewMargin" /> with the given <paramref name="marginName" />.
    /// </summary>
    /// <param name="marginName">The name of the <see cref="T:Microsoft.VisualStudio.Text.Editor.ITextViewMargin" />.</param>
    /// <returns>
    /// The <see cref="T:Microsoft.VisualStudio.Text.Editor.ITextViewMargin" /> named <paramref name="marginName" />, or null if no match is found.
    /// </returns>
    /// <remarks>
    /// A margin returns itself if it is passed its own name. If the name does not match and it is a container margin, it
    /// forwards the call to its children. Margin name comparisons are case-insensitive.
    /// </remarks>
    public ITextViewMargin GetTextViewMargin(string marginName)
    {
      return string.Compare(marginName, MatchMargin.Name, StringComparison.OrdinalIgnoreCase) == 0 ? this : (ITextViewMargin)null;
    }

    /// <summary>
    /// In our dipose, stop listening for events.
    /// </summary>
    public void Dispose()
    {
      if (!_isDisposed)
      {
        _matchMarginElement.Dispose();
        GC.SuppressFinalize(this);
        _isDisposed = true;
      }
    }
    #endregion

    private void ThrowIfDisposed()
    {
      if (_isDisposed)
        throw new ObjectDisposedException(Name);
    }
  }
}