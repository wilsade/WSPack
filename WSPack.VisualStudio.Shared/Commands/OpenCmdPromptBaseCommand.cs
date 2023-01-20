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

using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para exibição do Sobre
  /// </summary>
  internal abstract class OpenCmdPromptBaseCommand : BaseCommand
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenCmdPromptBaseCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected OpenCmdPromptBaseCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    protected abstract string GetLocalItem();

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      string localItem = GetLocalItem();
      if (string.IsNullOrEmpty(localItem))
        Utils.LogOutputMessage(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
      else
      {
        string batFile = RegistroVisualStudioObj.Instance.GetDeveloperCommandPromptPath();
        ProcessStartInfo pInfo;

        if (File.Exists(batFile))
          pInfo = new ProcessStartInfo("cmd.exe", "/K \"" + batFile + "\"");
        else
          pInfo = new ProcessStartInfo("cmd.exe", "/k");

        if (File.Exists(localItem))
          pInfo.WorkingDirectory = Path.GetDirectoryName(localItem);
        else if (Directory.Exists(localItem))
          pInfo.WorkingDirectory = localItem;

        Process.Start(pInfo);
      }
    }
  }
}