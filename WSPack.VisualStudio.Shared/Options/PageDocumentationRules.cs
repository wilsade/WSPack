using System;
using System.ComponentModel;
using System.Windows.Forms;

using WSPack.Lib.DocumentationObjects;
using WSPack.VisualStudio.Shared.UserControls;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Página de opção de regras de documentação
  /// </summary>
  public class PageDocumentationRules : BaseDialogPage
  {
    DocumentationRulesControl _documentationRulesControl;

    /// <summary>
    /// Inicialização da classe <see cref="PageDocumentationRules"/>
    /// </summary>
    public PageDocumentationRules()
    {
      OnGuardarOpcoes += PageDocumentationRules_OnGuardarOpcoes;
    }

    private void PageDocumentationRules_OnGuardarOpcoes(object sender, EventArgs e)
    {
      LoadDocumentationRules();
    }

    bool CreateDocumentationControl()
    {
      if (_documentationRulesControl == null)
      {
        _documentationRulesControl = new DocumentationRulesControl();
        return true;
      }
      return false;
    }

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
          _documentationRulesControl.OptionsPage = this;
        return _documentationRulesControl;
      }
    }

    /// <summary>
    /// Liberar o objeto
    /// </summary>
    /// <param name="disposing">true se disposing</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (_documentationRulesControl != null)
        {
          _documentationRulesControl.Dispose();
          _documentationRulesControl = null;
        }
      }
      base.Dispose(disposing);
    }

    /// <summary>
    /// Handles Windows Activate messages from the Visual Studio environment.
    /// </summary>
    /// <param name="e">[in] Arguments to event handler.</param>
    protected override void OnActivate(CancelEventArgs e)
    {
      base.OnActivate(e);
      CreateDocumentationControl();
      _documentationRulesControl.ExpandNodes(TypeNodeExpanded, MemberNodeExpanded);
    }

    /// <summary>
    /// Indica se o nodo de types está expandido
    /// </summary>
    [Browsable(false)]
    public bool TypeNodeExpanded { get; set; }

    /// <summary>
    /// Indica se o nodo de membros está expandido
    /// </summary>
    [Browsable(false)]
    public bool MemberNodeExpanded { get; set; }

    /// <summary>
    /// This method is called when the dialog page should load its default settings from the backing store.
    /// </summary>
    public override void LoadSettingsFromStorage()
    {
      base.LoadSettingsFromStorage();
      LoadDocumentationRules();
    }

    private void LoadDocumentationRules()
    {
      DocumentationParams documentationParams = DocumentationUtils.ReadDocumentationParams(WSPackConsts.DocumentationConfigPath);
      CreateDocumentationControl();
      _documentationRulesControl.Bind(documentationParams.RuleList);
    }

    /// <summary>
    /// Salvar as configurações no registro
    /// </summary>
    public override void SaveSettingsToStorage()
    {
      TypeNodeExpanded = _documentationRulesControl.IsTypeNodeExpanded;
      MemberNodeExpanded = _documentationRulesControl.IsMemberNodeExpanded;
      base.SaveSettingsToStorage();
      var ruleItems = _documentationRulesControl.GetRules();
      DocumentationParams documentationParams = DocumentationUtils.ReadDocumentationParams(WSPackConsts.DocumentationConfigPath);
      documentationParams.RuleList = ruleItems;
      DocumentationUtils.SaveDocumentationParams(documentationParams, WSPackConsts.DocumentationConfigPath);
    }
  }
}
