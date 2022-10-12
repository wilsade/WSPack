using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.Lib.Extensions
{
  /// <summary>
  /// Métodos estendidos para String
  /// </summary>
  public static class StringExtensions
  {
    /// <summary>
    /// Indica se a string é nula ou vazia
    /// </summary>
    /// <param name="str">string a ser verificada</param>
    /// <returns>se a string é nula ou vazia</returns>
    public static bool IsNullOrEmptyEx(this string str)
    {
      return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// Indica se a string é nula ou espaço
    /// </summary>
    /// <param name="str">string a ser verificada</param>
    /// <returns>se a string é nula ou espaço</returns>
    public static bool IsNullOrWhiteSpaceEx(this string str)
    {
      return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// Verificar se uma string contém um valor (Case Insensitive)
    /// </summary>
    /// <param name="str">string</param>
    /// <param name="value">Valor a ser procurado</param>
    /// <returns>true se o valor está presente na string</returns>
    public static bool ContainsInsensitive(this string str, string value)
    {
      if (str == null || value == null)
        return false;
      return str.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    /// <summary>
    /// Indica se uma string termina com um termo
    /// </summary>
    /// <param name="self">string a ser analisada</param>
    /// <param name="str">Termo para procurar</param>
    /// <returns>true se a string termina com o termo procurado</returns>
    public static bool EndsWithInsensitive(this string self, string str)
    {
      if (self.IsNullOrWhiteSpaceEx() || str.IsNullOrWhiteSpaceEx())
        return false;
      return self.EndsWith(str, StringComparison.OrdinalIgnoreCase);
    }
  }
}
