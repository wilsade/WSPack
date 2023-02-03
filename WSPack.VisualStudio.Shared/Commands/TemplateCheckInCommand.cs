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

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;
using WSPack.VisualStudio.Shared.Options;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para exibição do Sobre
  /// </summary>
  internal sealed class TemplateCheckInCommand : BaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.TemplateCheckIn;

    /// <summary>
    /// Initializes a new instance of the <see cref="TemplateCheckInCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected TemplateCheckInCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static TemplateCheckInCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new TemplateCheckInCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      _ = WSPackPackage.ParametrosTemplateCheckIn;
      WSPackPackage.Instance.ShowOptionPage(typeof(PageTemplateCheckIn));
    }

    /// <summary>
    /// Gerar o Template com informações do Check In
    /// </summary>
    /// <param name="template">Template definido pelo usuário</param>
    /// <param name="changeSet">Informações do Changeset</param>
    /// <returns>Template formatado</returns>
    public static string GerarTemplateCheckIn(string template, Changeset changeSet)
    {
      var strArquivos = new StringBuilder();
      if (changeSet.Changes.Length > 0)
      {
        foreach (Change esteChange in changeSet.Changes)
        {
          strArquivos.AppendLine(esteChange.Item.ServerItem);
        }
        strArquivos.Length -= 2;
      }

      template = template
        .Replace("_ChangesetId_", changeSet.ChangesetId.ToString())
        .Replace("_Comentario_", changeSet.Comment)
        .Replace("_DataCheckIn_", changeSet.CreationDate.ToString())
        .Replace("_Usuario_", changeSet.Owner)
        .Replace("_ListaArquivos_", strArquivos.ToString());

      return template;
    }
  }
}