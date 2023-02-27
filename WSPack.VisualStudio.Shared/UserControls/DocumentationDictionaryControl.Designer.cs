namespace WSPack.VisualStudio.Shared.UserControls
{
  partial class DocumentationDictionaryControl
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentationDictionaryControl));
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.colNomeItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.colSummary = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.menuGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.menuItemGerarItensPadrao = new System.Windows.Forms.ToolStripMenuItem();
      this.gbxInformacoes = new System.Windows.Forms.GroupBox();
      this.memoInformacoes = new System.Windows.Forms.RichTextBox();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
      this.menuGrid.SuspendLayout();
      this.gbxInformacoes.SuspendLayout();
      this.SuspendLayout();
      // 
      // dataGridView1
      // 
      this.dataGridView1.AllowUserToResizeRows = false;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
      this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNomeItem,
            this.colSummary});
      this.dataGridView1.ContextMenuStrip = this.menuGrid;
      resources.ApplyResources(this.dataGridView1, "dataGridView1");
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
      this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
      this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
      this.dataGridView1.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView1_UserDeletingRow);
      // 
      // colNomeItem
      // 
      this.colNomeItem.DataPropertyName = "ItemName";
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.colNomeItem.DefaultCellStyle = dataGridViewCellStyle2;
      resources.ApplyResources(this.colNomeItem, "colNomeItem");
      this.colNomeItem.Name = "colNomeItem";
      // 
      // colSummary
      // 
      this.colSummary.DataPropertyName = "Summary";
      resources.ApplyResources(this.colSummary, "colSummary");
      this.colSummary.Name = "colSummary";
      // 
      // menuGrid
      // 
      this.menuGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemGerarItensPadrao});
      this.menuGrid.Name = "menuGrid";
      resources.ApplyResources(this.menuGrid, "menuGrid");
      this.menuGrid.Opening += new System.ComponentModel.CancelEventHandler(this.menuGrid_Opening);
      // 
      // menuItemGerarItensPadrao
      // 
      this.menuItemGerarItensPadrao.Name = "menuItemGerarItensPadrao";
      resources.ApplyResources(this.menuItemGerarItensPadrao, "menuItemGerarItensPadrao");
      this.menuItemGerarItensPadrao.Click += new System.EventHandler(this.menuItemGerarItensPadrao_Click);
      // 
      // gbxInformacoes
      // 
      this.gbxInformacoes.Controls.Add(this.memoInformacoes);
      resources.ApplyResources(this.gbxInformacoes, "gbxInformacoes");
      this.gbxInformacoes.Name = "gbxInformacoes";
      this.gbxInformacoes.TabStop = false;
      // 
      // memoInformacoes
      // 
      resources.ApplyResources(this.memoInformacoes, "memoInformacoes");
      this.memoInformacoes.Name = "memoInformacoes";
      this.memoInformacoes.ReadOnly = true;
      // 
      // DocumentationDictionaryControl
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.dataGridView1);
      this.Controls.Add(this.gbxInformacoes);
      this.Name = "DocumentationDictionaryControl";
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
      this.menuGrid.ResumeLayout(false);
      this.gbxInformacoes.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.DataGridViewTextBoxColumn colNomeItem;
    private System.Windows.Forms.DataGridViewTextBoxColumn colSummary;
    private System.Windows.Forms.GroupBox gbxInformacoes;
    private System.Windows.Forms.RichTextBox memoInformacoes;
    private System.Windows.Forms.ContextMenuStrip menuGrid;
    private System.Windows.Forms.ToolStripMenuItem menuItemGerarItensPadrao;
  }
}
