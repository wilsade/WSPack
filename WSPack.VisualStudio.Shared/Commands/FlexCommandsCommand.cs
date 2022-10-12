using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.Shell;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;

namespace WSPack.VisualStudio.Shared.Commands
{
  abstract class FlexCommandsCommand : BaseCommand
  {
    EnvDTE.Command _flexCommand;
    readonly Dictionary<string, (EnvDTE.Command Comando, string Texto)> _dicCommands;

    /// <summary>
    /// Inicialização da classe: <see cref="FlexCommandsCommand"/>.
    /// </summary>
    /// <param name="package">Package</param>
    /// <param name="commandService">Command service</param>
    protected FlexCommandsCommand(AsyncPackage package, OleMenuCommandService commandService) : base(package, commandService)
    {
      _dicCommands = new Dictionary<string, (EnvDTE.Command, string)>();
    }

    /// <summary>
    /// Texto padrão do comando
    /// </summary>
    protected abstract string DefaultText { get; }

    /// <summary>
    /// Retornar os comandos Flex na ordem de prioridade
    /// </summary>
    protected abstract (string Name, string Text)[] FlexCommandsList { get; }

    /// <summary>
    /// Indica se o comando estará visível se não estiver habilitado
    /// </summary>
    protected virtual bool VisibiityOnDisabled => true;

    void CheckCommands()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      _flexCommand = null;
      foreach (var item in FlexCommandsList)
      {
        if (_dicCommands.ContainsKey(item.Name))
          continue;
        _dicCommands.Add(item.Name, (WSPackPackage.Dte.Commands?.Item(item.Name), item.Text));
      }
    }

    protected override void BeforeExecute(object sender, EventArgs e)
    {
      CheckCommands();
      _menu.Text = DefaultText;
      _flexCommand = null;
      foreach (var item in _dicCommands.Values)
      {
        if (item.Comando?.IsAvailable == true)
        {
          _flexCommand = item.Comando;
          _menu.Text = item.Texto;
          break;
        }
      }
      _menu.Enabled = _flexCommand != null;
      if (!_menu.Enabled)
        _menu.Visible = VisibiityOnDisabled;
      else
        _menu.Visible = true;

      if (FlexSourceControlExplorerCommand.Instance != null && FlexSourceControlExplorerCommand.Instance._menu.Enabled)
      {
        if (FlexGitRepositoryCommand.Instance?._menu.Enabled == true)
          FlexGitRepositoryCommand.Instance._menu.Visible = false;
        if (FlexGitChanges.Instance?._menu.Enabled == true)
          FlexGitChanges.Instance._menu.Visible = false;
      }
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      try
      {
        if (_flexCommand == null)
        {
          MessageBoxUtils.ShowWarning(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
          return;
        }
        string comando = _flexCommand.LocalizedName;
        if (comando.IsNullOrWhiteSpaceEx())
          comando = _flexCommand.Name;
        WSPackPackage.Dte.ExecuteCommand(comando);
      }
      catch (Exception ex)
      {
        MessageBoxUtils.ShowError(ex.Message);
      }
    }
  }
}
