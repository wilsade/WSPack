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
  /// Comando para testar uma caixa de diálogo
  /// </summary>
  internal sealed class MessageBoxTesterCommand : BaseCommand
  {
    string _titulo, _texto;

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.MessageBoxTester;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageBoxTesterCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public MessageBoxTesterCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static MessageBoxTesterCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new MessageBoxTesterCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      using (var form = new MessageBoxTesterForm(msg =>
      {
        CopyLocalPathBaseCommand.CopyToClipboard(msg);
      }))
      {
        if (!string.IsNullOrEmpty(_titulo))
          form.Titulo = _titulo;
        if (!string.IsNullOrEmpty(_texto))
          form.Texto = _texto;

        if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
          _titulo = form.Titulo;
          _texto = form.Texto;
        }
      }
    }
  }
}