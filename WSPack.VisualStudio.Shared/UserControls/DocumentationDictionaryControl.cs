using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using WSPack.Lib;
using WSPack.Lib.DocumentationObjects;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Options;

namespace WSPack.VisualStudio.Shared.UserControls
{
  /// <summary>
  /// DocumentationGeneralControl
  /// </summary>
  public partial class DocumentationDictionaryControl : UserControl
  {
    DataTable _dt;
    /// <summary>
    /// Valores default para as opções de documentação desta página
    /// </summary>
    protected List<DictionaryItem> LstDefaultValues;

    #region Construtor
    /// <summary>
    /// Cria um instância da classe <see cref="DocumentationDictionaryControl" />.
    /// </summary>
    public DocumentationDictionaryControl()
    {
      InitializeComponent();
    }
    #endregion

    private void CriarDataTable()
    {
      if (_dt == null)
      {
        _dt = new DataTable
        {
          CaseSensitive = true
        };

        var coluna = _dt.Columns.Add(nameof(DocumentationBaseItem.ItemName), typeof(string));
        coluna.AllowDBNull = false;
        coluna.Unique = true;
        coluna.Caption = colNomeItem.HeaderText;
        _dt.PrimaryKey = new DataColumn[] { coluna };

        coluna = _dt.Columns.Add(nameof(DocumentationBaseItem.Summary), typeof(string));
        coluna.AllowDBNull = false;
      }
    }

    #region Propriedades
    /// <summary>
    /// Gets or sets the reference to the underlying OptionsPage object.
    /// </summary>
    public PageDocumentationBaseGridDictionary OptionsPage { get; set; }

    /// <summary>
    /// Texto contendo informações sobre a tela
    /// </summary>
    public string InformationText
    {
      get { return memoInformacoes.Text; }
      set { memoInformacoes.Text = value; }
    }
    #endregion

    #region Métodos    
    /// <summary>
    /// Faz o bind dos parâmetros
    /// </summary>
    /// <param name="dictionaryItems">Lista do dicionário dos parâmetros</param>
    /// <param name="lstDefaultValues">Lista de default values</param>
    public void Bind(List<DictionaryItem> dictionaryItems, List<DictionaryItem> lstDefaultValues)
    {
      CriarDataTable();
      LstDefaultValues = lstDefaultValues;

      _dt.Rows.Clear();
      dictionaryItems.ForEach(item => _dt.Rows.Add(item.ItemName, item.Summary));
      dataGridView1.DataSource = _dt;
    }

    /// <summary>
    /// Retornar os parâmetros do dicionário de itens
    /// </summary>
    public List<DictionaryItem> GetDicionario()
    {
      var lst = new List<DictionaryItem>();
      foreach (DataGridViewRow esteRow in dataGridView1.Rows)
      {
        string itemName = Convert.ToString(esteRow.Cells[colNomeItem.Name].Value);
        string itemValue = Convert.ToString(esteRow.Cells[colSummary.Name].Value);
        if (!string.IsNullOrWhiteSpace(itemName) && !string.IsNullOrWhiteSpace(itemValue))
        {
          lst.Add(new DictionaryItem(itemName, itemValue, false));
        }
      }
      return lst;
    }

    /// <summary>
    /// Finalizar edições
    /// </summary>
    public void GridEndEdit()
    {
      dataGridView1.EndEdit();
    }

    #endregion

    private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      dataGridView1.Rows[e.RowIndex].ErrorText = null;
    }

    private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      dataGridView1.Rows[e.RowIndex].ErrorText = e.Exception.Message;
      e.Cancel = true;
    }

    private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      e.Cancel = !MessageBoxUtils.ShowConfirmationYesNo(ResourcesLib.StrDesejaExcluirItemLista);
    }

    private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
      if (e.ColumnIndex == 0 && e.RowIndex >= 0)
      {
        string valor = Convert.ToString(e.FormattedValue);
        if (valor.Contains(" "))
        {
          dataGridView1.Rows[e.RowIndex].ErrorText = ResourcesLib.StrNaoPodeTerEspacos;
          e.Cancel = true;
        }
      }
    }

    private void menuItemGerarItensPadrao_Click(object sender, EventArgs e)
    {
      if (MessageBoxUtils.ShowConfirmationYesNo(ResourcesLib.StrConfirmaGeracaoItensDefault))
      {
        foreach (DictionaryItem estePadrao in LstDefaultValues)
        {
          if (_dt.Rows.Find(estePadrao.ItemName) == null)
          {
            _dt.Rows.Add(estePadrao.ItemName, estePadrao.Summary);
          }
        }
      }
    }

    private void menuGrid_Opening(object sender, CancelEventArgs e)
    {
      GridEndEdit();
    }
  }
}
