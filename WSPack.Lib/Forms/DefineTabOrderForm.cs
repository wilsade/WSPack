using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WSPack.Lib.Properties;

namespace WSPack.Lib.Forms
{
  /// <summary>
  /// Form para alteração de tabulação de uma lista de controles
  /// </summary>
  public partial class DefineTabOrderForm : Form
  {
    readonly List<Control> _lstControles;
    readonly string _controlName;

    /// <summary>
    /// Cria uma instância da classe <see cref="DefineTabOrderForm"/>
    /// </summary>
    /// <param name="controlName">Nome do controle que possui os controles</param>
    /// <param name="lstControles">Lista de controles para definir ordem de tabulação.</param>
    public DefineTabOrderForm(string controlName, List<Control> lstControles)
    {
      InitializeComponent();
      _lstControles = lstControles;
      _controlName = controlName;
    }

    void Mover(int indice)
    {
      int i = viewControles.SelectedItems[0].Index;
      ListViewItem item = viewControles.SelectedItems[0];

      viewControles.Items.RemoveAt(i);
      viewControles.Items.Insert(i + indice, item);
      viewControles.Focus();
      item.Focused = true;
      item.Selected = true;

      Seletor.SetSelectedComponents(new object[] { item.Tag as Control });
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      var lista = ListaControlesNaOrdemTabulacao;
      for (int i = 0; i < lista.Count; i++)
      {
        var lstProps = System.ComponentModel.TypeDescriptor.GetProperties(lista[i]);
        var tabProp = lstProps["TabIndex"];
        tabProp.SetValue(lista[i], i);
      }
      DialogResult = System.Windows.Forms.DialogResult.OK;
    }

    private void TabOrderForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
        DialogResult = System.Windows.Forms.DialogResult.Cancel;
      else if (e.Control && e.KeyCode == Keys.Up)
        viewControles_KeyDown(sender, e);
      else if (e.Control && e.KeyCode == Keys.Down)
        viewControles_KeyDown(sender, e);
      else if (e.KeyCode == (Keys.LButton | Keys.ShiftKey))
        Opacity = 0.15D;
    }

    private void TabOrderForm_Shown(object sender, EventArgs e)
    {
      lbControlePai.Text = _controlName;
      viewControles.BeginUpdate();
      try
      {
        foreach (var esteControle in _lstControles.OrderBy(x => x.TabIndex))
        {
          if (UtilsLib.IsValidTabOrderControl(esteControle, false))
          {
            ListViewItem item = new ListViewItem(UtilsLib.FormatControlName(esteControle))
            {
              Tag = esteControle
            };
            if (esteControle.Controls.Count > 0 && UtilsLib.IsValidTabOrderControl(esteControle, true))
            {
              item.Font = new Font(viewControles.Font, FontStyle.Italic);
              item.ForeColor = Color.Green;
            }
            viewControles.Items.Add(item);
          }
        }
      }
      finally
      {
        viewControles.EndUpdate();
      }

      if (viewControles.Items.Count > 0)
        viewControles.Items[0].Selected = true;
    }

    private void viewControles_SelectedIndexChanged(object sender, EventArgs e)
    {
      btnUp.Enabled = viewControles.SelectedItems.Count > 0 && viewControles.SelectedItems[0].Index >= 1;
      btnDown.Enabled = viewControles.SelectedItems.Count > 0 && viewControles.SelectedItems[0].Index <= viewControles.Items.Count - 2;
      if (viewControles.SelectedItems.Count > 0)
      {
        Control controleSelecao = (viewControles.SelectedItems[0].Tag as Control);
        Seletor.SetSelectedComponents(new object[] { controleSelecao });
      }
    }

    private void viewControles_DoubleClick(object sender, EventArgs e)
    {
      if (viewControles.SelectedItems.Count > 0)
      {
        Control controle = viewControles.SelectedItems[0].Tag as Control;
        if (controle.Controls.Count > 0 && UtilsLib.IsValidTabOrderControl(controle, true))
        {
          using (DefineTabOrderForm form = new DefineTabOrderForm(UtilsLib.FormatControlName(controle), controle.Controls.OfType<Control>().ToList()))
          {
            Opacity = 0.10D;
            try
            {
              form.Seletor = Seletor;
              form.ShowDialog();
            }
            finally
            {
              Opacity = 1D;
            }
          }
        }
      }
    }

    private void btnUp_Click(object sender, EventArgs e)
    {
      Mover(-1);
    }

    private void btnDown_Click(object sender, EventArgs e)
    {
      Mover(1);
    }

    #region Propriedades    
    /// <summary>
    /// (Gets) Lista de controles na ordem de tabulação
    /// </summary>
    List<Control> ListaControlesNaOrdemTabulacao
    {
      get
      {
        List<Control> lst = new List<Control>();
        foreach (ListViewItem esteitem in viewControles.Items)
        {
          lst.Add(esteitem.Tag as Control);
        }
        return lst;
      }
    }

    /// <summary>
    /// (Gets or sets) Objeto para manipular a seleção da tela em Design
    /// </summary>
    public System.ComponentModel.Design.ISelectionService Seletor { get; set; }
    #endregion

    private void TabOrderForm_Load(object sender, EventArgs e)
    {
      btnUp.Image = ResourcesLib.pngArrowUp;
      btnDown.Image = ResourcesLib.pngArrowDown;
    }

    private void viewControles_KeyDown(object sender, KeyEventArgs e)
    {
      if (viewControles.SelectedItems.Count > 0)
      {
        if (e.Control && e.KeyCode == Keys.Up && btnUp.Enabled)
        {
          e.Handled = true;
          btnUp_Click(sender, e);
        }
        else if (e.Control && e.KeyCode == Keys.Down && btnDown.Enabled)
        {
          e.Handled = true;
          btnDown_Click(sender, e);
        }
        else if (e.KeyCode == Keys.Enter)
          viewControles_DoubleClick(sender, e);
      }
    }

    private void TabOrderForm_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == (Keys.LButton | Keys.ShiftKey))
        Opacity = 1D;
    }
  }
}
