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
using WSPack.Lib.Items;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para gerenciar os favoritos do TFS
  /// </summary>
  internal sealed class TFSFavoritesManagerCommand : BaseCommand, ITFSFavoritesManagerForm
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.TFSFavoritesManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="TFSFavoritesManagerCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected TFSFavoritesManagerCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      _menu.Enabled = Utils.GetTeamFoundationServerExt()?.ActiveProjectContext?.DomainUri != null;
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static TFSFavoritesManagerCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new TFSFavoritesManagerCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      using (var form = new TFSFavoritesManagerForm(this))
      {
        form.ShowDialog();
      }
    }

    /// <summary>
    /// Url do servidor do TFS
    /// </summary>
    public string DomainUri => Utils.GetTeamFoundationServerExt()?.ActiveProjectContext?.DomainUri;

    public static List<TFSFavoritesParams> LoadFromXML()
    {
      var lst = XmlUtils.ReadXMLParams<List<TFSFavoritesParams>>(WSPackConsts.TFSFavoritesConfigPath);
      return lst;
    }

    /// <summary>
    /// Ler o XML com os favoritos salvos
    /// </summary>
    /// <returns>Lista de favoritos salvos</returns>
    public List<TFSFavoritesParams> LoadFrom()
    {
      return LoadFromXML();
    }

    /// <summary>
    /// Salvar os favoritos
    /// </summary>
    /// <param name="lst">Lista de favoritos a serem salvos</param>
    public void Save(List<TFSFavoritesParams> lst)
    {
      XmlUtils.SaveXMLParams(lst, WSPackConsts.TFSFavoritesConfigPath);
    }

    /// <summary>
    /// Indica se um item do TFS existe
    /// </summary>
    /// <param name="serverItem">Item do TFS</param>
    /// <returns>true se um item do TFS existe</returns>
    public bool ItemExists(string serverItem)
    {
      var server = Utils.GetTeamFoundationServerExt().GetVersionControlServer();
      var item = server.TryGetItem(serverItem);
      return item != null;
    }

    /// <summary>
    /// Abre um diálogo para escolher um item do TFS
    /// </summary>
    /// <param name="preSelectServerItem">Pré-selecionar um item (pode ser null)</param>
    /// <returns>Item escolhido; null, se nenhum item foi escolhido</returns>
    public string ShowChooseTFSItemDialog(string preSelectServerItem)
    {
      var server = Utils.GetTeamFoundationServerExt().GetVersionControlServer();
      var item = server.ShowChooseTFSItemDialog(preSelectServerItem);
      return item;
    }
  }
}