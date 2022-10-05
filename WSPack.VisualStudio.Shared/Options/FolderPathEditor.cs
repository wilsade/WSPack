using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Fornecer editor para procurar por uma pasta
  /// </summary>
  /// <seealso cref="BaseEditor" />
  public class FolderPathEditor : BaseEditor
  {
    /// <summary>
    /// Recuperar o valor da propriedade
    /// </summary>
    /// <param name="context">An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information.</param>
    /// <param name="oldValue">Valor antigo</param>
    /// <returns>Valor da propriedade</returns>
    protected override string GetValue(ITypeDescriptorContext context, string oldValue)
    {
      using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
      {
        if (!string.IsNullOrEmpty(oldValue))
          dlg.SelectedPath = oldValue;
        if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
          return dlg.SelectedPath;
        }
      }
      return BaseDialogPage.BaseConfigPath;
    }
  }
}
