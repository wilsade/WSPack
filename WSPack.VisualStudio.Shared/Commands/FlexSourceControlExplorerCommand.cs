using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using Microsoft.VisualStudio.Shell;

using WSPack.Lib;

namespace WSPack.VisualStudio.Shared.Commands
{
  class FlexSourceControlExplorerCommand : FlexCommandsCommand
  {
    private const string SOURCE_CONTROL_EXPLORER = "Source Control Explorer";

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.FlexSourceControlExplorerToolbar;

    /// <summary>
    /// Inicialização da classe: <see cref="FlexSourceControlExplorerCommand"/>.
    /// </summary>
    /// <param name="package">Package</param>
    /// <param name="commandService">Command service</param>
    public FlexSourceControlExplorerCommand(AsyncPackage package, OleMenuCommandService commandService) : base(package, commandService)
    {
    }

    /// <summary>
    /// Devolve a instãncia da classe: <see cref="FlexSourceControlExplorerCommand"/>
    /// </summary>
    /// <value>Instância da classe: <see cref="FlexSourceControlExplorerCommand"/></value>
    public static FlexSourceControlExplorerCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new FlexSourceControlExplorerCommand(package, commandService);
    }

    /// <summary>
    /// Indica se o comando está disponível
    /// </summary>
    /// <value>true se o comando está disponível</value>
    public static bool IsAvailable => Instance == null ||
          Instance._menu.Enabled;

    /// <summary>
    /// Texto padrão do comando
    /// </summary>
    protected override string DefaultText => SOURCE_CONTROL_EXPLORER;

    /// <summary>
    /// Indica se o comando estará visível se não estiver habilitado
    /// </summary>
    protected override bool VisibiityOnDisabled => false;

    /// <summary>
    /// Retornar os comandos Flex na ordem de prioridade
    /// </summary>
    protected override (string Name, string Text)[] FlexCommandsList =>
      new (string, string)[] { (CommandNames.ViewTfsSourceControlExplorer, SOURCE_CONTROL_EXPLORER) };
  }
}
