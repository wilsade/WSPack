namespace WSPack.Lib.Forms
{
  partial class MessageBoxTesterForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageBoxTesterForm));
      this.label1 = new System.Windows.Forms.Label();
      this.edtTitulo = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.memoTexto = new System.Windows.Forms.RichTextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.cbBotoes = new System.Windows.Forms.ComboBox();
      this.btnTestarMessage = new System.Windows.Forms.Button();
      this.label4 = new System.Windows.Forms.Label();
      this.cbIcone = new System.Windows.Forms.ComboBox();
      this.label5 = new System.Windows.Forms.Label();
      this.cbBotaoDefault = new System.Windows.Forms.ComboBox();
      this.btnFechar = new System.Windows.Forms.Button();
      this.btnCodigo = new System.Windows.Forms.Button();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.cbxIncluirNamespace = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // label1
      // 
      resources.ApplyResources(this.label1, "label1");
      this.label1.Name = "label1";
      // 
      // edtTitulo
      // 
      resources.ApplyResources(this.edtTitulo, "edtTitulo");
      this.edtTitulo.Name = "edtTitulo";
      this.edtTitulo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edtTitulo_KeyDown);
      // 
      // label2
      // 
      resources.ApplyResources(this.label2, "label2");
      this.label2.Name = "label2";
      // 
      // memoTexto
      // 
      resources.ApplyResources(this.memoTexto, "memoTexto");
      this.memoTexto.Name = "memoTexto";
      // 
      // label3
      // 
      resources.ApplyResources(this.label3, "label3");
      this.label3.Name = "label3";
      // 
      // cbBotoes
      // 
      this.cbBotoes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbBotoes.FormattingEnabled = true;
      resources.ApplyResources(this.cbBotoes, "cbBotoes");
      this.cbBotoes.Name = "cbBotoes";
      // 
      // btnTestarMessage
      // 
      resources.ApplyResources(this.btnTestarMessage, "btnTestarMessage");
      this.btnTestarMessage.Name = "btnTestarMessage";
      this.btnTestarMessage.UseVisualStyleBackColor = true;
      this.btnTestarMessage.Click += new System.EventHandler(this.btnTestarMessage_Click);
      // 
      // label4
      // 
      resources.ApplyResources(this.label4, "label4");
      this.label4.Name = "label4";
      // 
      // cbIcone
      // 
      this.cbIcone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbIcone.FormattingEnabled = true;
      resources.ApplyResources(this.cbIcone, "cbIcone");
      this.cbIcone.Name = "cbIcone";
      // 
      // label5
      // 
      resources.ApplyResources(this.label5, "label5");
      this.label5.Name = "label5";
      // 
      // cbBotaoDefault
      // 
      this.cbBotaoDefault.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbBotaoDefault.FormattingEnabled = true;
      resources.ApplyResources(this.cbBotaoDefault, "cbBotaoDefault");
      this.cbBotaoDefault.Name = "cbBotaoDefault";
      // 
      // btnFechar
      // 
      this.btnFechar.DialogResult = System.Windows.Forms.DialogResult.OK;
      resources.ApplyResources(this.btnFechar, "btnFechar");
      this.btnFechar.Name = "btnFechar";
      this.btnFechar.UseVisualStyleBackColor = true;
      // 
      // btnCodigo
      // 
      resources.ApplyResources(this.btnCodigo, "btnCodigo");
      this.btnCodigo.Name = "btnCodigo";
      this.toolTip1.SetToolTip(this.btnCodigo, resources.GetString("btnCodigo.ToolTip"));
      this.btnCodigo.UseVisualStyleBackColor = true;
      this.btnCodigo.Click += new System.EventHandler(this.btnCodigo_Click);
      // 
      // cbxIncluirNamespace
      // 
      resources.ApplyResources(this.cbxIncluirNamespace, "cbxIncluirNamespace");
      this.cbxIncluirNamespace.Name = "cbxIncluirNamespace";
      this.toolTip1.SetToolTip(this.cbxIncluirNamespace, resources.GetString("cbxIncluirNamespace.ToolTip"));
      this.cbxIncluirNamespace.UseVisualStyleBackColor = true;
      // 
      // MessageBoxTesterForm
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.cbxIncluirNamespace);
      this.Controls.Add(this.btnFechar);
      this.Controls.Add(this.btnCodigo);
      this.Controls.Add(this.btnTestarMessage);
      this.Controls.Add(this.cbBotaoDefault);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.cbIcone);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.cbBotoes);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.memoTexto);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.edtTitulo);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "MessageBoxTesterForm";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.Shown += new System.EventHandler(this.MessageBoxTesterForm_Shown);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MessageBoxTesterForm_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox edtTitulo;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.RichTextBox memoTexto;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox cbBotoes;
    private System.Windows.Forms.Button btnTestarMessage;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ComboBox cbIcone;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.ComboBox cbBotaoDefault;
    private System.Windows.Forms.Button btnFechar;
    private System.Windows.Forms.Button btnCodigo;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.CheckBox cbxIncluirNamespace;
  }
}

