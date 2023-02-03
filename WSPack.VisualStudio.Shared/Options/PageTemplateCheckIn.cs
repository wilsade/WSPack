using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using WSPack.VisualStudio.Shared.UserControls;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Página com template de Check In
  /// </summary>
  [ClassInterface(ClassInterfaceType.AutoDual)]
  [Guid("2382A8DA-7BE8-44D6-8034-3340A138CDCF")]
  public class PageTemplateCheckIn : BaseDialogPage
  {
    TemplateCheckInControl _templateCheckInControl;

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
        if (_templateCheckInControl == null)
        {
          _templateCheckInControl = new TemplateCheckInControl
          {
            OptionsPage = this
          };
        }
        return _templateCheckInControl;
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
        if (_templateCheckInControl != null)
        {
          _templateCheckInControl.Dispose();
          _templateCheckInControl = null;
        }
      }
      base.Dispose(disposing);
    }

    /// <summary>
    /// Handles "apply" messages from the Visual Studio environment.
    /// </summary>
    /// <param name="e">Parâmetros</param>
    /// <devdoc>
    /// This method is called when VS wants to save the user's
    /// changes then the dialog is dismissed.
    /// </devdoc>
    protected override void OnApply(PageApplyEventArgs e)
    {
      if (e.ApplyBehavior == ApplyKind.Apply)
      {
        TemplateCheckIn = _templateCheckInControl.TemplateCheckIn;
      }
      base.OnApply(e);
    }

    /// <summary>
    /// Template check in
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string TemplateCheckIn { get; set; }
  }
}