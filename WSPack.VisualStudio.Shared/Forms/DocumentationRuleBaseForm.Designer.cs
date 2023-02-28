namespace WSPack.VisualStudio.Shared.Forms
{
  /// <summary>
  /// DocumentationRuleBaseForm
  /// </summary>
  /// <seealso cref="System.Windows.Forms.Form" />
  partial class DocumentationRuleBaseForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentationRuleBaseForm));
      Lib.DocumentationObjects.ConditionItem conditionItem1 = new Lib.DocumentationObjects.ConditionItem();
      this.pnlBottom = new System.Windows.Forms.Panel();
      this.btnCancelar = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.gbxNomeRegra = new System.Windows.Forms.GroupBox();
      this.edtNomeRegra = new System.Windows.Forms.TextBox();
      this.lbNomeRegra = new System.Windows.Forms.Label();
      this.cbValidoPara = new System.Windows.Forms.ComboBox();
      this.lbValidoPara = new System.Windows.Forms.Label();
      this.gbxSummary = new System.Windows.Forms.GroupBox();
      this.btnMacroRemarks = new System.Windows.Forms.Button();
      this.btnMacroReturns = new System.Windows.Forms.Button();
      this.btnMacroSummary = new System.Windows.Forms.Button();
      this.edtRemarks = new System.Windows.Forms.TextBox();
      this.lbRemarks = new System.Windows.Forms.Label();
      this.edtReturns = new System.Windows.Forms.TextBox();
      this.lbReturns = new System.Windows.Forms.Label();
      this.edtSummary = new System.Windows.Forms.TextBox();
      this.lbSummary = new System.Windows.Forms.Label();
      this.controleCondicao = new UserControls.DocumentationConditionControl();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.pnlBottom.SuspendLayout();
      this.gbxNomeRegra.SuspendLayout();
      this.gbxSummary.SuspendLayout();
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
      // gbxNomeRegra
      // 
      this.gbxNomeRegra.Controls.Add(this.edtNomeRegra);
      this.gbxNomeRegra.Controls.Add(this.lbNomeRegra);
      this.gbxNomeRegra.Controls.Add(this.cbValidoPara);
      this.gbxNomeRegra.Controls.Add(this.lbValidoPara);
      resources.ApplyResources(this.gbxNomeRegra, "gbxNomeRegra");
      this.gbxNomeRegra.Name = "gbxNomeRegra";
      this.gbxNomeRegra.TabStop = false;
      // 
      // edtNomeRegra
      // 
      resources.ApplyResources(this.edtNomeRegra, "edtNomeRegra");
      this.edtNomeRegra.Name = "edtNomeRegra";
      this.edtNomeRegra.TextChanged += new System.EventHandler(this.edtNomeRegra_TextChanged);
      // 
      // lbNomeRegra
      // 
      resources.ApplyResources(this.lbNomeRegra, "lbNomeRegra");
      this.lbNomeRegra.Name = "lbNomeRegra";
      // 
      // cbValidoPara
      // 
      this.cbValidoPara.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      resources.ApplyResources(this.cbValidoPara, "cbValidoPara");
      this.cbValidoPara.FormattingEnabled = true;
      this.cbValidoPara.Name = "cbValidoPara";
      this.cbValidoPara.SelectedIndexChanged += new System.EventHandler(this.cbValidoPara_SelectedIndexChanged);
      // 
      // lbValidoPara
      // 
      resources.ApplyResources(this.lbValidoPara, "lbValidoPara");
      this.lbValidoPara.Name = "lbValidoPara";
      // 
      // gbxSummary
      // 
      this.gbxSummary.Controls.Add(this.btnMacroRemarks);
      this.gbxSummary.Controls.Add(this.btnMacroReturns);
      this.gbxSummary.Controls.Add(this.btnMacroSummary);
      this.gbxSummary.Controls.Add(this.edtRemarks);
      this.gbxSummary.Controls.Add(this.lbRemarks);
      this.gbxSummary.Controls.Add(this.edtReturns);
      this.gbxSummary.Controls.Add(this.lbReturns);
      this.gbxSummary.Controls.Add(this.edtSummary);
      this.gbxSummary.Controls.Add(this.lbSummary);
      resources.ApplyResources(this.gbxSummary, "gbxSummary");
      this.gbxSummary.Name = "gbxSummary";
      this.gbxSummary.TabStop = false;
      // 
      // btnMacroRemarks
      // 
      resources.ApplyResources(this.btnMacroRemarks, "btnMacroRemarks");
      this.btnMacroRemarks.Name = "btnMacroRemarks";
      this.btnMacroRemarks.UseVisualStyleBackColor = true;
      this.btnMacroRemarks.Click += new System.EventHandler(this.btnMacroRemarks_Click);
      // 
      // btnMacroReturns
      // 
      resources.ApplyResources(this.btnMacroReturns, "btnMacroReturns");
      this.btnMacroReturns.Name = "btnMacroReturns";
      this.btnMacroReturns.UseVisualStyleBackColor = true;
      this.btnMacroReturns.Click += new System.EventHandler(this.btnMacroReturns_Click);
      // 
      // btnMacroSummary
      // 
      resources.ApplyResources(this.btnMacroSummary, "btnMacroSummary");
      this.btnMacroSummary.Name = "btnMacroSummary";
      this.btnMacroSummary.UseVisualStyleBackColor = true;
      this.btnMacroSummary.Click += new System.EventHandler(this.btnMacroSummary_Click);
      // 
      // edtRemarks
      // 
      resources.ApplyResources(this.edtRemarks, "edtRemarks");
      this.edtRemarks.Name = "edtRemarks";
      // 
      // lbRemarks
      // 
      resources.ApplyResources(this.lbRemarks, "lbRemarks");
      this.lbRemarks.Name = "lbRemarks";
      // 
      // edtReturns
      // 
      resources.ApplyResources(this.edtReturns, "edtReturns");
      this.edtReturns.Name = "edtReturns";
      // 
      // lbReturns
      // 
      resources.ApplyResources(this.lbReturns, "lbReturns");
      this.lbReturns.Name = "lbReturns";
      // 
      // edtSummary
      // 
      resources.ApplyResources(this.edtSummary, "edtSummary");
      this.edtSummary.Name = "edtSummary";
      // 
      // lbSummary
      // 
      resources.ApplyResources(this.lbSummary, "lbSummary");
      this.lbSummary.Name = "lbSummary";
      // 
      // controleCondicao
      // 
      this.controleCondicao.Caption = "Definição da condição:";
      conditionItem1.IgnoreCase = false;
      conditionItem1.NameValue = "";
      conditionItem1.SearchCondition = Lib.DocumentationObjects.SearchConditionsEnum.Any;
      this.controleCondicao.Condition = conditionItem1;
      resources.ApplyResources(this.controleCondicao, "controleCondicao");
      this.controleCondicao.Name = "controleCondicao";
      // 
      // DocumentationRuleBaseForm
      // 
      this.AcceptButton = this.btnOK;
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.gbxSummary);
      this.Controls.Add(this.controleCondicao);
      this.Controls.Add(this.gbxNomeRegra);
      this.Controls.Add(this.pnlBottom);
      this.KeyPreview = true;
      this.Name = "DocumentationRuleBaseForm";
      this.Load += new System.EventHandler(this.DocumentationRuleBaseForm_Load);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DocumentationRuleBaseForm_KeyDown);
      this.pnlBottom.ResumeLayout(false);
      this.gbxNomeRegra.ResumeLayout(false);
      this.gbxNomeRegra.PerformLayout();
      this.gbxSummary.ResumeLayout(false);
      this.gbxSummary.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel pnlBottom;
    private System.Windows.Forms.Button btnCancelar;
    private UserControls.DocumentationConditionControl controleCondicao;
    private System.Windows.Forms.GroupBox gbxNomeRegra;
    private System.Windows.Forms.ComboBox cbValidoPara;
    private System.Windows.Forms.Label lbValidoPara;
    private System.Windows.Forms.TextBox edtNomeRegra;
    private System.Windows.Forms.Label lbNomeRegra;
    private System.Windows.Forms.GroupBox gbxSummary;
    private System.Windows.Forms.TextBox edtSummary;
    private System.Windows.Forms.Label lbSummary;
    private System.Windows.Forms.TextBox edtReturns;
    private System.Windows.Forms.Label lbReturns;
    private System.Windows.Forms.TextBox edtRemarks;
    private System.Windows.Forms.Label lbRemarks;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnMacroRemarks;
    private System.Windows.Forms.Button btnMacroReturns;
    private System.Windows.Forms.Button btnMacroSummary;
    private System.Windows.Forms.ToolTip toolTip1;
  }
}