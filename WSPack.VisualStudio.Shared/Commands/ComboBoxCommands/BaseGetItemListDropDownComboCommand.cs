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
  /// Definir ação para preenchimento de um DropDownCombo
  /// </summary>
  internal abstract class BaseGetItemListDropDownComboCommand : BaseCommand
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseGetItemListDropDownComboCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected BaseGetItemListDropDownComboCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Recuperar a lista de items do ComboBox
    /// </summary>
    /// <returns>Lista de items do ComboBox</returns>
    protected abstract string[] GetItems();

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if ((e == null) || (e == EventArgs.Empty))
      {
        // We should never get here; EventArgs are required.
        Trace.WriteLine("parametros necessários");
        return;
      }

      if (!(e is OleMenuCmdEventArgs eventArgs))
        return;

      object inParam = eventArgs.InValue;
      IntPtr vOut = eventArgs.OutValue;

      if (inParam != null)
      {
        Trace.WriteLine("parametro ilegal");
        return;
      }

      else if (vOut != IntPtr.Zero)
      {
        System.Runtime.InteropServices.Marshal.GetNativeVariantForObject(GetItems(), vOut);
      }

      else
      {
        Trace.WriteLine("Parâmetros requeridos");
      }
    }
  }
}