using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;

namespace WSPack.Lib.Forms
{
  /// <summary>
  /// Tela para informar um item para busca no Solution Explorer
  /// </summary>
  public partial class SolutionExplorerLocateItemForm : BaseForm
  {
    readonly Func<Dictionary<string, object>> _funcGetAllItens;
    Dictionary<string, object> _lstAllItens;

    /// <summary>
    /// Inicialização da classe: <see cref="SolutionExplorerLocateItemForm"/>.
    /// </summary>
    public SolutionExplorerLocateItemForm(Func<Dictionary<string, object>> getAllItensHandler)
    {
      InitializeComponent();
      _funcGetAllItens = getAllItensHandler;
    }

    /// <summary>
    /// Item informado
    /// </summary>
    public string Item
    {
      get { return cbItem.Text; }
      set { cbItem.MakeFirst(value); }
    }

    void CarregarItensCombo(IEnumerable<string> lstItens)
    {
      cbItem.BeginUpdate();
      IEnumerable<string> lstToFill;
      if (rbTodosItens.Checked)
        lstToFill = lstItens;
      else if (rbProjetos.Checked)
        lstToFill = lstItens.Where(x => x.EndsWithInsensitive(".csproj"));
      else if (rbCSharp.Checked)
        lstToFill = lstItens.Where(x => x.EndsWithInsensitive(".cs"));
      else
      {
        if (edtFiltro.Text.IsNullOrWhiteSpaceEx() || edtFiltro.Text.Length <= 2)
          lstToFill = lstItens;
        else
          lstToFill = lstItens.Where(x => x.ContainsInsensitive(edtFiltro.Text));
      }

      try
      {
        cbItem.Items.Clear();
        foreach (var item in lstToFill)
        {
          cbItem.Items.Add(item);
        }
        if (cbItem.Items.Count > 0)
          cbItem.SelectedIndex = 0;
      }
      finally
      {
        lbTotalItens.Text = cbItem.Items.Count.ToString();
        cbItem.EndUpdate();
      }
    }

    private void btnCarregarItens_Click(object sender, EventArgs e)
    {
      Cursor = Cursors.WaitCursor;
      btnCarregarItens.Enabled = false;
      try
      {
        _lstAllItens = _funcGetAllItens();
        CarregarItensCombo(_lstAllItens.Select(x => x.Key));
        if (cbItem.Items.Count > 0)
          cbItem.DroppedDown = true;
      }
      finally
      {
        btnCarregarItens.Enabled = true;
        Cursor = Cursors.Default;
      }
    }

    private void SolutionExplorerLocateItemForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter && BotaoOKHabilitado)
      {
        if (!cbItem.DroppedDown)
        {
          DialogResult = DialogResult.OK;
          Close();
        }
      }
    }

    private void rbTodosItens_CheckedChanged(object sender, EventArgs e)
    {
      edtFiltro.Enabled = rbContem.Checked;
      if (_lstAllItens != null)
      {
        var lstToFill = _lstAllItens.Select(x => x.Key);
        CarregarItensCombo(lstToFill);
      }
    }

    private void edtFiltro_TextChanged(object sender, EventArgs e)
    {
      if (_lstAllItens != null)
      {
        CheckErrorProvider();
        var lstToFill = _lstAllItens.Select(x => x.Key);
        CarregarItensCombo(lstToFill);
      }
    }

    private void CheckErrorProvider()
    {
      if (!rbContem.Checked || edtFiltro.Text.IsNullOrWhiteSpaceEx() || edtFiltro.Text.Length > 2)
        errorProvider1.SetError(edtFiltro, "");
      else
        errorProvider1.SetError(edtFiltro, "Informe 3 ou mais caracteres");
    }
  }
}
