using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

using EnvDTE;

using EnvDTE80;

using Microsoft.Internal.VisualStudio.Shell;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;
using WSPack.Lib.WPF.Model;
using WSPack.VisualStudio.Shared.Extensions;

namespace WSPack.VisualStudio.Shared.MEFObjects.Bookmarks
{
  /// <summary>
  /// Classe para gerenciar marcadores
  /// </summary>
  class BookmarkController
  {
    Dictionary<string, BindingList<Bookmark>> _dicBookmarks;
    internal static BookmarkController _instance;
    readonly DTE2 _dte;
    EventHandler _onBookmarksAlterados;
    readonly SolutionEvents _solutionEvents;
    private bool _isBookmarkBarProvided;
    static readonly object _locker = new object();
    bool _carregouBookmarks = false;

    #region Construtores internos
    static BookmarkController()
    {
      ThreadHelper.ThrowIfNotOnUIThread($"Construtor estático do {nameof(BookmarkController)}");
    }

    private BookmarkController()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      _dte = WSPackPackage.Dte;
      if (_dte == null)
        _dte = Package.GetGlobalService(typeof(SDTE)) as DTE2;

      Utils.LogDebugMessage("1/2 Marcadores: assinando eventos de solution");
      _solutionEvents = _dte.Events.SolutionEvents;
      _solutionEvents.AfterClosing += FechouSolution;
      _solutionEvents.Opened += AbriuSolution;
      Utils.LogDebugMessage("2/2 Marcadores: eventos de solution assinados");
    }
    #endregion

    static void CreateSingleton()
    {
      lock (_locker)
      {
        Utils.LogDebugMessage("Marcadores: criando singleton");
        ThreadHelper.ThrowIfNotOnUIThread();
        _instance = new BookmarkController
        {
          _dicBookmarks = new Dictionary<string, BindingList<Bookmark>>(StringComparer.OrdinalIgnoreCase)
        };

        if (!_instance._carregouBookmarks)
        {
          var persiste = WSPackPackage.Instance.GetService<SVsSolutionPersistence, IVsSolutionPersistence>(false);
          if (persiste != null)
            WSPackPackage.Instance.LoadUserOptions(persiste, 0);
        }
      }
    }

    void AbriuSolution()
    {
      OnBindingChanged();
      _carregouBookmarks = true;
    }

    private void FechouSolution()
    {
      OnBookmarksChanged();
      _carregouBookmarks = false;
    }

    /// <summary>
    /// Adicionar um marcador lista. O marcador é inserido conforme seu número
    /// </summary>
    /// <param name="bm">Marcador a ser inserido</param>
    void AdicionarBookmarkLista(Bookmark bm)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      BindingList<Bookmark> lst;
      if (_dicBookmarks.ContainsKey(_dte.Solution.FullName))
        lst = _dicBookmarks[_dte.Solution.FullName];
      else
      {
        lst = new BindingList<Bookmark>();
        _dicBookmarks.Add(_dte.Solution.FullName, lst);
      }

