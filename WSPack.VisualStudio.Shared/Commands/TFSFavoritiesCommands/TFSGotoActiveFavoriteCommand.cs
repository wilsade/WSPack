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
  /// Comando para ir para o item de favorito selecinado no combo de Favoritos
  /// </summary>
  internal sealed class TFSGotoActiveFavoriteCommand : BaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.GotoFavoritoAtualTFS;

    /// <summary>
    /// Initializes a new instance of the <see cref="TFSGotoActiveFavoriteCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected TFSGotoActiveFavoriteCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      _menu.Enabled = ComboBoxSSEClickCommand.Instance.FavoritoSelecionado != null &&
        Utils.GetVersionControlServerExt() != null;
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static TFSGotoActiveFavoriteCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new TFSGotoActiveFavoriteCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var vcExt = Utils.GetVersionControlServerExt();
      LocateInTFSBaseCommand.NavigateToServerItem(vcExt, ComboBoxSSEClickCommand.Instance.FavoritoSelecionado);
    }
  }
}