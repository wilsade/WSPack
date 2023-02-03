using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.TeamFoundation;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;
using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Items;
using WSPack.Lib.Properties;
using WSPack.Lib.Forms;
using WSPack.VisualStudio.Shared.Extensions;
using WSPack.VisualStudio.Shared.Commands;

namespace WSPack.VisualStudio.Shared.Forms
{
  /// <summary>
  /// Form para busca de changesets
  /// </summary>
  public partial class SearchChangesetsForm : Form
  {
    class BuscaChangestsConstantes
    {
      internal static readonly string ChangesetId = "ChangesetId";
      internal static readonly string Committer = "Owner";
      internal static readonly string CreationDate = "CreationDate";
      internal static readonly string Comment = "Comment";
      internal static readonly string CheckinNote = "Check In Note";
    }

    TeamProject _projetoAtual;
    readonly SenderTypes _senderType;
    private readonly string _searchChangesetsConfigPath;

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="SearchChangesetsForm"/>
    /// </summary>
    public SearchChangesetsForm(SenderTypes senderType, string searchChangesetsConfigPath)
    {
      InitializeComponent();
      _senderType = senderType;
      _searchChangesetsConfigPath = searchChangesetsConfigPath;
    }
    #endregion

    /// <summary>
    /// (Gets or sets) Nº do Changeset
    /// </summary>
    public int ChangesetId { get; set; }
    /// <summary>
    /// (Gets or sets) Caminho local da Branch de origem
    /// </summary>
    public string BranchOrigem { get; set; }
    /// <summary>
    /// (Gets or sets) Caminho local da Branch de destino
    /// </summary>
    public string BranchDestino { get; set; }
    /// <summary>
    /// (Gets or sets) Caminho server da Branch de destino
    /// </summary>
    public string BranchDestinoServer { get; set; }

    /// <summary>
    /// (Gets) TeamFoundationServer conectado
    /// </summary>
    TeamFoundationServerExt TFSExt
    {
      get
      {
        if (_tfsExt == null)
          _tfsExt = WSPackPackage.Dte.GetTeamFoundationServerExt();
        return _tfsExt;
      }
    }
    TeamFoundationServerExt _tfsExt;

    /// <summary>
    /// (Gets) Controle de fonte do TFS
    /// </summary>
    VersionControlServer VCServer
    {
      get
      {
        if (_vcServer == null)
          _vcServer = TFSExt.GetVersionControlServer();
        return _vcServer;
      }
    }
    VersionControlServer _vcServer;

    /// <summary>
    /// (Gets) Source Control Explorer aberto na IDE do VS
    /// </summary>
    VersionControlExt VCExt
    {
      get
      {
        if (_vcExt == null)
          _vcExt = WSPackPackage.Dte.GetVersionControlExt();
        return _vcExt;
      }
    }
    VersionControlExt _vcExt;

    /// <summary>
    /// (Gets) Projetos registrados no TFS
    /// </summary>
    TeamProject[] TFSProjetcts
    {
      get
      {
        if (_tfsProjects == null)
        {
          _tfsProjects = VCServer.GetAllTeamProjects(false);
        }
        return _tfsProjects;
      }
    }
    TeamProject[] _tfsProjects;

    string TFSProjectsToString()
    {
      int i = 1;
      StringBuilder str = new StringBuilder();
      foreach (TeamProject esteProjeto in TFSProjetcts)
      {
        str.AppendFormat("{0}: ", i++).AppendLine(esteProjeto.ServerItem);
      }
      return str.ToString();
    }

