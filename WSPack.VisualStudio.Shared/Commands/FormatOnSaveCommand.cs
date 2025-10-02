using System;
using System.Linq;

using Microsoft.VisualStudio.Shell;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.VisualStudio.Shared.Options;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para formatar o documento ao salvar
  /// </summary>
  internal sealed class FormatOnSaveCommand : BaseCommand
  {
    const string GuidComandos = "{5EFC7975-14BC-11CF-9B2B-00AA00573819}";
    const int IDComandoSave = 331;
    readonly EnvDTE.CommandEvents _commandEventsSave;
    EnvDTE.Command _comandoFormatDocument, _comandoLineEnding;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormatOnSaveCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    public FormatOnSaveCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      try
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        _commandEventsSave = WSPackPackage.Dte.Events.get_CommandEvents(GuidComandos, IDComandoSave);
        _commandEventsSave.BeforeExecute -= CommandEvents_BeforeExecute;
        _commandEventsSave.BeforeExecute += CommandEvents_BeforeExecute;
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"Erro ao registrar FormatOnSaveCommand: {ex.Message}");
      }
    }

    bool CanExecute(out PageGeneral pageGeneral)
    {
      pageGeneral = WSPackPackage.GetParametersPage<PageGeneral>();

      // Usuário não quer usar
      if (!pageGeneral.FormatOnSaveOptions.UseFormatOnSave)
        return false;

      // Extensões válidas?
      string quaisExtensoes = pageGeneral.FormatOnSaveOptions.ValidExtensions;
      if (string.IsNullOrEmpty(quaisExtensoes))
        return false;

      if (!GetActiveDocumentPath(out string path))
        return false;

      if (!IsExtensaoValida(path, quaisExtensoes))
        return false;
      return true;
    }

    void TryProcessCommand(ref EnvDTE.Command commandInstance, string commandName)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      try
      {
        if (commandInstance == null)
          commandInstance = WSPackPackage.Dte.Commands?.Item(commandName);
        if (commandInstance != null && commandInstance.IsAvailable)
          WSPackPackage.Dte.ExecuteCommand(commandName);
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"Não foi possível formatar o documento utilizando o comando [{commandName}]: {ex.Message}");
      }
    }

    void CommandEvents_BeforeExecute(string guid, int id, object customIn, object customOut, ref bool cancelDefault)
    {
      try
      {
        if (!CanExecute(out PageGeneral pageGeneral))
          return;

        ThreadHelper.ThrowIfNotOnUIThread();

        TryProcessCommand(ref _comandoFormatDocument, CommandNames.EditFormatDocument);
        if (pageGeneral.FormatOnSaveOptions.NormalizeLineEndings != NormalizeLineEndingsOptions.Default)
          TryProcessCommand(ref _comandoLineEnding, $"Edit.{pageGeneral.FormatOnSaveOptions.NormalizeLineEndings}");
      }
      catch (Exception ex)
      {
        Utils.LogDebugError("Não consegui formatar o documento: " + ex.Message);
      }
    }

    public static bool IsExtensaoValida(string documentFullPath, string quaisExtensoes)
    {
      string extensaoAtual = System.IO.Path.GetExtension(documentFullPath).Replace(".", string.Empty);
      var splitted = quaisExtensoes.Split(new string[] { ";", " " }, StringSplitOptions.RemoveEmptyEntries);
      return splitted.Any(x => x.Replace(".", "").EqualsInsensitive(extensaoAtual));
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static FormatOnSaveCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new FormatOnSaveCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      throw new NotImplementedException();
    }
  }
}