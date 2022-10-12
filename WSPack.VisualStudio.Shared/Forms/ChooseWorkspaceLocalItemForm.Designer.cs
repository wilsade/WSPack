namespace WSPack.VisualStudio.Shared.Forms
{
  partial class ChooseWorkspaceLocalItemForm
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
      this.viewItens = new System.Windows.Forms.ListView();
      this.colWorkspace = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colLocalItem = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.SuspendLayout();
      // 
      // viewItens
      // 
      this.viewItens.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colWorkspace,
            this.colLocalItem});
      this.viewItens.Dock = System.Windows.Forms.DockStyle.Fill;
      this.viewItens.FullRowSelect = true;
      this.viewItens.HideSelection = false;
      this.viewItens.Location = new System.Drawing.Point(0, 0);
      this.viewItens.MultiSelect = false;
      this.viewItens.Name = "viewItens";
      this.viewItens.ShowGroups = false;
      this.viewItens.Size = new System.Drawing.Size(728, 300);
      this.viewItens.TabIndex = 5;
      this.viewItens.UseCompatibleStateImageBehavior = false;
      this.viewItens.View = System.Windows.Forms.View.Details;
      this.viewItens.SelectedIndexChanged += new System.EventHandler(this.viewItens_SelectedIndexChanged);
      this.viewItens.DoubleClick += new System.EventHandler(this.viewItens_DoubleClick);
      // 
      // colWorkspace
      // 
      this.colWorkspace.Text = "Workspace";
      this.colWorkspace.Width = 120;
      // 
      // colLocalItem
      // 
      this.colLocalItem.Text = "Item";
      this.colLocalItem.Width = 550;
      // 
      // ChooseWorkspaceLocalItemForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(728, 347);
      this.Controls.Add(this.viewItens);
      this.Name = "ChooseWorkspaceLocalItemForm";
      this.Text = "Escolher um Workspace / Item local";
      this.Load += new System.EventHandler(this.ChooseWorkspaceLocalItemForm_Load);
      this.Shown += new System.EventHandler(this.ChooseWorkspaceLocalItemForm_Shown);
      this.Controls.SetChildIndex(this.viewItens, 0);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView viewItens;
    private System.Windows.Forms.ColumnHeader colWorkspace;
    private System.Windows.Forms.ColumnHeader colLocalItem;
  }
}