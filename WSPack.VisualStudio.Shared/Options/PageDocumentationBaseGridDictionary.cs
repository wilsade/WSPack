using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using WSPack.Lib.DocumentationObjects;
using WSPack.VisualStudio.Shared.UserControls;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Página para definição de uma grid para itens de documentação
  /// </summary>
  public abstract class PageDocumentationBaseGridDictionary : BaseDialogPage
  {
    DocumentationDictionaryControl _documentationDictionaryControl;

    bool CreateDocumentationControl()
    {
      if (_documentationDictionaryControl == null)
      {
        _documentationDictionaryControl = new DocumentationDictionaryControl()
        {
          InformationText = GetInformationText()
        };
        return true;
      }
      return false;
    }

    /// <summary>
    /// Texto contendo informações sobre a tela
    /// </summary>
    /// <returns>Texto contendo informações sobre a tela</returns>
    protected abstract string GetInformationText();

    /// <summary>
    /// Gets the window an instance of DialogPage that it uses as its user interface.
    /// </summary>
    /// <devdoc>
    /// The window this dialog page will use for its UI.
    /// This window handle must be constant, so if you are
    /// returning a Windows Forms control you must make sure
    /// it does not recreate its handle.  If the window object
    /// implements IComponent it will be sited by the 
    /// dialog page so it can get access to global services.
    /// </devdoc>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    protected override IWin32Window Window
    {
      get
      {
        if (CreateDocumentationControl())
          _documentationDictionaryControl.OptionsPage = this;
        return _documentationDictionaryControl;
      }
    }

    /// <summary>
    /// Handles "deactivate" messages from the Visual Studio environment.
    /// </summary>
    /// <devdoc>
    /// This method is called when VS wants to deactivate this
    /// page.  If this handler sets e.Cancel, the deactivation will not occur.
    /// </devdoc>
    /// <remarks>
    /// A "deactivate" message is sent when focus changes to a different page in
    /// the dialog.
    /// </remarks>
    protected override void OnDeactivate(CancelEventArgs e)
    {
      _documentationDictionaryControl.GridEndEdit();
    }

    /// <summary>
    /// Liberar o objeto
    /// </summary>
    /// <param name="disposing">true se disposing</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (_documentationDictionaryControl != null)
        {
          _documentationDictionaryControl.Dispose();
          _documentationDictionaryControl = null;
        }
      }
      base.Dispose(disposing);
    }

    /// <summary>
    /// Ler a lista de itens do XML de parâmetros de documentação
    /// </summary>
    /// <param name="documentationParams">Parâmetros de documentação</param>
    /// <returns>lista de itens do XML de parâmetros de documentação</returns>
    protected abstract List<DictionaryItem> ReadList(DocumentationParams documentationParams);

    /// <summary>
    /// Atualizar a lista com os valores do controle
    /// </summary>
    /// <param name="documentationParams">Parâmetros de documentação</param>
    /// <param name="dictionaryItems">Itens do controle</param>
    protected abstract void UpdateList(DocumentationParams documentationParams, List<DictionaryItem> dictionaryItems);

    /// <summary>
    /// Recuperar a lista de itens default
    /// </summary>
    /// <returns>Default itens</returns>
    protected abstract List<DictionaryItem> GetDefaultItens();

    /// <summary>
    /// This method is called when the dialog page should load its default settings from the backing store.
    /// </summary>
    public override void LoadSettingsFromStorage()
    {
      base.LoadSettingsFromStorage();
      DocumentationParams documentationParams = DocumentationUtils.ReadDocumentationParams(WSPackConsts.DocumentationConfigPath);
      CreateDocumentationControl();
      _documentationDictionaryControl.Bind(ReadList(documentationParams), GetDefaultItens());
    }

    /// <summary>
    /// Salvar as configurações no registro
    /// </summary>
    public override void SaveSettingsToStorage()
    {
      base.SaveSettingsToStorage();
      var dictionaryItems = _documentationDictionaryControl.GetDicionario();
      DocumentationParams documentationParams = DocumentationUtils.ReadDocumentationParams(WSPackConsts.DocumentationConfigPath);
      UpdateList(documentationParams, dictionaryItems);
      DocumentationUtils.SaveDocumentationParams(documentationParams, WSPackConsts.DocumentationConfigPath);
    }
  }
}
