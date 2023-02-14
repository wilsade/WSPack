using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WSPack.Lib.Extensions;
using WSPack.Lib.Items;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Commands;

namespace WSPack.VisualStudio.Shared.Forms
{
  /// <summary>
  /// Definir serviços para a interface de geração de resources
  /// </summary>
  public interface IGenerateResourceForm
  {
    /// <summary>
    /// Nome do arquivo de resource
    /// </summary>
    ResourcesFileItem ArquivoResource { get; set; }

    /// <summary>
    /// Prefixo
    /// </summary>
    string Prefixo { get; set; }

    /// <summary>
    /// Nome do resource
    /// </summary>
    string NomeResource { get; set; }

    /// <summary>
    /// Valor do resource
    /// </summary>
    string ValorResource { get; set; }

    /// <summary>
    /// Código gerado
    /// </summary>
    string CodigoGerado { get; }

    /// <summary>
    /// Indica qual Resource foi escolhida
    /// </summary>
    ResourceEntry SelectedResource { get; }

    /// <summary>
    /// Carregar no comboBox os arquivos de resources
    /// </summary>
    /// <param name="lstFiles">Lista de arquivos</param>
    void SetResourcesFiles(IEnumerable<ResourcesFileItem> lstFiles);
  }

  /// <summary>
  /// Form para geração de ResourceStrings
  /// </summary>
  public partial class GenerateResourceForm : Form, IGenerateResourceForm
  {
    DataTable _dt;
    ResourceEntry _resourceEntry;

    const string ColunaMarcar = "colMarcar";
    const string ColunaNome = "colNome";
    const string ColunaValor = "colValor";
    const string ColunaComentario = "colComentario";

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe: <see cref="GenerateResourceForm"/>
    /// </summary>
    public GenerateResourceForm()
    {
      InitializeComponent();
      Self = this;
      CreateDataTable();
    }
    #endregion

    #region Propriedades
    /// <summary>
    /// Serviços para a interface de geração de resources
    /// </summary>
    public IGenerateResourceForm Self { get; }

    /// <summary>
    /// Nome do arquivo de resource
    /// </summary>
    ResourcesFileItem IGenerateResourceForm.ArquivoResource
    {
      get => cbArquivoResources.SelectedItem as ResourcesFileItem;
      set
      {
        if (value != null && cbArquivoResources.Items.Count > 0)
        {
          int i = cbArquivoResources.Items.IndexOf(value);
          if (i >= 0)
            cbArquivoResources.SelectedItem = value;
          else
            cbArquivoResources.SelectedIndex = 0;
        }
      }
    }

    /// <summary>
    /// Prefixo
    /// </summary>
    string IGenerateResourceForm.Prefixo
    {
      get => edtPrefixo.Text;
      set => edtPrefixo.Text = value;
    }

    /// <summary>
    /// Nome do resource
    /// </summary>
    string IGenerateResourceForm.NomeResource
    {
      get => edtNomeResource.Text;
      set
      {
        if (!value.IsNullOrWhiteSpaceEx())
        {
          var parts = value.RemoveAccents().Split(new char[]
          {
            ' ', ',', '.', '?', '!', '(', ')' , '-', ';', '/', '\\', ':', '"', '\'', '<', '>', '\r', '\n', '{', '}', '*'
          }, StringSplitOptions.RemoveEmptyEntries);
          var str = new StringBuilder();
          int usouPalavra = 0;
          for (int i = 0; i < parts.Length; i++)
          {
            string word = parts[i];
            if (word.Length >= 3)
            {
              usouPalavra++;
              str.Append(word.ToFirstCharToUpper());
              if (usouPalavra == 6)
                break;
            }
          }
          edtNomeResource.Text = str.ToString();
        }
      }
    }

    /// <summary>
    /// Valor do resource
    /// </summary>
    string IGenerateResourceForm.ValorResource
    {
      get => memoValorResource.Text;
      set
      {
        memoValorResource.Text = value;
      }
    }

    /// <summary>
    /// Código gerado
    /// </summary>
    string IGenerateResourceForm.CodigoGerado => edtCodigoGerado.Text;

    /// <summary>
    /// Indica qual Resource foi escolhida
    /// </summary>
    ResourceEntry IGenerateResourceForm.SelectedResource => _resourceEntry;

    #endregion

    #region Métodos

