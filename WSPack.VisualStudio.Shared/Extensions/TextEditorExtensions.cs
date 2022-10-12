using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Projection;
using Microsoft.VisualStudio.TextManager.Interop;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WSPack.VisualStudio.Shared;

namespace WSPack.VisualStudio.Shared.Extensions
{
  static class TextEditorExtensions
  {
    /// <summary>
    /// Indica se o ContextType do buffer é um text
    /// </summary>
    /// <param name="buffer">Buffer</param>
    /// <returns>true se text document</returns>
    static bool IsTextDocument(this ITextBuffer buffer) => buffer?.ContentType?.IsOfType("text") ?? true;

    /// <summary>
    /// Gets the root text buffer.
    /// </summary>
    /// <param name="buffer">Buffer</param>
    /// <returns>Root text buffer</returns>
    public static ITextBuffer GetRootTextBuffer(this ITextBuffer buffer)
    {
      if (buffer.ContentType.TypeName == "HTMLXProjection")
      {
        var projectionBuffer = buffer as IProjectionBuffer;
        return projectionBuffer == null ? buffer : projectionBuffer.SourceBuffers.FirstOrDefault(b => IsTextDocument(b));
      }
      else
      {
        return IsTextDocument(buffer) ? buffer : null;
      }
    }

    /// <summary>
    /// Gets the text document.
    /// </summary>
    /// <param name="textBuffer">Text buffer</param>
    /// <returns></returns>
    public static ITextDocument GetTextDocument(this ITextBuffer textBuffer)
    {
      if (textBuffer == null)
        return null;

      var rc = textBuffer.Properties.TryGetProperty(typeof(ITextDocument), out ITextDocument textDoc);
      if (rc == true)
        return textDoc;
      else
        return null;
    }

    /// <summary>
    /// Recuperar o caminho do documento
    /// </summary>
    /// <param name="textView">The text view.</param>
    /// <returns>Caminho do documento ou null</returns>
    public static string GetDocumentPath(this IWpfTextView textView)
    {
      if (textView == null || textView.TextBuffer == null || textView.TextBuffer.Properties == null)
        return null;

      if (textView.TextBuffer.Properties.TryGetProperty(typeof(ITextDocument), out ITextDocument textDocument))
      {
        return textDocument.FilePath;
      }

      else
      {
        textDocument = textView.TextBuffer.GetRootTextBuffer()?.GetTextDocument();
        if (textDocument?.FilePath != null)
        {
          return textDocument.FilePath;
        }
      }
      return null;
    }

    /// <summary>
    /// Retornar a linha inicial de um SnapshotSpan
    /// </summary>
    /// <param name="snapshotSpan">snapshotSpan</param>
    /// <returns>linha inicial de um SnapshotSpan</returns>
    public static int GetStartLine(this SnapshotSpan snapshotSpan)
    {
      int? linha = snapshotSpan.Snapshot?.GetLineFromPosition(snapshotSpan.Start)?.LineNumber;
      return linha ?? 0;
    }

    /// <summary>
    /// Retornar a linha inicial de um SnapshotSpan
    /// </summary>
    /// <param name="snapshotSpan">snapshotSpan</param>
    /// <returns>linha inicial de um SnapshotSpan</returns>
    public static int GetEndLine(this SnapshotSpan snapshotSpan)
    {
      int? linha = snapshotSpan.Snapshot?.GetLineFromPosition(snapshotSpan.End)?.LineNumber;
      return linha ?? 0;
    }

    /// <summary>
    /// Gets the WPF text view.
    /// </summary>
    /// <param name="doc">Document</param>
    /// <returns>View</returns>
    public static IWpfTextView GetWpfTextView(this EnvDTE.Document doc)
    {
      if (doc == null)
        return null;

      if (doc.GetTextBufferAndView(out _, out IWpfTextView view))
      {
        return view;
      }
      return null;
    }

    /// <summary>
    /// Gets the text buffer and view.
    /// </summary>
    /// <param name="document">Document</param>
    /// <param name="buffer">Buffer</param>
    /// <param name="wpfView">WPF view</param>
    /// <returns></returns>
    public static bool GetTextBufferAndView(this EnvDTE.Document document, out ITextBuffer buffer, out IWpfTextView wpfView)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      return GetTextBufferAndView(document?.FullName, out buffer, out wpfView);
    }

    /// <summary>
    /// Gets the text buffer and view.
    /// </summary>
    /// <param name="filePath">File path</param>
    /// <param name="buffer">Buffer</param>
    /// <param name="wpfView">WPF view</param>
    /// <returns></returns>
    public static bool GetTextBufferAndView(string filePath, out ITextBuffer buffer, out IWpfTextView wpfView)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      buffer = null;
      wpfView = null;
      if (string.IsNullOrEmpty(filePath))
        return false;

      using (ServiceProvider serviceProvider = new ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)WSPackPackage.Dte))
      {
        var componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
        var editorAdapterFactoryService = componentModel.GetService<IVsEditorAdaptersFactoryService>();

        if (VsShellUtilities.IsDocumentOpen(
          serviceProvider,
          filePath,
          Guid.Empty,
          out IVsUIHierarchy uiHierarchy,
          out uint itemID,
          out IVsWindowFrame windowFrame))
        {
          IVsTextView view = VsShellUtilities.GetTextView(windowFrame);
          if (view.GetBuffer(out IVsTextLines lines) == 0)
          {
            if (lines is IVsTextBuffer buf)
            {
              buffer = editorAdapterFactoryService.GetDataBuffer(buf);
              wpfView = editorAdapterFactoryService.GetWpfTextView(view);
              return true;
            }
          }
        }
      }
      return false;
    }


    /// <summary>
    /// Gets the enconding.
    /// </summary>
    /// <param name="textDocument">Text document</param>
    /// <returns></returns>
    public static string GetEnconding(this ITextDocument textDocument)
    {
      if (textDocument == null)
        return null;

      try
      {
        byte[] preamble = textDocument.Encoding.GetPreamble();
        string bom = preamble != null && preamble.Length > 2 ? " - BOM" : string.Empty;

        return textDocument.Encoding.EncodingName + bom;
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"Erro ao recuperar enconding: {ex.Message}");
        return string.Empty;
      }
    }

    /// <summary>
    /// Recuperar o caminho completo do arquivo com base no buffer
    /// </summary>
    /// <param name="textBuffer">Text buffer</param>
    /// <returns>caminho completo do arquivo com base no buffer</returns>
    public static string GetFilePath(this ITextBuffer textBuffer)
    {
      GetFilePath(textBuffer, out string path);
      return path;
    }

    /// <summary>
    /// Recuperar o caminho completo do arquivo com base no buffer
    /// </summary>
    /// <param name="textBuffer">Text buffer</param>
    /// <param name="filePath">File path</param>
    /// <returns>caminho completo do arquivo com base no buffer</returns>
    public static bool GetFilePath(this ITextBuffer textBuffer, out string filePath)
    {
      filePath = null;
      if (textBuffer?.IsTextDocument() == true && textBuffer.Properties.TryGetProperty(typeof(ITextDocument), out ITextDocument doc))
      {
        filePath = doc.FilePath;

        if (string.IsNullOrEmpty(filePath))
        {
          Utils.LogDebugMessage($"{nameof(GetFilePath)} é nulo");
        }

        return true;
      }
      else
      {
        var textDoc = textBuffer.GetRootTextBuffer()?.GetTextDocument();
        if (textDoc?.FilePath != null)
        {
          filePath = textDoc.FilePath;
          return true;
        }
      }
      return false;
    }
  }
}
