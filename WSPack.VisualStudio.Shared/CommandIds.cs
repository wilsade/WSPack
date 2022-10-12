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

    internal const int CopyLocalPath = 0x0107;
    internal const int CopyLocalPathSolutionExplorer = 0x0108;

    internal const int FlexGitRepository = 0x0184;
    internal const int FlexSourceControlExplorerToolbar = 0x0185;
    internal const int FlexGitChanges = 0x0186;
  }
}
