using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.Lib
{
  /// <summary>
  /// Implementar a interface IWin32Window
  /// </summary>
  public class Win32WindowWrapper : System.Windows.Forms.IWin32Window
  {
    /// <summary>
    /// Cria uma instância da classe <see cref="Win32WindowWrapper"/>
    /// </summary>
    /// <param name="handle">The handle.</param>
    public Win32WindowWrapper(IntPtr handle)
    {
      Handle = handle;
    }

    /// <summary>
    /// Gets the handle to the window represented by the implementer.
    /// </summary>
    public IntPtr Handle { get; }
  }
}