    /// <summary>
    /// Carregar Workspace
    /// </summary>
    void CarregarWorkspace()
    {
      cbLocalProcura.Items.Clear();

      // Carregar mapeamento do Workspace selecionado
      if (VCExt != null && VCExt.Explorer != null && VCExt.Explorer.Workspace != null)
      {
        foreach (var esteFolder in VCExt.Explorer.Workspace.Folders)
        {
          if (cbLocalProcura.Items.IndexOf(esteFolder.ServerItem) == -1)
            cbLocalProcura.Items.Add(esteFolder.ServerItem);
        }
      }

      else
      {
        // Carregar todos os mapeamentos
        Workspace[] lstWorkspace = VCServer.QueryWorkspaces(null, VCServer.AuthorizedUser, Environment.MachineName);
        foreach (Workspace esteWs in lstWorkspace)
        {
          foreach (var esteFolder in esteWs.Folders)
          {
            if (cbLocalProcura.Items.IndexOf(esteFolder.ServerItem) == -1)
              cbLocalProcura.Items.Add(esteFolder.ServerItem);
          }
        }
      }

      // Carregar o último item procurado
      if (!string.IsNullOrEmpty(cbLocalProcura.Text))
      {
        try
        {
          if (VCServer.HasWorkspaceForServerItem(cbLocalProcura.Text))
          {
            int i = cbLocalProcura.Items.IndexOf(cbLocalProcura.Text);
            if (i == -1)
            {
              cbLocalProcura.Items.Insert(0, cbLocalProcura.Text);
              cbLocalProcura.SelectedIndex = 0;
            }
            else
              cbLocalProcura.SelectedIndex = i;
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.GetCompleteMessage(true), ResourcesLib.StrErro, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }

      // Carregar o item selecionado no Source Control Explorer
      // Verificar se estamos no: $/
      if (_senderType == SenderTypes.SourceControlExplorer && VCExt.GetSelectedItem() != null && VCExt.GetSelectedItem().SourceServerPath != null &&
        VCExt.GetSelectedItem().SourceServerPath.Length > 2)
      {
        string serverPath = VCExt.GetSelectedItem().SourceServerPath;
        int i = cbLocalProcura.Items.IndexOf(serverPath);
        if (i == -1)
        {
          cbLocalProcura.Items.Insert(0, serverPath);
          cbLocalProcura.SelectedIndex = 0;
        }

        // O item já existe. Vamos selecioná-lo
        else
        {
          cbLocalProcura.SelectedIndex = i;
        }
      }

      if (cbLocalProcura.Items.Count > 0 && cbLocalProcura.SelectedIndex == -1)
        cbLocalProcura.SelectedIndex = 0;
    }

    private void btnFechar_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void SalvarParametros()
    {
      if (bindingSource1.DataSource is SearchChangesetsParams changesetsParams)
      {
        if (cbxChangesetEspecifico.Checked)
          changesetsParams.LastChangeset = (int)edtChangesetId.Value;
        else
          changesetsParams.LastChangeset = null;
        XmlUtils.SaveXMLParams(changesetsParams, _searchChangesetsConfigPath);
      }
    }

    private void btnPesquisar_Click(object sender, EventArgs e)
    {
      if (!ValidouParametrosPesquisa())
        return;

      btnPesquisar.Enabled = false;
      Cursor = Cursors.WaitCursor;
      try
      {
        Pesquisar();
        SalvarParametros();
      }
      catch (ItemNotMappedException mappEx)
      {
        MessageBoxUtils.ShowError(ResourcesLib.StrNaoExisteMapeamentoNesteLocal, cbLocalProcura.Text, mappEx.Message);
      }
      catch (IdentityNotFoundException idEx)
      {
        string detalhes = idEx.Message;
        if (idEx.InnerException != null)
          detalhes = idEx.InnerException.Message;
        MessageBoxUtils.ShowError(ResourcesLib.StrUsuarioInvalidoTFS, edtUsuario.Text, detalhes);
      }
      catch (Exception ex)
      {
        MessageBoxUtils.ShowError(ex.Message);
      }
      finally
      {
        Cursor = Cursors.Default;
        btnPesquisar.Enabled = true;
      }

    }

    private void Pesquisar()
    {
      string usuario = cbxFiltrarUsuario.Checked ? edtUsuario.Text : null;
      string comentario = cbxFiltrarComentario.Checked ? edtComentario.Text : string.Empty;

      #region Filtrar Datas
      DateTime? dtInicio = null, dtTermino = null;
      if (cbxFiltrarData.Checked)
      {
        dtInicio = edtDataInicio.Value;
        dtTermino = edtDataFim.Value;
      }
      #endregion

      List<CheckInNotesItem> lstFiltroNotas = new List<CheckInNotesItem>();
      #region Filtrar Checkin Notes
      if (cbxFiltrarCheckInNotes.Enabled && cbxFiltrarCheckInNotes.Checked)
      {
        GridNotasToList(lstFiltroNotas);
      }
      #endregion

      #region Filtrar arquivos
      List<string> lstArquivos = new List<string>();
      if (cbxFiltrarArquivos.Checked)
      {
        string[] lstArq = edtArquivos.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string s in lstArq)
        {
          lstArquivos.Add(s.Trim());
        }
      }
      #endregion

      // Faz a procura no TFS
      List<Changeset> lst;
      if (!cbxChangesetEspecifico.Checked)
        lst = VCServer.SearchChangesets(cbLocalProcura.Text, false, 0,
          usuario, comentario, dtInicio, dtTermino, lstFiltroNotas, lstArquivos);
      else
      {
        var cs = VCServer.ChangeSetDetails(Convert.ToInt32(edtChangesetId.Value));
        lst = new List<Changeset>();
        if (cs != null)
          lst.Add(cs);
      }

      gridChangesets.DataSource = lst;
      AjustarGrid();

      btnChangesetDetails.Enabled = btnMerge.Enabled = gridChangesets.Rows.Count > 0;

      lbTotalRegistros.Text = string.Format(ResourcesLib.StrTotalRegistros, gridChangesets.Rows.Count);
      if (gridChangesets.Rows.Count > 0)
        gridChangesets.CurrentCell = gridChangesets.Rows[0].Cells[BuscaChangestsConstantes.ChangesetId];

      tabControl1.SelectedTab = pageResultado;
    }

    private void GridNotasToList(List<CheckInNotesItem> lstFiltroNotas)
    {
      CheckInNotesItem filtraNota;
      foreach (DataGridViewRow estaRow in gridNotas.Rows)
      {
        if (Convert.ToBoolean(estaRow.Cells[colCheck.Name].Value))
        {
          filtraNota = new CheckInNotesItem
          {
            CheckInNoteName = Convert.ToString(estaRow.Cells[colCheckinNote.Name].Value),
            Operador = (OperatorTypes)Enum.Parse(typeof(OperatorTypes), estaRow.Cells[colOperador.Name].Value.ToString()),
            CheckInNoteValue = Convert.ToString(estaRow.Cells[colValor.Name].Value)
          };
          lstFiltroNotas.Add(filtraNota);
        }
      }
    }

    /// <summary>
    /// Ajustar as colunas da grid após a procura de changeSet
    /// </summary>
    private void AjustarGrid()
    {
      foreach (DataGridViewColumn estaColuna in gridChangesets.Columns)
      {
        if (estaColuna.Name != BuscaChangestsConstantes.ChangesetId &&
          estaColuna.Name != BuscaChangestsConstantes.Committer &&
          estaColuna.Name != BuscaChangestsConstantes.CreationDate &&
          estaColuna.Name != BuscaChangestsConstantes.Comment &&
          estaColuna.Name != BuscaChangestsConstantes.CheckinNote)
          estaColuna.Visible = false;
      }

      gridChangesets.Columns[BuscaChangestsConstantes.ChangesetId].DisplayIndex = 1;
      gridChangesets.Columns[BuscaChangestsConstantes.ChangesetId].HeaderText = ResourcesLib.StrChangesetId;

      gridChangesets.Columns[BuscaChangestsConstantes.CreationDate].DisplayIndex = 2;
      gridChangesets.Columns[BuscaChangestsConstantes.CreationDate].HeaderText = ResourcesLib.StrDataCheckIn;
      gridChangesets.Columns[BuscaChangestsConstantes.CreationDate].Width = 130;

      gridChangesets.Columns[BuscaChangestsConstantes.Committer].DisplayIndex = 3;
      gridChangesets.Columns[BuscaChangestsConstantes.Committer].HeaderText = ResourcesLib.StrUsuario;

      gridChangesets.Columns[BuscaChangestsConstantes.Comment].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
      gridChangesets.Columns[BuscaChangestsConstantes.Comment].HeaderText = ResourcesLib.StrComentario;
      gridChangesets.Columns[BuscaChangestsConstantes.Comment].Width = 600;
    }

    private bool ValidouParametrosPesquisa()
    {
      StringBuilder str = new StringBuilder();

      if (!cbxChangesetEspecifico.Checked)
      {
        // Local da procura
        if (string.IsNullOrEmpty(cbLocalProcura.Text))
          str.AppendLine(ResourcesLib.StrInformarLocalProcura);

        // Local da procura representa projeto válido
        _projetoAtual = TFSProjetcts.Where(x => cbLocalProcura.Text.ContainsInsensitive(x.Name)).FirstOrDefault();
        if (_projetoAtual == null)
        {
          MessageBoxUtils.ShowWarning(ResourcesLib.StrLocalProcuraNaoRepresentaProjetoValido, TFSProjectsToString());
          cbxFiltrarCheckInNotes.Checked = false;
          return false;
        }

        // Data
        if (cbxFiltrarData.Checked)
        {
          if (edtDataInicio.Value.Date > edtDataFim.Value.Date)
            str.AppendLine(ResourcesLib.StrDataFinalMaiorIgualInicial);
        }

        // Usuario
        if (cbxFiltrarUsuario.Checked)
        {
          if (string.IsNullOrEmpty(edtUsuario.Text))
            str.AppendLine(ResourcesLib.StrInformarUsuario);
        }

        // Comentário
        if (cbxFiltrarComentario.Checked)
        {
          if (string.IsNullOrEmpty(edtComentario.Text))
            str.AppendLine(ResourcesLib.StrInformarComentario);
        }

        // Arquivos
        if (cbxFiltrarArquivos.Checked)
        {
          if (string.IsNullOrEmpty(edtArquivos.Text))
            str.AppendLine(ResourcesLib.StrInformarArquivos);
        }

        // Check In Notes
        if (cbxFiltrarCheckInNotes.Enabled && cbxFiltrarCheckInNotes.Checked)
        {
          foreach (DataGridViewRow estaRow in gridNotas.Rows)
          {
            if (Convert.ToBoolean(estaRow.Cells[colCheck.Name].Value) &&
              string.IsNullOrEmpty(Convert.ToString(estaRow.Cells[colValor.Name].Value)))
            {
              OperatorTypes operador = (OperatorTypes)Enum.Parse(typeof(OperatorTypes), estaRow.Cells[colOperador.Name].Value.ToString());
              if (operador != OperatorTypes.IsNotNull && operador != OperatorTypes.IsNull)
                str.AppendLine(string.Format(ResourcesLib.StrCheckNoteNaoInformado, Convert.ToString(estaRow.Cells[colCheckinNote.Name].Value)));
            }
          }
        }
      }

      if (str.Length > 0)
        MessageBoxUtils.ShowWarning(str.ToString());

      return str.Length == 0;
    }

    private void cbxFiltrarData_CheckedChanged(object sender, EventArgs e)
    {
      edtDataInicio.Enabled = edtDataFim.Enabled = cbxFiltrarData.Checked;
      if (cbxFiltrarData.Checked)
        edtDataInicio.Focus();
    }

    private void cbxFiltrarUsuario_CheckedChanged(object sender, EventArgs e)
    {
      edtUsuario.Enabled = cbxFiltrarUsuario.Checked;
      if (cbxFiltrarUsuario.Checked)
        edtUsuario.Focus();
    }

    private void cbxFiltrarComentario_CheckedChanged(object sender, EventArgs e)
    {
      edtComentario.Enabled = cbxFiltrarComentario.Checked;
      if (cbxFiltrarComentario.Checked)
        edtComentario.Focus();
    }

    private void cbxFiltrarArquivos_CheckedChanged(object sender, EventArgs e)
    {
      edtArquivos.Enabled = cbxFiltrarArquivos.Checked;
      lbAvisoFiltroArquivoPerformance.Visible = cbxFiltrarArquivos.Checked;
      if (cbxFiltrarArquivos.Checked)
        edtArquivos.Focus();
    }

    private void gridNotas_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      // Coluna: Filtrar
      if (e.ColumnIndex == gridNotas.Columns.IndexOf(colCheck) && e.RowIndex >= 0)
      {
        DataGridViewCell celulaFiltrar = gridNotas.Rows[e.RowIndex].Cells[e.ColumnIndex];
        bool habilitarDigitarValor = Convert.ToBoolean(celulaFiltrar.Value);
        gridNotas.Rows[e.RowIndex].Cells[colValor.Name].ReadOnly =
          gridNotas.Rows[e.RowIndex].Cells[colOperador.Name].ReadOnly = !habilitarDigitarValor;
      }

      // Coluna: Operador
      else if (e.ColumnIndex == gridNotas.Columns.IndexOf(colOperador) && e.RowIndex >= 0)
      {
        DataGridViewCell celulaOperador = gridNotas.Rows[e.RowIndex].Cells[e.ColumnIndex];
        OperatorTypes operadorSelecionado = (OperatorTypes)Enum.Parse(typeof(OperatorTypes), celulaOperador.Value.ToString());
        gridNotas.Rows[e.RowIndex].Cells[colValor.Name].ReadOnly = operadorSelecionado == OperatorTypes.IsNotNull ||
          operadorSelecionado == OperatorTypes.IsNull;
      }
    }

