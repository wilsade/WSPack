namespace WSPack.Lib.Forms
{
  partial class BaseForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseForm));
      this.panel1 = new System.Windows.Forms.Panel();
      this.btnCancelar = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.btnCancelar);
      this.panel1.Controls.Add(this.btnOK);
      resources.ApplyResources(this.panel1, "panel1");
      this.panel1.Name = "panel1";
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
      // BaseForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panel1);
      this.KeyPreview = true;
      this.Name = "BaseForm";
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BaseForm_KeyDown);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button btnCancelar;
    private System.Windows.Forms.Button btnOK;
  }
}