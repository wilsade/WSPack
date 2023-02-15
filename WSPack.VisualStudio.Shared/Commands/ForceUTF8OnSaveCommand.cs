using System;
using System.IO;
using System.Text;

using EnvDTE;

using Microsoft.VisualStudio.Shell;

using WSPack.Lib.Extensions;
using WSPack.VisualStudio.Shared.Options;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para forçar o encoding do documento para UTF8 ao salvar
  /// </summary>
  internal sealed class ForceUTF8OnSaveCommand : BaseCommand
  {
    readonly DocumentEvents _documentEvents;

    /// <summary>
    /// Initializes a new instance of the <see cref="ForceUTF8OnSaveCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected ForceUTF8OnSaveCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      _documentEvents = WSPackPackage.Dte.Events.DocumentEvents;
      _documentEvents.DocumentSaved -= DocumentEvents_DocumentSaved;
      _documentEvents.DocumentSaved += DocumentEvents_DocumentSaved;
    }

    void DocumentEvents_DocumentSaved(Document document)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      // then it's not a text file
      if (document.Kind != "{8E7B96A8-E33D-11D0-A6D5-00C04FB67F6A}")
        return;

      var fullName = document.FullName;

      // Verificar extensões válidas!
      if (!ValidouExtensaoDocumento(fullName))
        return;

      bool isJava = false;
      if (fullName.EndsWithInsensitive(".java"))
      {
        Utils.LogOutputMessage($"{Path.GetFileName(fullName)}: arquivos java serão convertidos para UTF-8 (no BOM).");
        isJava = true;
      }

      ForcarEncoding(fullName, isJava);
    }

    private static void ForcarEncoding(string fullName, bool isJava)
    {
      try
      {
        using (var stream = new FileStream(fullName, FileMode.Open))
        {
          var reader = new StreamReader(stream, Encoding.Default, true);
          try
          {
            reader.Read();
            var preambleBytes = reader.CurrentEncoding.GetPreamble();
            if (preambleBytes.Length == 3 &&
                preambleBytes[0] == 0xEF && preambleBytes[1] == 0xBB && preambleBytes[2] == 0xBF &&
                reader.CurrentEncoding.EncodingName == Encoding.UTF8.EncodingName)
            {
              stream.Close();
              return;
            }

            string text;

            try
            {
              stream.Position = 0;
              reader = new StreamReader(stream, new UTF8Encoding(true, true));
              text = reader.ReadToEnd();
              stream.Close();
              File.WriteAllText(fullName, text, new UTF8Encoding(!isJava));
              Utils.LogDebugMessage($"Convertido '{fullName}' para UTF-8 (BOM).");
            }
            catch (DecoderFallbackException)
            {
              stream.Position = 0;
              reader = new StreamReader(stream, Encoding.Default);
              text = reader.ReadToEnd();
              stream.Close();
              File.WriteAllText(fullName, text, new UTF8Encoding(!isJava));
              Utils.LogDebugMessage($"Convertido '{fullName}' para UTF-8 (BOM).");
            }
          }
          finally
          {
            reader.Dispose();
          }
        }

      }
      catch (Exception ex)
      {
        Utils.LogOutputMessageForceShow($"Erro ao converter arquivo '{Path.GetFileName(fullName)}' para UTF8 (BOM): {ex.GetCompleteMessage()}");
      }
    }

    static bool ValidouExtensaoDocumento(string docFullPath)
    {
      // Verificar extensões válidas!
      try
      {
        PageGeneral pageGeneral = WSPackPackage.GetParametersPage<PageGeneral>();
        // Usuário não quer usar
        if (!pageGeneral.ForceUTF8OnSaveOptions.ForceUTF8OnSave)
          return false;

        string quaisExtensoes = pageGeneral.ForceUTF8OnSaveOptions.ValidExtensions;
        if (string.IsNullOrEmpty(quaisExtensoes))
          return false;

        if (!FormatOnSaveCommand.IsExtensaoValida(docFullPath, quaisExtensoes))
          return false;
        return true;
      }
      catch (Exception ex)
      {
        Utils.LogDebugError(ex.GetCompleteMessage());
        return false;
      }
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static ForceUTF8OnSaveCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new ForceUTF8OnSaveCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      throw new NotImplementedException();
    }
  }
}