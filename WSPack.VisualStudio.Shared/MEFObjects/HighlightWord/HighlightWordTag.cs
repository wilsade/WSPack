using Microsoft.VisualStudio.Text.Tagging;

namespace WSPack.VisualStudio.Shared.MEFObjects.HighlightWord
{
  class HighlightWordTag : TextMarkerTag
  {
    public HighlightWordTag()
      : base(HighlightWordFormatDefinition.HighlightWordFormatDefinitionName)
    { }
  }
}