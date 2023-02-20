using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;

using WSPack.Lib.Extensions;

namespace WSPack.VisualStudio.Shared.MEFObjects.HighlightWord
{
  /// <summary>
  /// Helper class to handle the rendering of the match margin.
  /// </summary>
  public class MatchMarginElement : FrameworkElement
  {
    private readonly IWpfTextView _textView;
    private readonly IEditorFormatMap _editorFormatMap;
    NormalizedSnapshotSpanCollection _newSpans;
    readonly IVerticalScrollBar _verticalScrollBar;
    private readonly string _path;
    const double MarkPadding = 1.0;
    const double MarkThickness = 4.0;

    readonly Brush _marginMatchBrush;

    /// <summary>
    /// Constructor for the MatchMarginElement.
    /// </summary>
    /// <param name="textView">ITextView to which this MatchMargenElement will be attached.</param>
    /// <param name="factory">Instance of the MatchMarginFactory that is creating the margin.</param>
    /// <param name="scrollBar">Vertical scrollbar of the ITextViewHost that contains <paramref name="textView" />.</param>
    /// <param name="path">Caminho do documento</param>
    public MatchMarginElement(IWpfTextView textView, MatchMarginProvider factory, IVerticalScrollBar scrollBar, string path)
    {
      _textView = textView;
      _verticalScrollBar = scrollBar;
      _path = path;
      _ = _textView.Properties.GetOrCreateSingletonProperty(nameof(MatchMarginElement),
        () => this);

      _textView.Options.OptionChanged -= OnOptionChanged;
      _textView.Options.OptionChanged += OnOptionChanged;

      Enabled = true;
      Width = 6.0;
      IsHitTestVisible = false;
      _editorFormatMap = factory.EditorFormatMapService.GetEditorFormatMap(textView);
      _marginMatchBrush = GetBrush(MatchColorFormat.EditorMatchColorName, EditorFormatDefinition.ForegroundBrushId);
    }

    private void OnOptionChanged(object sender, EditorOptionChangedEventArgs e)
    {
      Enabled = WSPackPackage.Instance != null &&
        WSPackPackage.ParametrosMEFObjects.UseSearchWord;
      Visibility = Enabled ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    /// Indica se o elemento está habilitado
    /// </summary>
    public bool Enabled { get; private set; } = false;

    /// <summary>
    /// Liberar recursos
    /// </summary>
    public void Dispose()
    {
      // Liberar recursos
      _textView.Options.OptionChanged -= OnOptionChanged;
    }

    /// <summary>
    /// Atualização das palavras encontradas na barra vertical
    /// </summary>
    /// <param name="newSpans">Palavras encontradas</param>
    public void UpdateMarks(NormalizedSnapshotSpanCollection newSpans)
    {
      _newSpans = newSpans;
      this.InvalidateVisual();
    }

    /// <summary>
    /// Override for the FrameworkElement's OnRender. When called, redraw
    /// all of the markers 
    /// </summary>
    protected override void OnRender(DrawingContext drawingContext)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      base.OnRender(drawingContext);

      var lstMatches = new List<int>();
      if (_newSpans?.Count > 0)
      {
        IList<SnapshotSpan> matches = _newSpans.ToList();
        double lastY = double.MinValue;
        int markerCount = Math.Min(500, matches.Count);
        for (int i = 0; (i < markerCount); ++i)
        {
          int index = (int)(i * (long)matches.Count / markerCount);
          SnapshotPoint matchStart = matches[index].Start;

          try
          {
            double y = Math.Floor(_verticalScrollBar.GetYCoordinateOfBufferPosition(matchStart.TranslateTo(_textView.TextSnapshot, PointTrackingMode.Negative)));
            if (y + MarkThickness > lastY)
            {
              lastY = y;
              DrawMark(drawingContext, _marginMatchBrush, y);

              // O editor começa da linha 1. O objeto começa da linha 0. Vamos adicionar +1
              int linhaMatchStart = matchStart.GetContainingLine().LineNumber;
              lstMatches.Add(linhaMatchStart + 1);
            }
          }
          catch (Exception ex)
          {
            Utils.LogDebugError($"OnRender MatchMarginElement: {ex.Message}");
          }
        }
      }

#warning CheckBookmarks
      //CheckBookmarks(lstMatches, drawingContext);
    }

