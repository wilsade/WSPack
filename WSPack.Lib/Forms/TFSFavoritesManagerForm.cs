using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using WSPack.Lib.Extensions;
using WSPack.Lib.Items;
using WSPack.Lib.Properties;

namespace WSPack.Lib.Forms
{
  /// <summary>
  /// Form para gerenciar favoritos
  /// </summary>
  public partial class TFSFavoritesManagerForm : Form
  {
    List<TFSFavoritesParams> _lstFavoritos;
    List<TFSFavoritesParams> _lstRows;
    readonly ITFSFavoritesManagerForm _favoritesManagerForm;

    bool IsPrimeiraLinha
    {
      get { return grid.CurrentRow != null && grid.CurrentRow.Index == 0; }
    }

    bool IsUltimaLinha
    {
      get { return grid.CurrentRow != null && grid.CurrentRow.Index == grid.RowCount - 1; }
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="TFSFavoritesManagerForm"/>
    /// </summary>
    public TFSFavoritesManagerForm(ITFSFavoritesManagerForm managerForm)
    {
      InitializeComponent();
      _favoritesManagerForm = managerForm;
    }

    /// <summary>
    /// Ajustar o índica de cada favorito do TFS
    /// </summary>
    /// <param name="lst">Lista a ser ordenada</param>
    public static void AdjustIndex(List<TFSFavoritesParams> lst)
    {
      var qry = lst.GroupBy(f => f.TFSUrl);
      foreach (var item in qry)
      {
        int i = 1;
        foreach (TFSFavoritesParams esteItem in item.OrderBy(x => x.Index))
        {
          esteItem.Index = i++;
        }
      }
    }

    /// <summary>
    /// Filtrar e recuperar os favoritos do servidor da Url do TFS
    /// </summary>
    /// <param name="lstFavoritos">Lista contendo todos os favoritos</param>
    /// <param name="serverUrl">Server URL</param>
    /// <returns>Lista do servidor ordenada</returns>
    public static List<TFSFavoritesParams> GetOrderedListFromServer(List<TFSFavoritesParams> lstFavoritos, string serverUrl) =>
      lstFavoritos.Where(x => x.TFSUrl.EqualsInsensitive(serverUrl)).OrderBy(x => x.Index).ToList();

    /// <summary>
    /// Adicionar um novo Favorito
    /// </summary>
    /// <param name="lstFavoritos">Lista de favoritos</param>
    /// <param name="serverUrl">Endereço do servidor do TFS</param>
    /// <param name="serverItem">Caminho do item no servidor</param>
    /// <param name="caption">Caption do item</param>
    /// <param name="onSave">Responsável por salvar os favoritos</param>
    /// <returns>Favorito adicionado ou recuperado</returns>
    public static TFSFavoritesParams AdicionarFavorito(List<TFSFavoritesParams> lstFavoritos,
      string serverUrl, string serverItem, string caption, Action<List<TFSFavoritesParams>> onSave)
    {
      TFSFavoritesParams row = lstFavoritos.FirstOrDefault(x => x.TFSUrl.EqualsInsensitive(serverUrl) && x.ServerItem.EqualsInsensitive(serverItem));
      if (row == null)
      {
        int maximo = 0;
        var listaFavoritos = lstFavoritos.Where(x => x.TFSUrl == serverUrl);
        if (listaFavoritos.Any())
          maximo = listaFavoritos.Max(i => i.Index);

        row = new TFSFavoritesParams()
        {
          TFSUrl = serverUrl,
          ServerItem = serverItem,
          Caption = caption,
          Index = maximo + 1
        };
        lstFavoritos.Add(row);

        AdjustIndex(lstFavoritos);

        onSave(lstFavoritos);
      }

      return row;
    }

    private void TFSFavoritesManagerForm_Load(object sender, EventArgs e)
    {
      Icon = ResourcesLib.icoFavorites;
      menuItemInserir.Image = ResourcesLib.imgCreate;
      menuItemEditar.Image = ResourcesLib.imgPencil;
      menuItemExcluirFavorito.Image = ResourcesLib.imgDelete;

      edtServidor.Text = _favoritesManagerForm.DomainUri;
      btnInserir.ToolTipText = menuItemInserir.ToolTipText;
      btnEditar.ToolTipText = menuItemEditar.ToolTipText;
      btnRemover.ToolTipText = menuItemExcluirFavorito.ToolTipText;

      _lstFavoritos = _favoritesManagerForm.LoadFrom();
      AdjustIndex(_lstFavoritos);
      _lstRows = GetOrderedListFromServer(_lstFavoritos, edtServidor.Text);
      BindGrid();
      grid_SelectionChanged(sender, e);
    }

    private void BindGrid()
    {
      grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      grid.DataSource = _lstRows;

      DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
      ComponentResourceManager resources = new ComponentResourceManager(typeof(TFSFavoritesManagerForm));
      resources.ApplyResources(grid.Columns[ServerItem.Name], ServerItem.Name);
      resources.ApplyResources(grid.Columns[Caption.Name], Caption.Name);
      resources.ApplyResources(grid.Columns[Index.Name], Index.Name);
      dataGridViewCellStyle1.Font = new Font("Courier New", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
      grid.Columns[ServerItem.Name].DefaultCellStyle = dataGridViewCellStyle1;

      int tamanhoServer = grid.Columns[ServerItem.Name].Width;
      int tamanhoCaption = grid.Columns[Caption.Name].Width;
      int tamanhoIndex = grid.Columns[Index.Name].Width;
      grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

      grid.Columns[ServerItem.Name].Width = tamanhoServer;
      grid.Columns[Caption.Name].Width = tamanhoCaption;
      grid.Columns[Index.Name].Width = tamanhoIndex;
      grid.Columns[Index.Name].ReadOnly = true;

      grid.Columns[nameof(TFSFavoritesParams.TFSUrl)].Visible = false;
      for (int i = 4; i < grid.Columns.Count; i++)
      {
        grid.Columns[i].Visible = false;
      }
      btnRemover.Enabled = _lstRows.Count > 0;
    }

    private void TFSFavoritesManagerForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      Save();
    }

    private void Save()
    {
      _favoritesManagerForm.Save(_lstFavoritos);
    }

    private void btnRemover_Click(object sender, EventArgs e)
    {
      if (grid.CurrentRow == null)
        return;

      TFSFavoritesParams row = (TFSFavoritesParams)grid.CurrentRow.DataBoundItem;

      if (MessageBoxUtils.ShowWarningYesNo(ResourcesLib.StrConfirmaExclusaoFavorito, row.ServerItem))
      {
        int indiceLinha = grid.CurrentRow.Index;
        int indiceColuna = grid.CurrentCell.ColumnIndex;
        grid.DataSource = null;

        _lstFavoritos.Remove(row);

        Save();
        _lstRows = GetOrderedListFromServer(_lstFavoritos, edtServidor.Text);
        BindGrid();

        // Ajustar o índice
        if (grid.Rows.Count > 0)
        {
          if (indiceLinha >= grid.Rows.Count)
            indiceLinha = grid.Rows.Count - 1;
          grid[indiceColuna, indiceLinha].Selected = true;
          grid.CurrentCell = grid[indiceColuna, indiceLinha];
        }
      }
    }

    private void menuGrid_Opening(object sender, CancelEventArgs e)
    {
      menuItemEditar.Enabled = btnEditar.Enabled = grid.CurrentRow != null;
      menuItemExcluirFavorito.Enabled = btnRemover.Enabled;
    }

    private void grid_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        DataGridView.HitTestInfo info = grid.HitTest(e.X, e.Y);
        if (info != null && info.ColumnIndex >= 0 && info.RowIndex >= 0)
        {
          grid.ClearSelection();
          grid[info.ColumnIndex, info.RowIndex].Selected = true;
          grid.CurrentCell = grid[info.ColumnIndex, info.RowIndex];
        }
      }
    }

