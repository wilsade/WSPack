﻿using System;
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
  internal sealed class ComboBoxSSEPopulateCommand : BaseGetItemListDropDownComboCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.SSEGetItemsComboBox;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComboBoxSSEPopulateCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public ComboBoxSSEPopulateCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static ComboBoxSSEPopulateCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new ComboBoxSSEPopulateCommand(package, commandService);
    }

    /// <summary>
    /// Recuperar a lista de items do ComboBox
    /// </summary>
    /// <returns>Lista de items do ComboBox</returns>
    protected override string[] GetItems()
    {
      var TFSExt = Utils.GetTeamFoundationServerExt();
      if (TFSExt != null)
      {
        var lst = TFSFavoritesManagerCommand.LoadFromXML();
        var listaFavoritos = TFSFavoritesManagerForm.GetOrderedListFromServer(lst, TFSExt.ActiveProjectContext.DomainUri);
        return listaFavoritos.Select(x => x.Caption).Union(new string[] { Constantes.GerenciadorFavoritos }).ToArray();
      }
      return new string[] { };
    }
  }
}