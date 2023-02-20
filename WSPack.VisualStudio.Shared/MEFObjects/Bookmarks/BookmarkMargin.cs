using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using EnvDTE80;

using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

namespace WSPack.VisualStudio.Shared.MEFObjects.Bookmarks
{
  /// <summary>
  /// Margem lateral para Bookmarks
  /// </summary>
  class BookmarkMargin : Border, IWpfTextViewMargin
  {
    internal const string MarginName = "WSPackBookmarkMargin";
    internal const string vsViewKindCode = "{7651A701-06E5-11D1-8EBD-00A0C90F26EA}";
    internal const string vsViewKindDefault = "{00000000-0000-0000-0000-000000000000}";
    const double TamanhoMargem = 10;
    const double LeftStartMarcadorNaMargem = 0.2;
    const string SimboloIdentificaMarcadores = "X";
    internal const double ComprimentoBordaMarcador = 9.0;
    internal const double AlturaBordaMarcador = 14.0;

    static DTE2 _dte;
    readonly IWpfTextView _textView;
    readonly Canvas _marginCanvas;
    string _documentPath;

    bool _isDisposed = false;
    bool _margemQuerInserirBookmark;

    static DTE2 Dte
    {
      get
      {
        if (_dte == null)
        {
          _dte = WSPackPackage.Dte;
          if (_dte == null)
            _dte = Package.GetGlobalService(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE)) as DTE2;
        }
        return _dte;
      }
    }

    #region Construtor

    /// <summary>
    /// Cria uma instância da classe <see cref="BookmarkMargin" /></summary>
    /// <param name="textView">The text view.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public BookmarkMargin(IWpfTextView textView)
    {
      _documentPath = null;
      _textView = textView;

      _marginCanvas = new Canvas
      {
        Background = Brushes.LightGray
      };
      _margemQuerInserirBookmark = false;

      ClipToBounds = true;
      Background = new SolidColorBrush(Colors.Transparent);
      BorderBrush = new SolidColorBrush(Colors.Transparent);
      Width = TamanhoMargem;

      Child = _marginCanvas;

      Margin = new Thickness(2, 0, 2, 0);
      _marginCanvas.ToolTip = ResourcesLib.StrCliqueAlternarMarcadores;
      _marginCanvas.SetResourceReference(Control.ForegroundProperty, EnvironmentColors.ComboBoxTextBrushKey);
      _marginCanvas.SetResourceReference(Control.BackgroundProperty, EnvironmentColors.ScrollBarBackgroundBrushKey);
      SetResourceReference(Panel.BackgroundProperty, EnvironmentColors.ScrollBarBackgroundBrushKey);

      _marginCanvas.MouseMove += (x, y) =>
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        if (_margemQuerInserirBookmark)
        {
          var margem = (x as Canvas).Parent as BookmarkMargin;
          MargemInsereBookmark(margem);
        }
      };

      MouseLeftButtonDown += (x, y) =>
      {
        _margemQuerInserirBookmark = true;
      };

      _textView.LayoutChanged += new EventHandler<TextViewLayoutChangedEventArgs>(TextViewLayoutChanged);
      _textView.ViewportHeightChanged += new EventHandler(TextViewViewportHeightChanged);

      if (BookmarkController._instance != null)
      {
        BookmarkController.Instance.BookmarksChanged -= new EventHandler(BookmarksChanged);
        BookmarkController.Instance.BookmarksChanged += new EventHandler(BookmarksChanged);
        BookmarkController.Instance.IsBookmarkBarProvided = true;
      }

