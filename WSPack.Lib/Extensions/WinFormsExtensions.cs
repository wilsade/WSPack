using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSPack.Lib.Extensions
{
  /// <summary>
  /// Definir métodos estendidos para classes de System.Windows.Forms
  /// </summary>
  public static class WinFormsExtensions
  {
    /// <summary>
    /// Exportar o conteúdo de uma Grid para HTML
    /// </summary>
    /// <param name="grid">Grid a ser exportada</param>
    /// <returns>HTML da Grid</returns>
    public static string ToHtml(this DataGridView grid)
    {
      var myBuilder = new StringBuilder();

      myBuilder.Append("<table border='1px' cellpadding='5' cellspacing='0' ");
      myBuilder.Append("style='border: solid 1px Silver; font-size: x-medium;'>");

      myBuilder.Append("<tr align='left' valign='top'>");
      for (int i = 0; i < grid.Columns.Count; i++)
      {
        DataGridViewColumn myColumn = grid.Columns[i];
        if (myColumn.Visible)
        {
          myBuilder.Append("<td align='left' valign='top' >");
          myBuilder.Append("<b>").Append(myColumn.Name).Append("<b>");
          myBuilder.Append("</td>");
        }
      }
      myBuilder.Append("</tr>");

      foreach (DataGridViewRow myRow in grid.Rows)
      {
        myBuilder.Append("<tr align='left' valign='top'>");
        for (int i = 0; i < grid.Columns.Count; i++)
        {
          DataGridViewColumn myColumn = grid.Columns[i];
          if (myColumn.Visible)
          {
            string valorCelula = Convert.ToString(myRow.Cells[myColumn.Name].Value);
            valorCelula = valorCelula.Replace(Environment.NewLine, "<br style='mso-data-placement:same-cell;' />");

            myBuilder.Append("<td align='left' valign='top'>");
            myBuilder.Append(valorCelula);
            myBuilder.Append("</td>");
          }
        }
        myBuilder.Append("</tr>");
      }
      myBuilder.Append("</table>");

      return myBuilder.ToString();
    }

    /// <summary>
    /// Fazer com que o novo item seja o primeiro da lista
    /// </summary>
    /// <param name="combo">Combobox</param>
    /// <param name="newItem">New item</param>
    public static void MakeFirst(this ComboBox combo, object newItem)
    {
      if (!Convert.ToString(newItem).IsNullOrWhiteSpaceEx())
      {
        int posicao = combo.Items.IndexOf(newItem);
        if (posicao >= 0)
        {
          combo.Items.RemoveAt(posicao);
        }
        combo.Items.Insert(0, newItem);
        combo.SelectedIndex = 0;
      }
    }
  }
}
