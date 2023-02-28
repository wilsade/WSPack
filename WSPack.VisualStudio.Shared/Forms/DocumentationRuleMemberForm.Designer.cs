namespace WSPack.VisualStudio.Shared.Forms
{
  partial class DocumentationRuleMemberForm
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
      Lib.DocumentationObjects.ConditionItem conditionItem1 = new Lib.DocumentationObjects.ConditionItem();
      Lib.DocumentationObjects.ConditionItem conditionItem2 = new Lib.DocumentationObjects.ConditionItem();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentationRuleMemberForm));
      this.controleCondicaoTipo = new WSPack.VisualStudio.Shared.UserControls.DocumentationConditionControl();
      this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
      this.SuspendLayout();
      // 
      // controleCondicaoTipo
      // 
      this.controleCondicaoTipo.Caption = "Definição da condição para o tipo:";
      conditionItem1.IgnoreCase = false;
      conditionItem1.NameValue = "";
      conditionItem1.SearchCondition = Lib.DocumentationObjects.SearchConditionsEnum.Any;
      this.controleCondicaoTipo.Condition = conditionItem1;
      this.controleCondicaoTipo.Dock = System.Windows.Forms.DockStyle.Top;
      this.controleCondicaoTipo.Location = new System.Drawing.Point(0, 174);
      this.controleCondicaoTipo.Name = "controleCondicaoTipo";
      this.controleCondicaoTipo.Size = new System.Drawing.Size(640, 100);
      this.controleCondicaoTipo.TabIndex = 2;
      // 
      // errorProvider1
      // 
      this.errorProvider1.ContainerControl = this;
      // 
      // DocumentationRuleMemberForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(640, 490);
      conditionItem2.IgnoreCase = false;
      conditionItem2.NameValue = "";
      conditionItem2.SearchCondition = Lib.DocumentationObjects.SearchConditionsEnum.Any;
      this.Condition = conditionItem2;
      this.Controls.Add(this.controleCondicaoTipo);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "DocumentationRuleMemberForm";
      this.Text = "Definição de regra para documentação de \'Membros\'";
      this.Load += new System.EventHandler(this.DocumentationRuleMemberForm_Load);
      this.Controls.SetChildIndex(this.controleCondicaoTipo, 0);
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private UserControls.DocumentationConditionControl controleCondicaoTipo;
    private System.Windows.Forms.ErrorProvider errorProvider1;
  }
}