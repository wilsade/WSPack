namespace WSPack.Lib.WPF.CommonLib
{
  /// <summary>
  /// Represents a view mode that displays data items in columns for a System.Windows.Controls.ListView control with auto sized columns based on the column content     
  /// </summary>
  public class AutoSizedGridView : System.Windows.Controls.GridView
  {
    /// <summary>
    /// Prepares a <see cref="T:System.Windows.Controls.ListViewItem" /> for display according to the definition of this <see cref="T:System.Windows.Controls.GridView" /> object.
    /// </summary>
    /// <param name="item">The <see cref="T:System.Windows.Controls.ListViewItem" /> to display.</param>
    protected override void PrepareItem(System.Windows.Controls.ListViewItem item)
    {
      foreach (System.Windows.Controls.GridViewColumn column in Columns)
      {
        AutoSizeColumns();
      }
      base.PrepareItem(item);
    }

    /// <summary>
    /// Ajustar o comprimento das colunas
    /// </summary>
    public void AutoSizeColumns()
    {
      foreach (System.Windows.Controls.GridViewColumn column in Columns)
      {
        if (double.IsNaN(column.Width))
          column.Width = column.ActualWidth;
        column.Width = double.NaN;
      }
    }
  }
}
