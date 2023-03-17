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
  /// Comando para localizar um item no TFS a partir do Souce Control Explorer
  /// </summary>
  internal sealed class SourceControlExplorerLocateItemCommand : BaseCommand
  {
    readonly List<string> _lstItensPesquisados;

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.SourceControlExplorerLocateItem;

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceControlExplorerLocateItemCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public SourceControlExplorerLocateItemCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      CreateKeyBindings("TeamFoundationContextmenus.SourceControlExplorer.SCELocateItemInTFS",
        "Global::Alt+Shift+F", false);
      _lstItensPesquisados = new List<string>();
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      _menu.Enabled = Utils.GetTeamFoundationServerExt()?.ActiveProjectContext?.DomainUri != null;
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static SourceControlExplorerLocateItemCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new SourceControlExplorerLocateItemCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var vcExt = Utils.GetVersionControlServerExt();
      if (vcExt == null)
      {
        MessageBoxUtils.ShowWarning(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
        return;
      }

      using (var form = new LookupListBaseForm()
      {
        Label = ResourcesLib.StrInformeCaminhoServerOuLocalItem
      })
      {
        form.StatusBarLabel = ResourcesLib.StrBuscaPorItemLocalPressioneOK;
        form.AoAlterarTextoProcura = (texto, lst) =>
        {
          form.StatusBarVisible = lst.Count == 0;
          return !string.IsNullOrEmpty(texto);
        };

        PreencherItens(form);
        if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
          var teamServer = Utils.GetTeamFoundationServerExt();
          string response = null;
          if (form.ItemSelecionado.Caminho.StartsWith("$/"))
          {
            if (teamServer.ItemExists(form.ItemSelecionado.Caminho, out string msg))
            {
              response = form.ItemSelecionado.Caminho;
              LocateInTFSBaseCommand.NavigateToServerItem(vcExt, form.ItemSelecionado.Caminho);
            }
            else
              MessageBoxUtils.ShowWarning(msg);
          }
          else
          {
            // Mesmo workspace ativo no Explorer?
            if (vcExt.TryGetServerItemForLocalItem(form.ItemSelecionado.Caminho, out var tupla))
            {
              if (teamServer.ItemExists(form.ItemSelecionado.Caminho, out var msg))
              {
                response = form.ItemSelecionado.Caminho;
                LocateInTFSBaseCommand.NavigateToServerItem(vcExt, tupla.ServerItem);
              }
              else
                MessageBoxUtils.ShowWarning(msg);
            }
            else
            {
              var vcServer = Utils.GetTeamFoundationServerExt().GetVersionControlServer();
              if (vcServer.GetWorkspaceForLocalItem(form.ItemSelecionado.Caminho, out _, out var serverItem))
              {
                if (vcServer.ItemExists(form.ItemSelecionado.Caminho, out var msg))
                {
                  response = form.ItemSelecionado.Caminho;
                  LocateInTFSBaseCommand.NavigateToServerItem(vcExt, serverItem);
                }
                else
                  MessageBoxUtils.ShowWarning(msg);
              }
              else
                MessageBoxUtils.ShowWarning(tupla.ErrorMessage);
            }
          }
          if (!response.IsNullOrWhiteSpaceEx())
            _lstItensPesquisados.MakeItemFirstInList(response);
        }
      }
    }

    private void PreencherItens(LookupListBaseForm form)
    {
      List<TFSFavoritesParams> lst = TFSFavoritesManagerCommand.LoadFromXML();
      var lstToFill = new List<string>(lst.Select(x => x.ServerItem));
      for (int i = _lstItensPesquisados.Count - 1; i >= 0; i--)
      {
        lstToFill.Insert(0, _lstItensPesquisados[i]);
      }
      form.Bind(lstToFill.Distinct().Select(x => new LookupGridItem(Path.GetFileName(x), x)));
    }
  }
}