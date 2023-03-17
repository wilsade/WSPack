using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para comparar arquivos alterados (TFS ou git)
  /// </summary>
  internal sealed class FlexCompareCommand : FlexCommandsCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.FlexCompare;

    /// <summary>
    /// Initializes a new instance of the <see cref="FlexCompareCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public FlexCompareCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static FlexCompareCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new FlexCompareCommand(package, commandService);
    }

    /// <summary>
    /// Texto padrão do comando
    /// </summary>
    protected override string DefaultText => "Flex Compare";

    /// <summary>
    /// Retornar os comandos Flex na ordem de prioridade
    /// </summary>
    protected override (string Name, string Text)[] FlexCommandsList =>
          new (string, string)[] {
      (CommandNames.FileTfsCompare, "Compare"),
      (CommandNames.TeamGitCompareWithUnmodified, "Compare with Unmodified") };
  }
}