    private void cbxFiltrarCheckInNotes_CheckedChanged(object sender, EventArgs e)
    {
      gridNotas.Enabled = cbxFiltrarCheckInNotes.Checked;
      if (cbxFiltrarCheckInNotes.Checked)
        PreencherCheckInNotes();
    }

    private void PreencherCheckInNotes()
    {
      gridNotas.Rows.Clear();

      try
      {
        if (string.IsNullOrEmpty(cbLocalProcura.Text))
        {
          MessageBoxUtils.ShowWarning(ResourcesLib.StrInformarLocalProcura);
          cbxFiltrarCheckInNotes.Checked = false;
          return;
        }

        _projetoAtual = TFSProjetcts.Where(x => cbLocalProcura.Text.ContainsInsensitive(x.ServerItem)).FirstOrDefault();
        if (_projetoAtual == null)
        {
          MessageBoxUtils.ShowWarning(ResourcesLib.StrLocalProcuraNaoRepresentaProjetoValido, TFSProjectsToString());
          cbxFiltrarCheckInNotes.Checked = false;
          return;
        }

        IEnumerable<string> notas = VCServer.GetCheckInNotes(_projetoAtual.ServerItem);
        foreach (string estaNota in notas)
        {
          int i = gridNotas.Rows.Add(false, estaNota, OperatorTypes.Equals.ToString(), string.Empty);
          gridNotas.Rows[i].Cells[colValor.Name].ReadOnly = gridNotas.Rows[i].Cells[colOperador.Name].ReadOnly = true;
        }
      }
      catch (Exception ex)
      {
        MessageBoxUtils.ShowError(ResourcesLib.StrNaoFoiPossivelRecuperarNotas, ex.Message);
        gridNotas.Enabled = false;
      }
    }

