namespace WSPack.VisualStudio.Shared.Forms
{
  partial class MergeHelperForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MergeHelperForm));
      this.gbxBranchOrigem = new System.Windows.Forms.GroupBox();
      this.cbBranchOrigem = new System.Windows.Forms.ComboBox();
      this.btnDetalhesChangeset = new System.Windows.Forms.Button();
      this.edtChangeset = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.edtWorkspace = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.gbxBranchDestino = new System.Windows.Forms.GroupBox();
      this.lbCarregandoLista = new System.Windows.Forms.Label();
      this.cbBranchDestino = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.cbxNaoMostrarTelaAposCheckIn = new System.Windows.Forms.CheckBox();
      this.ttProvider = new System.Windows.Forms.ToolTip(this.components);
      this.gbxBranchOrigem.SuspendLayout();
      this.gbxBranchDestino.SuspendLayout();
      this.SuspendLayout();
      // 
      // gbxBranchOrigem
      // 
      this.gbxBranchOrigem.Controls.Add(this.cbBranchOrigem);
      this.gbxBranchOrigem.Controls.Add(this.btnDetalhesChangeset);
      this.gbxBranchOrigem.Controls.Add(this.edtChangeset);
      this.gbxBranchOrigem.Controls.Add(this.label4);
      this.gbxBranchOrigem.Controls.Add(this.label2);
      resources.ApplyResources(this.gbxBranchOrigem, "gbxBranchOrigem");
      this.gbxBranchOrigem.Name = "gbxBranchOrigem";
      this.gbxBranchOrigem.TabStop = false;
      // 
      // cbBranchOrigem
      // 
      this.cbBranchOrigem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbBranchOrigem.FormattingEnabled = true;
      resources.ApplyResources(this.cbBranchOrigem, "cbBranchOrigem");
      this.cbBranchOrigem.Name = "cbBranchOrigem";
      this.cbBranchOrigem.Sorted = true;
      this.cbBranchOrigem.Leave += new System.EventHandler(this.cbBranchOrigem_Leave);
      // 
      // btnDetalhesChangeset
      // 
      resources.ApplyResources(this.btnDetalhesChangeset, "btnDetalhesChangeset");
      this.btnDetalhesChangeset.Name = "btnDetalhesChangeset";
      this.btnDetalhesChangeset.UseVisualStyleBackColor = true;
      this.btnDetalhesChangeset.Click += new System.EventHandler(this.btnDetalhesChangeset_Click);
      // 
      // edtChangeset
      // 
      resources.ApplyResources(this.edtChangeset, "edtChangeset");
      this.edtChangeset.Name = "edtChangeset";
      this.edtChangeset.ReadOnly = true;
      // 
      // label4
      // 
      resources.ApplyResources(this.label4, "label4");
      this.label4.Name = "label4";
      // 
      // label2
      // 
      resources.ApplyResources(this.label2, "label2");
      this.label2.Name = "label2";
      // 
      // edtWorkspace
      // 
      resources.ApplyResources(this.edtWorkspace, "edtWorkspace");
      this.edtWorkspace.Name = "edtWorkspace";
      this.edtWorkspace.ReadOnly = true;
      // 
      // label1
      // 
      resources.ApplyResources(this.label1, "label1");
      this.label1.Name = "label1";
      // 
      // gbxBranchDestino
      // 
      this.gbxBranchDestino.Controls.Add(this.lbCarregandoLista);
      this.gbxBranchDestino.Controls.Add(this.cbBranchDestino);
      this.gbxBranchDestino.Controls.Add(this.label3);
      resources.ApplyResources(this.gbxBranchDestino, "gbxBranchDestino");
      this.gbxBranchDestino.Name = "gbxBranchDestino";
      this.gbxBranchDestino.TabStop = false;
      // 
      // lbCarregandoLista
      // 
      resources.ApplyResources(this.lbCarregandoLista, "lbCarregandoLista");
      this.lbCarregandoLista.ForeColor = System.Drawing.Color.Blue;
      this.lbCarregandoLista.Name = "lbCarregandoLista";
      // 
      // cbBranchDestino
      // 
      this.cbBranchDestino.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbBranchDestino.FormattingEnabled = true;
      resources.ApplyResources(this.cbBranchDestino, "cbBranchDestino");
      this.cbBranchDestino.Name = "cbBranchDestino";
      this.cbBranchDestino.SelectedIndexChanged += new System.EventHandler(this.cbBranchDestino_SelectedIndexChanged);
      // 
      // label3
      // 
      resources.ApplyResources(this.label3, "label3");
      this.label3.Name = "label3";
      // 
      // btnOK
      // 
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      resources.ApplyResources(this.btnOK, "btnOK");
      this.btnOK.Name = "btnOK";
      this.ttProvider.SetToolTip(this.btnOK, resources.GetString("btnOK.ToolTip"));
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      resources.ApplyResources(this.btnCancel, "btnCancel");
      this.btnCancel.Name = "btnCancel";
      this.ttProvider.SetToolTip(this.btnCancel, resources.GetString("btnCancel.ToolTip"));
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // cbxNaoMostrarTelaAposCheckIn
      // 
      resources.ApplyResources(this.cbxNaoMostrarTelaAposCheckIn, "cbxNaoMostrarTelaAposCheckIn");
      this.cbxNaoMostrarTelaAposCheckIn.Name = "cbxNaoMostrarTelaAposCheckIn";
      this.cbxNaoMostrarTelaAposCheckIn.UseVisualStyleBackColor = true;
      this.cbxNaoMostrarTelaAposCheckIn.CheckedChanged += new System.EventHandler(this.cbxNaoMostrarTelaAposCheckIn_CheckedChanged);
      // 
      // MergeHelperForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.cbxNaoMostrarTelaAposCheckIn);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.gbxBranchDestino);
      this.Controls.Add(this.gbxBranchOrigem);
      this.Controls.Add(this.edtWorkspace);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "MergeHelperForm";
      this.Load += new System.EventHandler(this.MergeHelperForm_Load);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MergeHelperForm_KeyDown);
      this.gbxBranchOrigem.ResumeLayout(false);
      this.gbxBranchOrigem.PerformLayout();
      this.gbxBranchDestino.ResumeLayout(false);
      this.gbxBranchDestino.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox gbxBranchOrigem;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox edtWorkspace;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.GroupBox gbxBranchDestino;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox edtChangeset;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button btnDetalhesChangeset;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.ToolTip ttProvider;
    private System.Windows.Forms.ComboBox cbBranchDestino;
    private System.Windows.Forms.CheckBox cbxNaoMostrarTelaAposCheckIn;
    private System.Windows.Forms.ComboBox cbBranchOrigem;
    private System.Windows.Forms.Label lbCarregandoLista;
  }
}