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
  /// Comando para navegação de itens no Source Control Explorer
  /// </summary>
  internal abstract class SCENavigateBaseCommand : BaseCommand
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SCENavigateBaseCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected SCENavigateBaseCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      var instance = SCENavigationController.Instance;
      if (instance == null)
      {
        Utils.LogDebugMessage("TFS não carregado");
      }
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      _menu.Enabled = _menu.Enabled = Utils.GetTeamFoundationServerExt()?
        .ActiveProjectContext?.DomainUri != null;
      if (!_menu.Enabled)
        return;
      SCENavigationController.Instance.IniciarItemAtual();
      _menu.Enabled = ButtonEnabled;
    }

    /// <summary>
    /// Indica se o botão está habilitado
    /// </summary>
    protected abstract bool ButtonEnabled { get; }

    /// <summary>
    /// Acão do comando
    /// </summary>
    protected abstract void ProcessExecute();

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      ProcessExecute();
    }
  }
}