    /*
    double GetVerticalOffsetForBookmark(Bookmark bm)
    {
      try
      {
        ITextSnapshotLine acheiLinha = _textView.TextSnapshot.Lines.FirstOrDefault((ITextSnapshotLine x) => x.LineNumber == bm.Line - 1);
        if (acheiLinha != null)
        {
          int offset = acheiLinha.Start.Position + bm.Column;
          if (offset >= 0)
          {
            if (offset >= acheiLinha.Snapshot.Length)
              bm.Offset = acheiLinha.Snapshot.Length;
            else
              bm.Offset = offset;
            SnapshotPoint sp = new SnapshotPoint(acheiLinha.Snapshot, bm.Offset);
            double y = Math.Floor(_verticalScrollBar.GetYCoordinateOfBufferPosition(sp.TranslateTo(_textView.TextSnapshot, PointTrackingMode.Negative)));
            return y;
          }
          else
            return -1;
        }
        else
          return -1;
      }
      catch (Exception ex)
      {
        Utils.LogDebugError("GetVerticalOffsetForBookmark: " + ex.ToString());
        return -1;
      }
    }

    private void CheckBookmarks(List<int> lstMatches, DrawingContext drawingContext)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (!BookmarkController.Instance.IsBookmarkBarProvided)
        return;
      double ultimoVertical = 0;
      foreach (Bookmark esteMarcador in BookmarkController.Instance.Lista.OrderBy(x => x.Line))
      {
        bool pode = CanUpdateBookmark(esteMarcador);
        if (pode)
        {
          var verticalOffset = GetVerticalOffsetForBookmark(esteMarcador);
          if (verticalOffset > 0)
          {
            if (verticalOffset > (ultimoVertical + 9))
            {
              ultimoVertical = verticalOffset + 3;
              DrawBookmark(lstMatches, drawingContext, verticalOffset, esteMarcador);
            }

            else
            {
              ultimoVertical += 12;
              DrawBookmark(lstMatches, drawingContext, ultimoVertical, esteMarcador);
            }
          }
        }
      }
    }

    bool CanUpdateBookmark(Bookmark bookmark)
    {
      bool result;
      if (bookmark == null)
        result = false;

      else
      {
        result = _path.EqualsInsensitive(bookmark.FullName);
      }
      return result;
    }

    void DrawBookmark(List<int> lstMatches, DrawingContext drawingContext, double y, Bookmark bm)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      Border bloco = (Border)BookmarkMargin.CreateTextBlock(bm.Number.ToString(), string.Empty, true);

      if (lstMatches.IndexOf(bm.Line) >= 0)
        ((TextBlock)bloco.Child).Foreground = GetBrush(MatchColorFormat.EditorMatchColorName, EditorFormatDefinition.ForegroundBrushId);

      VisualBrush vb = new VisualBrush(bloco);
      var outroRec = new Rect(0, y, BookmarkMargin.ComprimentoBordaMarcador, BookmarkMargin.AlturaBordaMarcador);
      drawingContext.DrawRectangle(vb, null, outroRec);
    }
    */

    private void DrawMark(DrawingContext drawingContext, Brush brush, double y)
    {
      var rec = new Rect(MarkPadding, y - (MarkThickness * 0.5), this.Width - MarkPadding * 2.0, MarkThickness);
      drawingContext.DrawRectangle(brush, null, rec);
    }

    private Brush GetBrush(string name, string resource)
    {
      var rd = _editorFormatMap.GetProperties(name);

      if (rd.Contains(resource))
      {
        return rd[resource] as Brush;
      }

      return null;
    }
  }
}