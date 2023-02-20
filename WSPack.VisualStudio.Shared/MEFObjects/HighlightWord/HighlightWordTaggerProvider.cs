using System.ComponentModel.Composition;

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

using WSPack.VisualStudio.Shared.Extensions;

namespace WSPack.VisualStudio.Shared.MEFObjects.HighlightWord
{
  [Export(typeof(IViewTaggerProvider))]
  [ContentType("text")]
  [TagType(typeof(HighlightWordTag))]
  internal class HighlightWordTaggerProvider : IViewTaggerProvider
  {
    [Import]
    internal ITextSearchService TextSearchService { get; set; }

    [Import]
    internal ITextStructureNavigatorSelectorService TextStructureNavigatorSelector { get; set; }

    public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
    {
      // Usuário não quer usar MatchMargin
      if (WSPackPackage.Instance == null ||
        !WSPackPackage.ParametrosMEFObjects.UseSearchWord)
        return null;

      // QuickWatch?
      if (textView is IWpfTextView wpf)
      {
        string path = wpf.GetDocumentPath();
        if (string.IsNullOrEmpty(path) || path.IsTempTxt())
          return null;
      }

      //provide highlighting only on the top buffer 
      if (textView.TextBuffer != buffer)
        return null;

      ITextStructureNavigator textStructureNavigator =
          TextStructureNavigatorSelector.GetTextStructureNavigator(buffer);

      return new HighlightWordTagger(textView, buffer, TextSearchService, textStructureNavigator) as ITagger<T>;
    }
  }
}