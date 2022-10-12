using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSPack.Lib.Forms
{
  /// <summary>
  /// Form básico com alguns recursos
  /// </summary>
  public partial class BaseForm : Form
  {
    /// <summary>
    /// Inicialização da classe: <see cref="BaseForm"/>.
    /// </summary>
    public BaseForm()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Indica se o botão OK está ou não habilitado
    /// </summary>
    protected bool BotaoOKHabilitado
    {
      get { return btnOK.Enabled; }
      set { btnOK.Enabled = value; }
    }

    private void BaseForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
      {
        DialogResult = DialogResult.Cancel;
        Close();
      }
    }
  }
}
