namespace WSPack.VisualStudio.Shared.UserControls
{
  partial class DocumentationRulesControl
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentationRulesControl));
      this.pnlBottom = new System.Windows.Forms.Panel();
      this.pnlRight = new System.Windows.Forms.Panel();
      this.btnDown = new System.Windows.Forms.Button();
      this.btnUp = new System.Windows.Forms.Button();
      this.btnExcluir = new System.Windows.Forms.Button();
      this.btnEditar = new System.Windows.Forms.Button();
      this.btnAdd = new System.Windows.Forms.Button();
      this.viewRegras = new System.Windows.Forms.TreeView();
      this.imageList1 = new System.Windows.Forms.ImageList(this.components);
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.pnlRight.SuspendLayout();
      this.SuspendLayout();
      // 
      // pnlBottom
      // 
      resources.ApplyResources(this.pnlBottom, "pnlBottom");
      this.pnlBottom.Name = "pnlBottom";
      // 
      // pnlRight
      // 
      this.pnlRight.Controls.Add(this.btnDown);
      this.pnlRight.Controls.Add(this.btnUp);
      this.pnlRight.Controls.Add(this.btnExcluir);
      this.pnlRight.Controls.Add(this.btnEditar);
      this.pnlRight.Controls.Add(this.btnAdd);
      resources.ApplyResources(this.pnlRight, "pnlRight");
      this.pnlRight.Name = "pnlRight";
      // 
      // btnDown
      // 
      resources.ApplyResources(this.btnDown, "btnDown");
      this.btnDown.Image = global::WSPack.Lib.Properties.ResourcesLib.pngArrow_Down_vs;
      this.btnDown.Name = "btnDown";
      this.toolTip1.SetToolTip(this.btnDown, resources.GetString("btnDown.ToolTip"));
      this.btnDown.UseVisualStyleBackColor = true;
      this.btnDown.Click += new System.EventHandler(this.btnUp_Click);
      // 
      // btnUp
      // 
      resources.ApplyResources(this.btnUp, "btnUp");
      this.btnUp.Image = global::WSPack.Lib.Properties.ResourcesLib.pngArrow_Up_vs;
      this.btnUp.Name = "btnUp";
      this.toolTip1.SetToolTip(this.btnUp, resources.GetString("btnUp.ToolTip"));
      this.btnUp.UseVisualStyleBackColor = true;
      this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
      // 
      // btnExcluir
      // 
      resources.ApplyResources(this.btnExcluir, "btnExcluir");
      this.btnExcluir.Name = "btnExcluir";
      this.toolTip1.SetToolTip(this.btnExcluir, resources.GetString("btnExcluir.ToolTip"));
      this.btnExcluir.UseVisualStyleBackColor = true;
      this.btnExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
      // 
      // btnEditar
      // 
      resources.ApplyResources(this.btnEditar, "btnEditar");
      this.btnEditar.Name = "btnEditar";
      this.toolTip1.SetToolTip(this.btnEditar, resources.GetString("btnEditar.ToolTip"));
      this.btnEditar.UseVisualStyleBackColor = true;
      this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
      // 
      // btnAdd
      // 
      resources.ApplyResources(this.btnAdd, "btnAdd");
      this.btnAdd.Name = "btnAdd";
      this.toolTip1.SetToolTip(this.btnAdd, resources.GetString("btnAdd.ToolTip"));
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
      // 
      // viewRegras
      // 
      resources.ApplyResources(this.viewRegras, "viewRegras");
      this.viewRegras.ImageList = this.imageList1;
      this.viewRegras.Name = "viewRegras";
      this.viewRegras.ShowNodeToolTips = true;
      this.viewRegras.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.viewRegras_AfterSelect);
      this.viewRegras.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.viewRegras_NodeMouseDoubleClick);
      this.viewRegras.MouseDown += new System.Windows.Forms.MouseEventHandler(this.viewRegras_MouseDown);
      // 
      // imageList1
      // 
      this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      resources.ApplyResources(this.imageList1, "imageList1");
      this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // DocumentationRulesControl
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.viewRegras);
      this.Controls.Add(this.pnlRight);
      this.Controls.Add(this.pnlBottom);
      this.Name = "DocumentationRulesControl";
      this.pnlRight.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel pnlBottom;
    private System.Windows.Forms.Panel pnlRight;
    private System.Windows.Forms.TreeView viewRegras;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Button btnEditar;
    private System.Windows.Forms.Button btnExcluir;
    private System.Windows.Forms.ImageList imageList1;
    private System.Windows.Forms.Button btnUp;
    private System.Windows.Forms.Button btnDown;
    private System.Windows.Forms.ToolTip toolTip1;
  }
}
