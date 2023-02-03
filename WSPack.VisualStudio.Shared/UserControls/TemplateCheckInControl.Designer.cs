namespace WSPack.VisualStudio.Shared.UserControls
{
  partial class TemplateCheckInControl
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
      this.toolTipProvider = new System.Windows.Forms.ToolTip(this.components);
      this.lbListaArquivos = new System.Windows.Forms.Label();
      this.menuMacros = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.menuItemCopiar = new System.Windows.Forms.ToolStripMenuItem();
      this.lbComentario = new System.Windows.Forms.Label();
      this.lbUsuario = new System.Windows.Forms.Label();
      this.lbDataCheckIn = new System.Windows.Forms.Label();
      this.lbChagesetId = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.memoTemplate = new System.Windows.Forms.RichTextBox();
      this.menuMemo = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.panel1 = new System.Windows.Forms.Panel();
      this.gbxMacros = new System.Windows.Forms.GroupBox();
      this.menuMacros.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.gbxMacros.SuspendLayout();
      this.SuspendLayout();
      // 
      // lbListaArquivos
      // 
      this.lbListaArquivos.AutoSize = true;
      this.lbListaArquivos.ContextMenuStrip = this.menuMacros;
      this.lbListaArquivos.Font = new System.Drawing.Font("Courier New", 8.25F);
      this.lbListaArquivos.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lbListaArquivos.Location = new System.Drawing.Point(168, 41);
      this.lbListaArquivos.Name = "lbListaArquivos";
      this.lbListaArquivos.Size = new System.Drawing.Size(112, 14);
      this.lbListaArquivos.TabIndex = 0;
      this.lbListaArquivos.Text = "_ListaArquivos_";
      this.toolTipProvider.SetToolTip(this.lbListaArquivos, "Lista de arquivos do Check In");
      this.lbListaArquivos.DoubleClick += new System.EventHandler(this.lbChagesetId_DoubleClick);
      this.lbListaArquivos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbChagesetId_MouseDown);
      // 
      // menuMacros
      // 
      this.menuMacros.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCopiar});
      this.menuMacros.Name = "menuMacros";
      this.menuMacros.Size = new System.Drawing.Size(110, 26);
      this.menuMacros.Opening += new System.ComponentModel.CancelEventHandler(this.menuMacros_Opening);
      // 
      // menuItemCopiar
      // 
      this.menuItemCopiar.Name = "menuItemCopiar";
      this.menuItemCopiar.Size = new System.Drawing.Size(109, 22);
      this.menuItemCopiar.Text = "Copiar";
      this.menuItemCopiar.Click += new System.EventHandler(this.menuItemCopiar_Click);
      // 
      // lbComentario
      // 
      this.lbComentario.AutoSize = true;
      this.lbComentario.ContextMenuStrip = this.menuMacros;
      this.lbComentario.Font = new System.Drawing.Font("Courier New", 8.25F);
      this.lbComentario.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lbComentario.Location = new System.Drawing.Point(9, 41);
      this.lbComentario.Name = "lbComentario";
      this.lbComentario.Size = new System.Drawing.Size(91, 14);
      this.lbComentario.TabIndex = 0;
      this.lbComentario.Text = "_Comentario_";
      this.toolTipProvider.SetToolTip(this.lbComentario, "Comentário do Check In");
      this.lbComentario.DoubleClick += new System.EventHandler(this.lbChagesetId_DoubleClick);
      this.lbComentario.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbChagesetId_MouseDown);
      // 
      // lbUsuario
      // 
      this.lbUsuario.AutoSize = true;
      this.lbUsuario.ContextMenuStrip = this.menuMacros;
      this.lbUsuario.Font = new System.Drawing.Font("Courier New", 8.25F);
      this.lbUsuario.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lbUsuario.Location = new System.Drawing.Point(327, 22);
      this.lbUsuario.Name = "lbUsuario";
      this.lbUsuario.Size = new System.Drawing.Size(70, 14);
      this.lbUsuario.TabIndex = 0;
      this.lbUsuario.Text = "_Usuario_";
      this.toolTipProvider.SetToolTip(this.lbUsuario, "Usuário que fez o Check In");
      this.lbUsuario.DoubleClick += new System.EventHandler(this.lbChagesetId_DoubleClick);
      this.lbUsuario.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbChagesetId_MouseDown);
      // 
      // lbDataCheckIn
      // 
      this.lbDataCheckIn.AutoSize = true;
      this.lbDataCheckIn.ContextMenuStrip = this.menuMacros;
      this.lbDataCheckIn.Font = new System.Drawing.Font("Courier New", 8.25F);
      this.lbDataCheckIn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lbDataCheckIn.Location = new System.Drawing.Point(168, 22);
      this.lbDataCheckIn.Name = "lbDataCheckIn";
      this.lbDataCheckIn.Size = new System.Drawing.Size(98, 14);
      this.lbDataCheckIn.TabIndex = 0;
      this.lbDataCheckIn.Text = "_DataCheckIn_";
      this.toolTipProvider.SetToolTip(this.lbDataCheckIn, "Data/hora do Check In");
      this.lbDataCheckIn.DoubleClick += new System.EventHandler(this.lbChagesetId_DoubleClick);
      this.lbDataCheckIn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbChagesetId_MouseDown);
      // 
      // lbChagesetId
      // 
      this.lbChagesetId.AutoSize = true;
      this.lbChagesetId.ContextMenuStrip = this.menuMacros;
      this.lbChagesetId.Font = new System.Drawing.Font("Courier New", 8.25F);
      this.lbChagesetId.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lbChagesetId.Location = new System.Drawing.Point(9, 22);
      this.lbChagesetId.Name = "lbChagesetId";
      this.lbChagesetId.Size = new System.Drawing.Size(98, 14);
      this.lbChagesetId.TabIndex = 0;
      this.lbChagesetId.Text = "_ChangesetId_";
      this.toolTipProvider.SetToolTip(this.lbChagesetId, "Identificador do Changeset");
      this.lbChagesetId.DoubleClick += new System.EventHandler(this.lbChagesetId_DoubleClick);
      this.lbChagesetId.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbChagesetId_MouseDown);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.memoTemplate);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(554, 197);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Crie o texto desejado para composição das informações de um Check In:";
      this.toolTipProvider.SetToolTip(this.groupBox1, "Pressione Ctrl+Enter para quebras de linhas");
      // 
      // memoTemplate
      // 
      this.memoTemplate.AcceptsTab = true;
      this.memoTemplate.ContextMenuStrip = this.menuMemo;
      this.memoTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
      this.memoTemplate.Font = new System.Drawing.Font("Courier New", 9.75F);
      this.memoTemplate.Location = new System.Drawing.Point(3, 16);
      this.memoTemplate.Name = "memoTemplate";
      this.memoTemplate.Size = new System.Drawing.Size(548, 178);
      this.memoTemplate.TabIndex = 1;
      this.memoTemplate.Text = "";
      this.toolTipProvider.SetToolTip(this.memoTemplate, "Pressione Ctrl+Enter para quebras de linhas");
      this.memoTemplate.WordWrap = false;
      this.memoTemplate.TextChanged += new System.EventHandler(this.memoTemplate_TextChanged);
      // 
      // menuMemo
      // 
      this.menuMemo.Font = new System.Drawing.Font("Courier New", 9F);
      this.menuMemo.Name = "menuMemo";
      this.menuMemo.Size = new System.Drawing.Size(61, 4);
      // 
      // panel1
      // 
      this.panel1.AutoScroll = true;
      this.panel1.Controls.Add(this.gbxMacros);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel1.Location = new System.Drawing.Point(0, 197);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(554, 70);
      this.panel1.TabIndex = 2;
      // 
      // gbxMacros
      // 
      this.gbxMacros.Controls.Add(this.lbListaArquivos);
      this.gbxMacros.Controls.Add(this.lbComentario);
      this.gbxMacros.Controls.Add(this.lbUsuario);
      this.gbxMacros.Controls.Add(this.lbDataCheckIn);
      this.gbxMacros.Controls.Add(this.lbChagesetId);
      this.gbxMacros.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gbxMacros.Location = new System.Drawing.Point(0, 0);
      this.gbxMacros.Name = "gbxMacros";
      this.gbxMacros.Size = new System.Drawing.Size(554, 70);
      this.gbxMacros.TabIndex = 0;
      this.gbxMacros.TabStop = false;
      this.gbxMacros.Text = "Macros disponíveis:";
      // 
      // TemplateCheckInControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.panel1);
      this.Name = "TemplateCheckInControl";
      this.Size = new System.Drawing.Size(554, 267);
      this.menuMacros.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.gbxMacros.ResumeLayout(false);
      this.gbxMacros.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ToolTip toolTipProvider;
    private System.Windows.Forms.ContextMenuStrip menuMacros;
    private System.Windows.Forms.ToolStripMenuItem menuItemCopiar;
    private System.Windows.Forms.ContextMenuStrip menuMemo;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.RichTextBox memoTemplate;
    private System.Windows.Forms.GroupBox gbxMacros;
    private System.Windows.Forms.Label lbListaArquivos;
    private System.Windows.Forms.Label lbComentario;
    private System.Windows.Forms.Label lbUsuario;
    private System.Windows.Forms.Label lbDataCheckIn;
    private System.Windows.Forms.Label lbChagesetId;
    private System.Windows.Forms.Panel panel1;
  }
}
