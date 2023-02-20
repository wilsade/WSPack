using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace WSPack.VisualStudio.Shared.MEFObjects.FileMargin
{
  /// <summary>
  /// Export a <see cref="IWpfTextViewMarginProvider"/>, which returns an instance of the margin for the editor
  /// to use.
  /// </summary>
  [Export(typeof(IWpfTextViewMarginProvider))]
  [Name(FileMarginElement.FileMarginElementName)]
  [Order(After = PredefinedMarginNames.HorizontalScrollBar)] //Ensure that the margin occurs below the horizontal scrollbar
  [MarginContainer(PredefinedMarginNames.Bottom)] //Set the container to the bottom of the editor window
  [ContentType("text")] //Show this margin for all text-based types
  [TextViewRole(PredefinedTextViewRoles.Interactive)]
  public class FileMarginProvider : IWpfTextViewMarginProvider
  {
    [Import]
    internal Microsoft.VisualStudio.Shell.SVsServiceProvider ServiceProvider = null;

    /// <summary>
    /// Creates an <see cref="T:Microsoft.VisualStudio.Text.Editor.IWpfTextViewMargin" /> for the given <see cref="T:Microsoft.VisualStudio.Text.Editor.IWpfTextViewHost" />.
    /// </summary>
    /// <param name="wpfTextViewHost">The <see cref="T:Microsoft.VisualStudio.Text.Editor.IWpfTextViewHost" /> for which to create the <see cref="T:Microsoft.VisualStudio.Text.Editor.IWpfTextViewMargin" />.</param>
    /// <param name="marginContainer">The margin that will contain the newly-created margin.</param>
    /// <returns>
    /// The <see cref="T:Microsoft.VisualStudio.Text.Editor.IWpfTextViewMargin" />.
    /// The value may be null if this <see cref="T:Microsoft.VisualStudio.Text.Editor.IWpfTextViewMarginProvider" /> does not participate for this context.
    /// </returns>
    public IWpfTextViewMargin CreateMargin(IWpfTextViewHost wpfTextViewHost, IWpfTextViewMargin marginContainer)
    {
      if (WSPackPackage.Instance == null ||
        !WSPackPackage.ParametrosMEFObjects.UseFileMargin)
        return null;

      try
      {
        var margin = new FileMarginElement(wpfTextViewHost.TextView);
        return margin;
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"WSFileMargin: {ex.Message}");
      }
      return null;

    }
  }
}