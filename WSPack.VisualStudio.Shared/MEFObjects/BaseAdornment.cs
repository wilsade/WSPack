using System;
using System.Collections.Generic;
using System.Windows.Media;

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

using mvto = Microsoft.VisualStudio.Text.Outlining;

namespace WSPack.VisualStudio.Shared.MEFObjects
{
  abstract class BaseAdornment
  {
#pragma warning disable IDE1006
    protected ITextView _view;
#pragma warning restore IDE1006

    public BaseAdornment(ITextView view)
    {
      _view = view;
      _view.LayoutChanged += AlterouLayout;
      _view.Caret.PositionChanged += AlterouPosicao;
      _view.TextBuffer.ChangedLowPriority += AlterouTexto;
    }

    protected abstract void AlterouTexto(object sender, Microsoft.VisualStudio.Text.TextContentChangedEventArgs e);

    protected abstract void AlterouPosicao(object sender, CaretPositionChangedEventArgs e);

    protected abstract void AlterouLayout(object sender, TextViewLayoutChangedEventArgs e);

    /// <summary>
    /// Indica se a área visível foi alterada
    /// </summary>
    /// <param name="e">The <see cref="TextViewLayoutChangedEventArgs"/> instance containing the event data.</param>
    /// <returns>true se a área visível foi alterada</returns>
    protected static bool IsVisibleAreaChanged(TextViewLayoutChangedEventArgs e)
    {
      ViewState newState = e.NewViewState;
      ViewState oldState = e.OldViewState;
      bool isVisibleAreaChanged = newState.ViewportBottom == oldState.ViewportBottom &&
        newState.ViewportHeight == oldState.ViewportHeight &&
        newState.ViewportLeft == oldState.ViewportLeft &&
        newState.ViewportRight == oldState.ViewportRight &&
        newState.ViewportTop == oldState.ViewportTop &&
        newState.ViewportWidth == oldState.ViewportWidth;
      return isVisibleAreaChanged;
    }

    /// <summary>
    /// Recuperar a posição absoluta no texto com base em uma linha e coluna
    /// </summary>
    /// <param name="textSnapshot">The text snapshot.</param>
    /// <param name="line">The line.</param>
    /// <param name="column">The column.</param>
    /// <returns>posição absoluta no texto com base em uma linha e coluna</returns>
    protected int GetPosition(ITextSnapshot textSnapshot, int line, int column)
    {
      var startLine = textSnapshot.GetLineFromLineNumber(line);
      int startPosition = startLine.Start.Position + column;
      return startPosition;
    }

    /// <summary>
    /// Gets the collapsed regions safe.
    /// </summary>
    /// <param name="manager">Manager</param>
    /// <param name="ssSpan">Ss span</param>
    /// <param name="regions">Regions</param>
    /// <returns></returns>
    protected static bool GetCollapsedRegionsSafe(mvto.IOutliningManager manager, SnapshotSpan ssSpan, out IEnumerable<mvto.ICollapsed> regions)
    {
      regions = null;
      try
      {
        regions = manager.GetCollapsedRegions(ssSpan);
        return true;
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"GetCollapsedRegionsSafe: {ex.Message}");
        return false;
      }
    }

    /// <summary>
    /// Gets the geometry (safe)
    /// </summary>
    /// <param name="ssSpan">Ss span</param>
    /// <param name="geometry">Geometry</param>
    /// <returns></returns>
    protected bool GetGeometrySafe(SnapshotSpan ssSpan, out Geometry geometry)
    {
      geometry = null;
      try
      {
        if (_view.TextViewLines is IWpfTextViewLineCollection linhas)
        {
          geometry = linhas.GetMarkerGeometry(ssSpan);
          return geometry != null;
        }
        return false;
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"GetGeometrySafe: {ex.Message}");
        return false;
      }
    }
  }
}