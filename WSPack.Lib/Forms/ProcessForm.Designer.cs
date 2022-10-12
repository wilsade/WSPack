namespace WSPack.Lib.Forms
{
  /// <summary>
  /// Form para processamento
  /// </summary>
  partial class ProcessForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessForm));
      this.lbDescricao = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // lbDescricao
      // 
      this.lbDescricao.Cursor = System.Windows.Forms.Cursors.WaitCursor;
      resources.ApplyResources(this.lbDescricao, "lbDescricao");
      this.lbDescricao.Name = "lbDescricao";
      // 
      // ProcessForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ControlBox = false;
      this.Controls.Add(this.lbDescricao);
      this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ProcessForm";
      this.Activated += new System.EventHandler(this.ProcessForm_Activated);
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProcessForm_FormClosing);
      this.Load += new System.EventHandler(this.ProcessForm_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lbDescricao;
  }
}