    private void cbxChangesetEspecifico_CheckedChanged(object sender, EventArgs e)
    {
      pnlOpcoesPesquisa.Visible = !cbxChangesetEspecifico.Checked;
      edtChangesetId.Enabled = cbxChangesetEspecifico.Checked;
      btnChangesetEspecifico.Visible = cbxChangesetEspecifico.Checked;
      if (cbxChangesetEspecifico.Checked)
      {
        edtChangesetId.Focus();
        edtChangesetId.Select(0, edtChangesetId.Value.ToString().Length);
      }
    }

    private void cbLocalProcura_TextChanged(object sender, EventArgs e)
    {
      TeamProject achouProjeto = TFSProjetcts.Where(x => cbLocalProcura.Text.ContainsInsensitive(x.Name)).FirstOrDefault();
      if (achouProjeto != _projetoAtual)
      {
        gridNotas.Rows.Clear();
        cbxFiltrarCheckInNotes.Checked = false;
      }
      _projetoAtual = achouProjeto;

      btnPesquisar.Enabled = !string.IsNullOrEmpty(cbLocalProcura.Text);
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
      btnChangesetDetails.Visible = btnMerge.Visible = tabControl1.SelectedTab == pageResultado &&
        gridChangesets.Rows.Count > 0;
      btnVoltarPesquisa.Visible = tabControl1.SelectedTab == pageResultado;
    }

