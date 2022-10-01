using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using WSPack.Lib;

namespace WSPack.Builder
{
  internal static class BuilderObj
  {
    const string MsBuildArguments = "{0} /p:Configuration=builder /m /clp:errorsOnly /noLogo";

    public static bool Build(IEnumerable<string> lst)
    {
      bool success = true;
      foreach (var item in lst)
      {
        Console.WriteLine();
        Console.WriteLine($"Compilando: {item}");

        var tupla = CommandClass.Execute("msbuild", string.Format(MsBuildArguments, item));
        if (tupla.Success)
          CommandClass.WriteSuccess("  Sucesso");
        else
        {
          success = false;
          CommandClass.WriteError($"[ExitCode={tupla.ExitCode}]", false);
          Console.WriteLine($"  {tupla.Output}");
          if (!string.IsNullOrWhiteSpace(tupla.Error))
            Console.WriteLine(tupla.Error);
        }

      }
      return success;
    }
  }
}