      ThreadHelper.ThrowIfNotOnUIThread();
      UpdateBookmarks();
    }
    #endregion

    private void MargemInsereBookmark(BookmarkMargin margem)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      _margemQuerInserirBookmark = false;
      GetDocumentPath();
      int lineNumber = margem._textView.Selection.ActivePoint.Position.GetContainingLine().LineNumber;
      Bookmark bm = BookmarkController.Instance.Get(lineNumber, _documentPath);
      if (bm != null)
        BookmarkController.Instance.RemoveBookmark(bm);
      else
      {
        bm = BookmarkController.Instance.CreateBookmark(lineNumber, 1, _documentPath);
        Utils.LogDebugMessage(bm.ToString());
        margem._textView.Selection.Clear();
      }
    }

    /// <summary>
    /// Criar um elemento para identificar que há marcadores no documento
    /// </summary>
    /// <returns>Elemento para identificar que há marcadores no documento</returns>
    private Border CriarIdentificadorBookmarks()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      Border borda = (Border)CreateTextBlock(SimboloIdentificaMarcadores, ResourcesLib.strDocumentoPossuiMarcadores, true);
      borda.Background = Brushes.GreenYellow;

      _marginCanvas.Children.Add(borda);
      Canvas.SetTop(borda, 0);
      Canvas.SetLeft(borda, LeftStartMarcadorNaMargem);

      return borda;
    }

    internal static UIElement CreateTextBlock(Bookmark bookmark, bool useBorder = true)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      return CreateTextBlock(bookmark.Number.ToString(), bookmark.ToString(), useBorder);
    }

    internal static UIElement CreateTextBlock(string text, string toolTipText, bool useBorder = true)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      string numero = text;
      TextBlock textBlock = new TextBlock
      {
        Text = numero,
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center,
        FontFamily = new FontFamily("Consolas"),
        Foreground = new SolidColorBrush(Colors.Red),
        ToolTip = toolTipText
      };

      textBlock.PreviewMouseLeftButtonDown += (x, y) =>
      {
        y.Handled = true;

        TextBlock element = (TextBlock)x;
        if (int.TryParse(element.Text, out int number))
          BookmarkController.Instance.RemoveBookmark(number);
        else
        {
          Dte.ExecuteCommand("WSPack.JanelaMarcadores");
        }
      };

      Border border = new Border
      {
        Background = Brushes.LightGreen,
        CornerRadius = new CornerRadius(3.0),
        BorderThickness = new Thickness(1),
        BorderBrush = Brushes.DarkGreen,
        Width = ComprimentoBordaMarcador,
        Height = AlturaBordaMarcador,
        Child = textBlock
      };

      if (useBorder)
        return border;

      return textBlock;
    }

    void CreateGlyph(Bookmark bookmark, double verticalOffset)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (bookmark != null)
      {
        UIElement element = CreateTextBlock(bookmark);

        _marginCanvas.Children.Add(element);
        Canvas.SetTop(element, verticalOffset);
        Canvas.SetLeft(element, LeftStartMarcadorNaMargem);
      }
    }


    double GetVerticalOffsetForBookmark(Bookmark bookmark)
    {
      ITextSnapshotLine line = null;

      try
      {
        line = _textView.TextSnapshot.Lines.FirstOrDefault((ITextSnapshotLine x) => x.LineNumber == bookmark.Line - 1);
      }
      catch (Exception lineEx)
      {
#if DEBUG
        Dte.WriteInOutPutForceShow($"Erro ao recuperar linha no início: {lineEx}");
#endif
      }

      double result;
      if (line == null || line.LineNumber > bookmark.Line)
        result = -1.0;

      else
      {
#if DEBUG
        try
        {
          var bla = line.Start;
          System.Diagnostics.Trace.WriteLine(bla);
        }
        catch (Exception verLinhaValidaEx)
        {
          Dte.WriteInOutPutForceShow($"Start da linha? Deu erro aqui: {verLinhaValidaEx}");
        }
#endif

        // Estava dando erro de: Cannot access a disposed object Object name FormattedLine

#pragma warning disable S125 // Sections of code should not be commented out
        //IWpfTextViewLine wpfTextViewLine = _textView.TextViewLines.WpfTextViewLines.FirstOrDefault((IWpfTextViewLine tvl) => tvl.Start.Equals(line.Start));

        //int i = 0;
        int j = 0;
#pragma warning restore S125 // Sections of code should not be commented out
        IWpfTextViewLine wpfTextViewLine = null;
        System.Collections.ObjectModel.ReadOnlyCollection<IWpfTextViewLine> wpfLinhas = null;
        try
        {
          try
          {
            wpfLinhas = _textView.TextViewLines.WpfTextViewLines;
          }
          catch (Exception wpfLinhasEx)
          {
#if DEBUG
            Dte.WriteInOutPutForceShow($"Nem consegui acessar a propriedade WpfTextViewLines: {wpfLinhasEx.Message}");
            Dte.WriteInOutPut($"  Exceção completa: {wpfLinhasEx}");
            Dte.WriteInOutPut("  Saindo com -1");
#endif
            return -1;
          }

#if DEBUG
          bool peloMenosUmaLinhaValida = false;
#endif
          while (j < wpfLinhas.Count)
          {
            IWpfTextViewLine linhaAtual = wpfLinhas.ElementAt(j);
            if (!linhaAtual.IsValid)
            {
              j++;
              continue;
            }

#if DEBUG
            peloMenosUmaLinhaValida = true;
#endif
            if (linhaAtual.Start.Equals(line.Start))
            {
              wpfTextViewLine = linhaAtual;
              break;
            }
            j++;
          }

#if DEBUG
          if (!peloMenosUmaLinhaValida)
            Dte.WriteInOutPutForceShow("Nenhuma linha válida!!!");
#endif

          //foreach (IWpfTextViewLine esteLine in _textView.TextViewLines.WpfTextViewLines)

#pragma warning disable S125 // Sections of code should not be commented out
          //{
          //  if (esteLine.Start.Equals(line.Start))
          //  {
          //    wpfTextViewLine = esteLine;
          //    break;
          //  }
          //  i++;
          //}

        }
#pragma warning restore S125 // Sections of code should not be commented out
        catch (Exception ex)
        {
#if DEBUG          
          Dte.WriteInOutPutForceShow($"Erro no GetVerticalOffsetForBookmark do BoookMark: {ex.Message}");
          Dte.WriteInOutPut($"Exceção completa: {ex}");
          Dte.WriteInOutPut($"Qual linha deu problema: {j}");

          bool ok = true;
          try
          {
            Dte.WriteInOutPut($"_textView.TextViewLines?");
            Dte.WriteInOutPut($"  {_textView.TextViewLines}");
          }
          catch (Exception textViewLinesEx)
          {
            ok = false;
            Dte.WriteInOutPut($"textViewLinesEx: {textViewLinesEx}");
          }

          if (ok)
          {
            try
            {
              Dte.WriteInOutPut($"_textView.TextViewLines.WpfTextViewLines?");
              Dte.WriteInOutPut($"  {_textView.TextViewLines.WpfTextViewLines}");
            }
            catch (Exception wpfTextViewLinesEx)
            {
              ok = false;
              Dte.WriteInOutPut($"wpfTextViewLinesEx: {wpfTextViewLinesEx}");
            }
          }

          if (ok)
          {
            try
            {
              Dte.WriteInOutPut($"_textView.TextViewLines.WpfTextViewLines.ElementAt(i)");
              Dte.WriteInOutPut($"  {_textView.TextViewLines.WpfTextViewLines.ElementAt(j)}");
            }
            catch (Exception elementAtEx)
            {
              ok = false;
              Dte.WriteInOutPut($"elementAtEx: {elementAtEx}");
            }
          }

          if (ok)
          {
            try
            {
              Dte.WriteInOutPut($"_textView.TextViewLines.WpfTextViewLines.ElementAt(i).IsValid");
              Dte.WriteInOutPut($"  {_textView.TextViewLines.WpfTextViewLines.ElementAt(j).IsValid}");
            }
            catch (Exception isValidEx)
            {
              Dte.WriteInOutPut($"isValidEx: {isValidEx}");
            }
          }
#endif
        }

        bool flag = wpfTextViewLine == null || wpfTextViewLine.VisibilityState == VisibilityState.Hidden || wpfTextViewLine.VisibilityState == VisibilityState.Unattached;
        if (flag)
          result = -1.0;

        else
          result = wpfTextViewLine.TextTop - _textView.ViewportTop;
      }
      return result;
    }

    void UpdateBookmark(Bookmark bookmark)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (bookmark != null)
      {
        double verticalOffsetForBookmark = GetVerticalOffsetForBookmark(bookmark);
        if (verticalOffsetForBookmark >= 0.0)
        {
          CreateGlyph(bookmark, verticalOffsetForBookmark);
        }
      }
    }

    void UpdateBookmarks()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (_marginCanvas.Children.Count > 0)
        _marginCanvas.Children.Clear();

      if (_isDisposed || _textView.IsClosed)
        return;

      if (BookmarkController._instance == null)
        return;
      else if (Enabled)
      {
        BookmarkController.Instance.BookmarksChanged -= new EventHandler(BookmarksChanged);
        BookmarkController.Instance.BookmarksChanged += new EventHandler(BookmarksChanged);
        BookmarkController.Instance.IsBookmarkBarProvided = true;
      }

      if (WSPackPackage.Instance != null)
        BookmarkController.Instance.IsBookmarkBarProvided = Enabled;
      if (!Enabled)
        return;

      Border borda = null;
      var str = new StringBuilder();

      foreach (Bookmark esteMarcador in BookmarkController.Instance.Lista)
      {
        bool pode = CanUpdateBookmark(esteMarcador);
        if (pode)
        {
          if (borda == null)
          {
            borda = CriarIdentificadorBookmarks();
          }
          UpdateBookmark(esteMarcador);

          string aux = esteMarcador.ToString();
          var splitted = aux.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
          if (splitted.Length > 0)
            aux = splitted[0];
          str.AppendLine(aux);
        }
      }

      if (borda != null)
        ((TextBlock)borda.Child).ToolTip += Environment.NewLine + Environment.NewLine + str.ToString();
    }

    bool CanUpdateBookmark(Bookmark bookmark)
    {
      bool result;
      if (bookmark == null)
        result = false;

      else
      {
        GetDocumentPath();
        result = _documentPath.EqualsInsensitive(bookmark.FullName);
      }
      return result;
    }

    private void GetDocumentPath()
    {
      if (_documentPath == null)
        _documentPath = _textView.GetDocumentPath();
    }

    void BookmarksChanged(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      UpdateBookmarks();
      //await ThreadHelper.JoinableTaskFactory.RunAsync(() =>

#pragma warning disable S125 // Sections of code should not be commented out
      //{
      //  UpdateBookmarks();
      //  return null;
      //});

      //base.Dispatcher.Invoke(delegate
      //{
      //  UpdateBookmarks();
      //});
    }
