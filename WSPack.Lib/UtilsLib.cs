using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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


    /// <summary>
    /// Executar um comando
    /// </summary>
    /// <param name="filename">Filename</param>
    /// <param name="arguments">Arguments</param>
    /// <param name="outputString">Output string</param>
    /// <param name="outputErros">Erros do comando</param>
    /// <param name="defaultPath">Default path</param>
    /// <param name="timeout">Timeout</param>
    /// <returns>Código de retorno</returns>
    /// <exception cref="ApplicationException"></exception>
    public static int ExecuteCommand(string filename, string arguments, out string outputString, out string outputErros, string defaultPath = "", int timeout = 980000)
    {
      outputString = string.Empty;
      outputErros = string.Empty;
      using (var process = new Process())
      {
        process.StartInfo.FileName = filename;
        process.StartInfo.Arguments = arguments;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardInput = !Environment.UserInteractive;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.CreateNoWindow = true;
        if (!string.IsNullOrEmpty(defaultPath))
          process.StartInfo.WorkingDirectory = defaultPath;

        var output = new StringBuilder();
        var error = new StringBuilder();
        bool processoIniciou = false;

        var outputWaitHandle = new AutoResetEvent(false);
        var errorWaitHandle = new AutoResetEvent(false);

        void onOutput(object sender, DataReceivedEventArgs e)
        {
          if (e.Data == null)
          {
            if (!outputWaitHandle.SafeWaitHandle.IsClosed)
              outputWaitHandle.Set();
          }
          else
          {
            output.AppendLine(e.Data);
          }
        }

        void onErros(object sender, DataReceivedEventArgs e)
        {
          if (e.Data == null)
          {
            if (!errorWaitHandle.SafeWaitHandle.IsClosed)
              errorWaitHandle.Set();
          }
          else
          {
            error.AppendLine(e.Data);
          }
        }

        process.OutputDataReceived += onOutput;
        process.ErrorDataReceived += onErros;

        try
        {
          process.Start();
          processoIniciou = true;

          process.BeginOutputReadLine();
          process.BeginErrorReadLine();

          if (process.WaitForExit(timeout) && outputWaitHandle.WaitOne(timeout) && errorWaitHandle.WaitOne(timeout))
          {
            outputString += output.ToString();
            outputString += error.ToString();
            outputErros += error.ToString();
            return process.ExitCode;
          }
          return -1;
        }
        catch (Exception ex)
        {
          var sb = new StringBuilder("Erro ao executar o comando!" + Environment.NewLine);
          sb.AppendFormat("FileName..: {0}\n", process.StartInfo.FileName);
          sb.AppendFormat("Argumentos: {0}\n", process.StartInfo.Arguments);
          sb.AppendFormat("Usuário...: {0}\n", process.StartInfo.UserName);
          sb.AppendLine(ex.ToString());
          throw new ApplicationException(sb.ToString(), ex);
        }

        finally
        {
          if (processoIniciou)
            process.CancelErrorRead();
          process.OutputDataReceived -= onOutput;
          process.ErrorDataReceived -= onErros;
          outputWaitHandle.Dispose();
          errorWaitHandle.Dispose();
          process.Close();
        }
      }
    }
  }
}
