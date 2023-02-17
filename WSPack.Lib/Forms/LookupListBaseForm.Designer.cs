namespace WSPack.Lib.Forms
{
  partial class LookupListBaseForm
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LookupListBaseForm));
      this.edtItem = new System.Windows.Forms.TextBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.grid = new System.Windows.Forms.DataGridView();
      this.Nome = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Caminho = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
      this.statusBar = new System.Windows.Forms.StatusStrip();
      this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
      this.statusBar.SuspendLayout();
      this.SuspendLayout();
      // 
      // edtItem
      // 
      this.edtItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.edtItem.Location = new System.Drawing.Point(8, 20);
      this.edtItem.Name = "edtItem";
      this.edtItem.Size = new System.Drawing.Size(720, 20);
      this.edtItem.TabIndex = 1;
      this.edtItem.TextChanged += new System.EventHandler(this.edtItem_TextChanged);
      this.edtItem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edtItem_KeyDown);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.edtItem);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
      this.groupBox1.Location = new System.Drawing.Point(0, 9);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(740, 54);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Pesquise o item a ser localizado:";
      // 
      // grid
      // 
      this.grid.AllowUserToAddRows = false;
      this.grid.AllowUserToDeleteRows = false;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
      this.grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.grid.AutoGenerateColumns = false;
      this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Nome,
            this.Caminho});
      this.grid.DataSource = this.bindingSource1;
      this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.grid.Location = new System.Drawing.Point(0, 63);
      this.grid.MultiSelect = false;
      this.grid.Name = "grid";
      this.grid.ReadOnly = true;
      this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.grid.Size = new System.Drawing.Size(740, 274);
      this.grid.TabIndex = 1;
      this.grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
      // 
      // Nome
      // 
      this.Nome.DataPropertyName = "Nome";
      this.Nome.HeaderText = "Nome";
      this.Nome.Name = "Nome";
      this.Nome.ReadOnly = true;
      // 
      // Caminho
      // 
      this.Caminho.DataPropertyName = "Caminho";
      this.Caminho.HeaderText = "Caminho";
      this.Caminho.Name = "Caminho";
      this.Caminho.ReadOnly = true;
      // 
      // statusBar
      // 
      this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
      this.statusBar.Location = new System.Drawing.Point(0, 315);
      this.statusBar.Name = "statusBar";
      this.statusBar.Size = new System.Drawing.Size(740, 22);
      this.statusBar.TabIndex = 5;
      this.statusBar.Text = "statusStrip1";
      this.statusBar.Visible = false;
      // 
      // statusLabel
      // 
      this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.statusLabel.ForeColor = System.Drawing.Color.Blue;
      this.statusLabel.Name = "statusLabel";
      this.statusLabel.Size = new System.Drawing.Size(127, 17);
      this.statusLabel.Text = "toolStripStatusLabel1";
      // 
      // LookupListBaseForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(740, 384);
      this.Controls.Add(this.grid);
      this.Controls.Add(this.statusBar);
      this.Controls.Add(this.groupBox1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(530, 250);
      this.Name = "LookupListBaseForm";
      this.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
      this.Text = "Localizar um item";
      this.Shown += new System.EventHandler(this.LookupListBaseForm_Shown);
      this.Controls.SetChildIndex(this.groupBox1, 0);
      this.Controls.SetChildIndex(this.statusBar, 0);
      this.Controls.SetChildIndex(this.grid, 0);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
      this.statusBar.ResumeLayout(false);
      this.statusBar.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.TextBox edtItem;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.DataGridView grid;
    private System.Windows.Forms.BindingSource bindingSource1;
    private System.Windows.Forms.DataGridViewTextBoxColumn Nome;
    private System.Windows.Forms.DataGridViewTextBoxColumn Caminho;
    private System.Windows.Forms.StatusStrip statusBar;
    private System.Windows.Forms.ToolStripStatusLabel statusLabel;
  }
}