      int indice = 0;
      foreach (var item in lst)
      {
        if (item.Number < bm.Number)
          indice++;
        else
          break;
      }
      lst.Insert(indice, bm);
    }

    void OnBookmarksChanged()
    {
      _onBookmarksAlterados?.Invoke(this, EventArgs.Empty);
    }

    void OnBindingChanged()
    {
      BindingChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Chave para escrever no arquivo .suo
    /// </summary>
    string GetChave
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        string nome = _dte.Solution.FullName;
        nome = System.IO.Path.GetFileName(nome);
        return nome;
      }
    }

    /// <summary>
    /// Um valor indicando se foi provida a barra de Bookmarks
    /// </summary>
    internal bool IsBookmarkBarProvided
    {
      get => _isBookmarkBarProvided;
      set
      {
        _isBookmarkBarProvided = value;
        OnBindingChanged();
      }
    }

    /// <summary>
    /// Limpar todos os marcadores da solution ativa
    /// </summary>
    internal void ClearAllBookmarks()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (_dicBookmarks.ContainsKey(_dte.Solution.FullName))
      {
        _dicBookmarks[_dte.Solution.FullName].Clear();
        OnBookmarksChanged();
      }
    }

    internal void RemoveBookmark(int number)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      Bookmark bm = Get(number);
      if (bm != null)
      {
        if (_dicBookmarks.ContainsKey(_dte.Solution.FullName))
        {
          _dicBookmarks[_dte.Solution.FullName].Remove(bm);
          OnBookmarksChanged();
        }
      }
    }

    internal void RemoveBookmark(Bookmark bookmark)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (bookmark != null)
      {
        RemoveBookmark(bookmark.Number);
      }
    }

    internal void GotoBookmark(int number)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      GotoBookmark(Get(number));
    }

    internal void GotoBookmark(Bookmark bookmark)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (bookmark != null)
      {
        if (_dte != null)
        {
          ProjectItem projectItem = _dte.Solution.FindProjectItem(bookmark.FullName);
          if (projectItem != null)
          {
            string vsView = bookmark.FullName.EndsWithInsensitive(".xaml") ?
              BookmarkMargin.vsViewKindDefault : BookmarkMargin.vsViewKindCode;

            Window window = projectItem.Open(vsView);
            if (window != null)
            {
              window.Activate();
              TextSelection textSelection = (TextSelection)window.Selection;
              textSelection.StartOfDocument(false);
              try
              {
                textSelection.MoveToLineAndOffset(bookmark.Line, bookmark.Column, false);
                if (textSelection.ActivePoint.DisplayColumn < bookmark.Column)
                  textSelection.MoveToDisplayColumn(bookmark.Line, textSelection.ActivePoint.DisplayColumn);
              }
              catch (ArgumentException argEx)
              {
                textSelection.EndOfDocument();
                System.Diagnostics.Trace.WriteLine($"Nâo foi possível ir para o marcador: {argEx.Message}");
                _dte.WriteInOutPut(bookmark.ToString());
              }
              catch (Exception ex)
              {
                _dte.WriteInOutPut(ex.Message);
              }
            }
          }
          else
            _dte.WriteInOutPutForceShow(ResourcesLib.StrMarcadorNaoEncontradoNaSolution
              .FormatWith(_dte.Solution.FullName, bookmark.ToString()));
        }
      }
    }

    internal void ToggleBookmark(Bookmark bookmark)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      Bookmark achou = Get(bookmark.Number);
      bool igual = bookmark.Equals(achou);
      if (igual)
      {
        if (_dicBookmarks.ContainsKey(_dte.Solution.FullName))
        {
          _dicBookmarks[_dte.Solution.FullName].Remove(achou);
        }
      }
      else
      {
        if (achou != null)
          RemoveBookmark(achou);
        AdicionarBookmarkLista(bookmark);
      }

      OnBookmarksChanged();
    }

    internal void ToggleBookmark(int number)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (_dte != null)
      {
        Document activeDocument = _dte.ActiveDocument;
        if (activeDocument != null)
        {
          TextSelection textSelection = (TextSelection)activeDocument.Selection;
          VirtualPoint activePoint = textSelection.ActivePoint;
          Bookmark bookmark = new Bookmark(string.Empty, number, activePoint.Line, activePoint.DisplayColumn,
            activeDocument.FullName, activePoint.AbsoluteCharOffset);
          ToggleBookmark(bookmark);
        }
      }
    }

    /// <summary>
    /// Lista interna de marcadores
    /// </summary>
    /// <returns>Lista interna de marcadores</returns>
    internal BindingList<Bookmark> GetBindingList()
    {
      ThreadHelper.ThrowIfNotOnUIThread();

      if (!_isBookmarkBarProvided)
        return new BindingList<Bookmark>();

      if (_dicBookmarks.ContainsKey(_dte.Solution.FullName))
        return _dicBookmarks[_dte.Solution.FullName];
      else
      {
        BindingList<Bookmark> lst = new BindingList<Bookmark>();
        _dicBookmarks.Add(_dte.Solution.FullName, lst);
        return lst;
      }
    }

    /// <summary>
    /// Recuperar um bookmark definido no número: <paramref name="number"/>
    /// </summary>
    /// <param name="number">Nº do marcador a ser verificado</param>
    /// <returns>Bookmar definido; null, caso contrário</returns>
    internal Bookmark Get(int number)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (_dicBookmarks.ContainsKey(_dte.Solution.FullName))
        return _dicBookmarks[_dte.Solution.FullName].FirstOrDefault(b => b.Number == number);
      else
        return null;
    }

    /// <summary>
    /// Recuperar um Bookmark definido para a <paramref name="lineNumber"/> no arquivo <paramref name="fullPath"/>
    /// </summary>
    /// <param name="lineNumber">Nº da linha</param>
    /// <param name="fullPath">Nome completo do arquivo</param>
    /// <returns>Bookmar definido; null, caso contrário</returns>
    internal Bookmark Get(int lineNumber, string fullPath)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (_dicBookmarks.ContainsKey(_dte.Solution.FullName))
        return _dicBookmarks[_dte.Solution.FullName].FirstOrDefault(b => b != null && b.Line == lineNumber &&
        b.FullName.EqualsInsensitive(fullPath));
      else
        return null;
    }

    /// <summary>
    /// Criar um bookmar na primeira posição disponível
    /// </summary>
    /// <param name="lineNumber">Nº da linha</param>
    /// <param name="column">Nº da coluna</param>
    /// <param name="fullPath">Nome completo do arquivo</param>
    /// <returns>Bookmark criado; null se não haviam posições disponíveis</returns>
    internal Bookmark CreateBookmark(int lineNumber, int column, string fullPath)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (_dicBookmarks.ContainsKey(_dte.Solution.FullName))
      {
        if (_dicBookmarks[_dte.Solution.FullName].Count == 10)
          return null;
      }

      // Vamos começar a procurar na posição 1
      int numero = 1;
      Bookmark achou;
      do
      {
        // Se não achou, é porque a posição está disponível
        achou = Get(numero);
        if (achou == null)
          break;
        else
          numero++;
      } while (numero < 10);

      // Se achou, vamos verificar se a posição zero está disponível
      if (achou != null)
      {
        numero = 0;
        achou = Get(numero);
        if (achou != null)
          return null;
      }

      TextSelection textSelection = (TextSelection)_dte.ActiveDocument.Selection;
      VirtualPoint activePoint = textSelection.ActivePoint;
      Bookmark bm = new Bookmark(string.Empty, numero, lineNumber, column, fullPath, activePoint.AbsoluteCharOffset);
      ToggleBookmark(bm);
      return bm;
    }

    /// <summary>
    /// Occurs when [bookmarks changed].
    /// </summary>
    public event EventHandler BookmarksChanged
    {
      add
      {
        _onBookmarksAlterados = (EventHandler)Delegate.Combine(_onBookmarksAlterados, value);
      }
      remove
      {
        _onBookmarksAlterados = (EventHandler)Delegate.Remove(_onBookmarksAlterados, value);
      }
    }

    /// <summary>
    /// Acontece quando o bind da lista de bookmark é alterado
    /// </summary>
    public event EventHandler BindingChanged;

    /// <summary>
    /// Retornar uma instância do controlador de marcadores
    /// </summary>
    public static BookmarkController Instance
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread($"Acesso à propriedade {nameof(BookmarkController)}.{nameof(Instance)}");
        if (_instance == null)
          CreateSingleton();
        return _instance;
      }
    }

    /// <summary>
    /// (Gets) Lista de marcadores definidos
    /// </summary>
    public IList<Bookmark> Lista
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        List<Bookmark> lstRetorno = new List<Bookmark>();
        if (!_dicBookmarks.ContainsKey(_dte.Solution.FullName))
          return lstRetorno;

        foreach (var item in _dicBookmarks[_dte.Solution.FullName])
        {
          item.IsEnabled = _dte.Solution.FindProjectItem(item.FullName) != null;
          lstRetorno.Add(item);
        }
        return lstRetorno;
      }
    }

    #region IVsPersistSolutionOpts Members

    private void WriteOptions(System.IO.Stream storageStream)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      using (var bw = new System.IO.BinaryWriter(storageStream))
      {
        var x = new System.Xml.Serialization.XmlSerializer(typeof(BindingList<Bookmark>));
        string nome = _dte.Solution.FullName;
        if (_dicBookmarks.ContainsKey(nome))
          x.Serialize(storageStream, _dicBookmarks[nome]);
      }
    }

    private void LoadOptions(System.IO.Stream storageStream)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      using (var bReader = new System.IO.BinaryReader(storageStream))
      {
        var x = new System.Xml.Serialization.XmlSerializer(typeof(BindingList<Bookmark>));
        try
        {
          string nome = _dte.Solution.FullName;
          BindingList<Bookmark> o = x.Deserialize(storageStream) as BindingList<Bookmark>;
          _dicBookmarks[nome] = o;
          _carregouBookmarks = true;
        }
        catch (Exception ex)
        {
          Trace.Write("Erro ao carregar marcadores");
          Trace.Write($"{_dte.Solution.FullName}: {ex.Message}");
        }
      }
    }

    public int ReadUserOptions(IStream pOptionsStream, string pszKey)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      try
      {
        using (var wrapper = new DataStreamFromComStream(pOptionsStream))
        {
          if (pszKey == GetChave)
          {
            LoadOptions(wrapper);
          }
        }
        return VSConstants.S_OK;
      }
      catch (Exception comEx)
      {
        Trace.Write(comEx.ToString());
        return VSConstants.S_FALSE;
      }
      finally
      {
        if (System.Runtime.InteropServices.Marshal.IsComObject(pOptionsStream))
          System.Runtime.InteropServices.Marshal.ReleaseComObject(pOptionsStream);
      }
    }

    public int WriteUserOptions(IStream pOptionsStream, string pszKey)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      try
      {

        using (var wrapper = new DataStreamFromComStream(pOptionsStream))
        {
          if (pszKey == GetChave)
          {
            WriteOptions(wrapper);
          }
        }

        return VSConstants.S_OK;
      }
      catch (Exception comEx)
      {
        Trace.Write(comEx.ToString());
        return VSConstants.S_FALSE;
      }
      finally
      {
        if (System.Runtime.InteropServices.Marshal.IsComObject(pOptionsStream))
          System.Runtime.InteropServices.Marshal.ReleaseComObject(pOptionsStream);
      }
    }

    public int LoadUserOptions(IVsPersistSolutionOpts host, IVsSolutionPersistence pPersistence, uint grfLoadOpts)
    {
      try
      {
        Trace.WriteLine($"LoadUserOptions.grfLoadOpts: {grfLoadOpts}");
        ThreadHelper.ThrowIfNotOnUIThread();
        int i = pPersistence.LoadPackageUserOpts(host, GetChave);
      }
      catch (Exception comEx)
      {
        Trace.WriteLine(comEx.ToString());
        return VSConstants.S_FALSE;
      }
      finally
      {
        if (System.Runtime.InteropServices.Marshal.IsComObject(pPersistence))
          System.Runtime.InteropServices.Marshal.ReleaseComObject(pPersistence);
      }

      return VSConstants.S_OK;
    }

    public int SaveUserOptions(IVsPersistSolutionOpts host, IVsSolutionPersistence pPersistence)
    {
      try
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        int i = pPersistence.SavePackageUserOpts(host, GetChave);
      }
      catch (Exception comEx)
      {
        Trace.WriteLine(comEx.ToString());
        return VSConstants.S_FALSE;
      }
      finally
      {
        if (System.Runtime.InteropServices.Marshal.IsComObject(pPersistence))
          System.Runtime.InteropServices.Marshal.ReleaseComObject(pPersistence);
      }

      return VSConstants.S_OK;
    }
    #endregion
  }
}