    /// <summary>
    /// Carregar no comboBox os arquivos de resources
    /// </summary>
    /// <param name="lstFiles">Lista de arquivos</param>
    void IGenerateResourceForm.SetResourcesFiles(IEnumerable<ResourcesFileItem> lstFiles)
    {
      cbArquivoResources.SelectedIndexChanged -= cbArquivoResources_SelectedIndexChanged;
      try
      {
        cbArquivoResources.Items.Clear();
        lstFiles.ForEach(x => cbArquivoResources.Items.Add(x));
      }
      finally
      {
        cbArquivoResources.SelectedIndexChanged += cbArquivoResources_SelectedIndexChanged;
      }
    }

    /// <summary>
    /// Exibe o form para definição do Resource
    /// </summary>
    /// <param name="resourceEntry">Representa um item do arquivo de ResourceString</param>
    /// <param name="createNew">true se o resource deverá ser criado</param>
    /// <returns>true se o form foi confirmado pelo usuário</returns>
    public bool ShowDialog(out ResourceEntry resourceEntry, out bool createNew)
    {
      DialogResult dialogResult = ShowDialog();
      if (dialogResult == DialogResult.OK)
      {
        resourceEntry = _resourceEntry;
        createNew = rbGerarNovoResource.Checked;
        return true;
      }
      else
      {
        resourceEntry = null;
        createNew = false;
        return false;
      }
    }
    #endregion

    private void CreateDataTable()
    {
      _dt = new DataTable();

      _dt.Columns.Add(ColunaNome, typeof(string));
      colNome.DataPropertyName = ColunaNome;

      _dt.Columns.Add(ColunaValor, typeof(string));
      colValor.DataPropertyName = ColunaValor;

      _dt.Columns.Add(ColunaComentario, typeof(string));
      colComentario.DataPropertyName = ColunaComentario;

      gridResources.DataSource = bindingSource1;
      bindingSource1.DataSource = _dt;
    }

    private void cbArquivoResources_SelectedIndexChanged(object sender, EventArgs e)
    {
      _dt.BeginLoadData();
      try
      {
        _dt.Rows.Clear();
        using (var oReader = new ResXResourceReader(Self.ArquivoResource.ResxFileName))
        {
          oReader.UseResXDataNodes = true;
          foreach (DictionaryEntry esteItem in oReader)
          {
            var node = (ResXDataNode)esteItem.Value;
            if (node.FileRef == null)
            {
              var obj = new ResourceEntry(node);
              _dt.Rows.Add(obj.Name, obj.Value, obj.Comment);
            }
          }
        }
        toolTip1.SetToolTip(cbArquivoResources, Self.ArquivoResource.ResxFileName);
      }
      finally
      {
        //SizeColumns();
        HabilitarBotaoOK();
        _dt.EndLoadData();
      }
    }

    /*
    private void SizeColumns()
    {
      List<int> lstWidth = new List<int>();
      gridResources.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      foreach (DataGridViewColumn item in gridResources.Columns)
      {
        lstWidth.Add(item.Width);
      }
      gridResources.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
      for (int i = 0; i < lstWidth.Count; i++)
      {
        gridResources.Columns[i].Width = lstWidth[i];
      }
    }*/

    private void GenerateResourceForm_Shown(object sender, EventArgs e)
    {
      cbCondicao.SelectedIndex = 0;
      cbArquivoResources.SelectedIndexChanged -= cbArquivoResources_SelectedIndexChanged;
      cbArquivoResources.SelectedIndexChanged += cbArquivoResources_SelectedIndexChanged;

      //cbArquivoResources_SelectedIndexChanged(sender, e);
      //SizeColumns();
      edtNomeResource.Focus();
      cbCampos.SelectedIndex = 0;

      if (rbUsarResourceExistente.Tag == null)
        rbGerarNovoResource.Checked = true;
      else
      {
        gridResources.CurrentRow.Cells[ColunaMarcar].Value = colMarcar.TrueValue;
        HabilitarBotaoOK();
      }

    }

    string MontaNome => string.Concat(edtPrefixo.Text, edtNomeResource.Text).Replace("*", "");
    bool HasResource => _dt.AsEnumerable().Any(x => x[ColunaNome].ToString().EqualsInsensitive(MontaNome));

    private void edtNomeResource_TextChanged(object sender, EventArgs e)
    {
      cbCampos.SelectedIndex = 0;
      _ = MontaNome;
      if (!ProcurarItemGrid(ColunaNome, MontaNome))
      {
        ProcurarItemGrid(ColunaValor, memoValorResource.Text);
      }

      //errorProvider1.Clear();
      //if (!monta.IsNullOuWhiteSpaces())
      //{
      //if (!monta.All(c => (c == '_') || char.IsLetterOrDigit(c)) || char.IsDigit(monta.FirstOrDefault()))
      //errorProvider1.SetError(edtNomeResource, Properties.Resources.StrCaracteresInvalidos);
      //        else 
      //if (HasResource)
      //errorProvider1.SetError(edtNomeResource, Properties.Resources.StrItemExistente.FormatWith(monta));
      //}
      CheckErros();
      MontaCodigoCSharp();
      HabilitarBotaoOK();
    }

