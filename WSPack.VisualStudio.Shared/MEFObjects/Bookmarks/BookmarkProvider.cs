using System;
using System.ComponentModel.Composition;

using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace WSPack.VisualStudio.Shared.MEFObjects.Bookmarks
{
  /// <summary>
  /// Provedor de margem lateral para os marcadores
  /// </summary>
  [Export(typeof(IWpfTextViewMarginProvider))]
  [TextViewRole(PredefinedTextViewRoles.Document)]
  [ContentType("text")]
  [MarginContainer(PredefinedMarginNames.LeftSelection)]
  [Name(BookmarkMargin.MarginName)]
  [Order(After = PredefinedMarginNames.OverviewChangeTracking)]
  internal sealed class BookmarkProvider : IWpfTextViewMarginProvider
  {
    /*
     * FUNCIONOU
     *   PredefinedMarginNames.LeftSelection
     *     After = PredefinedMarginNames.Outlining
     *     After = PredefinedMarginNames.Suggestion
     *     After = PredefinedMarginNames.OverviewChangeTracking
     *     Before = PredefinedMarginNames.ZoomControl
     *     After = PredefinedMarginNames.FileHealthIndicator
     *     Before = PredefinedMarginNames.Glyph
     *     After = PredefinedMarginNames.Glyph
     *     Before = PredefinedMarginNames.LineNumber
     * NÃO FUNCIONOU
     *   PredefinedMarginNames.LeftSelection
     *     Before = PredefinedMarginNames.Outlining
     *     Before = PredefinedMarginNames.Suggestion
     *     Before = PredefinedMarginNames.OverviewChangeTracking
     *     After = PredefinedMarginNames.LineNumber
     *     After = PredefinedMarginNames.OverviewError
     *     Before = PredefinedMarginNames.OverviewError
     *     After = PredefinedMarginNames.OverviewSourceImage
     *     Before = PredefinedMarginNames.OverviewSourceImage
     *     After = PredefinedMarginNames.ZoomControl
     *     Before = PredefinedMarginNames.FileHealthIndicator
     **/

    /// <summary>
    /// Creates an <see cref="T:Microsoft.VisualStudio.Text.Editor.IWpfTextViewMargin" /> for the given <see cref="T:Microsoft.VisualStudio.Text.Editor.IWpfTextViewHost" />.
    /// </summary>
    /// <param name="wpfTextViewHost">The <see cref="T:Microsoft.VisualStudio.Text.Editor.IWpfTextViewHost" /> for which to create the <see cref="T:Microsoft.VisualStudio.Text.Editor.IWpfTextViewMargin" />.</param>
    /// <param name="marginContainer">The margin that will contain the newly-created margin.</param>
    /// <returns>
    /// The <see cref="T:Microsoft.VisualStudio.Text.Editor.IWpfTextViewMargin" />.
    /// </returns>
    public IWpfTextViewMargin CreateMargin(IWpfTextViewHost wpfTextViewHost, IWpfTextViewMargin marginContainer)
    {
      if (wpfTextViewHost == null)
      {
        BookmarkController.Instance.IsBookmarkBarProvided = false;
        return null;
      }

      try
      {
        IWpfTextViewMargin textViewMargin = new BookmarkMargin(wpfTextViewHost.TextView);
        return textViewMargin;
      }
      catch (Exception ex)
      {
        Utils.LogDebugMessage($"Não foi possível criar a margem: {ex.Message}");
      }
      return null;
    }
  }
}