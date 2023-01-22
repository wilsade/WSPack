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

    /// <summary>
    /// Substituir texto sem considerar maiúsculas/minúsculas
    /// </summary>
    /// <param name="str">Texto original</param>
    /// <param name="find">Procurar por</param>
    /// <param name="replace">Substituir por</param>
    /// <returns>string substituida</returns>
    public static string ReplaceInsensitive(this string str, string find, string replace)
    {
      if (str.IsNullOrWhiteSpaceEx())
        return str;
      return Microsoft.VisualBasic.Strings.Replace(str, find, replace, 1, -1, Microsoft.VisualBasic.CompareMethod.Text);
    }

    /// <summary>
    /// Formatar uma string
    /// </summary>
    /// <param name="self">string a ser formatada</param>
    /// <param name="args">Argumentos</param>
    /// <returns>string formatada</returns>
    public static string FormatWith(this string self, params object[] args)
    {
      return string.Format(self, args);
    }

    /// <summary>
    /// Verificar se uma string é igual à outra (Case Insensitive)
    /// </summary>
    /// <param name="str">String</param>
    /// <param name="value">Value</param>
    /// <returns>true se as strings são iguais</returns>
    public static bool EqualsInsensitive(this string str, string value)
    {
      return string.Equals(str, value, StringComparison.OrdinalIgnoreCase);
    }
  }
}
