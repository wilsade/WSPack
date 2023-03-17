using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using EnvDTE;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

using VSLangProj;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;
using WSPack.Lib.Items;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;
using WSPack.VisualStudio.Shared.Forms;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para geração de Resource String
  /// </summary>
  internal sealed class GenerateResourceCommand : BaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.GenerateResource;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenerateResourceCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public GenerateResourceCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      if (!CreateKeyBindings("Edit.WSPack.GerarResource", "Text Editor::Ctrl+R, S", false))
        CreateKeyBindings("Edit.WSPack.GerarResource", "Editor de Texto::Ctrl+R, S", false);
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread($"{nameof(GenerateResourceCommand)}:{nameof(_menu_BeforeQueryStatus)}");
      _menu.Enabled = WSPackPackage.Instance != null &&
        WSPackPackage.Dte != null &&
        WSPackPackage.Dte.GetActiveDocument(out var doc) &&
        doc.FullName.IsCSharpFile() &&
        doc.Selection != null;
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static GenerateResourceCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new GenerateResourceCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      TextSelection selection = (TextSelection)WSPackPackage.Dte.ActiveDocument.Selection;
      string palavra = selection.Text;

      // Existe texto selecionado
      if (palavra.IsNullOrWhiteSpaceEx())
      {
        var sel = new SelectionReg(selection.Parent);
        palavra = GenerateResourceUtils.LocateString(sel, true);
      }

      if (!palavra.IsNullOrWhiteSpaceEx())
      {
        // Tentar buscar uma string sobre o cursor
        List<ResourcesFileItem> lstResources = GenerateResourceUtils.GetResources();
        if (lstResources.Any())
        {
          if (palavra.StartsWith("\""))
            palavra = palavra.Substring(1);
          if (palavra.EndsWith("\""))
            palavra = palavra.Substring(0, palavra.Length - 1);

          // Exibição do form para geração do resource
          GenerateResourceUtils.ShowForm(selection, lstResources, palavra);
        }
        else
          MessageBoxUtils.ShowInformation(ResourcesLib.StrArquivoResourcesNaoEncontrado);
      }
      else
        MessageBoxUtils.ShowInformation(ResourcesLib.StrTextoNaoSelecionado);
    }
  }
}