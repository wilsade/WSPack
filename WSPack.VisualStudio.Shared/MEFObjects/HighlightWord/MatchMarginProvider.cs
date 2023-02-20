using System.ComponentModel.Composition;

using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

using WSPack.VisualStudio.Shared.Extensions;

namespace WSPack.VisualStudio.Shared.MEFObjects.HighlightWord
{
  /// <summary>
  /// Export a <see cref="IWpfTextViewMarginProvider"/>, which returns an instance of the margin for the editor to use
  /// </summary>
  /// <seealso cref="IWpfTextViewMarginProvider" />
  [MarginContainer(PredefinedMarginNames.VerticalScrollBar)]
  [Order(After = PredefinedMarginNames.OverviewChangeTracking, Before = PredefinedMarginNames.OverviewMark)]
  [TextViewRole(PredefinedTextViewRoles.Interactive)]
  [Export(typeof(IWpfTextViewMarginProvider))]
  [Name(MatchMargin.Name)]
  [ContentType("text")]
  public class MatchMarginProvider : IWpfTextViewMarginProvider
  {
#pragma warning disable 649
    [Import]
    internal IEditorFormatMapService EditorFormatMapService = null;

    [Export]
    [Name(MatchMargin.MatchMarginAdornmentLayerName)]
    [Order(After = PredefinedAdornmentLayers.Outlining, Before = PredefinedAdornmentLayers.Selection)]
    internal AdornmentLayerDefinition matchLayerDefinition;
#pragma warning restore 649

    /// <summary>
    /// Create an instance of the MatchMargin in the specified <see cref="IWpfTextViewHost"/>.
    /// </summary>
    /// <param name="textViewHost">The <see cref="IWpfTextViewHost"/> in which the MatchMargin will be displayed.</param>
    /// <param name="containerMargin">containerMargin</param>
    /// <returns>The newly created MatchMargin.</returns>
    public IWpfTextViewMargin CreateMargin(IWpfTextViewHost textViewHost, IWpfTextViewMargin containerMargin)
    {
      // Usuário não quer usar MatchMargin
      if (WSPackPackage.Instance == null ||
        !WSPackPackage.ParametrosMEFObjects.UseSearchWord)
        return null;

      // QuickWatch?
      string path = textViewHost.TextView.GetDocumentPath();
      if (string.IsNullOrEmpty(path) || path.IsTempTxt())
        return null;

      if (!(containerMargin is IVerticalScrollBar containerMarginAsVerticalScrollBar))
      {
        var scrollBarMargin = containerMargin.GetTextViewMargin(PredefinedMarginNames.VerticalScrollBar);
        containerMarginAsVerticalScrollBar = (IVerticalScrollBar)scrollBarMargin;
      }

      if (containerMarginAsVerticalScrollBar != null)
      {
        return new MatchMargin(textViewHost, containerMarginAsVerticalScrollBar, path, this);
      }
      else
        return null;
    }
  }
}