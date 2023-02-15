using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Identificadores dos comandos
  /// </summary>
  static class CommandIds
  {
    internal const int About = 0x0100;
    internal const int Parameters = 0x0101;
    internal const int LocateInTFS = 0x0102;
    internal const int LocateInTFSSolutionExplorer = 0x0103;
    internal const int LocateInWindowsCodeEditor = 0x0104;
    internal const int LocateInWindowsSolutionExplorer = 0x0105;
    internal const int LocateInSolutionExplorer = 0x0106;
    internal const int CopyLocalPath = 0x0107;
    internal const int CopyLocalPathSolutionExplorer = 0x0108;
    internal const int CopyLocalPathSourceControlExplorer = 0x0109;
    internal const int CopyServerPath = 0x0110;

    internal const int CopyServerPathSolutionExplorer = 0x0111;
    internal const int CopyServerPathSourceControlExplorer = 0x0112;
    internal const int OpenInEditor = 0x0113;
    internal const int OpenInEditorSolutionExplorer = 0x0114;
    internal const int OpenInEditorSourceControlExplorer = 0x0115;
    internal const int OpenOutputPath = 0x0116;
    internal const int OpenOutputPathSolutionExplorer = 0x0117;
    internal const int OpenCmdPrompt = 0x0118;
    internal const int OpenCmdPromptSolutionExplorer = 0x0119;
    internal const int DestroyTFSItem = 0x0120;

    internal const int LocateInWindowsSourceControlExplorer = 0x0121;
    internal const int DefineTabOrder = 0x0122;



    internal const int SearchChangesets = 0x0128;
    internal const int SearchChangesetsSourceControlExplorer = 0x0129;


    internal const int TemplateCheckIn = 0x0156;
    internal const int CollapseAllIncludedPendingChanges = 0x0157;
    internal const int CollapseAllExcludedPendingChanges = 0x0158;
    internal const int GenerateResource = 0x0159;
    internal const int MessageBoxTester = 0x0160;
    internal const int WorkOffline = 0x0161;
    internal const int SolutionExplorerLocalizarItem = 0x0163;



    internal const int ProjectCommandLineArgs = 0x0165;
    internal const int ProjectCommandLineArgsStartupProject = 0x0166;

    internal const int MergeInViewHistory = 0x0168;
    internal const int RestartVisualStudio = 0x0169;
    internal const int RestartVisualStudioAsADM = 0x0170;
    internal const int VariaviesAmbiente = 0x0177;
    internal const int ActivityLog = 0x0178;
    internal const int DiagnosticLog = 0x0179;
    internal const int FlexUndo = 0x0180;

    internal const int FlexGitRepository = 0x0184;
    internal const int FlexSourceControlExplorerToolbar = 0x0185;
    internal const int FlexGitChanges = 0x0186;
    internal const int ChangeSourceControl = 0x0188;
    internal const int DisconnectAndClose = 0x0190;


  }
}
