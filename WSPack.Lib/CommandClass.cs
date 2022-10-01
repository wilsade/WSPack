using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WSPack.Lib
{
  /// <summary>
  /// Classe para execução de processos e interação com o Console
  /// </summary>
  public static class CommandClass
  {
    const int BaseTimeout = 240_000;

    private static string GetErrorOrOutput(StringBuilder strOutput, StringBuilder strErro)
    {
      var outputAux = strOutput.ToString();
      var erroAux = strErro.ToString();
      if (!string.IsNullOrWhiteSpace(erroAux))
        return erroAux;
      return outputAux;
    }

    /// <summary>
    /// Escrever no Console com uma cor
    /// </summary>
    /// <param name="str">string a ser escrita</param>
    /// <param name="consoleColor">Cor desejada</param>
    /// <param name="breakLine">true para escrever com quebra de linha</param>
    public static void Write(string str, ConsoleColor consoleColor, bool breakLine = true)
    {
      var color = Console.ForegroundColor;
      try
      {
        Console.ForegroundColor = consoleColor;
        if (breakLine)
          Console.WriteLine(str);
        else
          Console.Write(str);
      }
      finally
      {
        Console.ForegroundColor = color;
      }
    }

    /// <summary>
    /// Escrever no console uma mensagem com coloração indicando erro
    /// </summary>
    /// <param name="str">string a ser escrita</param>
    /// <param name="breakLine">true para escrever com quebra de linha</param>
    public static void WriteError(string str, bool breakLine = true)
    {
      Write(str, ConsoleColor.Red, breakLine);
    }

    /// <summary>
    /// Escrever no console uma mensagem com coloração indicando aviso
    /// </summary>
    /// <param name="str">string a ser escrita</param>
    /// <param name="breakLine">true para escrever com quebra de linha</param>
    public static void WriteWarning(string str, bool breakLine = true)
    {
      Write(str, ConsoleColor.Yellow, breakLine);
    }

    /// <summary>
    /// Escrever no console uma mensagem com coloração indicando sucesso
    /// </summary>
    /// <param name="str">string a ser escrita</param>
    /// <param name="breakLine">true para escrever com quebra de linha</param>
    public static void WriteSuccess(string str, bool breakLine = true)
    {
      Write(str, ConsoleColor.Green, breakLine);
    }

    /// <summary>
    /// Executar um processo
    /// </summary>
    /// <param name="fileName">Processo a ser executado</param>
    /// <param name="args">Argumentos do processo</param>
    /// <param name="workingDir">Diretório do processo</param>
    /// <param name="timeout">Timeout</param>
    /// <returns>Informações sobre a execução do processo</returns>
    public static (bool Success, int ExitCode, string ErrorOrOutput, string Output, string Error) Execute(
      string fileName, string args, string workingDir = null, int timeout = BaseTimeout)
    {
      var startInfo = new ProcessStartInfo(fileName, args)
      {
        WorkingDirectory = workingDir,
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        CreateNoWindow = true
      };

      using (var process = new Process() { StartInfo = startInfo })
      {
        var output = new StringBuilder();
        var error = new StringBuilder();

        using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
        using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
        {
          process.OutputDataReceived += (sender, e) =>
          {
            if (e.Data == null)
              outputWaitHandle.Set();
            else
              output.AppendLine(e.Data);
          };
          process.ErrorDataReceived += (sender, e) =>
          {
            if (e.Data == null)
              errorWaitHandle.Set();
            else
              error.AppendLine(e.Data);
          };

          process.Start();

          process.BeginOutputReadLine();
          process.BeginErrorReadLine();

          if (process.WaitForExit(timeout) &&
              outputWaitHandle.WaitOne(timeout) &&
              errorWaitHandle.WaitOne(timeout))
          {
            return (process.ExitCode == 0, process.ExitCode, GetErrorOrOutput(output, error), output.ToString(), error.ToString());
          }
          else
          {
            return (false, -1, GetErrorOrOutput(output, error), output.ToString(), error.ToString());
          }
        }
      }
    }

  }
}