    private void grid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
      if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
      {
        string valor = Convert.ToString(e.FormattedValue);

        // Estamos na coluna serverItem e o conteúdo foi alterado
        if (grid.Columns[e.ColumnIndex].Name == ServerItem.Name &&
          Convert.ToString(grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) != valor)
        {
          if (!_favoritesManagerForm.ItemExists(valor))
          {
            MessageBoxUtils.ShowError(ResourcesLib.StrItemNaoEncontrado, valor);
            e.Cancel = true;
          }
        }
      }
    }

    private void grid_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      if (e.Exception is NoNullAllowedException ||
        e.Exception is ArgumentException)
      {
        MessageBoxUtils.ShowError(ResourcesLib.StrConteudoCampoNuloInvalido);
        e.Cancel = true;
      }

      else if (e.Exception != null)
      {
        MessageBoxUtils.ShowError(e.Exception.Message);
        e.Cancel = true;
      }
    }

    private void grid_SelectionChanged(object sender, EventArgs e)
    {
      btnUp.Enabled = !IsPrimeiraLinha;
      btnDown.Enabled = !IsUltimaLinha;
    }

    private void btnUp_Click(object sender, EventArgs e)
    {
      TFSFavoritesParams itemAtual, itemParaTrocar;
      int celulaAtual = grid.CurrentCell.ColumnIndex;

      if (sender == btnUp)
      {
        itemAtual = (TFSFavoritesParams)grid.CurrentRow.DataBoundItem;
        itemParaTrocar = (TFSFavoritesParams)grid.Rows[grid.CurrentRow.Index - 1].DataBoundItem;

        itemAtual.Index--;
        itemParaTrocar.Index++;

      }
      else
      {
        itemAtual = (TFSFavoritesParams)grid.CurrentRow.DataBoundItem;
        itemParaTrocar = (TFSFavoritesParams)grid.Rows[grid.CurrentRow.Index + 1].DataBoundItem;

        itemAtual.Index++;
        itemParaTrocar.Index--;
      }

      int linhaNova = itemAtual.Index - 1;

      Save();
      _lstRows = GetOrderedListFromServer(_lstFavoritos, edtServidor.Text);
      BindGrid();

      grid.CurrentCell = grid.Rows[linhaNova].Cells[celulaAtual];
      grid_SelectionChanged(sender, e);
    }

    private void menuItemEditar_Click(object sender, EventArgs e)
    {
      if (grid.CurrentRow != null)
      {
        string serverItem = Convert.ToString(grid.CurrentRow.Cells[ServerItem.Name].Value);
        string item = _favoritesManagerForm.ShowChooseTFSItemDialog(serverItem);
        if (!string.IsNullOrEmpty(item))
        {
          FileInfo info = new FileInfo(item);
          string caption = Microsoft.VisualBasic.Interaction.InputBox(
            ResourcesLib.StrInformeNomeFavorito, ResourcesLib.StrCriandoFavorito, info.Name);
          if (!caption.IsNullOrEmptyEx())
          {
            var celulaAtual = grid.CurrentCell;
            grid.EndEdit();
            grid.CurrentCell = grid.CurrentRow.Cells[ServerItem.Name];
            ((TFSFavoritesParams)grid.CurrentRow.DataBoundItem).ServerItem = item;
            grid.CurrentCell = grid.CurrentRow.Cells[Caption.Name];
            ((TFSFavoritesParams)grid.CurrentRow.DataBoundItem).Caption = caption;
            grid.CurrentCell = celulaAtual;
          }
        }
      }
    }

    private void TFSFavoritesManagerForm_KeyDown(object sender, KeyEventArgs e)
    {
      // Insert: Inserir
      if (e.KeyCode == Keys.Insert)
        btnInserir_Click(sender, e);

      // F4: Editar
      if (e.KeyCode == Keys.F4 && grid.CurrentRow != null)
        menuItemEditar_Click(sender, e);

      // Delete: Excluir
      else if (e.KeyCode == Keys.Delete)
        btnRemover_Click(sender, e);

      // Alt+Up: Mover para cima
      else if (e.Alt && e.KeyCode == Keys.Up && btnUp.Enabled)
      {
        e.Handled = true;
        btnUp_Click(btnUp, e);
      }

      // Alt+Down: Mover para baixo
      else if (e.Alt && e.KeyCode == Keys.Down && btnDown.Enabled)
      {
        e.Handled = true;
        btnUp_Click(btnDown, e);
      }

      // F2: Tentar editar a ordem
      else if (e.KeyCode == Keys.F2 && grid.CurrentCell == grid.CurrentRow.Cells[Index.Name])
        MessageBoxUtils.ShowInformation(ResourcesLib.StrUtilizeBotoesSetaAlterarOrdem);

      // Esc: Sair
      else if (e.KeyCode == Keys.Escape)
        Close();
    }

    private void btnInserir_Click(object sender, EventArgs e)
    {
      string serverItem = grid.CurrentRow != null ? Convert.ToString(grid.CurrentRow.Cells[ServerItem.Name].Value) : null;
      string item = _favoritesManagerForm.ShowChooseTFSItemDialog(serverItem);
      if (!string.IsNullOrEmpty(item))
      {
        if (_lstRows.Any(x => x.ServerItem.EqualsInsensitive(item)))
        {
          MessageBoxUtils.ShowWarning(ResourcesLib.StrFavoritoExistente, item);
        }
        else
        {
          FileInfo info = new FileInfo(item);
          string caption = Microsoft.VisualBasic.Interaction.InputBox(
            ResourcesLib.StrInformeNomeFavorito, ResourcesLib.StrCriandoFavorito, info.Name);
          if (!caption.IsNullOrEmptyEx())
          {
            AdicionarFavorito(_lstFavoritos, edtServidor.Text, item, caption, _favoritesManagerForm.Save);
            Save();
            _lstRows = GetOrderedListFromServer(_lstFavoritos, edtServidor.Text);
            BindGrid();
            grid.CurrentCell = grid.Rows[grid.RowCount - 1].Cells[ServerItem.Name];
            grid_SelectionChanged(sender, e);
          }
        }
      }
    }

    private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (grid.Columns[e.ColumnIndex].Name == Index.Name)
        MessageBoxUtils.ShowInformation(ResourcesLib.StrUtilizeBotoesSetaAlterarOrdem);
    }

  }
}
