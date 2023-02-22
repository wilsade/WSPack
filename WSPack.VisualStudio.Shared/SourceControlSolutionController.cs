using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using EnvDTE;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.VisualStudio.Shared.Commands;

namespace WSPack.VisualStudio.Shared
{
  class SourceControlSolutionController : IVsSolutionEvents3, IVsSolutionLoadEvents
  {
    readonly SolutionEvents _solutionEvents;
    readonly Dictionary<string, DateTime> _dicSolutionPorHoraPull;
    readonly Command _pullCommand;

    public SourceControlSolutionController()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      _pullCommand = WSPackPackage.Dte.Commands.Item(CommandNames.TeamGitPull);
      _dicSolutionPorHoraPull = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
      int hr;
      hr = WSPackPackage.Instance.SolutionSevice.AdviseSolutionEvents(this, out uint pdwCookie);
      Marshal.ThrowExceptionForHR(hr);

      _solutionEvents = WSPackPackage.Dte.Events.SolutionEvents;
      _solutionEvents.Opened += new _dispSolutionEvents_OpenedEventHandler(SolutionEvents_Opened);
      SolutionEvents_Opened();
    }

    void SolutionEvents_Opened()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      GitPullToolbarSolutionExplorerCommand.Instance?.SetCanShowIsGit(false);
      if (!string.IsNullOrWhiteSpace(WSPackPackage.Dte?.Solution?.FullName))
      {
        SetSourceControlForSolution(WSPackPackage.Dte.Solution.FullName, false);
      }
    }

    void SetSourceControlForSolution(string solutionFullName, bool isBeforeOpen)
    {
      if (!Utils.IsSolutionGitControlled(solutionFullName))
        return;

      if (!ChangeSourceControlCommand.Instance.IsGit())
        ChangeSourceControlCommand.Instance.RegisterGit();
      GitPullToolbarSolutionExplorerCommand.Instance?.SetCanShowIsGit(true);
      if (!isBeforeOpen)
        CheckPullTime(solutionFullName, WSPackPackage.ParametrosGerais.FazerPullAoAbrirSolution);
    }

    private void CheckPullTime(string solutionFullName, GitPullOnOpenSolutionOptions pullOption)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (pullOption == GitPullOnOpenSolutionOptions.Nao)
        return;

      var agora = DateTime.Now;
      DateTime lastPull;
      if (!_dicSolutionPorHoraPull.ContainsKey(solutionFullName))
      {
        lastPull = DateTime.MinValue;
        _dicSolutionPorHoraPull.Add(solutionFullName, lastPull);
      }
      else
        lastPull = _dicSolutionPorHoraPull[solutionFullName];

      TimeSpan diferenca = DateTime.Now - lastPull;
      double diffEmHoras = diferenca.TotalHours;

      bool okToPull;
      switch (pullOption)
      {
        case GitPullOnOpenSolutionOptions.ACadaHora:
          okToPull = diffEmHoras >= 1;
          break;
        case GitPullOnOpenSolutionOptions.ACada2Horas:
          okToPull = diffEmHoras >= 2;
          break;
        case GitPullOnOpenSolutionOptions.ACada3Horas:
          okToPull = diffEmHoras >= 3;
          break;
        case GitPullOnOpenSolutionOptions.ACada4Horas:
          okToPull = diffEmHoras >= 4;
          break;
        case GitPullOnOpenSolutionOptions.UmaVezAoDia:
          okToPull = lastPull.AddDays(1).Date <= agora.Date;
          break;
        default:
          okToPull = true;
          break;
      }
      if (okToPull)
      {
        if (_pullCommand.IsAvailable)
          WSPackPackage.Dte.ExecuteCommand(CommandNames.TeamGitPull);
        else
          _ = Utils.TryPullAsync(solutionFullName);
        _dicSolutionPorHoraPull[solutionFullName] = agora;
      }
      else
      {
        Utils.LogOutputMessage("Pull não realizado!".NewLine() +
          $"Parâmetro para fazer Pull ao abrir Solution: {pullOption}".NewLine() +
          $"Data /Hora do último Pull: {lastPull}".NewLine() +
          $"Data/Hora atual..........: {DateTime.Now}".NewLine());
      }
    }

    public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
    {
      return VSConstants.S_OK;
    }

    public int OnBeforeOpenSolution(string pszSolutionFilename)
    {
      SetSourceControlForSolution(pszSolutionFilename, true);
      return VSConstants.S_OK;
    }

    public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
    {
      return VSConstants.S_OK;
    }

    public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
    {
      return VSConstants.S_OK;
    }

    public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
    {
      return VSConstants.S_OK;
    }

    public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
    {
      return VSConstants.S_OK;
    }

    public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
    {
      return VSConstants.S_OK;
    }

    public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
    {
      return VSConstants.S_OK;
    }

    public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
    {
      return VSConstants.S_OK;
    }

    public int OnBeforeCloseSolution(object pUnkReserved)
    {
      return VSConstants.S_OK;
    }

    public int OnAfterCloseSolution(object pUnkReserved)
    {
      return VSConstants.S_OK;
    }

    public int OnAfterMergeSolution(object pUnkReserved)
    {
      return VSConstants.S_OK;
    }

    public int OnBeforeOpeningChildren(IVsHierarchy pHierarchy)
    {
      return VSConstants.S_OK;
    }

    public int OnAfterOpeningChildren(IVsHierarchy pHierarchy)
    {
      return VSConstants.S_OK;
    }

    public int OnBeforeClosingChildren(IVsHierarchy pHierarchy)
    {
      return VSConstants.S_OK;
    }

    public int OnAfterClosingChildren(IVsHierarchy pHierarchy)
    {
      return VSConstants.S_OK;
    }

    public int OnBeforeBackgroundSolutionLoadBegins()
    {
      return VSConstants.S_OK;
    }

    public int OnQueryBackgroundLoadProjectBatch(out bool pfShouldDelayLoadToNextIdle)
    {
      pfShouldDelayLoadToNextIdle = false;
      return VSConstants.S_OK;
    }

    public int OnBeforeLoadProjectBatch(bool fIsBackgroundIdleBatch)
    {
      return VSConstants.S_OK;
    }

    public int OnAfterLoadProjectBatch(bool fIsBackgroundIdleBatch)
    {
      return VSConstants.S_OK;
    }

    public int OnAfterBackgroundSolutionLoadComplete()
    {
      return VSConstants.S_OK;
    }
  }
}