    private void ChangesetDetails(int changeset)
    {
      var form = new ProcessForm();
      Hide();
      form.Show();
      try
      {
        VCExt.ViewChangesetDetails(changeset);
      }
      catch (Exception ex)
      {
        MessageBoxUtils.ShowError(ResourcesLib.StrNaoFoiPossivelRecuperarChangeset, ex.Message);
      }
      finally
      {
        form.Self.PodeSair = true;
        form.Self.Close();
        Show();
      }
    }

    private void gridChangesets_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
      {
        int changeset = Convert.ToInt32(gridChangesets.Rows[e.RowIndex].Cells[BuscaChangestsConstantes.ChangesetId].Value);
        ChangesetDetails(changeset);
      }
    }

    private void menuItemChangesetDetails_Click(object sender, EventArgs e)
    {
      if (gridChangesets.CurrentRow != null)
      {
        int changeset = Convert.ToInt32(gridChangesets.CurrentRow.Cells[BuscaChangestsConstantes.ChangesetId].Value);
        ChangesetDetails(changeset);
      }
    }

    private void menuItemMerge_Click(object sender, EventArgs e)
    {
      if (gridChangesets.CurrentRow != null)
      {
        btnMerge_Click(sender, e);
      }
    }

    private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
    {
      e.Cancel = gridChangesets.Rows.Count == 0;
    }

