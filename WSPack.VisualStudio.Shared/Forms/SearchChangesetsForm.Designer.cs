using WSPack.Lib.Items;

namespace WSPack.VisualStudio.Shared.Forms
{
  partial class SearchChangesetsForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchChangesetsForm));
      this.pnlBottom = new System.Windows.Forms.Panel();
      this.btnVoltarPesquisa = new System.Windows.Forms.Button();
      this.btnChangesetDetails = new System.Windows.Forms.Button();
      this.btnMerge = new System.Windows.Forms.Button();
      this.btnFechar = new System.Windows.Forms.Button();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.pageParametros = new System.Windows.Forms.TabPage();
      this.btnChangesetEspecifico = new System.Windows.Forms.Button();
      this.edtChangesetId = new System.Windows.Forms.NumericUpDown();
      this.cbxChangesetEspecifico = new System.Windows.Forms.CheckBox();
      this.pnlOpcoesPesquisa = new System.Windows.Forms.Panel();
      this.btnChooseItem = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.cbLocalProcura = new System.Windows.Forms.ComboBox();
      this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
      this.gbxFiltrarCheckInNotes = new System.Windows.Forms.GroupBox();
      this.gridNotas = new System.Windows.Forms.DataGridView();
      this.colCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      this.colCheckinNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colOperador = new System.Windows.Forms.DataGridViewComboBoxColumn();
      this.colValor = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.cbxFiltrarCheckInNotes = new System.Windows.Forms.CheckBox();
      this.lbAvisoFiltroArquivoPerformance = new System.Windows.Forms.Label();
      this.cbxFiltrarUsuario = new System.Windows.Forms.CheckBox();
      this.btnPesquisar = new System.Windows.Forms.Button();
      this.edtArquivos = new System.Windows.Forms.TextBox();
      this.edtUsuario = new System.Windows.Forms.TextBox();
      this.cbxFiltrarArquivos = new System.Windows.Forms.CheckBox();
      this.edtDataInicio = new System.Windows.Forms.DateTimePicker();
      this.edtComentario = new System.Windows.Forms.TextBox();
      this.edtDataFim = new System.Windows.Forms.DateTimePicker();
      this.cbxFiltrarComentario = new System.Windows.Forms.CheckBox();
      this.label3 = new System.Windows.Forms.Label();
      this.cbxFiltrarData = new System.Windows.Forms.CheckBox();
      this.label4 = new System.Windows.Forms.Label();
      this.edtServidor = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.pageResultado = new System.Windows.Forms.TabPage();
      this.pnlGrid = new System.Windows.Forms.Panel();
      this.gridChangesets = new System.Windows.Forms.DataGridView();
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.menuItemChangesetDetails = new System.Windows.Forms.ToolStripMenuItem();
      this.menuItemGerarTemplateCheckIn = new System.Windows.Forms.ToolStripMenuItem();
      this.menuItemSeparador01 = new System.Windows.Forms.ToolStripSeparator();
      this.menuItemMerge = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.menuItemExportarResultado = new System.Windows.Forms.ToolStripMenuItem();
      this.lbTotalRegistros = new System.Windows.Forms.Label();
      this.ttProvider = new System.Windows.Forms.ToolTip(this.components);
      this.pnlBottom.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.pageParametros.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.edtChangesetId)).BeginInit();
      this.pnlOpcoesPesquisa.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
      this.gbxFiltrarCheckInNotes.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.gridNotas)).BeginInit();
      this.pageResultado.SuspendLayout();
      this.pnlGrid.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.gridChangesets)).BeginInit();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // pnlBottom
      // 
      this.pnlBottom.Controls.Add(this.btnVoltarPesquisa);
      this.pnlBottom.Controls.Add(this.btnChangesetDetails);
      this.pnlBottom.Controls.Add(this.btnMerge);
      this.pnlBottom.Controls.Add(this.btnFechar);
      resources.ApplyResources(this.pnlBottom, "pnlBottom");
      this.pnlBottom.Name = "pnlBottom";
      // 
      // btnVoltarPesquisa
      // 
      resources.ApplyResources(this.btnVoltarPesquisa, "btnVoltarPesquisa");
      this.btnVoltarPesquisa.Name = "btnVoltarPesquisa";
      this.ttProvider.SetToolTip(this.btnVoltarPesquisa, resources.GetString("btnVoltarPesquisa.ToolTip"));
      this.btnVoltarPesquisa.UseVisualStyleBackColor = true;
      this.btnVoltarPesquisa.Click += new System.EventHandler(this.btnVoltarPesquisa_Click);
      // 
      // btnChangesetDetails
      // 
      resources.ApplyResources(this.btnChangesetDetails, "btnChangesetDetails");
      this.btnChangesetDetails.Name = "btnChangesetDetails";
      this.btnChangesetDetails.UseVisualStyleBackColor = true;
      this.btnChangesetDetails.Click += new System.EventHandler(this.btnChangesetDetails_Click);
      // 
      // btnMerge
      // 
      resources.ApplyResources(this.btnMerge, "btnMerge");
      this.btnMerge.Name = "btnMerge";
      this.ttProvider.SetToolTip(this.btnMerge, resources.GetString("btnMerge.ToolTip"));
      this.btnMerge.UseVisualStyleBackColor = true;
      this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
      // 
      // btnFechar
      // 
      resources.ApplyResources(this.btnFechar, "btnFechar");
      this.btnFechar.Name = "btnFechar";
      this.ttProvider.SetToolTip(this.btnFechar, resources.GetString("btnFechar.ToolTip"));
      this.btnFechar.UseVisualStyleBackColor = true;
      this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.pageParametros);
      this.tabControl1.Controls.Add(this.pageResultado);
      resources.ApplyResources(this.tabControl1, "tabControl1");
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
      // 
      // pageParametros
      // 
      this.pageParametros.Controls.Add(this.btnChangesetEspecifico);
      this.pageParametros.Controls.Add(this.edtChangesetId);
      this.pageParametros.Controls.Add(this.cbxChangesetEspecifico);
      this.pageParametros.Controls.Add(this.pnlOpcoesPesquisa);
      this.pageParametros.Controls.Add(this.edtServidor);
      this.pageParametros.Controls.Add(this.label1);
      resources.ApplyResources(this.pageParametros, "pageParametros");
      this.pageParametros.Name = "pageParametros";
      this.pageParametros.UseVisualStyleBackColor = true;
      // 
      // btnChangesetEspecifico
      // 
      resources.ApplyResources(this.btnChangesetEspecifico, "btnChangesetEspecifico");
      this.btnChangesetEspecifico.Name = "btnChangesetEspecifico";
      this.btnChangesetEspecifico.UseVisualStyleBackColor = true;
      this.btnChangesetEspecifico.Click += new System.EventHandler(this.btnChangesetEspecifico_Click);
      // 
      // edtChangesetId
      // 
      resources.ApplyResources(this.edtChangesetId, "edtChangesetId");
      this.edtChangesetId.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
      this.edtChangesetId.Name = "edtChangesetId";
      this.ttProvider.SetToolTip(this.edtChangesetId, resources.GetString("edtChangesetId.ToolTip"));
      // 
      // cbxChangesetEspecifico
      // 
      resources.ApplyResources(this.cbxChangesetEspecifico, "cbxChangesetEspecifico");
      this.cbxChangesetEspecifico.Name = "cbxChangesetEspecifico";
      this.ttProvider.SetToolTip(this.cbxChangesetEspecifico, resources.GetString("cbxChangesetEspecifico.ToolTip"));
      this.cbxChangesetEspecifico.UseVisualStyleBackColor = true;
      this.cbxChangesetEspecifico.CheckedChanged += new System.EventHandler(this.cbxChangesetEspecifico_CheckedChanged);
      // 
      // pnlOpcoesPesquisa
      // 
      this.pnlOpcoesPesquisa.Controls.Add(this.btnChooseItem);
      this.pnlOpcoesPesquisa.Controls.Add(this.label2);
      this.pnlOpcoesPesquisa.Controls.Add(this.cbLocalProcura);
      this.pnlOpcoesPesquisa.Controls.Add(this.gbxFiltrarCheckInNotes);
      this.pnlOpcoesPesquisa.Controls.Add(this.lbAvisoFiltroArquivoPerformance);
      this.pnlOpcoesPesquisa.Controls.Add(this.cbxFiltrarUsuario);
      this.pnlOpcoesPesquisa.Controls.Add(this.btnPesquisar);
      this.pnlOpcoesPesquisa.Controls.Add(this.edtArquivos);
      this.pnlOpcoesPesquisa.Controls.Add(this.edtUsuario);
      this.pnlOpcoesPesquisa.Controls.Add(this.cbxFiltrarArquivos);
      this.pnlOpcoesPesquisa.Controls.Add(this.edtDataInicio);
      this.pnlOpcoesPesquisa.Controls.Add(this.edtComentario);
      this.pnlOpcoesPesquisa.Controls.Add(this.edtDataFim);
      this.pnlOpcoesPesquisa.Controls.Add(this.cbxFiltrarComentario);
      this.pnlOpcoesPesquisa.Controls.Add(this.label3);
      this.pnlOpcoesPesquisa.Controls.Add(this.cbxFiltrarData);
      this.pnlOpcoesPesquisa.Controls.Add(this.label4);
      resources.ApplyResources(this.pnlOpcoesPesquisa, "pnlOpcoesPesquisa");
      this.pnlOpcoesPesquisa.Name = "pnlOpcoesPesquisa";
      // 
      // btnChooseItem
      // 
      resources.ApplyResources(this.btnChooseItem, "btnChooseItem");
      this.btnChooseItem.Name = "btnChooseItem";
      this.ttProvider.SetToolTip(this.btnChooseItem, resources.GetString("btnChooseItem.ToolTip"));
      this.btnChooseItem.UseVisualStyleBackColor = true;
      this.btnChooseItem.Click += new System.EventHandler(this.btnChooseItem_Click);
      // 
      // label2
      // 
      resources.ApplyResources(this.label2, "label2");
      this.label2.Name = "label2";
      // 
      // cbLocalProcura
      // 
      this.cbLocalProcura.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.cbLocalProcura.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.cbLocalProcura.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource1, "LocalProcura", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cbLocalProcura.FormattingEnabled = true;
      resources.ApplyResources(this.cbLocalProcura, "cbLocalProcura");
      this.cbLocalProcura.Name = "cbLocalProcura";
      this.ttProvider.SetToolTip(this.cbLocalProcura, resources.GetString("cbLocalProcura.ToolTip"));
      this.cbLocalProcura.TextChanged += new System.EventHandler(this.cbLocalProcura_TextChanged);
      // 
      // bindingSource1
      // 
      this.bindingSource1.DataSource = typeof(WSPack.Lib.Items.SearchChangesetsParams);
      // 
      // gbxFiltrarCheckInNotes
      // 
      this.gbxFiltrarCheckInNotes.Controls.Add(this.gridNotas);
      this.gbxFiltrarCheckInNotes.Controls.Add(this.cbxFiltrarCheckInNotes);
      resources.ApplyResources(this.gbxFiltrarCheckInNotes, "gbxFiltrarCheckInNotes");
      this.gbxFiltrarCheckInNotes.Name = "gbxFiltrarCheckInNotes";
      this.gbxFiltrarCheckInNotes.TabStop = false;
      // 
      // gridNotas
      // 
      this.gridNotas.AllowUserToAddRows = false;
      this.gridNotas.AllowUserToDeleteRows = false;
      this.gridNotas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.gridNotas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheck,
            this.colCheckinNote,
            this.colOperador,
            this.colValor});
      resources.ApplyResources(this.gridNotas, "gridNotas");
      this.gridNotas.Name = "gridNotas";
      this.gridNotas.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridNotas_CellValueChanged);
      this.gridNotas.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridNotas_DataError);
      // 
      // colCheck
      // 
      resources.ApplyResources(this.colCheck, "colCheck");
      this.colCheck.Name = "colCheck";
      // 
      // colCheckinNote
      // 
      resources.ApplyResources(this.colCheckinNote, "colCheckinNote");
      this.colCheckinNote.Name = "colCheckinNote";
      this.colCheckinNote.ReadOnly = true;
      this.colCheckinNote.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.colCheckinNote.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // colOperador
      // 
      resources.ApplyResources(this.colOperador, "colOperador");
      this.colOperador.Name = "colOperador";
      // 
      // colValor
      // 
      resources.ApplyResources(this.colValor, "colValor");
      this.colValor.Name = "colValor";
      this.colValor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      this.colValor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
      // 
      // cbxFiltrarCheckInNotes
      // 
      resources.ApplyResources(this.cbxFiltrarCheckInNotes, "cbxFiltrarCheckInNotes");
      this.cbxFiltrarCheckInNotes.Name = "cbxFiltrarCheckInNotes";
      this.cbxFiltrarCheckInNotes.UseVisualStyleBackColor = true;
      this.cbxFiltrarCheckInNotes.CheckedChanged += new System.EventHandler(this.cbxFiltrarCheckInNotes_CheckedChanged);
      // 
      // lbAvisoFiltroArquivoPerformance
      // 
      resources.ApplyResources(this.lbAvisoFiltroArquivoPerformance, "lbAvisoFiltroArquivoPerformance");
      this.lbAvisoFiltroArquivoPerformance.ForeColor = System.Drawing.Color.Red;
      this.lbAvisoFiltroArquivoPerformance.Name = "lbAvisoFiltroArquivoPerformance";
      // 
      // cbxFiltrarUsuario
      // 
      resources.ApplyResources(this.cbxFiltrarUsuario, "cbxFiltrarUsuario");
      this.cbxFiltrarUsuario.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSource1, "FiltrarUsuario", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cbxFiltrarUsuario.Name = "cbxFiltrarUsuario";
      this.cbxFiltrarUsuario.UseVisualStyleBackColor = true;
      this.cbxFiltrarUsuario.CheckedChanged += new System.EventHandler(this.cbxFiltrarUsuario_CheckedChanged);
      // 
      // btnPesquisar
      // 
      resources.ApplyResources(this.btnPesquisar, "btnPesquisar");
      this.btnPesquisar.Name = "btnPesquisar";
      this.ttProvider.SetToolTip(this.btnPesquisar, resources.GetString("btnPesquisar.ToolTip"));
      this.btnPesquisar.UseVisualStyleBackColor = true;
      this.btnPesquisar.Click += new System.EventHandler(this.btnPesquisar_Click);
      // 
      // edtArquivos
      // 
      this.edtArquivos.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource1, "Arquivos", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      resources.ApplyResources(this.edtArquivos, "edtArquivos");
      this.edtArquivos.Name = "edtArquivos";
      this.ttProvider.SetToolTip(this.edtArquivos, resources.GetString("edtArquivos.ToolTip"));
      // 
      // edtUsuario
      // 
      this.edtUsuario.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource1, "Usuario", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      resources.ApplyResources(this.edtUsuario, "edtUsuario");
      this.edtUsuario.Name = "edtUsuario";
      // 
      // cbxFiltrarArquivos
      // 
      resources.ApplyResources(this.cbxFiltrarArquivos, "cbxFiltrarArquivos");
      this.cbxFiltrarArquivos.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSource1, "FiltrarArquivos", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cbxFiltrarArquivos.Name = "cbxFiltrarArquivos";
      this.cbxFiltrarArquivos.UseVisualStyleBackColor = true;
      this.cbxFiltrarArquivos.CheckedChanged += new System.EventHandler(this.cbxFiltrarArquivos_CheckedChanged);
      // 
      // edtDataInicio
      // 
      resources.ApplyResources(this.edtDataInicio, "edtDataInicio");
      this.edtDataInicio.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindingSource1, "DataInicial", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.edtDataInicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.edtDataInicio.Name = "edtDataInicio";
      // 
      // edtComentario
      // 
      this.edtComentario.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource1, "Comentario", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      resources.ApplyResources(this.edtComentario, "edtComentario");
      this.edtComentario.Name = "edtComentario";
      // 
      // edtDataFim
      // 
      resources.ApplyResources(this.edtDataFim, "edtDataFim");
      this.edtDataFim.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.bindingSource1, "DataFinal", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.edtDataFim.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
      this.edtDataFim.Name = "edtDataFim";
      // 
      // cbxFiltrarComentario
      // 
      resources.ApplyResources(this.cbxFiltrarComentario, "cbxFiltrarComentario");
      this.cbxFiltrarComentario.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSource1, "FiltrarComentario", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cbxFiltrarComentario.Name = "cbxFiltrarComentario";
      this.cbxFiltrarComentario.UseVisualStyleBackColor = true;
      this.cbxFiltrarComentario.CheckedChanged += new System.EventHandler(this.cbxFiltrarComentario_CheckedChanged);
      // 
      // label3
      // 
      resources.ApplyResources(this.label3, "label3");
      this.label3.Name = "label3";
      // 
      // cbxFiltrarData
      // 
      resources.ApplyResources(this.cbxFiltrarData, "cbxFiltrarData");
      this.cbxFiltrarData.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSource1, "FiltrarData", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
      this.cbxFiltrarData.Name = "cbxFiltrarData";
      this.cbxFiltrarData.UseVisualStyleBackColor = true;
      this.cbxFiltrarData.CheckedChanged += new System.EventHandler(this.cbxFiltrarData_CheckedChanged);
      // 
      // label4
      // 
      resources.ApplyResources(this.label4, "label4");
      this.label4.Name = "label4";
      // 
      // edtServidor
      // 
      resources.ApplyResources(this.edtServidor, "edtServidor");
      this.edtServidor.Name = "edtServidor";
      this.edtServidor.ReadOnly = true;
      this.edtServidor.TabStop = false;
      // 
      // label1
      // 
      resources.ApplyResources(this.label1, "label1");
      this.label1.Name = "label1";
      // 
      // pageResultado
      // 
      this.pageResultado.Controls.Add(this.pnlGrid);
      resources.ApplyResources(this.pageResultado, "pageResultado");
      this.pageResultado.Name = "pageResultado";
      this.pageResultado.UseVisualStyleBackColor = true;
      // 
      // pnlGrid
      // 
      this.pnlGrid.Controls.Add(this.gridChangesets);
      this.pnlGrid.Controls.Add(this.lbTotalRegistros);
      resources.ApplyResources(this.pnlGrid, "pnlGrid");
      this.pnlGrid.Name = "pnlGrid";
      // 
      // gridChangesets
      // 
      this.gridChangesets.AllowUserToAddRows = false;
      this.gridChangesets.AllowUserToDeleteRows = false;
      this.gridChangesets.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
      this.gridChangesets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.gridChangesets.ContextMenuStrip = this.contextMenuStrip1;
      resources.ApplyResources(this.gridChangesets, "gridChangesets");
      this.gridChangesets.Name = "gridChangesets";
      this.gridChangesets.ReadOnly = true;
      this.gridChangesets.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridChangesets_CellDoubleClick);
      this.gridChangesets.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.gridChangesets_DataBindingComplete);
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemChangesetDetails,
            this.menuItemGerarTemplateCheckIn,
            this.menuItemSeparador01,
            this.menuItemMerge,
            this.toolStripMenuItem1,
            this.menuItemExportarResultado});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
      this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
      // 
      // menuItemChangesetDetails
      // 
      this.menuItemChangesetDetails.Name = "menuItemChangesetDetails";
      resources.ApplyResources(this.menuItemChangesetDetails, "menuItemChangesetDetails");
      this.menuItemChangesetDetails.Click += new System.EventHandler(this.menuItemChangesetDetails_Click);
      // 
      // menuItemGerarTemplateCheckIn
      // 
      this.menuItemGerarTemplateCheckIn.Name = "menuItemGerarTemplateCheckIn";
      resources.ApplyResources(this.menuItemGerarTemplateCheckIn, "menuItemGerarTemplateCheckIn");
      this.menuItemGerarTemplateCheckIn.Click += new System.EventHandler(this.menuItemGerarTemplateCheckIn_Click);
      // 
      // menuItemSeparador01
      // 
      this.menuItemSeparador01.Name = "menuItemSeparador01";
      resources.ApplyResources(this.menuItemSeparador01, "menuItemSeparador01");
      // 
      // menuItemMerge
      // 
      this.menuItemMerge.Name = "menuItemMerge";
      resources.ApplyResources(this.menuItemMerge, "menuItemMerge");
      this.menuItemMerge.Click += new System.EventHandler(this.menuItemMerge_Click);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
      // 
      // menuItemExportarResultado
      // 
      this.menuItemExportarResultado.Name = "menuItemExportarResultado";
      resources.ApplyResources(this.menuItemExportarResultado, "menuItemExportarResultado");
      this.menuItemExportarResultado.Click += new System.EventHandler(this.menuItemExportarResultado_Click);
      // 
      // lbTotalRegistros
      // 
      resources.ApplyResources(this.lbTotalRegistros, "lbTotalRegistros");
      this.lbTotalRegistros.Name = "lbTotalRegistros";
      // 
      // SearchChangesetsForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabControl1);
      this.Controls.Add(this.pnlBottom);
      this.KeyPreview = true;
      this.Name = "SearchChangesetsForm";
      this.Load += new System.EventHandler(this.SearchChangesetsForm_Load);
      this.Shown += new System.EventHandler(this.SearchChangesetsForm_Shown);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchChangesetsForm_KeyDown);
      this.pnlBottom.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.pageParametros.ResumeLayout(false);
      this.pageParametros.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.edtChangesetId)).EndInit();
      this.pnlOpcoesPesquisa.ResumeLayout(false);
      this.pnlOpcoesPesquisa.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
      this.gbxFiltrarCheckInNotes.ResumeLayout(false);
      this.gbxFiltrarCheckInNotes.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.gridNotas)).EndInit();
      this.pageResultado.ResumeLayout(false);
      this.pnlGrid.ResumeLayout(false);
      this.pnlGrid.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.gridChangesets)).EndInit();
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel pnlBottom;
    private System.Windows.Forms.Button btnChangesetDetails;
    private System.Windows.Forms.Button btnMerge;
    private System.Windows.Forms.Button btnFechar;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage pageParametros;
    private System.Windows.Forms.TextBox edtServidor;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TabPage pageResultado;
    private System.Windows.Forms.ComboBox cbLocalProcura;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnPesquisar;
    private System.Windows.Forms.ToolTip ttProvider;
    private System.Windows.Forms.CheckBox cbxFiltrarData;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.DateTimePicker edtDataFim;
    private System.Windows.Forms.DateTimePicker edtDataInicio;
    private System.Windows.Forms.TextBox edtUsuario;
    private System.Windows.Forms.CheckBox cbxFiltrarUsuario;
    private System.Windows.Forms.CheckBox cbxFiltrarComentario;
    private System.Windows.Forms.TextBox edtComentario;
    private System.Windows.Forms.CheckBox cbxFiltrarArquivos;
    private System.Windows.Forms.TextBox edtArquivos;
    private System.Windows.Forms.GroupBox gbxFiltrarCheckInNotes;
    private System.Windows.Forms.DataGridView gridNotas;
    private System.Windows.Forms.CheckBox cbxFiltrarCheckInNotes;
    private System.Windows.Forms.Label lbAvisoFiltroArquivoPerformance;
    private System.Windows.Forms.NumericUpDown edtChangesetId;
    private System.Windows.Forms.CheckBox cbxChangesetEspecifico;
    private System.Windows.Forms.Panel pnlOpcoesPesquisa;
    private System.Windows.Forms.Button btnChangesetEspecifico;
    private System.Windows.Forms.Panel pnlGrid;
    private System.Windows.Forms.DataGridView gridChangesets;
    private System.Windows.Forms.Label lbTotalRegistros;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem menuItemChangesetDetails;
    private System.Windows.Forms.ToolStripSeparator menuItemSeparador01;
    private System.Windows.Forms.ToolStripMenuItem menuItemMerge;
    private System.Windows.Forms.Button btnVoltarPesquisa;
    private System.Windows.Forms.ToolStripMenuItem menuItemGerarTemplateCheckIn;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem menuItemExportarResultado;
    private System.Windows.Forms.Button btnChooseItem;
    private System.Windows.Forms.DataGridViewCheckBoxColumn colCheck;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCheckinNote;
    private System.Windows.Forms.DataGridViewComboBoxColumn colOperador;
    private System.Windows.Forms.DataGridViewTextBoxColumn colValor;
    private System.Windows.Forms.BindingSource bindingSource1;
  }
}