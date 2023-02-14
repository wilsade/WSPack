namespace WSPack.VisualStudio.Shared.Forms
{
  partial class GenerateResourceForm
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
      if (_dt != null)
        _dt.Dispose();
      _dt = null;
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenerateResourceForm));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.rbUsarResourceExistente = new System.Windows.Forms.RadioButton();
      this.rbGerarNovoResource = new System.Windows.Forms.RadioButton();
      this.gridResources = new System.Windows.Forms.DataGridView();
      this.gbxCriacaoResource = new System.Windows.Forms.GroupBox();
      this.btnCancelar = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.edtCodigoGerado = new System.Windows.Forms.TextBox();
      this.lbCodigo = new System.Windows.Forms.Label();
      this.memoComentario = new System.Windows.Forms.RichTextBox();
      this.lbComentario = new System.Windows.Forms.Label();
      this.memoValorResource = new System.Windows.Forms.RichTextBox();
      this.lbValorResource = new System.Windows.Forms.Label();
      this.edtNomeResource = new System.Windows.Forms.TextBox();
      this.lbNomeResource = new System.Windows.Forms.Label();
      this.edtPrefixo = new System.Windows.Forms.TextBox();
      this.lbPrefixo = new System.Windows.Forms.Label();
      this.cbArquivoResources = new System.Windows.Forms.ComboBox();
      this.lbArquivoResource = new System.Windows.Forms.Label();
      this.gbxResources = new System.Windows.Forms.GroupBox();
      this.pnlNavigator = new System.Windows.Forms.Panel();
      this.edtProcurar = new System.Windows.Forms.TextBox();
      this.pnlSeparator3 = new System.Windows.Forms.Panel();
      this.cbCondicao = new System.Windows.Forms.ComboBox();
      this.pnlSeparator2 = new System.Windows.Forms.Panel();
      this.cbCampos = new System.Windows.Forms.ComboBox();
      this.lbProcurar = new System.Windows.Forms.Label();
      this.pnlSeparator = new System.Windows.Forms.Panel();
      this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
      this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
      this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
      this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
      this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
      this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
      this.colMarcar = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      this.colNome = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colValor = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colComentario = new System.Windows.Forms.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.gridResources)).BeginInit();
      this.gbxCriacaoResource.SuspendLayout();
      this.gbxResources.SuspendLayout();
      this.pnlNavigator.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
      this.bindingNavigator1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
      this.SuspendLayout();
      // 
      // rbUsarResourceExistente
      // 
      resources.ApplyResources(this.rbUsarResourceExistente, "rbUsarResourceExistente");
      this.rbUsarResourceExistente.Name = "rbUsarResourceExistente";
      this.toolTip1.SetToolTip(this.rbUsarResourceExistente, resources.GetString("rbUsarResourceExistente.ToolTip"));
      this.rbUsarResourceExistente.UseVisualStyleBackColor = true;
      // 
      // rbGerarNovoResource
      // 
      resources.ApplyResources(this.rbGerarNovoResource, "rbGerarNovoResource");
      this.rbGerarNovoResource.Name = "rbGerarNovoResource";
      this.toolTip1.SetToolTip(this.rbGerarNovoResource, resources.GetString("rbGerarNovoResource.ToolTip"));
      this.rbGerarNovoResource.UseVisualStyleBackColor = true;
      this.rbGerarNovoResource.CheckedChanged += new System.EventHandler(this.rbGerarNovoResource_CheckedChanged);
      // 
      // gridResources
      // 
      this.gridResources.AllowUserToAddRows = false;
      this.gridResources.AllowUserToDeleteRows = false;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
      this.gridResources.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.gridResources.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
      this.gridResources.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.gridResources.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMarcar,
            this.colNome,
            this.colValor,
            this.colComentario});
      this.gridResources.Cursor = System.Windows.Forms.Cursors.Default;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.gridResources.DefaultCellStyle = dataGridViewCellStyle2;
      resources.ApplyResources(this.gridResources, "gridResources");
      this.gridResources.MultiSelect = false;
      this.gridResources.Name = "gridResources";
      this.gridResources.ShowCellToolTips = false;
      this.toolTip1.SetToolTip(this.gridResources, resources.GetString("gridResources.ToolTip"));
      this.gridResources.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridResources_CellContentClick);
      this.gridResources.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridResources_CellContentDoubleClick);
      this.gridResources.SelectionChanged += new System.EventHandler(this.gridResources_SelectionChanged);
      // 
      // gbxCriacaoResource
      // 
      this.gbxCriacaoResource.Controls.Add(this.btnCancelar);
      this.gbxCriacaoResource.Controls.Add(this.btnOK);
      this.gbxCriacaoResource.Controls.Add(this.rbUsarResourceExistente);
      this.gbxCriacaoResource.Controls.Add(this.rbGerarNovoResource);
      this.gbxCriacaoResource.Controls.Add(this.edtCodigoGerado);
      this.gbxCriacaoResource.Controls.Add(this.lbCodigo);
      this.gbxCriacaoResource.Controls.Add(this.memoComentario);
      this.gbxCriacaoResource.Controls.Add(this.lbComentario);
      this.gbxCriacaoResource.Controls.Add(this.memoValorResource);
      this.gbxCriacaoResource.Controls.Add(this.lbValorResource);
      this.gbxCriacaoResource.Controls.Add(this.edtNomeResource);
      this.gbxCriacaoResource.Controls.Add(this.lbNomeResource);
      this.gbxCriacaoResource.Controls.Add(this.edtPrefixo);
      this.gbxCriacaoResource.Controls.Add(this.lbPrefixo);
      this.gbxCriacaoResource.Controls.Add(this.cbArquivoResources);
      this.gbxCriacaoResource.Controls.Add(this.lbArquivoResource);
      resources.ApplyResources(this.gbxCriacaoResource, "gbxCriacaoResource");
      this.gbxCriacaoResource.Name = "gbxCriacaoResource";
      this.gbxCriacaoResource.TabStop = false;
      // 
      // btnCancelar
      // 
      resources.ApplyResources(this.btnCancelar, "btnCancelar");
      this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancelar.Name = "btnCancelar";
      this.btnCancelar.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      resources.ApplyResources(this.btnOK, "btnOK");
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Name = "btnOK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // edtCodigoGerado
      // 
      resources.ApplyResources(this.edtCodigoGerado, "edtCodigoGerado");
      this.edtCodigoGerado.Name = "edtCodigoGerado";
      this.edtCodigoGerado.ReadOnly = true;
      // 
      // lbCodigo
      // 
      resources.ApplyResources(this.lbCodigo, "lbCodigo");
      this.lbCodigo.Name = "lbCodigo";
      // 
      // memoComentario
      // 
      resources.ApplyResources(this.memoComentario, "memoComentario");
      this.memoComentario.Name = "memoComentario";
      // 
      // lbComentario
      // 
      resources.ApplyResources(this.lbComentario, "lbComentario");
      this.lbComentario.Name = "lbComentario";
      // 
      // memoValorResource
      // 
      resources.ApplyResources(this.memoValorResource, "memoValorResource");
      this.memoValorResource.Name = "memoValorResource";
      this.memoValorResource.TextChanged += new System.EventHandler(this.memoValorResource_TextChanged);
      // 
      // lbValorResource
      // 
      resources.ApplyResources(this.lbValorResource, "lbValorResource");
      this.lbValorResource.Name = "lbValorResource";
      // 
      // edtNomeResource
      // 
      resources.ApplyResources(this.edtNomeResource, "edtNomeResource");
      this.edtNomeResource.Name = "edtNomeResource";
      this.edtNomeResource.TextChanged += new System.EventHandler(this.edtNomeResource_TextChanged);
      this.edtNomeResource.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edtNomeResource_KeyDown);
      // 
      // lbNomeResource
      // 
      resources.ApplyResources(this.lbNomeResource, "lbNomeResource");
      this.lbNomeResource.Name = "lbNomeResource";
      // 
      // edtPrefixo
      // 
      resources.ApplyResources(this.edtPrefixo, "edtPrefixo");
      this.edtPrefixo.Name = "edtPrefixo";
      this.edtPrefixo.TextChanged += new System.EventHandler(this.cbPrefixo_TextChanged);
      this.edtPrefixo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbPrefixo_KeyDown);
      // 
      // lbPrefixo
      // 
      resources.ApplyResources(this.lbPrefixo, "lbPrefixo");
      this.lbPrefixo.Name = "lbPrefixo";
      // 
      // cbArquivoResources
      // 
      resources.ApplyResources(this.cbArquivoResources, "cbArquivoResources");
      this.cbArquivoResources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbArquivoResources.FormattingEnabled = true;
      this.cbArquivoResources.Name = "cbArquivoResources";
      // 
      // lbArquivoResource
      // 
      resources.ApplyResources(this.lbArquivoResource, "lbArquivoResource");
      this.lbArquivoResource.Name = "lbArquivoResource";
      // 
      // gbxResources
      // 
      this.gbxResources.Controls.Add(this.gridResources);
      this.gbxResources.Controls.Add(this.pnlNavigator);
      resources.ApplyResources(this.gbxResources, "gbxResources");
      this.gbxResources.Name = "gbxResources";
      this.gbxResources.TabStop = false;
      // 
      // pnlNavigator
      // 
      this.pnlNavigator.Controls.Add(this.edtProcurar);
      this.pnlNavigator.Controls.Add(this.pnlSeparator3);
      this.pnlNavigator.Controls.Add(this.cbCondicao);
      this.pnlNavigator.Controls.Add(this.pnlSeparator2);
      this.pnlNavigator.Controls.Add(this.cbCampos);
      this.pnlNavigator.Controls.Add(this.lbProcurar);
      this.pnlNavigator.Controls.Add(this.pnlSeparator);
      this.pnlNavigator.Controls.Add(this.bindingNavigator1);
      resources.ApplyResources(this.pnlNavigator, "pnlNavigator");
      this.pnlNavigator.Name = "pnlNavigator";
      // 
      // edtProcurar
      // 
      resources.ApplyResources(this.edtProcurar, "edtProcurar");
      this.edtProcurar.Name = "edtProcurar";
      this.edtProcurar.TextChanged += new System.EventHandler(this.edtProcurar_TextChanged);
      // 
      // pnlSeparator3
      // 
      resources.ApplyResources(this.pnlSeparator3, "pnlSeparator3");
      this.pnlSeparator3.Name = "pnlSeparator3";
      // 
      // cbCondicao
      // 
      resources.ApplyResources(this.cbCondicao, "cbCondicao");
      this.cbCondicao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbCondicao.FormattingEnabled = true;
      this.cbCondicao.Items.AddRange(new object[] {
            resources.GetString("cbCondicao.Items"),
            resources.GetString("cbCondicao.Items1")});
      this.cbCondicao.Name = "cbCondicao";
      // 
      // pnlSeparator2
      // 
      resources.ApplyResources(this.pnlSeparator2, "pnlSeparator2");
      this.pnlSeparator2.Name = "pnlSeparator2";
      // 
      // cbCampos
      // 
      resources.ApplyResources(this.cbCampos, "cbCampos");
      this.cbCampos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbCampos.FormattingEnabled = true;
      this.cbCampos.Items.AddRange(new object[] {
            resources.GetString("cbCampos.Items"),
            resources.GetString("cbCampos.Items1"),
            resources.GetString("cbCampos.Items2")});
      this.cbCampos.Name = "cbCampos";
      this.cbCampos.SelectedIndexChanged += new System.EventHandler(this.cbCampos_SelectedIndexChanged);
      // 
      // lbProcurar
      // 
      resources.ApplyResources(this.lbProcurar, "lbProcurar");
      this.lbProcurar.Name = "lbProcurar";
      // 
      // pnlSeparator
      // 
      resources.ApplyResources(this.pnlSeparator, "pnlSeparator");
      this.pnlSeparator.Name = "pnlSeparator";
      // 
      // bindingNavigator1
      // 
      this.bindingNavigator1.AddNewItem = null;
      this.bindingNavigator1.BindingSource = this.bindingSource1;
      this.bindingNavigator1.CountItem = null;
      this.bindingNavigator1.DeleteItem = null;
      resources.ApplyResources(this.bindingNavigator1, "bindingNavigator1");
      this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem});
      this.bindingNavigator1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
      this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
      this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
      this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
      this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
      this.bindingNavigator1.Name = "bindingNavigator1";
      this.bindingNavigator1.PositionItem = null;
      // 
      // bindingNavigatorMoveFirstItem
      // 
      this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      resources.ApplyResources(this.bindingNavigatorMoveFirstItem, "bindingNavigatorMoveFirstItem");
      this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
      // 
      // bindingNavigatorMovePreviousItem
      // 
      this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      resources.ApplyResources(this.bindingNavigatorMovePreviousItem, "bindingNavigatorMovePreviousItem");
      this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
      // 
      // bindingNavigatorMoveNextItem
      // 
      this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      resources.ApplyResources(this.bindingNavigatorMoveNextItem, "bindingNavigatorMoveNextItem");
      this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
      // 
      // bindingNavigatorMoveLastItem
      // 
      this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      resources.ApplyResources(this.bindingNavigatorMoveLastItem, "bindingNavigatorMoveLastItem");
      this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
      // 
      // errorProvider1
      // 
      this.errorProvider1.ContainerControl = this;
      // 
      // colMarcar
      // 
      this.colMarcar.FalseValue = "false";
      resources.ApplyResources(this.colMarcar, "colMarcar");
      this.colMarcar.Name = "colMarcar";
      this.colMarcar.TrueValue = "true";
      // 
      // colNome
      // 
      this.colNome.DataPropertyName = "colNome";
      resources.ApplyResources(this.colNome, "colNome");
      this.colNome.Name = "colNome";
      this.colNome.ReadOnly = true;
      // 
      // colValor
      // 
      this.colValor.DataPropertyName = "colValor";
      resources.ApplyResources(this.colValor, "colValor");
      this.colValor.Name = "colValor";
      this.colValor.ReadOnly = true;
      // 
      // colComentario
      // 
      this.colComentario.DataPropertyName = "colComentario";
      resources.ApplyResources(this.colComentario, "colComentario");
      this.colComentario.Name = "colComentario";
      this.colComentario.ReadOnly = true;
      // 
      // GenerateResourceForm
      // 
      this.AcceptButton = this.btnOK;
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.gbxResources);
      this.Controls.Add(this.gbxCriacaoResource);
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "GenerateResourceForm";
      this.Shown += new System.EventHandler(this.GenerateResourceForm_Shown);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GenerateResourceForm_KeyDown);
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.gridResources)).EndInit();
      this.gbxCriacaoResource.ResumeLayout(false);
      this.gbxCriacaoResource.PerformLayout();
      this.gbxResources.ResumeLayout(false);
      this.pnlNavigator.ResumeLayout(false);
      this.pnlNavigator.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
      this.bindingNavigator1.ResumeLayout(false);
      this.bindingNavigator1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.BindingSource bindingSource1;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.GroupBox gbxCriacaoResource;
    private System.Windows.Forms.Button btnCancelar;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.RadioButton rbUsarResourceExistente;
    private System.Windows.Forms.RadioButton rbGerarNovoResource;
    private System.Windows.Forms.TextBox edtCodigoGerado;
    private System.Windows.Forms.Label lbCodigo;
    private System.Windows.Forms.RichTextBox memoComentario;
    private System.Windows.Forms.Label lbComentario;
    private System.Windows.Forms.RichTextBox memoValorResource;
    private System.Windows.Forms.Label lbValorResource;
    private System.Windows.Forms.TextBox edtNomeResource;
    private System.Windows.Forms.Label lbNomeResource;
    private System.Windows.Forms.TextBox edtPrefixo;
    private System.Windows.Forms.Label lbPrefixo;
    private System.Windows.Forms.ComboBox cbArquivoResources;
    private System.Windows.Forms.Label lbArquivoResource;
    private System.Windows.Forms.GroupBox gbxResources;
    private System.Windows.Forms.DataGridView gridResources;
    private System.Windows.Forms.Panel pnlNavigator;
    private System.Windows.Forms.TextBox edtProcurar;
    private System.Windows.Forms.ComboBox cbCampos;
    private System.Windows.Forms.Label lbProcurar;
    private System.Windows.Forms.Panel pnlSeparator;
    private System.Windows.Forms.BindingNavigator bindingNavigator1;
    private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
    private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
    private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
    private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
    private System.Windows.Forms.ErrorProvider errorProvider1;
    private System.Windows.Forms.ComboBox cbCondicao;
    private System.Windows.Forms.Panel pnlSeparator3;
    private System.Windows.Forms.Panel pnlSeparator2;
    private System.Windows.Forms.DataGridViewCheckBoxColumn colMarcar;
    private System.Windows.Forms.DataGridViewTextBoxColumn colNome;
    private System.Windows.Forms.DataGridViewTextBoxColumn colValor;
    private System.Windows.Forms.DataGridViewTextBoxColumn colComentario;
  }
}