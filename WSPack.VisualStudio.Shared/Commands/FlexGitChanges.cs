using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Shell;

using WSPack.Lib;

namespace WSPack.VisualStudio.Shared.Commands
{
  class FlexGitChanges : FlexCommandsCommand
  {
    private const string GIT_CHANGES = "Git Changes";

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.FlexGitChanges;

    /// <summary>
    /// Inicialização da classe: <see cref="FlexGitChanges"/>.
    /// </summary>
    /// <param name="package">Package</param>
    /// <param name="commandService">Command service</param>
    public FlexGitChanges(AsyncPackage package, OleMenuCommandService commandService) : base(package, commandService)
    {
      Instance = this;
    }

    /// <summary>
    /// Devolve a instãncia da classe: <see cref="FlexGitChanges"/>
    /// </summary>
    /// <value>Instância da classe: <see cref="FlexGitChanges"/></value>
    public static FlexGitChanges Instance { get; private set; }

    /// <summary>
    /// Texto padrão do comando
    /// </summary>
    protected override string DefaultText => GIT_CHANGES;

    /// <summary>
    /// Indica se o comando estará visível se não estiver habilitado
    /// </summary>
    protected override bool VisibiityOnDisabled => false;

    /// <summary>
    /// Retornar os comandos Flex na ordem de prioridade
    /// </summary>
    protected override (string Name, string Text)[] FlexCommandsList =>
          new (string, string)[] { (CommandNames.TeamGitGoToGitChanges, GIT_CHANGES) };
  }
}
