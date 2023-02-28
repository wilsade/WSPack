namespace WSPack.VisualStudio.Shared.UserControls
{
  /// <summary>
  /// DocumentationConditionControl
  /// </summary>
  /// <seealso cref="System.Windows.Forms.UserControl" />
  partial class DocumentationConditionControl
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
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.cbCondicoes = new System.Windows.Forms.ComboBox();
      this.cbxIgnoreCase = new System.Windows.Forms.CheckBox();
      this.edtNome = new System.Windows.Forms.TextBox();
      this.lbNome = new System.Windows.Forms.Label();
      this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.cbCondicoes);
      this.groupBox1.Controls.Add(this.cbxIgnoreCase);
      this.groupBox1.Controls.Add(this.edtNome);
      this.groupBox1.Controls.Add(this.lbNome);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(372, 100);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Definição da condição:";
      // 
      // cbCondicoes
      // 
      this.cbCondicoes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbCondicoes.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.cbCondicoes.FormattingEnabled = true;
      this.cbCondicoes.Location = new System.Drawing.Point(21, 42);
      this.cbCondicoes.Name = "cbCondicoes";
      this.cbCondicoes.Size = new System.Drawing.Size(110, 22);
      this.cbCondicoes.TabIndex = 1;
      this.cbCondicoes.SelectedIndexChanged += new System.EventHandler(this.cbCondicoes_SelectedIndexChanged);
      // 
      // cbxIgnoreCase
      // 
      this.cbxIgnoreCase.AutoSize = true;
      this.cbxIgnoreCase.Location = new System.Drawing.Point(21, 69);
      this.cbxIgnoreCase.Name = "cbxIgnoreCase";
      this.cbxIgnoreCase.Size = new System.Drawing.Size(179, 17);
      this.cbxIgnoreCase.TabIndex = 3;
      this.cbxIgnoreCase.Text = "Ignorar Maiúsculas / Minúsculas";
      this.cbxIgnoreCase.UseVisualStyleBackColor = true;
      // 
      // edtNome
      // 
      this.edtNome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      this.edtNome.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.edtNome.Location = new System.Drawing.Point(137, 42);
      this.edtNome.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
      this.edtNome.Name = "edtNome";
      this.edtNome.Size = new System.Drawing.Size(208, 21);
      this.edtNome.TabIndex = 2;
      this.edtNome.TextChanged += new System.EventHandler(this.edtNome_TextChanged);
      this.edtNome.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edtNome_KeyDown);
      // 
      // lbNome
      // 
      this.lbNome.AutoSize = true;
      this.lbNome.Location = new System.Drawing.Point(18, 26);
      this.lbNome.Name = "lbNome";
      this.lbNome.Size = new System.Drawing.Size(38, 13);
      this.lbNome.TabIndex = 0;
      this.lbNome.Text = "Nome:";
      // 
      // errorProvider1
      // 
      this.errorProvider1.ContainerControl = this;
      // 
      // DocumentationConditionControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "DocumentationConditionControl";
      this.Size = new System.Drawing.Size(372, 100);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.ComboBox cbCondicoes;
    private System.Windows.Forms.CheckBox cbxIgnoreCase;
    private System.Windows.Forms.TextBox edtNome;
    private System.Windows.Forms.Label lbNome;
    private System.Windows.Forms.ErrorProvider errorProvider1;
  }
}
