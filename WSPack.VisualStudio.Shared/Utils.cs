using System;
using System.Diagnostics;

using WSPack.VisualStudio.Shared.Extensions;

namespace WSPack.VisualStudio.Shared
{
  static class Utils
  {
    internal static bool IsDebugOrDebugModeEnabled
    {
      get
      {
#if DEBUG
        return true;
#else
        return WSPackPackage.ParametrosGerais.EnableDebugMode;
#endif
      }
    }

    /// <summary>
    /// Escrever no Output apenas em modo de Debug
    /// </summary>
    /// <param name="message">Message</param>
    public static void LogDebugError(string message)
    {
      if (!IsDebugOrDebugModeEnabled)
        return;

      try
      {
        WSPackPackage.Dte.WriteInOutPut(true, $"*** ERRO ***{Environment.NewLine}{message}");
        Trace.WriteLine(message);
      }
      catch (Exception ex)
      {
        Trace.WriteLine(ex.Message);
      }
    }
  }
}