using System.ComponentModel.Composition;
using System.Windows.Media;

using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace WSPack.VisualStudio.Shared.MEFObjects.HighlightWord
{
  [Export(typeof(EditorFormatDefinition))]
  [Name(HighlightWordFormatDefinitionName)]
  [UserVisible(true)]
  [Order(Before = Priority.Default)]
  class HighlightWordFormatDefinition : MarkerFormatDefinition
  {
    internal const string HighlightWordFormatDefinitionName = "MarkerFormatDefinition/HighlightWordFormatDefinition";

    public HighlightWordFormatDefinition()
    {
      ForegroundColor = Color.FromRgb(147, 112, 219);

      string tema = WSPackPackage.GetInstalledColorTheme();
      if (string.Equals(tema, Microsoft.VisualStudio.Shell.KnownColorThemes.Dark.ToString()))
        BackgroundColor = Color.FromRgb(132, 30, 204);
      else
        BackgroundColor = Color.FromRgb(228, 219, 246);

      DisplayName = "Highlight Word";
      ZOrder = 5;
    }
  }
}