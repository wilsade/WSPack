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
  /// Comando para manipulação de itens em um ComboBox
  /// </summary>
  internal abstract class BaseDropDownComboCommand : BaseCommand
  {
    /// <summary>
    /// Item Selecionado no Combo
    /// </summary>
    protected string ItemSelecionadoCombo;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseDropDownComboCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected BaseDropDownComboCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Recuperar a lista de items do ComboBox
    /// </summary>
    /// <returns>Lista de items do ComboBox</returns>
    protected abstract string[] GetItems();

    /// <summary>
    /// Ação que acontece quando um item é selecionado
    /// </summary>
    /// <param name="item">Item selecionado.</param>
    protected abstract void ItemSelected(string item);

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (e == EventArgs.Empty)
      {
        Trace.WriteLine("Argumentos vazio");
        return;
      }

      if (!(e is OleMenuCmdEventArgs eventArgs))
      {
        // We should never get here; EventArgs are required.
        Trace.WriteLine("Desconhecido");
        return;
      }

      string newChoice = eventArgs.InValue as string;
      IntPtr vOut = eventArgs.OutValue;

      if (vOut != IntPtr.Zero && newChoice != null)
      {
        Trace.WriteLine("Parâmetro(s) não permitido(s)");
        return;
      }

      else if (vOut != IntPtr.Zero)
      {
        // when vOut is non-NULL, the IDE is requesting the current value for the combo
        var lista = GetItems();

        if (lista.FirstOrDefault(x => x.Equals(ItemSelecionadoCombo)) != null)
          System.Runtime.InteropServices.Marshal.GetNativeVariantForObject(ItemSelecionadoCombo, vOut);
        else
          System.Runtime.InteropServices.Marshal.GetNativeVariantForObject(null, vOut);
      }

      else if (newChoice != null)
      {
        var lista = GetItems();
        // new value was selected or typed in
        // see if it is one of our items
        bool validInput = false;
        int indexInput = -1;
        for (indexInput = 0; indexInput < lista.Count(); indexInput++)
        {
          if (String.Compare(lista.ElementAt(indexInput), newChoice, StringComparison.OrdinalIgnoreCase) == 0)
          {
            validInput = true;
            break;
          }
        }

        if (validInput)
        {
          ItemSelecionadoCombo = lista.ElementAt(indexInput);
          ItemSelected(ItemSelecionadoCombo);
        }
        else
        {
          Trace.WriteLine("Parâmetro inválido");
          return;
        }
      }

      else
      {
        // We should never get here
        Trace.WriteLine("Desconhecido");
      }

    }
  }
}