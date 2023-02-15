namespace WSPack.Lib.Forms
{
  partial class SolutionExplorerLocateItemForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolutionExplorerLocateItemForm));
      this.label1 = new System.Windows.Forms.Label();
      this.cbItem = new System.Windows.Forms.ComboBox();
      this.btnCarregarItens = new System.Windows.Forms.Button();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.panel2 = new System.Windows.Forms.Panel();
      this.lbTotalItens = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.rbTodosItens = new System.Windows.Forms.RadioButton();
      this.rbProjetos = new System.Windows.Forms.RadioButton();
      this.rbCSharp = new System.Windows.Forms.RadioButton();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.edtFiltro = new System.Windows.Forms.TextBox();
      this.rbContem = new System.Windows.Forms.RadioButton();
      this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
      this.panel2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
      this.SuspendLayout();
      // 
      // label1
      // 
      resources.ApplyResources(this.label1, "label1");
      this.label1.Name = "label1";
      // 
      // cbItem
      // 
      resources.ApplyResources(this.cbItem, "cbItem");
      this.cbItem.FormattingEnabled = true;
      this.cbItem.Name = "cbItem";
      this.toolTip1.SetToolTip(this.cbItem, resources.GetString("cbItem.ToolTip"));
      // 
      // btnCarregarItens
      // 
      resources.ApplyResources(this.btnCarregarItens, "btnCarregarItens");
      this.btnCarregarItens.Name = "btnCarregarItens";
      this.toolTip1.SetToolTip(this.btnCarregarItens, resources.GetString("btnCarregarItens.ToolTip"));
      this.btnCarregarItens.UseVisualStyleBackColor = true;
      this.btnCarregarItens.Click += new System.EventHandler(this.btnCarregarItens_Click);
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.lbTotalItens);
      this.panel2.Controls.Add(this.label2);
      this.panel2.Controls.Add(this.cbItem);
      this.panel2.Controls.Add(this.label1);
      this.panel2.Controls.Add(this.btnCarregarItens);
      resources.ApplyResources(this.panel2, "panel2");
      this.panel2.Name = "panel2";
      // 
      // lbTotalItens
      // 
      resources.ApplyResources(this.lbTotalItens, "lbTotalItens");
      this.lbTotalItens.Name = "lbTotalItens";
      // 
      // label2
      // 
      resources.ApplyResources(this.label2, "label2");
      this.label2.Name = "label2";
      // 
      // rbTodosItens
      // 
      resources.ApplyResources(this.rbTodosItens, "rbTodosItens");
      this.rbTodosItens.Checked = true;
      this.rbTodosItens.Name = "rbTodosItens";
      this.rbTodosItens.TabStop = true;
      this.rbTodosItens.UseVisualStyleBackColor = true;
      this.rbTodosItens.CheckedChanged += new System.EventHandler(this.rbTodosItens_CheckedChanged);
      // 
      // rbProjetos
      // 
      resources.ApplyResources(this.rbProjetos, "rbProjetos");
      this.rbProjetos.Name = "rbProjetos";
      this.rbProjetos.UseVisualStyleBackColor = true;
      this.rbProjetos.CheckedChanged += new System.EventHandler(this.rbTodosItens_CheckedChanged);
      // 
      // rbCSharp
      // 
      resources.ApplyResources(this.rbCSharp, "rbCSharp");
      this.rbCSharp.Name = "rbCSharp";
      this.rbCSharp.UseVisualStyleBackColor = true;
      this.rbCSharp.CheckedChanged += new System.EventHandler(this.rbTodosItens_CheckedChanged);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.edtFiltro);
      this.groupBox1.Controls.Add(this.rbContem);
      this.groupBox1.Controls.Add(this.rbCSharp);
      this.groupBox1.Controls.Add(this.rbTodosItens);
      this.groupBox1.Controls.Add(this.rbProjetos);
      resources.ApplyResources(this.groupBox1, "groupBox1");
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.TabStop = false;
      // 
      // edtFiltro
      // 
      resources.ApplyResources(this.edtFiltro, "edtFiltro");
      this.edtFiltro.Name = "edtFiltro";
      this.edtFiltro.TextChanged += new System.EventHandler(this.edtFiltro_TextChanged);
      // 
      // rbContem
      // 
      resources.ApplyResources(this.rbContem, "rbContem");
      this.rbContem.Name = "rbContem";
      this.rbContem.UseVisualStyleBackColor = true;
      this.rbContem.CheckedChanged += new System.EventHandler(this.rbTodosItens_CheckedChanged);
      // 
      // errorProvider1
      // 
      this.errorProvider1.ContainerControl = this;
      // 
      // SolutionExplorerLocateItemForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.panel2);
      this.Name = "SolutionExplorerLocateItemForm";
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SolutionExplorerLocateItemForm_KeyDown);
      this.Controls.SetChildIndex(this.panel2, 0);
      this.Controls.SetChildIndex(this.groupBox1, 0);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cbItem;
    private System.Windows.Forms.Button btnCarregarItens;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.RadioButton rbTodosItens;
    private System.Windows.Forms.RadioButton rbProjetos;
    private System.Windows.Forms.RadioButton rbCSharp;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox edtFiltro;
    private System.Windows.Forms.RadioButton rbContem;
    private System.Windows.Forms.Label lbTotalItens;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ErrorProvider errorProvider1;
  }
}