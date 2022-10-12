﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Shell;

using WSPack.Lib;

namespace WSPack.VisualStudio.Shared.Commands
{
  class FlexGitRepositoryCommand : FlexCommandsCommand
  {
    private const string GIT_REPOSITORY = "Git Repository";

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.FlexGitRepository;

    /// <summary>
    /// Inicialização da classe: <see cref="FlexGitRepositoryCommand"/>.
    /// </summary>
    /// <param name="package">Package</param>
    /// <param name="commandService">Command service</param>
    public FlexGitRepositoryCommand(AsyncPackage package, OleMenuCommandService commandService) : base(package, commandService)
    {
      Instance = this;
    }

    /// <summary>
    /// Devolve a instãncia da classe: <see cref="FlexGitRepositoryCommand"/>
    /// </summary>
    /// <value>Instância da classe: <see cref="FlexGitRepositoryCommand"/></value>
    public static FlexGitRepositoryCommand Instance { get; private set; }

    /// <summary>
    /// Texto padrão do comando
    /// </summary>
    protected override string DefaultText => GIT_REPOSITORY;

    /// <summary>
    /// Indica se o comando estará visível se não estiver habilitado
    /// </summary>
    protected override bool VisibiityOnDisabled => false;

    /// <summary>
    /// Retornar os comandos Flex na ordem de prioridade
    /// </summary>
    protected override (string Name, string Text)[] FlexCommandsList =>
          new (string, string)[] { (CommandNames.ViewGitRepositoryWindow, GIT_REPOSITORY) };
  }
}
