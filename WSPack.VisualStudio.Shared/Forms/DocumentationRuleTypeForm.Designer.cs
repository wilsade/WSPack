namespace WSPack.VisualStudio.Shared.Forms
{
  partial class DocumentationRuleTypeForm
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
      Lib.DocumentationObjects.ConditionItem conditionItem1 = new Lib.DocumentationObjects.ConditionItem();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentationRuleTypeForm));
      this.SuspendLayout();
      // 
      // DocumentationRuleTypeForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(599, 395);
      conditionItem1.IgnoreCase = false;
      conditionItem1.NameValue = "";
      conditionItem1.SearchCondition = Lib.DocumentationObjects.SearchConditionsEnum.Any;
      this.Condition = conditionItem1;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "DocumentationRuleTypeForm";
      this.Text = "Definição de regra para documentação de \'Tipos\'";
      this.ResumeLayout(false);

    }

    #endregion
  }
}