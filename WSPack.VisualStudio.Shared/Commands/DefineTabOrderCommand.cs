using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using swf = System.Windows.Forms;

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
  /// Comando para definição de ordem de tabulação WinForms
  /// </summary>
  internal sealed class DefineTabOrderCommand : BaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.DefineTabOrder;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefineTabOrderCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected DefineTabOrderCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    [System.Diagnostics.DebuggerStepThrough]
    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      _menu.Enabled = false;

      ThreadHelper.ThrowIfNotOnUIThread();
      try
      {
        if (WSPackPackage.Dte?.ActiveWindow?.Document != null &&
          WSPackPackage.Dte?.ActiveDocument?.ActiveWindow != null &&
          WSPackPackage.Dte?.ActiveWindow?.Object != null)
          _menu.Enabled = WSPackPackage.Dte.ActiveDocument.ActiveWindow.Object is IDesignerHost;
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"menuItem_BeforeQueryStatus {(nameof(DefineTabOrderCommand))}: {ex.Message}");
      }
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static DefineTabOrderCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new DefineTabOrderCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var document = WSPackPackage.Dte.ActiveDocument;
      if ((document?.ActiveWindow?.Object) == null)
        return;

      IDesignerHost iHost = document.ActiveWindow.Object as IDesignerHost;
      if (iHost?.Container == null || iHost.Container.Components == null)
        return;

      var lstControles = new List<swf.Control>();

      //SWF.Control.ControlCollection colecao;
      IEnumerable<swf.Control> colecao;
      swf.Control controle;
      string nome;

      ISelectionService seletor = iHost.GetService(typeof(ISelectionService)) as ISelectionService;
      if (seletor != null && seletor.PrimarySelection != null)
      {
        controle = seletor.PrimarySelection as swf.Control;
        colecao = controle.Controls.OfType<swf.Control>();
      }
      else
      {
        colecao = iHost.Container.Components.OfType<swf.Control>();
        controle = iHost.RootComponent as swf.Control;
      }

      nome = UtilsLib.FormatControlName(controle);
      if (colecao.Any())
      {
        foreach (swf.Control esteControle in colecao)
        {
          if (esteControle != iHost.RootComponent)
          {
            lstControles.Add(esteControle);
          }
        }

        using (var form = new DefineTabOrderForm(nome, lstControles))
        {
          form.Seletor = seletor;
          form.ShowDialog();
        }
      }
      else
        MessageBoxUtils.ShowInformation(ResourcesLib.StrNaoExistemControlesDentroItem.FormatWith(nome));
    }
  }
}