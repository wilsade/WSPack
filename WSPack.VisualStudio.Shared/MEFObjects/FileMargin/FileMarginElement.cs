using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Commands;
using WSPack.VisualStudio.Shared.Extensions;

namespace WSPack.VisualStudio.Shared.MEFObjects.FileMargin
{
  /// <summary>
  /// A class detailing the margin's visual definition including both size and content.
  /// </summary>
  /// <seealso cref="System.Windows.Controls.Canvas" />
  /// <seealso cref="Microsoft.VisualStudio.Text.Editor.IWpfTextViewMargin" />
  public sealed class FileMarginElement : Canvas, IWpfTextViewMargin
  {
    readonly string _filePath = string.Empty;

    /// <summary>
    /// The margin name
    /// </summary>
    public const string FileMarginElementName = "WSPackFileMargin";

    private readonly IWpfTextView _textView;
    readonly ITextDocument _textDocument;
    private bool _isDisposed = false;

    /// <summary>
    /// Cria uma instância da classe <see cref="FileMarginElement" />
    /// </summary>
    /// <param name="textView">Text view</param>
    public FileMarginElement(IWpfTextView textView)
    {
      _textView = textView;

      ITextBuffer TextBuffer = _textView.TextBuffer;
      _textDocument = TextBuffer.GetTextDocument();
      if (_textDocument == null)
        _textDocument = _textView.TextBuffer.GetRootTextBuffer()?.GetTextDocument();

      if (_textDocument == null || _textDocument.FilePath == null || _textDocument.FilePath.IsTempTxt() ||
        _textDocument.FilePath.ContainsInsensitive("VisualStudio_Debugger"))
      {
        return;
      }

      _filePath = _textDocument.FilePath;

      Height = 20;
      ClipToBounds = true;
      Background = new SolidColorBrush(Color.FromArgb(255, 232, 232, 236));
      var label = new Label
      {
        Content = _filePath
      };

      label.SetResourceReference(Control.ForegroundProperty, EnvironmentColors.ComboBoxTextBrushKey);
      label.SetResourceReference(Control.BackgroundProperty, EnvironmentColors.ScrollBarBackgroundBrushKey);
      base.SetResourceReference(Panel.BackgroundProperty, EnvironmentColors.ScrollBarBackgroundBrushKey);

      string enconding = _textDocument.GetEnconding();

      label.ToolTip = string.Format(ResourcesLib.StrToolTipLabelRodape,
        !string.IsNullOrEmpty(enconding) ?
        Environment.NewLine + "Encoding: " + enconding + Environment.NewLine + _filePath :
        _filePath);

      label.MouseLeftButtonUp += (x, y) =>
      {
        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
          if (!LocateInWindowsBaseCommand.Locate(_filePath, out string msg))
            Utils.LogOutputMessage(msg);
        }
        else
          CopyLocalPathBaseCommand.CopyToClipboard(_filePath);
      };

      label.MouseRightButtonUp += (x, y) =>
      {
        if (WSPackPackage.Dte != null)
        {
          VersionControlExt VCExt = Utils.GetVersionControlServerExt();
          if (VCExt != null)
          {
            if (VCExt.TryGetServerItemForLocalItem(_filePath, out var tupla))
              CopyLocalPathBaseCommand.CopyToClipboard(tupla.ServerItem);
            else if (Utils.TryGetGitServerItem(_filePath, out string serverItem))
              CopyLocalPathBaseCommand.CopyToClipboard(serverItem);
            else
              MessageBoxUtils.ShowInformation(tupla.ErrorMessage);
          }

          else
            MessageBoxUtils.ShowInformation(ResourcesLib.StrSourceControlExplorerNaoConfigurado);
        }
        else
          MessageBoxUtils.ShowInformation(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
      };

      Children.Add(label);
    }

    private void ThrowIfDisposed()
    {
      if (_isDisposed)
        throw new ObjectDisposedException(FileMarginElementName);
    }

    #region IWpfTextViewMargin Members

    /// <summary>
    /// Gets the <see cref="T:System.Windows.FrameworkElement" /> that renders the margin.
    /// </summary>
    public System.Windows.FrameworkElement VisualElement
    {
      // Since this margin implements Canvas, this is the object which renders
      // the margin.
      get
      {
        ThrowIfDisposed();
        return this;
      }
    }

    #endregion

    #region ITextViewMargin Members

    /// <summary>
    /// Tamanho da margem
    /// </summary>
    public double MarginSize
    {
      // Since this is a horizontal margin, its width will be bound to the width of the text view.
      // Therefore, its size is its height.
      get
      {
        ThrowIfDisposed();
        return ActualHeight;
      }
    }

    /// <summary>
    /// Indica se o objeto está habilitado
    /// </summary>
    public bool Enabled
    {
      // The margin should always be enabled
      get
      {
        ThrowIfDisposed();
        return true;
      }
    }

    /// <summary>
    /// Returns an instance of the margin if this is the margin that has been requested.
    /// </summary>
    /// <param name="marginName">The name of the margin requested</param>
    /// <returns>An instance of EditorMargin or null</returns>
    public ITextViewMargin GetTextViewMargin(string marginName)
    {
      return (marginName == FileMarginElementName) ? this : null;
    }


    /// <summary>
    /// Liberar o objeto da memória
    /// </summary>
    public void Dispose()
    {
      if (!_isDisposed)
      {
        GC.SuppressFinalize(this);
        _isDisposed = true;
      }
    }
    #endregion
  }
}