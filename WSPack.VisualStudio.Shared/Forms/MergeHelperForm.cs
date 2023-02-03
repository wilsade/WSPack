using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Controls;
using Microsoft.VisualStudio.TeamFoundation;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;
using WSPack.Lib;
using WSPack.Lib.Properties;
using WSPack.Lib.Forms;
using WSPack.VisualStudio.Shared.Extensions;

namespace WSPack.VisualStudio.Shared.Forms
{
  /// <summary>
  /// Form para Merge
  /// </summary>
  public partial class MergeHelperForm : Form
  {
    int _changesetId;
    Changeset _cs;
    VersionControlServer _vcServer;
    TeamFoundationServerExt _tfs;
    Workspace _ws;

    #region Construtores
    /// <summary>
    /// Cria uma instância da classe <see cref="MergeHelperForm" /></summary>
    /// <param name="changesetId">Nº do Changeset</param>
    /// <param name="naoExibirTelaAposCheckIn">Nao exibir tela apos check in</param>
    public MergeHelperForm(int changesetId, bool naoExibirTelaAposCheckIn)
    {
      InitializeComponent();
      _changesetId = changesetId;
      CheckExibirTela(naoExibirTelaAposCheckIn);
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="MergeHelperForm" /></summary>
    /// <param name="changeset">Changeset</param>
    /// <param name="naoExibirTelaAposCheckIn">Nao exibir tela apos check in</param>
    public MergeHelperForm(Changeset changeset, bool naoExibirTelaAposCheckIn)
    {
      InitializeComponent();
      _cs = changeset;
      CheckExibirTela(naoExibirTelaAposCheckIn);
    }
    #endregion

    void CheckExibirTela(bool marcado)
    {
      cbxNaoMostrarTelaAposCheckIn.CheckedChanged -= cbxNaoMostrarTelaAposCheckIn_CheckedChanged;
      cbxNaoMostrarTelaAposCheckIn.Checked = marcado;
      cbxNaoMostrarTelaAposCheckIn.CheckedChanged += cbxNaoMostrarTelaAposCheckIn_CheckedChanged;
    }

    /// <summary>
    /// (Gets) Caminho local da branch de origem
    /// </summary>
    public string BranchOrigemLocalPath { get; private set; }

    /// <summary>
    /// (Gets) Caminho local da branch de destino
    /// </summary>
    public string BranchDestinoLocalPath { get; private set; }

    /// <summary>
    /// (Gets) Caminho server da branch de destino
    /// </summary>
    public string BranchDestinoServerPath { get; private set; }

    /// <summary>
    /// (Gets) Um valor indicando se a tela de merge será exibida após o Check In
    /// </summary>
    /// <value>
    /// "true" se "nao mostrar tela merge"; senão, "false".
    /// </value>
    public bool NaoMostrarTelaMerge
    {
      get { return cbxNaoMostrarTelaAposCheckIn.Checked; }
    }

    private void MergeHelperForm_Load(object sender, EventArgs e)
    {
      Icon = ResourcesLib.icoChoose_32;

      _tfs = Utils.GetTeamFoundationServerExt();
      _vcServer = _tfs.GetVersionControlServer();

      CarregarChangeset(_vcServer);
      CarregarBranchOrigem(_cs);
      CarregarWorkspace(_vcServer, _cs);
    }

    /// <summary>
    /// Descobre qual a branch que é comum a uma lista de itens
    /// </summary>
    void CarregarBranchOrigem(Changeset cs)
    {
      cbBranchOrigem.Items.Clear();
      IEnumerable<string> listaServerItens = cs.Changes.Select(x => x.Item.ServerItem);
      foreach (var estePath in listaServerItens)
      {
        string tempPath = "$";
        string[] splited = estePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        if (splited.Length > 1)
        {
          for (int i = 1; i < splited.Length; i++)
          {
            tempPath += "/" + splited[i];
            if (cbBranchOrigem.Items.IndexOf(tempPath) == -1)
              cbBranchOrigem.Items.Add(tempPath);
          }
        }
      }
    }

    private void CarregarWorkspace(VersionControlServer vcServer, Changeset cs)
    {
      if (vcServer.GetWorkspaceForServerItem(cs.Changes[0].Item.ServerItem, out var lstWorkspaceLocalItem))
      {
        (Workspace Ws, string LocalItem) item = Utils.ChooseItem(lstWorkspaceLocalItem);
        _ws = item.Ws;
        edtWorkspace.Text = _ws.Name;
      }
      else
        MessageBoxUtils.ShowWarning(ResourcesLib.StrNaoFoiPossivelRecuperarWorkspace);
    }

    private void CarregarChangeset(VersionControlServer vcServer)
    {
      if (_cs == null)
        _cs = vcServer.ChangeSetDetails(_changesetId);
      else
        _changesetId = _cs.ChangesetId;
      edtChangeset.Text = _changesetId.ToString();
    }

    private void CarregarCandidatos(VersionControlServer vcServer, string branchOrigem)
    {
      lbCarregandoLista.Visible = true;
      try
      {
        cbBranchDestino.Items.Clear();
        var itens = vcServer.QueryMergeRelationships(branchOrigem)
          .Where(x => !x.IsDeleted).OrderBy(y => y.Item);
        foreach (var item in itens)
        {
          cbBranchDestino.Items.Add(item.Item);
        }
        if (cbBranchDestino.Items.Count > 0)
          cbBranchDestino.SelectedIndex = 0;
      }
      finally
      {
        lbCarregandoLista.Visible = false;
      }
    }

    private void cbxNaoMostrarTelaAposCheckIn_CheckedChanged(object sender, EventArgs e)
    {
      if (cbxNaoMostrarTelaAposCheckIn.Checked)
        MessageBoxUtils.ShowInformation(ResourcesLib.StrAvisoTelaMergeNaoSeraMostrado);
    }

    private void btnDetalhesChangeset_Click(object sender, EventArgs e)
    {
      btnDetalhesChangeset.Enabled = false;
      var form = new ProcessForm();
      form.Show();
      try
      {
        ProcessStartInfo pInfo = RegistroVisualStudioObj.Instance.TFProcessStartInfo();

        pInfo.Arguments = string.Format(
          "changeset /collection:{0} {1}", _tfs.ActiveProjectContext.DomainUri, _changesetId);
        pInfo.WorkingDirectory = RegistroVisualStudioObj.Instance.DirectoryFullPath;

        Process p = Process.Start(pInfo);
        p.WaitForExit();

      }
      catch (Exception ex)
      {
        MessageBoxUtils.ShowError(ex.Message);
      }
      finally
      {
        form.Self.PodeSair = true;
        form.Self.Close();
        btnDetalhesChangeset.Enabled = true;
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      BranchOrigemLocalPath = _ws.GetLocalItemForServerItem(cbBranchOrigem.Text);
      BranchDestinoLocalPath = _ws.GetLocalItemForServerItem(cbBranchDestino.Text);
      BranchDestinoServerPath = cbBranchDestino.Text;

      var candidatosMerge = _vcServer.GetMergeCandidates(BranchOrigemLocalPath,
        BranchDestinoLocalPath, RecursionType.Full);

      // Todos os merges já foram feitos na branch origem/destino
      if (candidatosMerge.Length == 0)
      {
        MessageBoxUtils.ShowWarning(ResourcesLib.StrNaoHaCandidatosMergeNesteChangeset,
          _changesetId, BranchOrigemLocalPath, BranchDestinoLocalPath);
        DialogResult = DialogResult.None;
        return;
      }

      var candidato = candidatosMerge.FirstOrDefault(c => c.Changeset.ChangesetId == _changesetId);
      // Este changeset não é um candidato
      if (candidato == null)
      {
        MessageBoxUtils.ShowWarning(ResourcesLib.StrNaoHaCandidatosMergeNesteChangeset,
          _changesetId, BranchOrigemLocalPath, BranchDestinoLocalPath);
        DialogResult = DialogResult.None;
        return;
      }

      DialogResult = DialogResult.OK;
    }

    private void cbBranchDestino_SelectedIndexChanged(object sender, EventArgs e)
    {
      btnOK.Enabled = cbBranchDestino.SelectedIndex >= 0;
    }

    private void cbBranchOrigem_Leave(object sender, EventArgs e)
    {
      btnOK.Enabled = false;
      if (!string.IsNullOrEmpty(cbBranchOrigem.Text))
        CarregarCandidatos(_vcServer, cbBranchOrigem.Text);
    }

    private void MergeHelperForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (!cbBranchOrigem.DroppedDown && !cbBranchDestino.DroppedDown)
      {
        if (e.KeyCode == Keys.Escape)
          DialogResult = DialogResult.Cancel;
      }
    }

  }
}
