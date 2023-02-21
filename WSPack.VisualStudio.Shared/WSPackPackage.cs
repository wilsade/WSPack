using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using EnvDTE80;

using Microsoft;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Settings;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared;
using WSPack.VisualStudio.Shared.Commands;
using WSPack.VisualStudio.Shared.MEFObjects.Bookmarks;
using WSPack.VisualStudio.Shared.Options;

using Task = System.Threading.Tasks.Task;

namespace WSPack
{
  /// <summary>
  /// Package do WSPack
  /// </summary>
  [InstalledProductRegistration("#110", "#112", Constantes.NumeroVersao, IconResourceID = 401)]
  [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
  [ProvideAutoLoad(UIContextGuids80.NoSolution, PackageAutoLoadFlags.BackgroundLoad)]
  [Guid(PackageGuidString)]
  [ProvideMenuResource("Menus.ctmenu", 1)]

  [ProvideOptionPage(typeof(PageGeneral), Constantes.WSPack, "01GeralX", 110, 113, true, new string[] { "Mudar as opções do WSPack" }, Sort = 1)]
  [ProvideOptionPage(typeof(PageMEFObjects), Constantes.WSPack, "03ComponentesX", 110, 115, true, new string[] { "MEF Components" }, Sort = 3)]
  [ProvideOptionPage(typeof(PageTemplateCheckIn), Constantes.WSPack, "05TemplateCheckInX", 110, 114, true, new string[] { "Template Check In" }, Sort = 5)]

  [ProvideUIContextRule(UiNotSolutionBuilding,
        name: "Not building",
        expression: "(SolutionHasSingleProject | SolutionHasMultipleProjects) & !SolutionBuilding",
        termNames: new[] { "SolutionHasSingleProject", "SolutionHasMultipleProjects", "SolutionBuilding" },
        termValues: new[] { UIContextGuids80.SolutionHasSingleProject,
          UIContextGuids80.SolutionHasMultipleProjects, UIContextGuids80.SolutionBuilding }
  )]
  public sealed class WSPackPackage : AsyncPackage, IVsPersistSolutionOpts
  {
    private const string UiNotSolutionBuilding = "24551deb-f034-43e9-a279-0e541241687e";

    /// <summary>
    /// Devolve a instãncia da classe: <see cref="WSPackPackage"/>
    /// </summary>
    /// <value>Instância da classe: <see cref="WSPackPackage"/></value>
    public static WSPackPackage Instance { get; private set; }

    /// <summary>
    /// DTE
    /// </summary>
    public static DTE2 Dte;

    /// <summary>
    /// ServiceProvider
    /// </summary>
    public static System.IServiceProvider ServiceProvider => (System.IServiceProvider)Instance;

    /// <summary>
    /// Serviço de solution
    /// </summary>
    public IVsSolution SolutionSevice { get; private set; }

    /// <summary>
    /// Serviço para o controle de fonte
    /// </summary>
    public IVsGetScciProviderInterface ScciProviderInterface { get; private set; }

    /// <summary>
    /// Serviço para o controle de fonte
    /// </summary>
    public IVsRegisterScciProvider ScciProvider { get; private set; }

    /// <summary>
    /// WSPackPackage GUID string.
    /// </summary>
    public const string PackageGuidString = "fac153ab-13d6-4424-9548-cf4dfed7750f";

    /// <summary>
    /// Recuperar a página de opções: Parâmetros gerais
    /// </summary>
    public static PageGeneral ParametrosGerais => GetParametersPage<PageGeneral>();

    /// <summary>
    /// Página de opções de Template com informações do Check In
    /// </summary>
    public static PageTemplateCheckIn ParametrosTemplateCheckIn { get; private set; }

    /// <summary>
    /// Recuperar a página de opções de componentes: MEFObjects
    /// </summary>
    public static PageMEFObjects ParametrosMEFObjects => GetParametersPage<PageMEFObjects>();

    /// <summary>
    /// Initialization of the package; this method is called right after the package is sited, so this is the place
    /// where you can put all the initialization code that rely on services provided by VisualStudio.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
    /// <param name="progress">A provider for progress updates.</param>
    /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
    protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
    {
      // When initialized asynchronously, the current thread may be a background thread at this point.
      // Do any initialization that requires the UI thread after switching to the UI thread.
      await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
      Instance = this;

      Dte = await GetServiceAsync(typeof(SDTE)) as DTE2;
      Assumes.Present(Dte);

      ParametrosTemplateCheckIn = GetParametersPage<PageTemplateCheckIn>();

      OleMenuCommandService commandService = await GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;

      SolutionSevice = await GetServiceAsync(typeof(SVsSolution)) as IVsSolution;
      Assumes.Present(SolutionSevice);

      ScciProviderInterface = await GetServiceAsync(typeof(IVsRegisterScciProvider)) as IVsGetScciProviderInterface;
      Assumes.Present(ScciProviderInterface);

      ScciProvider = await GetServiceAsync(typeof(IVsRegisterScciProvider)) as IVsRegisterScciProvider;
      Assumes.Present(ScciProvider);

      await AboutCommand.InitializeAsync(this, commandService);
      await ParametersCommand.InitializeAsync(this, commandService);

      await CopyLocalPathSolutionExplorerCommand.InitializeAsync(this, commandService);
      await CopyLocalPathCodeEditorCommand.InitializeAsync(this, commandService);
      await CopyLocalPathSourceControlExplorerCommand.InitializeAsync(this, commandService);

      await LocateInTFSSolutionExplorerCommand.InitializeAsync(this, commandService);
      await LocateInTFSCodeEditorCommand.InitializeAsync(this, commandService);

      await FlexSourceControlExplorerCommand.InitializeAsync(this, commandService);
      await FlexGitChanges.InitializeAsync(this, commandService);
      await FlexGitRepositoryCommand.InitializeAsync(this, commandService);
      await FlexUndoCommand.InitializeAsync(this, commandService);
      await FlexViewHistoryCommand.InitializeAsync(this, commandService);
      await FlexCommitCommand.InitializeAsync(this, commandService);
      await FlexCompareCommand.InitializeAsync(this, commandService);

      await CopyServerPathSolutionExplorerCommand.InitializeAsync(this, commandService);
      await CopyServerPathCodeEditorCommand.InitializeAsync(this, commandService);
      await CopyServerPathSourceControlExplorerCommand.InitializeAsync(this, commandService);

      await LocateInWindowsSolutionExplorerCommand.InitializeAsync(this, commandService);
      await LocateInWindowsCodeEditorCommand.InitializeAsync(this, commandService);
      await LocateInWindowsSourceControlExplorerCommand.InitializeAsync(this, commandService);
      await OpenInEditorSolutionExplorerCommand.InitializeAsync(this, commandService);
      await OpenInEditorCodeEditorCommand.InitializeAsync(this, commandService);
      await OpenInEditorSourceControlExplorerCommand.InitializeAsync(this, commandService);

      await ActivityLogCommand.InitializeAsync(this, commandService);

      await OpenOutputPathSolutionExplorerCommand.InitializeAsync(this, commandService);
      await OpenOutputPathCommand.InitializeAsync(this, commandService);
      await OpenCmdPromptSolutionExplorerCommand.InitializeAsync(this, commandService);
      await OpenCmdPromptCodeEditorCommand.InitializeAsync(this, commandService);
      await ProjectCommandLineArgsCommand.InitializeAsync(this, commandService);
      await StartupProjectCommandLineArgsCommand.InitializeAsync(this, commandService);
      await LocateInSolutionExplorerCommand.InitializeAsync(this, commandService);
      await DestroyTFSItemCommand.InitializeAsync(this, commandService);
      await VariaveisAmbienteCommand.InitializeAsync(this, commandService);
      await DiagnosticLogCommand.InitializeAsync(this, commandService);
      await MessageBoxTesterCommand.InitializeAsync(this, commandService);
      await TemplateCheckInCommand.InitializeAsync(this, commandService);
      await SearchChangesetsCommand.InitializeAsync(this, commandService);
      await SearchChangesetsSourceControlExplorerCommand.InitializeAsync(this, commandService);
      await MergeHelperCommand.InitializeAsync(this, commandService);
      await MergeInViewHistoryCommand.InitializeAsync(this, commandService);
      await RestartCommand.InitializeAsync(this, commandService);
      await DisconnectAndCloseCommand.InitializeAsync(this, commandService);
      await WorkOfflineCommand.InitializeAsync(this, commandService);
      await CollapseAllIncludedPendingChangesCommand.InitializeAsync(this, commandService);
      await CollapseAllExcludedPendingChangesCommand.InitializeAsync(this, commandService);
      await DefineTabOrderCommand.InitializeAsync(this, commandService);
      await GenerateResourceCommand.InitializeAsync(this, commandService);
      await FormatOnSaveCommand.InitializeAsync(this, commandService);
      await ForceUTF8OnSaveCommand.InitializeAsync(this, commandService);
      await SolutionExplorerLocateItemCommand.InitializeAsync(this, commandService);
      await ChangeSourceControlCommand.InitializeAsync(this, commandService);
      await GitPullToolbarSolutionExplorerCommand.InitializeAsync(this, commandService);
      await GitChangesCollapseAllCommand.InitializeAsync(this, commandService);
      await TFSFavoritesManagerCommand.InitializeAsync(this, commandService);
      await SourceControlExplorerLocateItemCommand.InitializeAsync(this, commandService);
      await ComboBoxSSEPopulateCommand.InitializeAsync(this, commandService);
      await ComboBoxSSEClickCommand.InitializeAsync(this, commandService);
      await TFSAddedFavoritesCommand.InitializeAsync(this, commandService);
      await TFSAddFavoriteCommand.InitializeAsync(this, commandService);
      await TFSRemoveFavoriteCommand.InitializeAsync(this, commandService);
      await TFSGotoActiveFavoriteCommand.InitializeAsync(this, commandService);
      await ToggleBookmarkBaseCommand.InitializeAsync(this, commandService);
    }

    /// <summary>
    /// Gets the read only user settings storage.
    /// </summary>
    public SettingsStore GetReadOnlyUserSettingsStorage()
    {
      if (GetShellSettingsManager(out var manager))
      {
        SettingsStore settingsStore = manager.GetReadOnlySettingsStore(SettingsScope.UserSettings);
        return settingsStore;
      }
      return null;
    }

    /// <summary>
    /// Gets the writable user settings storage.
    /// </summary>
    public WritableSettingsStore GetWritableUserSettingsStorage()
    {
      if (GetShellSettingsManager(out var manager))
      {
        WritableSettingsStore settingsStore = manager.GetWritableSettingsStore(SettingsScope.UserSettings);
        return settingsStore;
      }
      return null;
    }

    /// <summary>
    /// Recuperar uma página de opções do WSPack
    /// </summary>
    /// <typeparam name="T">Tipo da página</typeparam>
    /// <returns>página de opções do WSPack</returns>
    public static T GetParametersPage<T>() where T : DialogPage
    {
      if (Instance == null)
        return null;

      var basePage = Instance.GetDialogPage(typeof(T));
      if (basePage != null)
      {
        var page = (T)basePage;
        return page;
      }
      return null;
    }

    /// <summary>
    /// Gets the pack service.
    /// </summary>
    /// <param name="t">Type do serviço a ser retornado.</param>
    /// <returns></returns>
    public async Task<T> GetPackServiceAsync<T>(Type t)
    {
      await JoinableTaskFactory.SwitchToMainThreadAsync();
      var servico = await GetServiceAsync(t);
      Assumes.Present(servico);
      return (T)servico;
    }


    /// <summary>
    /// Recuperar o Guid Id do esquema de cores instalado
    /// </summary>
    /// <returns>Guid Id do esquema de cores instalado</returns>
    public static string GetInstalledColorTheme()
    {
      try
      {
        dynamic colorThemeService = GetGlobalService(typeof(SVsColorThemeService));
        Guid id = colorThemeService.CurrentTheme.ThemeId;
        return id.ToString();
      }
      catch (Exception ex)
      {
        Utils.LogDebugError("GetInstalledColorTheme: " + ex.Message);
        return "";
      }
    }

    bool GetShellSettingsManager(out ShellSettingsManager manager)
    {
      manager = null;
      if (GetService(typeof(SVsSettingsManager)) is IVsSettingsManager obj)
      {
        manager = new ShellSettingsManager(obj);
        return true;
      }
      return false;
    }

    #region IVsPersistSolutionOpts Members

    //int IVsPersistSolutionOpts.LoadUserOptions(IVsSolutionPersistence pPersistence, uint grfLoadOpts)
    /// <summary>
    /// Loads user options for a given solution.
    /// </summary>
    /// <param name="pPersistence">[in] Pointer to the  interface on which the VSPackage should call its  method for each stream name it wants to read from the user options (.opt) file.</param>
    /// <param name="grfLoadOpts">[in] User options whose value is taken from the  DWORD.</param>
    /// <returns>If the method succeeds, it returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK" />. If it fails, it returns an error code.</returns>
    public int LoadUserOptions(IVsSolutionPersistence pPersistence, uint grfLoadOpts)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      return BookmarkController.Instance.LoadUserOptions(this, pPersistence, grfLoadOpts);
    }

    int IVsPersistSolutionOpts.ReadUserOptions(IStream pOptionsStream, string pszKey)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      return BookmarkController.Instance.ReadUserOptions(pOptionsStream, pszKey);
    }

    int IVsPersistSolutionOpts.SaveUserOptions(IVsSolutionPersistence pPersistence)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      int res = BookmarkController.Instance.SaveUserOptions(this, pPersistence);
      return res;
    }

    int IVsPersistSolutionOpts.WriteUserOptions(IStream pOptionsStream, string pszKey)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      return BookmarkController.Instance.WriteUserOptions(pOptionsStream, pszKey);
    }
    #endregion
  }

  /// <summary>
  /// SVsColorThemeService proxy
  /// </summary>
  [Guid("0D915B59-2ED7-472A-9DE8-9161737EA1C5")]
#pragma warning disable IDE1006 // Naming Styles
  interface SVsColorThemeService
#pragma warning restore IDE1006 // Naming Styles
  {
  }
}
