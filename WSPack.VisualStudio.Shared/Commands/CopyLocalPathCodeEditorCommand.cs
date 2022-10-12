﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

namespace WSPack.VisualStudio.Shared.Commands
{
  class CopyLocalPathCodeEditorCommand : CopyLocalPathBaseCommand
  {
    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.CopyLocalPath;

    #region Construtor
    /// <summary>
    /// Inicialização da classe <see cref="CopyLocalPathCodeEditorCommand"/>
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public CopyLocalPathCodeEditorCommand(AsyncPackage package, OleMenuCommandService commandService) : base(package, commandService)
    {
      Instance = this;
    }
    #endregion

    public static CopyLocalPathCodeEditorCommand Instance { get; private set; }

    /// <summary>
    /// Recuperar o item local conforme tipo de comando
    /// </summary>
    /// <returns>item local conforme tipo de comando</returns>
    public override List<(Workspace Ws, string LocalItem)> GetLocalItem()
    {
      var localItem = GetActiveDocumentPath();
      return new List<(Workspace Ws, string LocalItem)>
      {
        (null, localItem)
      };
    }
  }
}
