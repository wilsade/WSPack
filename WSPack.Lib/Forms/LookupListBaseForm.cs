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
using WSPack.Lib.Items;

namespace WSPack.Lib.Forms
{
  /// <summary>
  /// Tela para buscar um item
  /// </summary>
  public partial class LookupListBaseForm : BaseForm
  {
    IEnumerable<LookupGridItem> _lstItens;

    /// <summary>
    /// Inicialização da classe: <see cref="LookupListBaseForm"/>.
    /// </summary>
    public LookupListBaseForm()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Título da janela
    /// </summary>
    public string Caption
    {
      get { return Text; }
      set { Text = value; }
    }

    /// <summary>
    /// Rótulo de pesquisa
    /// </summary>
    public string Label
    {
      get { return groupBox1.Text; }
      set { groupBox1.Text = value; }
    }

    /// <summary>
    /// Indica se a barra de status está visível
    /// </summary>
    public bool StatusBarVisible
    {
      get { return statusBar.Visible; }
      set { statusBar.Visible = value; }
    }

    /// <summary>
    /// Rótulo da barra de status
    /// </summary>
    public string StatusBarLabel
    {
      get { return statusLabel.Text; }
      set { statusLabel.Text = value; }
    }

    /// <summary>
    /// Item selecionado
    /// </summary>
    public LookupGridItem ItemSelecionado
    {
      get
      {
        if (bindingSource1.CurrencyManager.Count > 0)
          return (LookupGridItem)bindingSource1.CurrencyManager.Current;
        else if (!edtItem.Text.IsNullOrWhiteSpaceEx())
          return new LookupGridItem(Path.GetFileName(edtItem.Text), edtItem.Text);
        else
          return null;
      }
    }

    /// <summary>
    /// Preencher a lista
    /// </summary>
    /// <param name="lstItens">Lista de itens</param>
    public void Bind(IEnumerable<LookupGridItem> lstItens)
    {
      if (_lstItens == null)
        _lstItens = lstItens;
      bindingSource1.DataSource = lstItens.ToList();
      bindingSource1.CurrencyManager.Position = 0;
      grid.AutoResizeColumns();
    }

    /// <summary>
    /// Acontece ao alterar o texto de procura
    /// </summary>
    public Func<string, IList<LookupGridItem>, bool> AoAlterarTextoProcura { get; set; }

    /// <summary>
    /// Método disparado ao alterar o texto do item de procura
    /// </summary>
    public void HabilitarBotaoOK()
    {
      if (AoAlterarTextoProcura != null)
        BotaoOKHabilitado = AoAlterarTextoProcura(edtItem.Text, (IList<LookupGridItem>)bindingSource1.List);
      else
        BotaoOKHabilitado = bindingSource1.CurrencyManager.List.Count > 0 &&
          bindingSource1.CurrencyManager.Current != null;
    }

    private void LookupListBaseForm_Shown(object sender, EventArgs e)
    {
      grid.AutoResizeColumns();
    }

    private void edtItem_TextChanged(object sender, EventArgs e)
    {
      if (edtItem.Text?.Length >= 2)
      {
        var filtrado = _lstItens.Where(x => x.Caminho.ContainsInsensitive(edtItem.Text) ||
          x.Nome.ContainsInsensitive(edtItem.Text));
        Bind(filtrado);
      }
      else
      {
        Bind(_lstItens);
      }
      HabilitarBotaoOK();
    }

    private void edtItem_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        if (BotaoOKHabilitado || bindingSource1.Count > 0)
        {
          DialogResult = DialogResult.OK;
          Close();
        }
      }

      if (bindingSource1.List.Count > 0)
      {
        if (e.KeyCode == Keys.Down)
        {
          if (bindingSource1.CurrencyManager.Position < bindingSource1.CurrencyManager.List.Count)
          {
            bindingSource1.CurrencyManager.Position += 1;
          }
        }
        else if (e.KeyCode == Keys.Up)
        {
          if (bindingSource1.CurrencyManager.Position > 0)
          {
            bindingSource1.CurrencyManager.Position -= 1;
          }
        }
      }
    }

    private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (ItemSelecionado != null && BotaoOKHabilitado)
      {
        DialogResult = DialogResult.OK;
        Close();
      }
    }
  }
}