#pragma warning restore S125 // Sections of code should not be commented out

    void TextViewLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (e.OldSnapshot.LineCount != e.NewSnapshot.LineCount && e.NewOrReformattedLines.Count > 0)
      {
        var lstBookmarkToAdd = new List<Bookmark>();

        // Se linhas foram incluídas, o índice dos bookmarks tem que aumentar; senão, diminui
        int ajusteLinhasBM = e.NewSnapshot.LineCount - e.OldSnapshot.LineCount;
        int primeiraLinhaAfetada = e.NewOrReformattedLines[0].EndIncludingLineBreak.GetContainingLine().LineNumber;
        int ultimaLinhaAfetada = e.NewOrReformattedLines[e.NewOrReformattedLines.Count - 1].EndIncludingLineBreak.GetContainingLine().LineNumber;

        IEnumerable<Bookmark> marcadores;
        if (primeiraLinhaAfetada == ultimaLinhaAfetada)
          marcadores = BookmarkController.Instance.Lista.Where(b => b.FullName == _documentPath &&
            b.Line > primeiraLinhaAfetada);
        else
          marcadores = BookmarkController.Instance.Lista.Where(b => b.FullName == _documentPath &&
            b.Line >= primeiraLinhaAfetada);

        foreach (var bm in marcadores)
        {
          lstBookmarkToAdd.Add(bm);
          BookmarkController.Instance.RemoveBookmark(bm);
        }

        lstBookmarkToAdd.ForEach(b =>
        {
          int novoIndice = b.Line + ajusteLinhasBM;

          // Se o novo índice for anterior à primeira linha afetada, quer dizer que excluímos a linha onde o marcador estava
          if (novoIndice >= primeiraLinhaAfetada)
          {
            var novo = new Bookmark(b.Name, b.Number, novoIndice, b.Column, b.FullName, b.Offset);
            BookmarkController.Instance.ToggleBookmark(novo);
          }
        });
      }

      UpdateBookmarks();
    }

    void TextViewViewportHeightChanged(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      UpdateBookmarks();
    }

    void ThrowIfDisposed()
    {
      if (_isDisposed)
      {
        throw new ObjectDisposedException(MarginName);
      }
    }

    #region IWpfTextViewMargin Members

    /// <summary>
    /// Gets the <see cref="T:System.Windows.FrameworkElement" /> that renders the margin.
    /// </summary>
    public FrameworkElement VisualElement
    {
      get
      {
        ThrowIfDisposed();
        return this;
      }
    }
    #endregion

    #region ITextViewMargin Members

    /// <summary>
    /// Determines whether the margin is enabled.
    /// </summary>
    public bool Enabled
    {
      get
      {
        ThrowIfDisposed();
        bool visivel = WSPackPackage.Instance != null &&
          WSPackPackage.ParametrosMEFObjects != null &&
          WSPackPackage.ParametrosMEFObjects.UseBookmarks;
        Visibility = visivel ? Visibility.Visible : Visibility.Collapsed;
        return visivel;
      }
    }

    /// <summary>
    /// Gets the <see cref="T:Microsoft.VisualStudio.Text.Editor.ITextViewMargin" /> with the specified margin name.
    /// </summary>
    /// <param name="marginName">The name of the <see cref="T:Microsoft.VisualStudio.Text.Editor.ITextViewMargin" />.</param>
    /// <returns>
    /// The <see cref="T:Microsoft.VisualStudio.Text.Editor.ITextViewMargin" /> named <paramref name="marginName" />, or null if no match is found.
    /// </returns>
    public ITextViewMargin GetTextViewMargin(string marginName)
    {
      return (marginName == MarginName) ? this : null;
    }

    /// <summary>
    /// Gets the size of the margin.
    /// </summary>
    public double MarginSize
    {
      get
      {
        ThrowIfDisposed();
        return ActualWidth;
      }
    }
    #endregion

    #region IDisposable Members

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="disposing">Disposing</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        _isDisposed = true;
      }
    }

    #endregion


  }
}