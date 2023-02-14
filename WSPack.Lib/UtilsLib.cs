using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using swf = System.Windows.Forms;

namespace WSPack.Lib
{
  /// <summary>
  /// Classe utilitária
  /// </summary>
  public static class UtilsLib
  {
    /// <summary>
    /// Indica se o controle é válido no comando de definição de tabulação
    /// </summary>
    /// <param name="controle">Controle a ser validado.</param>
    /// <param name="checkToolStripInheritance">Informe true para verificar controles do tipo 'ToolStrip'</param>
    /// <returns>true se o controle deverá ser mostrado na janela de tabulação</returns>
    public static bool IsValidTabOrderControl(swf.Control controle, bool checkToolStripInheritance)
    {
      var tipo = controle.GetType();
      bool isValid = !string.IsNullOrEmpty(controle.Name) &&
        tipo != typeof(swf.MenuItem);

      if (checkToolStripInheritance)
        isValid = isValid && tipo != typeof(swf.ToolStrip) && !tipo.IsSubclassOf(typeof(swf.ToolStrip));

      return isValid;
    }

    /// <summary>
    /// Formats the name of the control.
    /// </summary>
    /// <param name="controle">The controle.</param>
    /// <returns>Nome do contorle formatado</returns>
    public static string FormatControlName(swf.Control controle)
    {
      return $"{controle.Name}: {controle.GetType().Name}";
    }
  }
}