    private void btnMerge_Click(object sender, EventArgs e)
    {
      if (gridChangesets.CurrentRow != null)
      {
        if (gridChangesets.CurrentRow.DataBoundItem is Changeset changeSet)
        {
          if (changeSet.Changes.Length == 0)
          {
            Changeset aux = VCServer.ChangeSetDetails(changeSet.ChangesetId);
            if (aux != null)
              changeSet.Changes = aux.Changes;
          }
          if (changeSet.Changes.Length == 0)
            return;

          using (var form = new MergeHelperForm(changeSet.ChangesetId, !WSPackPackage.ParametrosGerais.AbrirTelaMerge))
          {
            if (form.ShowDialog() == DialogResult.OK)
            {
              ChangesetId = changeSet.ChangesetId;
              BranchOrigem = form.BranchOrigemLocalPath;
              BranchDestino = form.BranchDestinoLocalPath;
              BranchDestinoServer = form.BranchDestinoServerPath;
              WSPackPackage.ParametrosGerais.AbrirTelaMerge = !form.NaoMostrarTelaMerge;
              WSPackPackage.ParametrosGerais.SaveSettingsToStorage();
              Close();

              #region Fazer o merge

              _ = Task.Run(() =>
                {
                  RegistroVisualStudioObj.Instance.Merge(ChangesetId, BranchOrigem, BranchDestino,
                  (string erro) =>
                  {
                    Utils.LogOutputMessageSwitchToMainThread(erro);
                  });
                });

              #endregion

            }
            WSPackPackage.ParametrosGerais.AbrirTelaMerge = !form.NaoMostrarTelaMerge;
            WSPackPackage.ParametrosGerais.SaveSettingsToStorage();
          }
        }
      }
    }

    private void SearchChangesetsForm_Load(object sender, EventArgs e)
    {
      menuItemExportarResultado.Image = ResourcesLib.icoExportacao.ToBitmap();
      edtServidor.Text = VCServer.TeamProjectCollection.Uri.AbsoluteUri;

      edtChangesetId.Maximum = int.MaxValue;
      edtChangesetId.Minimum = 1;
      edtDataInicio.Value = DateTime.Now.Subtract(new TimeSpan(60, 0, 0, 0));
      edtDataFim.Value = DateTime.Now;

      SearchChangesetsParams searchChangesetsParams = XmlUtils.ReadXMLParams<SearchChangesetsParams>(_searchChangesetsConfigPath);
      bindingSource1.DataSource = searchChangesetsParams;

      _projetoAtual = null;

      CarregarWorkspace();
      var listaItens = typeof(OperatorTypes).GetFields().Select(x => x.Name).ToList();
      listaItens.ForEach(x => colOperador.Items.Add(x));
      colOperador.Items.RemoveAt(0);
    }

