using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Environment;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;
using System.Collections;
using System.Text;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para exibição das variáveis de ambiente
  /// </summary>
  internal sealed class VariaveisAmbienteCommand : BaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.VariaviesAmbiente;

    /// <summary>
    /// Initializes a new instance of the <see cref="VariaveisAmbienteCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected VariaveisAmbienteCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static VariaveisAmbienteCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new VariaveisAmbienteCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var dic1 = GetEnvironmentVariables(EnvironmentVariableTarget.Process);
      var dic2 = GetEnvironmentVariables(EnvironmentVariableTarget.User);
      var dic3 = GetEnvironmentVariables(EnvironmentVariableTarget.Machine);

      var lst1 = dic1.OfType<DictionaryEntry>().OrderBy(x => x.Key);
      var lst2 = dic2.OfType<DictionaryEntry>().OrderBy(x => x.Key);
      var lst3 = dic3.OfType<DictionaryEntry>().OrderBy(x => x.Key);

      var max1 = lst1.Max(x => x.Key.ToString().Length);
      var max2 = lst2.Max(x => x.Key.ToString().Length);
      var max3 = lst3.Max(x => x.Key.ToString().Length);
      var maior = new int[] { max1, max2, max3 }.Max();

      var builder = new StringBuilder(NewLine);
      builder.AppendLine("VARIÁVEIS DO PROCESSO:")
        .AppendLine(string.Join(NewLine, lst1.Select(x => $"{x.Key.ToString().PadRight(maior, '.')}: {x.Value}")));
      builder.AppendLine("\r\nVARIÁVEIS DO USUÁRIO:")
        .AppendLine(string.Join(NewLine, lst2.Select(x => $"{x.Key.ToString().PadRight(maior, '.')}: {x.Value}")));
      builder.AppendLine("\r\nVARIÁVEIS DA MÁQUINA:")
        .AppendLine(string.Join(NewLine, lst3.Select(x => $"{x.Key.ToString().PadRight(maior, '.')}: {x.Value}")));

      builder.AppendLine("\r\nPATH:")
        .AppendLine(ExpandEnvironmentVariables("%PATH%").Replace(";", NewLine));

      Utils.LogOutputMessageForceShow(builder.ToString().Trim());
    }
  }
}