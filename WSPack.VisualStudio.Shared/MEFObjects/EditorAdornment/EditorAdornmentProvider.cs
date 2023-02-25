using System.ComponentModel.Composition;

using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Outlining;
using Microsoft.VisualStudio.Utilities;

namespace WSPack.VisualStudio.Shared.MEFObjects.EditorAdornment
{
  /// <summary>
  /// Establishes an <see cref="IAdornmentLayer"/> to place the adornment on and exports the <see cref="IWpfTextViewCreationListener"/>
  /// that instantiates the adornment on the event of a <see cref="IWpfTextView"/>'s creation
  /// </summary>
  [Export(typeof(IWpfTextViewCreationListener))]
  [ContentType("csharp")]
  [TextViewRole(PredefinedTextViewRoles.Document)]
  internal sealed class EditorAdornmentProvider : IWpfTextViewCreationListener
  {
#pragma warning disable IDE0051, IDE0044

    /// <summary>
    /// Defines the adornment layer for the adornment. This layer is ordered
    /// after the selection layer in the Z-order
    /// </summary>
    [Export(typeof(AdornmentLayerDefinition))]
    [Name(EditorAdornmentElement.WSEditorAdornmentElementName)]
    [Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
    private AdornmentLayerDefinition _editorAdornmentLayer;

    /// <summary>
    /// The outlining manager service
    /// </summary>
    [Import(typeof(IOutliningManagerService))]
    public IOutliningManagerService OutliningManagerService;

#pragma warning restore IDE0051, IDE0044
    /// <summary>
    /// Called when a text view having matching roles is created over a text data model having a matching content type.
    /// Instantiates a MetricsAdornment manager when the textView is created.
    /// </summary>
    /// <param name="textView">The <see cref="IWpfTextView"/> upon which the adornment should be placed</param>
    public void TextViewCreated(IWpfTextView textView)
    {
#pragma warning disable IDE0059
      // The adornment will listen to any event that changes the layout (text changes, scrolling, etc)
      var editorAdornment = new EditorAdornmentElement(textView, OutliningManagerService);
      System.Diagnostics.Trace.WriteLine($"{nameof(editorAdornment)} criado");
#pragma warning restore IDE0059
    }
  }
}