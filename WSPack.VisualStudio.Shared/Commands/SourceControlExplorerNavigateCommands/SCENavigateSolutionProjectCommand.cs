using System.Linq;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para navegar para o primeito projeto/solution na árvore do SCE
  /// </summary>
  internal sealed class SCENavigateSolutionProjectCommand : SCENavigateBaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.SCENavigateSolutionProject;

    /// <summary>
    /// Initializes a new instance of the <see cref="SCENavigateSolutionProjectCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected SCENavigateSolutionProjectCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static SCENavigateSolutionProjectCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new SCENavigateSolutionProjectCommand(package, commandService);
    }

    protected override bool ButtonEnabled => SCENavigationController.Instance.CanNavigateSolutionProject();

    protected override void ProcessExecute()
    {
      var vcExt = Utils.GetVersionControlServerExt();
      var pastaAtual = vcExt.Explorer.CurrentFolderItem?.SourceServerPath;
      if (pastaAtual.IsNullOrWhiteSpaceEx())
      {
        Utils.LogOutputMessageForceShow(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
        return;
      }

      var vcServer = Utils.GetTeamFoundationServerExt().GetVersionControlServer();

      DeletedState deletedState = DeletedState.Any;
      if (!vcExt.Explorer.ShowDeletedItems)
        deletedState = DeletedState.NonDeleted;

      var itens = vcServer.GetItems(pastaAtual, VersionSpec.Latest,
        RecursionType.OneLevel,
        deletedState,
        ItemType.File,
        false).Items.Where(x => x.ServerItem.EndsWithInsensitive(".sln") || x.ServerItem.EndsWithInsensitive(".csproj"));
      if (!itens.Any())
      {
        Utils.LogOutputMessageForceShow($"Não existem Solutions/Projetos na pasta: {pastaAtual}");
        return;
      }

      Utils.LogOutputMessage($"Total de Solutions / Projetos encontrados na pasta {pastaAtual}: {itens.Count()}");

      Item primeiro = itens.First();
      Utils.LogOutputMessage($"Navegar para o primeiro item: {primeiro.ServerItem}\r\n");
      LocateInTFSBaseCommand.NavigateToServerItem(vcExt, primeiro.ServerItem);
    }
  }
}