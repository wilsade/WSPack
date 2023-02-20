using System.ComponentModel.Composition;
using System.Windows.Media;

using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace WSPack.VisualStudio.Shared.MEFObjects.HighlightWord
{
  [Export(typeof(EditorFormatDefinition))]
  [Name(EditorMatchColorName)]
  [UserVisible(true)]
  [Order(Before = Priority.Default)]
  internal sealed class MatchColorFormat : EditorFormatDefinition
  {
    public const string EditorMatchColorName = "EditorMatchColor";

    public MatchColorFormat()
    {
      DisplayName = "Editor Match color";
      ForegroundColor = Color.FromRgb(147, 112, 219);        //Color of the margin mark
      BackgroundColor = Color.FromRgb(228, 219, 246);        //Color of the adornment
    }
  }
}