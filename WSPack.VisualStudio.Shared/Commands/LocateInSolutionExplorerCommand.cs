using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EnvDTE;

using EnvDTE80;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para Localizar um item no Solution Explorer
  /// </summary>
  internal sealed class LocateInSolutionExplorerCommand : BaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.LocateInSolutionExplorer;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocateInSolutionExplorerCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public LocateInSolutionExplorerCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static LocateInSolutionExplorerCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new LocateInSolutionExplorerCommand(package, commandService);
    }

    static Window2 GetWindow(Windows2 windows, vsWindowType vsWindowType)
    {
      return windows.Cast<Window2>().FirstOrDefault((Window2 w) => w.Type == vsWindowType);
    }

    (Property TrackFileSelectionInExplorer, bool OldMarcou) GetTrackFileSelectionInExplorer()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      bool oldMarcou = false;
      Property propTrackFileSelectionInExplorer = null;
      try
      {
        propTrackFileSelectionInExplorer = WSPackPackage.Dte.Properties["Environment", "ProjectsAndSolution"].Item("TrackFileSelectionInExplorer");
        if (propTrackFileSelectionInExplorer.Value is bool boolTrack)
          oldMarcou = boolTrack;
        else
          Utils.LogDebugMessage("TrackFileSelectionInExplorer não é booleana!");
      }
      catch (Exception propEx)
      {
        // Não conseguimos pegar o valor.
        Utils.LogDebugError($"{nameof(GetTrackFileSelectionInExplorer)} Exception: {propEx.Message}");
      }
      return (propTrackFileSelectionInExplorer, oldMarcou);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      try
      {
        ThreadHelper.ThrowIfNotOnUIThread();

        // Tenta pegar o valor da propriedade: TrackFileSelectionInExplorer
        var tupla = GetTrackFileSelectionInExplorer();

        // Desmarca a opção do VS
        SetTrackFileSelectionInExplorer(tupla);

        // Tenta localizar e volta a opção do VS
        TryLocate(tupla);

      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"Execute LocateInSolutionExplorerCommand: {ex.Message}");
      }
    }

    private static void TryLocate((Property TrackFileSelectionInExplorer, bool OldMarcou) tupla)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      try
      {
        Window2 window = GetWindow((Windows2)WSPackPackage.Dte.Windows, vsWindowType.vsWindowTypeSolutionExplorer);
        if (window != null)
        {
          window.Activate();
          try
          {
            WSPackPackage.Dte.ExecuteCommand("SolutionExplorer.SyncWithActiveDocument");
          }
          catch (Exception ex)
          {
            Utils.LogDebugError($"{nameof(TryLocate)}, erro no ExecuteCommand: {ex.Message}");
          }
        }
      }
      finally
      {
        if (tupla.TrackFileSelectionInExplorer != null)
          tupla.TrackFileSelectionInExplorer.Value = tupla.OldMarcou;
      }
    }

    private static void SetTrackFileSelectionInExplorer((Property TrackFileSelectionInExplorer, bool OldMarcou) tupla)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (tupla.TrackFileSelectionInExplorer != null)
      {
        try
        {
          tupla.TrackFileSelectionInExplorer.Value = false;
        }
        catch (Exception setEx)
        {
          Utils.LogDebugError($"{nameof(SetTrackFileSelectionInExplorer)} exception: {setEx.Message}");
        }
      }
    }
  }
}