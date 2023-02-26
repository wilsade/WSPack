namespace WSPack.VisualStudio.Shared.Forms
{
  partial class DocumentationMacrosForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentationMacrosForm));
      this.pnlBottom = new System.Windows.Forms.Panel();
      this.btnCancelar = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.pnlTop = new System.Windows.Forms.Panel();
      this.cbxQuebrarLinhas = new System.Windows.Forms.CheckBox();
      this.edtTexto = new System.Windows.Forms.RichTextBox();
      this.gbxTree = new System.Windows.Forms.GroupBox();
      this.treeMacros = new System.Windows.Forms.TreeView();
      this.imageList1 = new System.Windows.Forms.ImageList(this.components);
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.pnlBottom.SuspendLayout();
      this.pnlTop.SuspendLayout();
      this.gbxTree.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      // 
      // pnlBottom
      // 
      this.pnlBottom.Controls.Add(this.btnCancelar);
      this.pnlBottom.Controls.Add(this.btnOK);
      resources.ApplyResources(this.pnlBottom, "pnlBottom");
      this.pnlBottom.Name = "pnlBottom";
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
      // 
      // label1
      // 
      resources.ApplyResources(this.label1, "label1");
      this.label1.Name = "label1";
      // 
      // pnlTop
      // 
      this.pnlTop.Controls.Add(this.cbxQuebrarLinhas);
      this.pnlTop.Controls.Add(this.edtTexto);
      this.pnlTop.Controls.Add(this.label1);
      resources.ApplyResources(this.pnlTop, "pnlTop");
      this.pnlTop.Name = "pnlTop";
      // 
      // cbxQuebrarLinhas
      // 
      resources.ApplyResources(this.cbxQuebrarLinhas, "cbxQuebrarLinhas");
      this.cbxQuebrarLinhas.Checked = true;
      this.cbxQuebrarLinhas.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbxQuebrarLinhas.Name = "cbxQuebrarLinhas";
      this.cbxQuebrarLinhas.UseVisualStyleBackColor = true;
      this.cbxQuebrarLinhas.CheckedChanged += new System.EventHandler(this.cbxQuebrarLinhas_CheckedChanged);
      // 
      // edtTexto
      // 
      resources.ApplyResources(this.edtTexto, "edtTexto");
      this.edtTexto.Name = "edtTexto";
      // 
      // gbxTree
      // 
      this.gbxTree.Controls.Add(this.treeMacros);
      resources.ApplyResources(this.gbxTree, "gbxTree");
      this.gbxTree.Name = "gbxTree";
      this.gbxTree.TabStop = false;
      // 
      // treeMacros
      // 
      resources.ApplyResources(this.treeMacros, "treeMacros");
      this.treeMacros.ImageList = this.imageList1;
      this.treeMacros.Name = "treeMacros";
      this.treeMacros.ShowNodeToolTips = true;
      this.treeMacros.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeMacros_MouseDoubleClick);
      // 
      // imageList1
      // 
      this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
      this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "CommentGroup_16x.png");
      this.imageList1.Images.SetKeyName(1, "Writeable_16x.png");
      // 
      // splitContainer1
      // 
      resources.ApplyResources(this.splitContainer1, "splitContainer1");
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.pnlTop);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.gbxTree);
      // 
      // DocumentationMacrosForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.pnlBottom);
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DocumentationMacrosForm";
      this.Shown += new System.EventHandler(this.DocumentationMacrosForm_Shown);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DocumentationMacrosForm_KeyDown);
      this.pnlBottom.ResumeLayout(false);
      this.pnlTop.ResumeLayout(false);
      this.pnlTop.PerformLayout();
      this.gbxTree.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel pnlBottom;
    private System.Windows.Forms.Button btnCancelar;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Panel pnlTop;
    private System.Windows.Forms.GroupBox gbxTree;
    private System.Windows.Forms.TreeView treeMacros;
    private System.Windows.Forms.ImageList imageList1;
    private System.Windows.Forms.RichTextBox edtTexto;
    private System.Windows.Forms.CheckBox cbxQuebrarLinhas;
    private System.Windows.Forms.SplitContainer splitContainer1;
  }
}