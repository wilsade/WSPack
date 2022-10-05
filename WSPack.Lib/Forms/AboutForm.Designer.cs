namespace WSPack.Lib.Forms
{
  public partial class AboutForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
      this.panel1 = new System.Windows.Forms.Panel();
      this.linkNomeProduto = new System.Windows.Forms.LinkLabel();
      this.labelCompanyName = new System.Windows.Forms.Label();
      this.labelCopyright = new System.Windows.Forms.Label();
      this.labelVersion = new System.Windows.Forms.Label();
      this.lbProduto = new System.Windows.Forms.Label();
      this.logoPictureBox = new System.Windows.Forms.PictureBox();
      this.panel2 = new System.Windows.Forms.Panel();
      this.btnOK = new System.Windows.Forms.Button();
      this.panel3 = new System.Windows.Forms.Panel();
      this.textBoxDescription = new System.Windows.Forms.RichTextBox();
      this.lbNumeroVersao = new System.Windows.Forms.Label();
      this.lbDireitos = new System.Windows.Forms.Label();
      this.lbEmpresa = new System.Windows.Forms.Label();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.lbEmpresa);
      this.panel1.Controls.Add(this.lbDireitos);
      this.panel1.Controls.Add(this.lbNumeroVersao);
      this.panel1.Controls.Add(this.linkNomeProduto);
      this.panel1.Controls.Add(this.labelCompanyName);
      this.panel1.Controls.Add(this.labelCopyright);
      this.panel1.Controls.Add(this.labelVersion);
      this.panel1.Controls.Add(this.lbProduto);
      this.panel1.Controls.Add(this.logoPictureBox);
      resources.ApplyResources(this.panel1, "panel1");
      this.panel1.Name = "panel1";
      // 
      // linkNomeProduto
      // 
      resources.ApplyResources(this.linkNomeProduto, "linkNomeProduto");
      this.linkNomeProduto.Name = "linkNomeProduto";
      this.linkNomeProduto.TabStop = true;
      // 
      // labelCompanyName
      // 
      resources.ApplyResources(this.labelCompanyName, "labelCompanyName");
      this.labelCompanyName.Name = "labelCompanyName";
      // 
      // labelCopyright
      // 
      resources.ApplyResources(this.labelCopyright, "labelCopyright");
      this.labelCopyright.Name = "labelCopyright";
      // 
      // labelVersion
      // 
      resources.ApplyResources(this.labelVersion, "labelVersion");
      this.labelVersion.Name = "labelVersion";
      // 
      // lbProduto
      // 
      resources.ApplyResources(this.lbProduto, "lbProduto");
      this.lbProduto.Name = "lbProduto";
      this.lbProduto.TabStop = true;
      // 
      // logoPictureBox
      // 
      resources.ApplyResources(this.logoPictureBox, "logoPictureBox");
      this.logoPictureBox.Name = "logoPictureBox";
      this.logoPictureBox.TabStop = false;
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.btnOK);
      resources.ApplyResources(this.panel2, "panel2");
      this.panel2.Name = "panel2";
      // 
      // btnOK
      // 
      resources.ApplyResources(this.btnOK, "btnOK");
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Name = "btnOK";
      this.btnOK.UseVisualStyleBackColor = true;
      // 
      // panel3
      // 
      this.panel3.Controls.Add(this.textBoxDescription);
      resources.ApplyResources(this.panel3, "panel3");
      this.panel3.Name = "panel3";
      // 
      // textBoxDescription
      // 
      resources.ApplyResources(this.textBoxDescription, "textBoxDescription");
      this.textBoxDescription.Name = "textBoxDescription";
      this.textBoxDescription.ReadOnly = true;
      // 
      // lbNumeroVersao
      // 
      resources.ApplyResources(this.lbNumeroVersao, "lbNumeroVersao");
      this.lbNumeroVersao.Name = "lbNumeroVersao";
      // 
      // lbDireitos
      // 
      resources.ApplyResources(this.lbDireitos, "lbDireitos");
      this.lbDireitos.Name = "lbDireitos";
      // 
      // lbEmpresa
      // 
      resources.ApplyResources(this.lbEmpresa, "lbEmpresa");
      this.lbEmpresa.Name = "lbEmpresa";
      // 
      // AboutForm
      // 
      this.AcceptButton = this.btnOK;
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panel3);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutForm";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Load += new System.EventHandler(this.AboutForm_Load);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AboutForm_KeyDown);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
      this.panel2.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label labelCompanyName;
    private System.Windows.Forms.Label labelCopyright;
    private System.Windows.Forms.Label labelVersion;
    private System.Windows.Forms.Label lbProduto;
    private System.Windows.Forms.PictureBox logoPictureBox;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Panel panel3;
    private System.Windows.Forms.RichTextBox textBoxDescription;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.LinkLabel linkNomeProduto;
    private System.Windows.Forms.Label lbEmpresa;
    private System.Windows.Forms.Label lbDireitos;
    private System.Windows.Forms.Label lbNumeroVersao;
  }
}
