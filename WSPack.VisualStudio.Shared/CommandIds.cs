﻿using System;
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

    internal const int CopyLocalPath = 0x0107;
    internal const int CopyLocalPathSolutionExplorer = 0x0108;
    internal const int CopyLocalPathSourceControlExplorer = 0x0109;
    internal const int CopyServerPath = 0x0110;

    internal const int CopyServerPathSolutionExplorer = 0x0111;
    internal const int CopyServerPathSourceControlExplorer = 0x0112;
    internal const int OpenInEditor = 0x0113;
    internal const int OpenInEditorSolutionExplorer = 0x0114;
    internal const int OpenInEditorSourceControlExplorer = 0x0115;

    internal const int LocateInWindowsSourceControlExplorer = 0x0121;

    internal const int FlexGitRepository = 0x0184;
    internal const int FlexSourceControlExplorerToolbar = 0x0185;
    internal const int FlexGitChanges = 0x0186;
  }
}
