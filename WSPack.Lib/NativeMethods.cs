using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.Lib
{
  /// <summary>
  /// Classe para manipular funções da API do Windowns
  /// </summary>
  public static class NativeMethods
  {
    /// <summary>
    /// FindWindow searches all windows for one which matches the window class name and/or window name
    /// </summary>
    /// <param name="lpClassName">The name of the window class of the window to find. To ignore the window's class, specify a null string</param>
    /// <param name="lpWindowName">The name of the title bar text of the window to find. To ignore the window's text, specify a null string</param>
    /// <returns></returns>
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    /// <summary>
    /// ShowWindow shows (or hides) a window in a certain manner. For example, the function can minimize, maximize, or restore a given window
    /// </summary>
    /// <param name="hWnd">The handle of the window to change the show status of</param>
    /// <param name="nCmdShow">One of the following flags specifying how to show the window (<see cref="SW_Modos"/>)</param>
    /// <returns>The function returns 0 if the window had been hidden before the call, or a non-zero value if it had been visible</returns>
    [DllImport("user32.dll")]
    public static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

    /// <summary>
    /// SetForegroundWindow makes the specified window the current foreground window and gives it the focus. This function should only be used with windows which your program owns
    /// </summary>
    /// <param name="hWnd">A handle to the window to set as the foreground window</param>
    /// <returns>The function returns 1 if successful, or 0 if an error occured</returns>
    [DllImport("user32.dll")]
    public static extern int SetForegroundWindow(IntPtr hWnd);
  }

  /// <summary>
  /// Flags specifying how to show the window
  /// </summary>
  public enum SW_Modos
  {
    /// <summary>
    /// Hides the window and activates another window
    /// </summary>
    SW_HIDE = 0,
    /// <summary>
    /// Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time
    /// </summary>
    SW_SHOWNORMAL = 1,
    /// <summary>
    /// Activates the window and displays it as a minimized window
    /// </summary>
    SW_SHOWMINIMIZED = 2,
    /// <summary>
    /// Activates the window and displays it as a maximized window
    /// </summary>
    SW_SHOWMAXIMIZED = 3,
    /// <summary>
    /// Maximizes the specified window
    /// </summary>
    SW_MAXIMIZE = 3,
    /// <summary>
    /// Displays a window in its most recent size and position. This value is similar to SW_SHOWNORMAL, except the window is not actived
    /// </summary>
    SW_SHOWNOACTIVATE = 4,
    /// <summary>
    /// Activates the window and displays it in its current size and position
    /// </summary>
    SW_SHOW = 5,
    /// <summary>
    /// Minimizes the specified window and activates the next top-level window in the Z order
    /// </summary>
    SW_MINIMIZE = 6,
    /// <summary>
    /// Displays the window as a minimized window. This value is similar to SW_SHOWMINIMIZED, except the window is not activated
    /// </summary>
    SW_SHOWMINNOACTIVE = 7,
    /// <summary>
    /// Displays the window in its current size and position. This value is similar to SW_SHOW, except the window is not activated
    /// </summary>
    SW_SHOWNA = 8,
    /// <summary>
    /// Activates and displays the window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when restoring a minimized window
    /// </summary>
    SW_RESTORE = 9,
    /// <summary>
    /// Sets the show state based on the SW_ value specified in the STARTUPINFO structure passed to the  CreateProcess function by the program that started the application
    /// </summary>
    SW_SHOWDEFAULT = 10,
    /// <summary>
    /// Windows 2000/XP: Minimizes a window, even if the thread that owns the window is not responding. This flag should only be used when minimizing windows from a different thread
    /// </summary>
    SW_FORCEMINIMIZE = 11
  }
}
