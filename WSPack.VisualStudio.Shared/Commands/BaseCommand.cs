using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;

using WSPack.Lib.Extensions;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Command handler
  /// </summary>
  internal abstract class BaseCommand
  {
    IVsTextManager _textManager;
    IVsEditorAdaptersFactoryService _editorAdapterFactory;

    /// <summary>
    /// Gets the service provider from the owner package.
    /// </summary>
    private IAsyncServiceProvider ServiceProvider => _package;

    /// <summary>
    /// VS Package that provides this command, not null.
    /// </summary>
    protected readonly AsyncPackage _package;

    /// <summary>
    /// Menu do comando
    /// </summary>
    protected readonly OleMenuCommand _menu;

    protected abstract void DoExecute(object sender, EventArgs e);

    /// <summary>
    /// Initializes a new instance of the <see cref="AboutCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected BaseCommand(AsyncPackage package, OleMenuCommandService commandService)
    {
      _package = package ?? throw new ArgumentNullException(nameof(package));
      commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

      if (CommandId != 0)
      {
        var menuCommandID = new CommandID(CommandSet, CommandId);
        _menu = new OleMenuCommand(Execute, menuCommandID);
        _menu.BeforeQueryStatus += BeforeExecute;
        commandService.AddCommand(_menu);
      }
    }

    /// <summary>
    /// Get command service
    /// </summary>
    protected async Task<OleMenuCommandService> GetCommandServiceAsync()
    {
      OleMenuCommandService obj = await WSPackPackage.Instance
        .GetPackServiceAsync<OleMenuCommandService>(typeof(IMenuCommandService))
        .ConfigureAwait(false);
      return obj;
    }

    /// <summary>
    /// Criar tecla de atalho para um comando
    /// </summary>
    /// <param name="commandName">Nome do comando.</param>
    /// <param name="shortcut">Atalho. Ex: global::Ctrl+1</param>
    /// <param name="rewrite">true para sobrescrever atalhos existentes</param>
    /// <returns>true se conseguiu criar o atalho</returns>
    protected static bool CreateKeyBindings(string commandName, string shortcut, bool rewrite)
    {
      bool ok = true;
      EnvDTE.Command comando = null;
      ThreadHelper.ThrowIfNotOnUIThread();
      try
      {
        if (!commandName.Contains("."))
          commandName = "WSPack." + commandName;
        comando = WSPackPackage.Dte.Commands.Item(commandName, -1);
      }
      catch (Exception ex)
      {
        Utils.LogDebugMessageForceShow("CreateKeyBindings, comando não encontrado: " + commandName + ": " + ex.Message);
      }

      if (comando != null)
      {
        object[] objeto = (object[])comando.Bindings;
        if (rewrite || objeto == null || objeto.Length == 0)
        {
          try
          {
            comando.Bindings = new object[1] { shortcut };
          }
          catch (Exception ex)
          {
            Utils.LogDebugError($"Atribuir shortcut [{shortcut}] para {commandName}: {ex.GetCompleteMessage()}");
            ok = false;
          }
        }
      }
      return ok;
    }

    /// <summary>
    /// Recuperar o EditorAdapterFactory
    /// </summary>
    IVsEditorAdaptersFactoryService EditorAdapterFactory
    {
      get
      {
        if (_editorAdapterFactory == null)
        {
          IComponentModel model = (IComponentModel)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SComponentModel));
          _editorAdapterFactory = model.GetService<IVsEditorAdaptersFactoryService>();
        }
        return _editorAdapterFactory;
      }
    }

    protected virtual void BeforeExecute(object sender, EventArgs e)
    {
      // Implementado nos filhos
    }

    /// <summary>
    /// Recuperar o caminho do documento ativo do CodeEditor
    /// </summary>
    /// <returns>true se foi possível recuperar um documento ativo do CodeEditor</returns>
    protected string GetActiveDocumentPath()
    {
      if (GetActiveDocumentPath(out var localItem))
        return localItem;
      return string.Empty;
    }

    /// <summary>
    /// Gets the editor active view.
    /// </summary>
    /// <returns>editor active view.</returns>
    IVsTextView GetEditorActiveView()
    {
      if (_textManager == null)
      {
        _textManager = _package.GetService<SVsTextManager, IVsTextManager>();
      }

      _textManager.GetActiveView(1, null, out IVsTextView textViewCurrent);
      return textViewCurrent;
    }

    /// <summary>
    /// Recuperar o caminho do documento ativo do CodeEditor
    /// </summary>
    /// <param name="localItem">Caminho completo do documento</param>
    /// <returns>true se foi possível recuperar um documento ativo do CodeEditor</returns>
    protected bool GetActiveDocumentPath(out string localItem)
    {
      localItem = WSPackPackage.Dte.GetDocumentPath();
      if (!localItem.IsNullOrEmptyEx())
        return true;

      IVsTextView editorView = GetEditorActiveView();
      if (editorView != null)
      {
        var wpfView = EditorAdapterFactory.GetWpfTextView(editorView);
        if (wpfView != null)
        {
          localItem = wpfView.GetDocumentPath();
          return true;
        }
      }
      localItem = null;
      return false;
    }

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public virtual int CommandId => 0;

    /// <summary>
    /// Command menu group (command set GUID).
    /// </summary>
    public static readonly Guid CommandSet = new Guid("d2aa537b-238c-489b-948c-e89f5badbcae");

    /// <summary>
    /// This function is the callback used to execute the command when the menu item is clicked.
    /// See the constructor to see how the menu item is associated with this function using
    /// OleMenuCommandService service and MenuCommand class.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    private void Execute(object sender, EventArgs e)
    {
      DoExecute(sender, e);
    }
  }
}
