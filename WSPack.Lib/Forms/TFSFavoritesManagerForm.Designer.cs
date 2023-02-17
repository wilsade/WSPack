namespace WSPack.Lib.Forms
{
  partial class TFSFavoritesManagerForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFSFavoritesManagerForm));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      this.pnlTop = new System.Windows.Forms.Panel();
      this.edtServidor = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.pnlBottom = new System.Windows.Forms.Panel();
      this.btnFechar = new System.Windows.Forms.Button();
      this.grid = new System.Windows.Forms.DataGridView();
      this.ServerItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Caption = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.menuGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.menuItemInserir = new System.Windows.Forms.ToolStripMenuItem();
      this.menuItemEditar = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.menuItemExcluirFavorito = new System.Windows.Forms.ToolStripMenuItem();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.btnInserir = new System.Windows.Forms.ToolStripButton();
      this.btnEditar = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.btnUp = new System.Windows.Forms.ToolStripButton();
      this.btnDown = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.btnRemover = new System.Windows.Forms.ToolStripButton();
      this.pnlTop.SuspendLayout();
      this.pnlBottom.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
      this.menuGrid.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // pnlTop
      // 
      this.pnlTop.Controls.Add(this.edtServidor);
      this.pnlTop.Controls.Add(this.label2);
      resources.ApplyResources(this.pnlTop, "pnlTop");
      this.pnlTop.Name = "pnlTop";
      // 
      // edtServidor
      // 
      resources.ApplyResources(this.edtServidor, "edtServidor");
      this.edtServidor.Name = "edtServidor";
      this.edtServidor.ReadOnly = true;
      this.edtServidor.TabStop = false;
      // 
      // label2
      // 
      resources.ApplyResources(this.label2, "label2");
      this.label2.Name = "label2";
      // 
      // pnlBottom
      // 
      this.pnlBottom.Controls.Add(this.btnFechar);
      resources.ApplyResources(this.pnlBottom, "pnlBottom");
      this.pnlBottom.Name = "pnlBottom";
      // 
      // btnFechar
      // 
      resources.ApplyResources(this.btnFechar, "btnFechar");
      this.btnFechar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnFechar.Name = "btnFechar";
      this.toolTip1.SetToolTip(this.btnFechar, resources.GetString("btnFechar.ToolTip"));
      this.btnFechar.UseVisualStyleBackColor = true;
      // 
      // grid
      // 
      this.grid.AllowUserToAddRows = false;
      this.grid.AllowUserToDeleteRows = false;
      this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ServerItem,
            this.Caption,
            this.Index});
      this.grid.ContextMenuStrip = this.menuGrid;
      resources.ApplyResources(this.grid, "grid");
      this.grid.MultiSelect = false;
      this.grid.Name = "grid";
      this.grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
      this.grid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.grid_CellValidating);
      this.grid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grid_DataError);
      this.grid.SelectionChanged += new System.EventHandler(this.grid_SelectionChanged);
      this.grid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grid_MouseDown);
      // 
      // ServerItem
      // 
      this.ServerItem.DataPropertyName = "ServerItem";
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ServerItem.DefaultCellStyle = dataGridViewCellStyle1;
      resources.ApplyResources(this.ServerItem, "ServerItem");
      this.ServerItem.Name = "ServerItem";
      // 
      // Caption
      // 
      this.Caption.DataPropertyName = "Caption";
      resources.ApplyResources(this.Caption, "Caption");
      this.Caption.Name = "Caption";
      // 
      // Index
      // 
      this.Index.DataPropertyName = "Index";
      resources.ApplyResources(this.Index, "Index");
      this.Index.Name = "Index";
      // 
      // menuGrid
      // 
      this.menuGrid.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.menuGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemInserir,
            this.menuItemEditar,
            this.toolStripMenuItem1,
            this.menuItemExcluirFavorito});
      this.menuGrid.Name = "menuGrid";
      resources.ApplyResources(this.menuGrid, "menuGrid");
      this.menuGrid.Opening += new System.ComponentModel.CancelEventHandler(this.menuGrid_Opening);
      // 
      // menuItemInserir
      // 
      this.menuItemInserir.Name = "menuItemInserir";
      resources.ApplyResources(this.menuItemInserir, "menuItemInserir");
      this.menuItemInserir.Click += new System.EventHandler(this.btnInserir_Click);
      // 
      // menuItemEditar
      // 
      this.menuItemEditar.Name = "menuItemEditar";
      resources.ApplyResources(this.menuItemEditar, "menuItemEditar");
      this.menuItemEditar.Click += new System.EventHandler(this.menuItemEditar_Click);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
      // 
      // menuItemExcluirFavorito
      // 
      this.menuItemExcluirFavorito.Name = "menuItemExcluirFavorito";
      resources.ApplyResources(this.menuItemExcluirFavorito, "menuItemExcluirFavorito");
      this.menuItemExcluirFavorito.Click += new System.EventHandler(this.btnRemover_Click);
      // 
      // toolStrip1
      // 
      this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnInserir,
            this.btnEditar,
            this.toolStripSeparator1,
            this.btnUp,
            this.btnDown,
            this.toolStripSeparator2,
            this.btnRemover});
      resources.ApplyResources(this.toolStrip1, "toolStrip1");
      this.toolStrip1.Name = "toolStrip1";
      // 
      // btnInserir
      // 
      this.btnInserir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.btnInserir.Image = global::WSPack.Lib.Properties.ResourcesLib.imgCreate;
      resources.ApplyResources(this.btnInserir, "btnInserir");
      this.btnInserir.Name = "btnInserir";
      this.btnInserir.Click += new System.EventHandler(this.btnInserir_Click);
      // 
      // btnEditar
      // 
      this.btnEditar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.btnEditar.Image = global::WSPack.Lib.Properties.ResourcesLib.imgPencil;
      resources.ApplyResources(this.btnEditar, "btnEditar");
      this.btnEditar.Name = "btnEditar";
      this.btnEditar.Click += new System.EventHandler(this.menuItemEditar_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
      // 
      // btnUp
      // 
      this.btnUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      resources.ApplyResources(this.btnUp, "btnUp");
      this.btnUp.Name = "btnUp";
      this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
      // 
      // btnDown
      // 
      this.btnDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      resources.ApplyResources(this.btnDown, "btnDown");
      this.btnDown.Name = "btnDown";
      this.btnDown.Click += new System.EventHandler(this.btnUp_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
      // 
      // btnRemover
      // 
      this.btnRemover.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.btnRemover.Image = global::WSPack.Lib.Properties.ResourcesLib.imgDelete;
      resources.ApplyResources(this.btnRemover, "btnRemover");
      this.btnRemover.Name = "btnRemover";
      this.btnRemover.Click += new System.EventHandler(this.btnRemover_Click);
      // 
      // TFSFavoritesManagerForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.grid);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.pnlBottom);
      this.Controls.Add(this.pnlTop);
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "TFSFavoritesManagerForm";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TFSFavoritesManagerForm_FormClosed);
      this.Load += new System.EventHandler(this.TFSFavoritesManagerForm_Load);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TFSFavoritesManagerForm_KeyDown);
      this.pnlTop.ResumeLayout(false);
      this.pnlTop.PerformLayout();
      this.pnlBottom.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
      this.menuGrid.ResumeLayout(false);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel pnlTop;
    private System.Windows.Forms.TextBox edtServidor;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Panel pnlBottom;
    private System.Windows.Forms.Button btnFechar;
    private System.Windows.Forms.DataGridView grid;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.ContextMenuStrip menuGrid;
    private System.Windows.Forms.ToolStripMenuItem menuItemExcluirFavorito;
    private System.Windows.Forms.DataGridViewTextBoxColumn ServerItem;
    private System.Windows.Forms.DataGridViewTextBoxColumn Caption;
    private System.Windows.Forms.DataGridViewTextBoxColumn Index;
    private System.Windows.Forms.ToolStripMenuItem menuItemEditar;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton btnInserir;
    private System.Windows.Forms.ToolStripButton btnRemover;
    private System.Windows.Forms.ToolStripButton btnEditar;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton btnUp;
    private System.Windows.Forms.ToolStripButton btnDown;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem menuItemInserir;
  }
}