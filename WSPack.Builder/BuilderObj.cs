using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.Builder
{
  internal static class BuilderObj
  {
    const string MsBuildArguments = "{0} /p:Configuration=builder /m /clp:errorsOnly /noLogo";

    static ProcessStartInfo GetProcessStartInfo(string solutionName) =>
      new ProcessStartInfo("msbuild", string.Format(MsBuildArguments, solutionName))
      {
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        CreateNoWindow = true,
      };

    public static bool Build(IEnumerable<string> lst)
    {
      bool success = true;
      foreach (var item in lst)
      {
        Console.WriteLine();
        Console.WriteLine($"Compilando: {item}");
        using (var p = Process.Start(GetProcessStartInfo(item)))
        {
          p.WaitForExit();
          if (p.ExitCode != 0)
          {
            var output = p.StandardOutput.ReadToEnd();
            var erros = p.StandardError.ReadToEnd();
            success = false;
            Console.WriteLine(output);
            if (!string.IsNullOrWhiteSpace(erros))
              Console.WriteLine(erros);
          }
          else
          {
            Console.WriteLine("  Sucesso");
          }
        }
      }
      return success;
    }
  }
}