    void CheckErros()
    {
      errorProvider1.Clear();

      bool TemErro(string texto)
      {
        if (!texto.All(c => (c == '_') || char.IsLetterOrDigit(c)) || char.IsDigit(texto.FirstOrDefault()))
          return true;
        return false;
      }

      if (TemErro(edtPrefixo.Text))
        errorProvider1.SetError(edtPrefixo, ResourcesLib.StrCaracteresInvalidos);

      if (TemErro(edtNomeResource.Text))
        errorProvider1.SetError(edtNomeResource, ResourcesLib.StrCaracteresInvalidos);
      else if (HasResource)
        errorProvider1.SetError(edtNomeResource, ResourcesLib.StrItemExistente.FormatWith(MontaNome));
    }

    private void MontaCodigoCSharp(DataGridViewRow useThisRow = null)
    {
      if (rbGerarNovoResource.Checked)
        edtCodigoGerado.Text = $"{Self.ArquivoResource.ClassName}.{MontaNome}";
      else
      {
        var reg = GetResourceEntryFromGrid(useThisRow ?? gridResources.CurrentRow);
        if (reg is ResourceEntry resourceEntry)
          edtCodigoGerado.Text = $"{Self.ArquivoResource.ClassName}.{resourceEntry.Name}";
      }
    }

    private void HabilitarBotaoOK()
    {
      if (rbGerarNovoResource.Checked)
      {
        btnOK.Enabled = !edtNomeResource.Text.IsNullOrWhiteSpaceEx() &&
          !memoValorResource.Text.IsNullOrWhiteSpaceEx() &&
          !HasResource &&
          errorProvider1.GetError(edtNomeResource).IsNullOrWhiteSpaceEx() &&
          errorProvider1.GetError(edtPrefixo).IsNullOrWhiteSpaceEx();
      }
      else
      {
        btnOK.Enabled = GetLinhaMarcadaGrid() != null;
      }
    }

    private void memoValorResource_TextChanged(object sender, EventArgs e)
    {
      cbCampos.SelectedIndex = 1;
      ProcurarItemGrid(ColunaValor, memoValorResource.Text);

      //if (rbUsarResourceExistente.Checked)
      //  return;

      //if (!memoValorResource.Text.IsNullOuWhiteSpaces())
      //  bindingSource1.Filter = $"{ColunaValor} like '%{memoValorResource.Text}%'";
      //else
      //  bindingSource1.Filter = null;
      HabilitarBotaoOK();
    }

    private void rbGerarNovoResource_CheckedChanged(object sender, EventArgs e)
    {
      // Habilitar grid ao usar resource existente
      gbxResources.Enabled = rbUsarResourceExistente.Checked;
      //if (gridResources.Enabled)
      //  gbxResources.Cursor = gridResources.Cursor = Cursors.Default;
      //else
      //  gbxResources.Cursor = gridResources.Cursor = Cursors.No;

      // Habilitar controles para geração de novo resource
      lbPrefixo.Enabled = edtPrefixo.Enabled =
        lbNomeResource.Enabled = edtNomeResource.Enabled =
        lbValorResource.Enabled = memoValorResource.Enabled =
        lbComentario.Enabled = memoComentario.Enabled = rbGerarNovoResource.Checked;

      if (rbUsarResourceExistente.Tag != null)
      {
        rbUsarResourceExistente.Tag = null;

      }

      //// Limpar filtro ao usar resource existente: assim podemos selecionar um item
      //if (rbUsarResourceExistente.Checked)
      //  bindingSource1.Filter = null;
      //else
      //  edtNomeResource_TextChanged(sender, e); // Atualizar a grid com possíveis filtros

      MontaCodigoCSharp();
      HabilitarBotaoOK();
    }

    private void GenerateResourceForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
        Close();
    }

    private void cbCampos_SelectedIndexChanged(object sender, EventArgs e)
    {
      gridResources.Sort(gridResources.Columns[cbCampos.SelectedIndex + 1], ListSortDirection.Ascending);
      edtProcurar.Text = string.Empty;
    }

    private void edtProcurar_TextChanged(object sender, EventArgs e)
    {
      string campo;
      if (cbCampos.SelectedIndex == 0)
        campo = ColunaNome;
      else if (cbCampos.SelectedIndex == 1)
        campo = ColunaValor;
      else
        campo = ColunaComentario;

      ProcurarItemGrid(campo, edtProcurar.Text);
    }

