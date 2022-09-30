using Microsoft.VisualStudio.Shell;

using System;
using System.Runtime.InteropServices;
using System.Threading;

using WSPack.Lib;
using WSPack.VisualStudio.Shared.Commands;

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
  public sealed class WSPackPackage : AsyncPackage
  {
    /// <summary>
    /// WSPackPackage GUID string.
    /// </summary>
    public const string PackageGuidString = "fac153ab-13d6-4424-9548-cf4dfed7750f";

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
      await AboutCommand.InitializeAsync(this);
    }

  }
}
