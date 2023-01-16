using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Fornecer editor para procurar por um arquivo
  /// </summary>
  /// <seealso cref="BaseEditor" />
  public abstract class BasePathEditor : BaseEditor
  {
    /// <summary>
    /// Retornar as opções do diálogo
    /// </summary>
    /// <returns>Extensão e filtro</returns>
    protected abstract (string DefaultExt, string Filter) GetDialogOptions();

    /// <summary>
    /// Recuperar o valor da propriedade
    /// </summary>
    /// <param name="context">An <see cref="ITypeDescriptorContext"/> that can be used to gain additional context information.</param>
    /// <param name="oldValue">Valor antigo</param>
    /// <returns>Valor da propriedade</returns>
    protected override string GetValue(ITypeDescriptorContext context, string oldValue)
    {
      using (var dlg = new System.Windows.Forms.OpenFileDialog())
      {
        var (DefaultExt, Filter) = GetDialogOptions();
        dlg.DefaultExt = DefaultExt;
        dlg.Filter = Filter;
        if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
          oldValue = dlg.FileName;
        }
      }

      return oldValue;
    }
  }

  /// <summary>
  /// Fornecer editor para procurar por um arquivo
  /// </summary>
  /// <seealso cref="BasePathEditor" />
  public class PathExeEditor : BasePathEditor
  {
    /// <summary>
    /// Retornar as opções do diálogo
    /// </summary>
    /// <returns>Extensão e filtro</returns>
    protected override (string DefaultExt, string Filter) GetDialogOptions()
    {
      return ("*.exe", "Executáveis|*.exe");
    }
  }

  /// <summary>
  /// Fornecer editor para procurar por um arquivo
  /// </summary>
  /// <seealso cref="BasePathEditor" />
  public class PathCfgEditor : BasePathEditor
  {
    /// <summary>
    /// Retornar as opções do diálogo
    /// </summary>
    /// <returns>Extensão e filtro</returns>
    protected override (string DefaultExt, string Filter) GetDialogOptions()
    {
      return ("*.cfg", "Configurações|*.cfg");
    }
  }
}