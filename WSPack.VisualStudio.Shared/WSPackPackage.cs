using EnvDTE80;

using Microsoft;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Settings;

using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;

using WSPack.Lib;
using WSPack.VisualStudio.Shared.Commands;
using WSPack.VisualStudio.Shared.Options;

using Task = System.Threading.Tasks.Task;

namespace WSPack
{
  /// <summary>
  /// Package do WSPack
  /// </summary>
  [InstalledProductRegistration("#110", "#112", Constantes.NumeroVersao, IconResourceID = 401)]
  [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
  [Guid(PackageGuidString)]
  [ProvideMenuResource("Menus.ctmenu", 1)]

  [ProvideOptionPage(typeof(PageGeneral), Constantes.WSPack, "01GeralX", 110, 113, true, new string[] { "Mudar as opções do WSPack" }, Sort = 1)]
  [ProvideOptionPage(typeof(PageMEFObjects), Constantes.WSPack, "03ComponentesX", 110, 115, true, new string[] { "MEF Components" }, Sort = 3)]

  public sealed class WSPackPackage : AsyncPackage
  {
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
    public static System.IServiceProvider ServiceProvider => (IServiceProvider)Instance;

    /// <summary>
    /// WSPackPackage GUID string.
    /// </summary>
    public const string PackageGuidString = "fac153ab-13d6-4424-9548-cf4dfed7750f";

    /// <summary>
    /// Recuperar a página de opções: Parâmetros gerais
    /// </summary>
    public static PageGeneral ParametrosGerais => GetParametersPage<PageGeneral>();

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

      OleMenuCommandService commandService = await GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
      await AboutCommand.InitializeAsync(this, commandService);
      await ParametersCommand.InitializeAsync(this, commandService);
      await CopyLocalPathSolutionExplorerCommand.InitializeAsync(this, commandService);
      await LocateInTFSSolutionExplorerCommand.InitializeAsync(this, commandService);
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

  }
}
