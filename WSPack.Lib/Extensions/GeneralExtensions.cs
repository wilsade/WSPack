using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.Lib.Extensions
{
  /// <summary>
  /// Métodos estendidos para classes em geral
  /// </summary>
  public static class GeneralExtensions
  {

    /// <summary>
    /// Recuperar todas as mensagens da pilha de exceção
    /// </summary>
    /// <param name="self">This</param>
    /// <param name="includeStack">true para incluir o stackTrace da exceção</param>
    /// <returns></returns>
    public static string GetCompleteMessage(this Exception self, bool includeStack = false)
    {
      if (self == null)
        return string.Empty;

      string retMessage = self.Message;

      if (self.InnerException != null)
      {
        retMessage += Environment.NewLine + "=> " + self.InnerException.GetCompleteMessage();
      }

      if (includeStack && !self.StackTrace.IsNullOrEmptyEx())
        retMessage += Environment.NewLine + self.StackTrace;
      return retMessage;
    }
  }
}