    static string EscapeLikeValue(string value)
    {
      StringBuilder sb = new StringBuilder(value.Length);
      for (int i = 0; i < value.Length; i++)
      {
        char c = value[i];
        switch (c)
        {
          case ']':
          case '[':
          case '%':
          case '*':
            sb.Append("[").Append(c).Append("]");
            break;
          case '\'':
            sb.Append("''");
            break;
          default:
            sb.Append(c);
            break;
        }
      }
      return sb.ToString();
    }

    private bool ProcurarItemGrid(string campo, string text)
    {
      rbUsarResourceExistente.Tag = null;
      if (_dt?.Rows.Count > 0)
      {
        bindingSource1.Position = 0;
        if (!text.IsNullOrWhiteSpaceEx())
        {
          int loc = 0;
          bool achou = false;
          try
          {
            //Busca parcial
            string condicao = cbCondicao.SelectedIndex == 0 ? "" : "%";
            string rowFilter = $"{campo} like '{condicao}{EscapeLikeValue(text)}%'";
            DataView dv = new DataView(bindingSource1.DataSource as DataTable)
            {
              RowFilter = rowFilter
            };
            if (dv.Count > 0)
            {
              loc = bindingSource1.Find(campo, dv[0][campo]);
              rowFilter = $"{campo} = '{EscapeLikeValue(text)}'";
              dv = new DataView(bindingSource1.DataSource as DataTable)
              {
                RowFilter = rowFilter
              };
              if (dv.Count == 1)
              {
                rbUsarResourceExistente.Checked = true;
                rbUsarResourceExistente.Tag = true;
              }
              achou = dv.Count == 1;
              //achou = true;
            }
          }
          catch (Exception ex)
          {
            Utils.LogDebugMessage($"Erro ao localizar item na grid: {ex.Message}");
          }
          bindingSource1.Position = loc;
          if (achou)
          {
            gridResources_CellContentClick(null, new DataGridViewCellEventArgs(0, gridResources.CurrentRow.Index));
            gridResources.CurrentRow.Cells[ColunaMarcar].Value = colMarcar.TrueValue;
            HabilitarBotaoOK();

          }
          return achou;
        }
      }
      return false;
    }

    static ResourceEntry GetResourceEntryFromGrid(DataGridViewRow row)
    {
      if (row?.DataBoundItem is DataRowView rowView)
      {
        return new ResourceEntry()
        {
          Name = rowView[ColunaNome].ToString(),
          Value = Convert.ToString(rowView[ColunaValor]),
          Comment = Convert.ToString(rowView[ColunaComentario])
        };
      }
      return null;
    }

    DataGridViewRow GetLinhaMarcadaGrid()
    {
      if (gridResources.Rows.Count > 0)
        return gridResources.Rows.OfType<DataGridViewRow>()
          .FirstOrDefault(x => x.Cells[ColunaMarcar].Value == colMarcar.TrueValue);
      else
        return null;
    }

    private void gridResources_SelectionChanged(object sender, EventArgs e)
    {
      MontaCodigoCSharp();
      HabilitarBotaoOK();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (rbUsarResourceExistente.Checked)
      {
        MontaCodigoCSharp(GetLinhaMarcadaGrid());
        _resourceEntry = GetResourceEntryFromGrid(GetLinhaMarcadaGrid());
      }
      else
        _resourceEntry = new ResourceEntry(MontaNome, memoValorResource.Text, memoComentario.Text);
    }

    private void edtNomeResource_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Multiply)
        e.SuppressKeyPress = true;
    }

    private void cbPrefixo_TextChanged(object sender, EventArgs e)
    {
      CheckErros();
      MontaCodigoCSharp();
      HabilitarBotaoOK();
    }

    private void cbPrefixo_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Space)
        e.SuppressKeyPress = true;
    }

    private void gridResources_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
      {
        MontaCodigoCSharp();
        _resourceEntry = GetResourceEntryFromGrid(gridResources.Rows[e.RowIndex]);
        DialogResult = _resourceEntry == null ? DialogResult.Cancel : DialogResult.OK;
        Close();
      }
    }

    private void gridResources_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex == 0 && e.RowIndex >= 0)
      {
        gridResources.CellContentClick -= gridResources_CellContentClick;
        try
        {
          foreach (DataGridViewRow esteLinha in gridResources.Rows)
          {
            if (e.RowIndex != esteLinha.Index)
              esteLinha.Cells[ColunaMarcar].Value = colMarcar.FalseValue;
          }
        }
        finally
        {
          gridResources.EndEdit();
          gridResources.CellContentClick += gridResources_CellContentClick;
          HabilitarBotaoOK();
        }
      }
    }
  }
}