    private void SearchChangesetsForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (!cbLocalProcura.DroppedDown)
      {
        if (e.KeyCode == Keys.Escape)
          btnFechar_Click(sender, e);

        else if (e.KeyCode == Keys.Enter)
        {
          e.SuppressKeyPress = true;
          btnPesquisar_Click(sender, e);
        }
      }
    }

    private void SearchChangesetsForm_Shown(object sender, EventArgs e)
    {
      if (_senderType == SenderTypes.MenuWSPack)
      {
        if (bindingSource1.DataSource is SearchChangesetsParams searchChangesetsParams && searchChangesetsParams.LastChangeset.HasValue)
        {
          cbxChangesetEspecifico.Checked = true;
          edtChangesetId.Value = searchChangesetsParams.LastChangeset.Value;
          edtChangesetId.Select(0, edtChangesetId.Value.ToString().Length);
          edtChangesetId.Focus();
        }
        else
          cbLocalProcura.Focus();
      }
      else
        cbLocalProcura.Focus();
    }

    private void btnChangesetEspecifico_Click(object sender, EventArgs e)
    {
      btnPesquisar_Click(sender, e);
    }

    private void btnVoltarPesquisa_Click(object sender, EventArgs e)
    {
      tabControl1.SelectedTab = pageParametros;
      if (cbLocalProcura.CanFocus)
        cbLocalProcura.Focus();
    }

    private void btnChangesetDetails_Click(object sender, EventArgs e)
    {
      menuItemChangesetDetails_Click(sender, e);
    }

    private void menuItemGerarTemplateCheckIn_Click(object sender, EventArgs e)
    {
      if (gridChangesets.CurrentRow != null)
      {
        if (string.IsNullOrWhiteSpace(WSPackPackage.ParametrosTemplateCheckIn.TemplateCheckIn))
        {
          if (MessageBoxUtils.ShowConfirmationYesNo(ResourcesLib.StrAviso + Environment.NewLine + Environment.NewLine +
            ResourcesLib.StrTemplateCheckInNaoDefinido + Environment.NewLine +
            ResourcesLib.StrDesejaDefinirTemplateAgora))
          {
            WSPackPackage.Dte.ExecuteCommand("WSPack.TemplateCheckin");
          }
        }

        if (!string.IsNullOrWhiteSpace(WSPackPackage.ParametrosTemplateCheckIn.TemplateCheckIn))
        {
          Changeset changeSet = gridChangesets.CurrentRow.DataBoundItem as Changeset;
          if (changeSet.Changes.Length == 0)
          {
            Changeset aux = VCServer.ChangeSetDetails(changeSet.ChangesetId);
            if (aux != null)
              changeSet.Changes = aux.Changes;
          }
          string templateGerado = TemplateCheckInCommand.GerarTemplateCheckIn(WSPackPackage.ParametrosTemplateCheckIn.TemplateCheckIn, changeSet);
          CopyLocalPathBaseCommand.CopyToClipboard(templateGerado);
          MessageBoxUtils.ShowInformation(ResourcesLib.StrCopiadoAreaTransferencia);
        }
      }
    }

    private void menuItemExportarResultado_Click(object sender, EventArgs e)
    {
      Cursor = Cursors.WaitCursor;
      try
      {
        using (SaveFileDialog dlg = new SaveFileDialog())
        {
          dlg.DefaultExt = ".xls";
          dlg.Filter = "Tabela|*.html|Tabela|*.xls";
          if (dlg.ShowDialog() == DialogResult.OK)
          {
            string conteudo = gridChangesets.ToHtml();
            conteudo = FormatarHtmlIncluirCabecalho(conteudo);

            //conteudo =
            //  " <span style=\"font-family: 'courier new', courier;\"> " +
            //  (treeMembros.SelectedNode as NodoBase).NodeDescription.Replace(Environment.NewLine, "<br>") + "</span><br><br>" +
            //  "Data/Hora: " + DateTime.Now.ToString() + "<br>" +
            //  conteudo;

            //if (File.Exists(dlg.FileName))
            //  File.Delete(dlg.FileName);
            using (StreamWriter writer = new StreamWriter(dlg.FileName, false, Encoding.Default) { AutoFlush = true })
            {
              writer.WriteLine(conteudo);
            }
            System.Diagnostics.Process.Start("explorer.exe", "/select," + dlg.FileName);
          }
        }
      }
      finally
      {
        Cursor = Cursors.Default;
      }

    }

    private string FormatarHtmlIncluirCabecalho(string html)
    {
      string patterCabecalho = " <span style=\"font-family: 'courier new', courier;\"> " +
        "{0}" + "</span><br><br>";

      StringBuilder str = new StringBuilder();

      str.Append("<b><u>Parâmetros utilizados na pesquisa</u></b><br><br>");

      // Changeset específico
      if (cbxChangesetEspecifico.Checked)
        str.Append("Changeset: ").Append(edtChangesetId.Value).Append("<br>");
      else
      {
        // Local da procura
        str.Append("<b>Local da procura:</b> ").Append(cbLocalProcura.Text).Append("<br>");

        // Data
        if (cbxFiltrarData.Checked)
          str.Append("<b>Data/hora.......:</b> \"").Append(edtDataInicio.Value).Append("\" até \"").Append(edtDataFim.Value).Append("\"<br>");

        // Usuário
        if (cbxFiltrarUsuario.Checked)
          str.Append("<b>Usuário.........:</b> ").Append(edtUsuario.Text).Append("<br>");

        // Comentário
        if (cbxFiltrarComentario.Checked)
          str.Append("<b>Comentário......:</b> ").Append(edtComentario.Text).Append("<br>");

        // Arquivos
        if (cbxFiltrarArquivos.Checked)
          str.Append("<b>Arquivos........:</b> ").Append(edtArquivos.Text).Append("<br>");

        // Check In Notes
        if (cbxFiltrarCheckInNotes.Enabled && cbxFiltrarCheckInNotes.Checked)
        {
          List<CheckInNotesItem> lstFiltroNotas = new List<CheckInNotesItem>();
          GridNotasToList(lstFiltroNotas);
          if (lstFiltroNotas.Count > 0)
          {
            str.Append("<b>Check In Notes..:</b>").Append("<br>");
            lstFiltroNotas.ToList().ForEach(nota =>
              {
                str.Append("&nbsp&nbsp<b>").Append(nota.CheckInNoteName).Append(":</b> ")
                  .Append(nota.Operador).Append(" ")
                  .Append(nota.CheckInNoteValue).Append("<br>");
              });
          }
        }
      }

      str.Append("<b><br>Relatório gerado em:</b> ").Append(DateTime.Now.ToString());

      html = string.Format(patterCabecalho, str.ToString()) + html;
      return html;
    }

    private void gridChangesets_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      if (gridChangesets.Columns[BuscaChangestsConstantes.CheckinNote] == null)
      {
        int i = gridChangesets.Columns.Add(BuscaChangestsConstantes.CheckinNote, BuscaChangestsConstantes.CheckinNote);
        gridChangesets.Columns[i].DisplayIndex = 0;
        gridChangesets.Columns[i].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        gridChangesets.Columns[i].Width = 300;
      }
      bool mostrarColunaCheckInNotes = false;
      foreach (DataGridViewRow esteRow in gridChangesets.Rows)
      {
        Changeset cs = esteRow.DataBoundItem as Changeset;
        if (cs?.CheckinNote != null && cs.CheckinNote.Values != null && cs.CheckinNote.Values.Length > 0)
        {
          mostrarColunaCheckInNotes = true;
          StringBuilder strNotas = new StringBuilder();
          cs.CheckinNote.Values.Where(nota => !string.IsNullOrEmpty(nota.Value)).ToList().
            ForEach(z =>
            {
              strNotas.Append("- ").Append(z.Name).Append(": ").AppendLine(z.Value).AppendLine("");
            });
          if (strNotas.Length >= 2)
            strNotas.Length -= 2;
          esteRow.Cells[BuscaChangestsConstantes.CheckinNote].Value = strNotas.ToString();
        }
        else
          esteRow.Cells[BuscaChangestsConstantes.CheckinNote].Value = string.Empty;
      }
      gridChangesets.Columns[BuscaChangestsConstantes.CheckinNote].Visible = mostrarColunaCheckInNotes;
    }

    private void btnChooseItem_Click(object sender, EventArgs e)
    {
      string local = cbLocalProcura.Text;
      try
      {
        if (!string.IsNullOrEmpty(local))
        {
          var item = VCServer.TryGetItem(cbLocalProcura.Text);
          if (item == null)
            throw new Exception(string.Format(ResourcesLib.StrInformarLocalProcura, cbLocalProcura.Text));
        }
      }
      catch (Exception)
      {
        local = "$/";
      }

      try
      {
        Assembly controlsAssembly = Assembly.Load(@"Microsoft.TeamFoundation.VersionControl.Controls");
        Type vcChooseItemDialogType = controlsAssembly.GetType("Microsoft.TeamFoundation.VersionControl.Controls.DialogChooseItem");

        ConstructorInfo ci = vcChooseItemDialogType.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new Type[] { typeof(VersionControlServer), typeof(string), typeof(string) },
                null);

        Form _chooseItemDialog = null;
        PropertyInfo _selectItemProperty = null;
        DialogResult dialogResult;

        _chooseItemDialog = (Form)ci.Invoke(new object[] { VCServer, string.IsNullOrEmpty(local) ||
          local.Length <= 2 ? "$/" : Path .GetDirectoryName(local), local });

        _chooseItemDialog.StartPosition = FormStartPosition.CenterParent;
        _selectItemProperty = vcChooseItemDialogType.GetProperty("SelectedItem", BindingFlags.Instance | BindingFlags.NonPublic);

        _chooseItemDialog.StartPosition = FormStartPosition.CenterScreen;
        if (_chooseItemDialog.ShowDialog(this) == DialogResult.OK)
        {
          dialogResult = _chooseItemDialog.DialogResult;
          Item selectedItem = (Item)_selectItemProperty.GetValue(_chooseItemDialog, null);
          string itemEscolhido = selectedItem.ServerItem;
          cbLocalProcura.Text = itemEscolhido;
        }
      }
      catch (Exception ex)
      {
        MessageBoxUtils.ShowError(ex.Message);
      }

    }

    private void gridNotas_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      System.Diagnostics.Trace.WriteLine(e.Exception.Message);
    }

  }
}
