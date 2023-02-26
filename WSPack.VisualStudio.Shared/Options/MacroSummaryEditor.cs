using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using WSPack.VisualStudio.Shared.DocumentationObjects.Macros;
using WSPack.VisualStudio.Shared.Forms;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Editor para propriedade de summary padrão
  /// </summary>
  public class MacroSummaryEditor : BaseEditor
  {
    /// <summary>
    /// Recuperar o valor da propriedade
    /// </summary>
    /// <param name="context">An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information.</param>
    /// <param name="oldValue">Valor antigo</param>
    /// <returns>Valor da propriedade</returns>
    protected override string GetValue(ITypeDescriptorContext context, string oldValue)
    {
      using (var form = new DocumentationMacrosForm())
      {
        if (context.PropertyDescriptor.Name == nameof(PageDocumentation.DefaultSummaryForMembers))
        {
          List<MacroGroupItems> lst = MacroMemberItems.CreateGroup();
          lst.ForEach(x => form.LoadMacros(x));
        }

        form.EditText = oldValue;
        if (form.ShowDialog() == DialogResult.OK)
        {
          return form.EditText;
        }
      }
      return oldValue;
    }
  }
}
