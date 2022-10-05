using System;
using System.Diagnostics;

using Microsoft.VisualStudio.Shell;

using WSPack.Lib;

namespace WSPack.VisualStudio.Shared.Extensions
{
  static class DteExtensions
  {

    /// <summary>
    /// Escrever na janela: Output
    /// </summary>
    /// <param name="applicationObject">Aplicação DTE2</param>
    /// <param name="forcePanelShow">true para forçar a exbibição do painel</param>
    /// <param name="message">A mensagem a ser escrita</param>
    /// <param name="parameters">Parâmetros da mensagem</param>
    public static void WriteInOutPut(this EnvDTE80.DTE2 applicationObject, bool forcePanelShow, string message, params string[] parameters)
    {
      try
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        EnvDTE.Window window = applicationObject.Windows.Item(EnvDTE.Constants.vsWindowKindOutput);
        EnvDTE.OutputWindow outputWindow = (EnvDTE.OutputWindow)window.Object;
        EnvDTE.OutputWindowPane owp = null;

        for (uint i = 1; i <= outputWindow.OutputWindowPanes.Count; i++)
        {
          string aux = outputWindow.OutputWindowPanes.Item(i).Name;
          if (aux == Constantes.WSPack)
          {
            owp = outputWindow.OutputWindowPanes.Item(i);
            break;
          }
        }

        if (owp == null)
          owp = outputWindow.OutputWindowPanes.Add(Constantes.WSPack);

        if (forcePanelShow)
        {
          window.Activate();
          owp.Activate();
        }
        else
          owp.Activate();

        if (parameters?.Length > 0)
          owp.OutputString(string.Format(message, parameters) + Environment.NewLine);
        else
          owp.OutputString(message + Environment.NewLine);
      }
      catch (Exception ex)
      {
        Trace.WriteLine($"WriteInOutPut: {ex.Message}");
      }
    }
  }
}