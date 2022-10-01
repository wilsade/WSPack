using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WSPack.Lib;

namespace WSPack.Builder
{
  internal class ProgramWSPackBuilder
  {
    enum ExitCodes
    {
      Sucess = 0,
      NoSolutions = 1,
      Error = 2
    }

    static int Main(string[] args)
    {
      IEnumerable<string> solutions = GetSolutions();
      if (solutions == null)
      {
        Console.Write("Não foram encontradas solutions");
        return (int)ExitCodes.NoSolutions;
      }

      Console.Clear();
      if (!BuilderObj.Build(solutions))
      {
        Console.Beep();
        Console.WriteLine();
        CommandClass.WriteError("Erro(s) de build");
        return (int)ExitCodes.Error;
      }

      return (int)ExitCodes.Sucess;
    }

    private static IEnumerable<string> GetSolutions()
    {
      var dirInfo = new DirectoryInfo(Path.GetDirectoryName(typeof(ProgramWSPackBuilder).Assembly.Location));
      while (dirInfo?.Parent != null)
      {
        var lst = dirInfo.EnumerateFiles("WSPack*.sln");
        if (lst.Any())
          return lst.Select(x => x.FullName);
        dirInfo = new DirectoryInfo(dirInfo.Parent.FullName);
      }
      return null;
    }
  }
}
