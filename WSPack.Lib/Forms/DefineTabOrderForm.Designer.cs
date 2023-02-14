namespace WSPack.Lib.Forms
{
  partial class DefineTabOrderForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefineTabOrderForm));
      this.panel1 = new System.Windows.Forms.Panel();
      this.lbControlePai = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.viewControles = new System.Windows.Forms.ListView();
      this.colControle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.btnUp = new System.Windows.Forms.Button();
      this.btnDown = new System.Windows.Forms.Button();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.lbControlePai);
      this.panel1.Controls.Add(this.label2);
      this.panel1.Controls.Add(this.label1);
      this.panel1.Controls.Add(this.btnCancel);
      this.panel1.Controls.Add(this.btnOK);
      resources.ApplyResources(this.panel1, "panel1");
      this.panel1.Name = "panel1";
      // 
      // lbControlePai
      // 
      resources.ApplyResources(this.lbControlePai, "lbControlePai");
      this.lbControlePai.ForeColor = System.Drawing.Color.Blue;
      this.lbControlePai.Name = "lbControlePai";
      // 
      // label2
      // 
      resources.ApplyResources(this.label2, "label2");
      this.label2.Name = "label2";
      // 
      // label1
      // 
      resources.ApplyResources(this.label1, "label1");
      this.label1.ForeColor = System.Drawing.Color.Green;
      this.label1.Name = "label1";
      this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
      // 
      // btnCancel
      // 
      resources.ApplyResources(this.btnCancel, "btnCancel");
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Name = "btnCancel";
      this.toolTip1.SetToolTip(this.btnCancel, resources.GetString("btnCancel.ToolTip"));
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      resources.ApplyResources(this.btnOK, "btnOK");
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Name = "btnOK";
      this.toolTip1.SetToolTip(this.btnOK, resources.GetString("btnOK.ToolTip"));
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // viewControles
      // 
      this.viewControles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colControle});
      resources.ApplyResources(this.viewControles, "viewControles");
      this.viewControles.MultiSelect = false;
      this.viewControles.Name = "viewControles";
      this.viewControles.UseCompatibleStateImageBehavior = false;
      this.viewControles.View = System.Windows.Forms.View.Details;
      this.viewControles.SelectedIndexChanged += new System.EventHandler(this.viewControles_SelectedIndexChanged);
      this.viewControles.DoubleClick += new System.EventHandler(this.viewControles_DoubleClick);
      this.viewControles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.viewControles_KeyDown);
      // 
      // colControle
      // 
      resources.ApplyResources(this.colControle, "colControle");
      // 
      // btnUp
      // 
      resources.ApplyResources(this.btnUp, "btnUp");
      this.btnUp.FlatAppearance.BorderSize = 0;
      this.btnUp.Name = "btnUp";
      this.toolTip1.SetToolTip(this.btnUp, resources.GetString("btnUp.ToolTip"));
      this.btnUp.UseVisualStyleBackColor = true;
      this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
      // 
      // btnDown
      // 
      resources.ApplyResources(this.btnDown, "btnDown");
      this.btnDown.FlatAppearance.BorderSize = 0;
      this.btnDown.Name = "btnDown";
      this.toolTip1.SetToolTip(this.btnDown, resources.GetString("btnDown.ToolTip"));
      this.btnDown.UseVisualStyleBackColor = true;
      this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
      // 
      // DefineTabOrderForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.btnDown);
      this.Controls.Add(this.btnUp);
      this.Controls.Add(this.viewControles);
      this.Controls.Add(this.panel1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DefineTabOrderForm";
      this.Load += new System.EventHandler(this.TabOrderForm_Load);
      this.Shown += new System.EventHandler(this.TabOrderForm_Shown);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TabOrderForm_KeyDown);
      this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TabOrderForm_KeyUp);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.ListView viewControles;
    private System.Windows.Forms.Button btnUp;
    private System.Windows.Forms.Button btnDown;
    private System.Windows.Forms.ColumnHeader colControle;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label lbControlePai;
    private System.Windows.Forms.Label label2;
  }
}