namespace WSPack.Lib.Forms
{
  partial class ProjectCommandLineArgsForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectCommandLineArgsForm));
      this.pnlBottom = new System.Windows.Forms.Panel();
      this.cbxQuebraLinhas = new System.Windows.Forms.CheckBox();
      this.btnCancelar = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.memoArgs = new System.Windows.Forms.RichTextBox();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.statusProjeto = new System.Windows.Forms.ToolStripStatusLabel();
      this.pnlBottom.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // pnlBottom
      // 
      this.pnlBottom.Controls.Add(this.cbxQuebraLinhas);
      this.pnlBottom.Controls.Add(this.btnCancelar);
      this.pnlBottom.Controls.Add(this.btnOK);
      resources.ApplyResources(this.pnlBottom, "pnlBottom");
      this.pnlBottom.Name = "pnlBottom";
      // 
      // cbxQuebraLinhas
      // 
      resources.ApplyResources(this.cbxQuebraLinhas, "cbxQuebraLinhas");
      this.cbxQuebraLinhas.Checked = true;
      this.cbxQuebraLinhas.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbxQuebraLinhas.Name = "cbxQuebraLinhas";
      this.cbxQuebraLinhas.UseVisualStyleBackColor = true;
      this.cbxQuebraLinhas.CheckedChanged += new System.EventHandler(this.cbxQuebraLinhas_CheckedChanged);
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
      // memoArgs
      // 
      resources.ApplyResources(this.memoArgs, "memoArgs");
      this.memoArgs.Name = "memoArgs";
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusProjeto});
      resources.ApplyResources(this.statusStrip1, "statusStrip1");
      this.statusStrip1.Name = "statusStrip1";
      // 
      // statusProjeto
      // 
      this.statusProjeto.Name = "statusProjeto";
      resources.ApplyResources(this.statusProjeto, "statusProjeto");
      // 
      // ProjectCommandLineArgsForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.memoArgs);
      this.Controls.Add(this.pnlBottom);
      this.Controls.Add(this.statusStrip1);
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ProjectCommandLineArgsForm";
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProjectCommandLineArgsForm_KeyDown);
      this.pnlBottom.ResumeLayout(false);
      this.pnlBottom.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel pnlBottom;
    private System.Windows.Forms.Button btnCancelar;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.RichTextBox memoArgs;
    private System.Windows.Forms.CheckBox cbxQuebraLinhas;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel statusProjeto;
  }
}