using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
    static string Split(this string target, Func<char, char, bool> shouldSplit, string splitFiller = " ")
    {
      if (target == null)
        throw new ArgumentNullException(nameof(target));

      if (shouldSplit == null)
        throw new ArgumentNullException(nameof(shouldSplit));

      if (string.IsNullOrEmpty(splitFiller))
        throw new ArgumentNullException(nameof(splitFiller));

      int targetLength = target.Length;

      // We know the resulting string is going to be atleast the length of target
      StringBuilder result = new StringBuilder(targetLength);

      result.Append(target[0]);

      var lst = new List<string>();
      bool achou = false;
      var strTempWord = new StringBuilder();
      strTempWord.Append(target[0]);

      // Loop from the second character to the last character.
      for (int i = 1; i < targetLength; ++i)
      {
        achou = false;
        char firstChar = target[i - 1];
        char secondChar = target[i];

        if (shouldSplit(firstChar, secondChar))
        {
          achou = true;
          lst.Add(strTempWord.ToString());
          strTempWord.Clear();
          // If a split should be performed add in the filler
          result.Append(splitFiller);
        }

        strTempWord.Append(secondChar);
        result.Append(secondChar);
      }
      if (!achou)
        lst.Add(strTempWord.ToString());

      return result.ToString();
    }

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

    /// <summary>
    /// Retornar apenas o nome do arquivo
    /// </summary>
    /// <param name="fileAndPath">Nome completo de um arquivo</param>
    /// <returns>Nome do arquivo</returns>
    public static string FileNameOnly(this string fileAndPath)
    {
      return Path.GetFileName(fileAndPath);
    }

    /// <summary>
    /// Indica se a string representa um arquivo CSharp
    /// </summary>
    /// <param name="str">string</param>
    /// <returns>true se a string representa um arquivo CSharp</returns>
    public static bool IsCSharpFile(this string str)
    {
      if (str.IsNullOrWhiteSpaceEx())
        return false;
      return str.EndsWithInsensitive(".cs");
    }

    /// <summary>
    /// Remover acentos
    /// </summary>
    /// <param name="text">Text</param>
    /// <returns></returns>
    public static string RemoveAccents(this string text)
    {
      StringBuilder sbReturn = new StringBuilder();
      var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
      foreach (char letter in arrayText)
      {
        if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
          sbReturn.Append(letter);
      }
      return sbReturn.ToString();
    }

    /// <summary>
    /// Converer a primeira letra da string para maiúscula
    /// </summary>
    /// <param name="self">string a ser convertida</param>
    /// <returns>string com a primeira letra em maiúscula</returns>
    public static string ToFirstCharToUpper(this string self)
    {
      if (string.IsNullOrWhiteSpace(self))
        return self;

      if (self.Length == 1)
        return self.ToUpperInvariant();

      return char.ToUpperInvariant(self[0]) + self.Substring(1);
    }

    /// <summary>
    /// Indica se o arquivo é somente leitura
    /// </summary>
    /// <param name="fileName">Nome do arquivo</param>
    /// <returns>true se o arquivo é somente leitura</returns>
    public static bool IsReadOnlyFile(this string fileName)
    {
      // Create a new FileInfo object.
      var fInfo = new FileInfo(fileName);

      // Return the IsReadOnly property value.
      return fInfo.IsReadOnly;
    }

    /// <summary>
    /// Concatenar quebra de linha em uma string
    /// </summary>
    /// <param name="str">string</param>
    /// <returns>string com quebra de linha</returns>
    public static string NewLine(this string str)
    {
      if (str != null)
        return str + Environment.NewLine;
      return str;
    }

    /// <summary>
    /// Separar as palavras presentes em uma string conforme delimitadores comuns/conhecidos
    /// </summary>
    /// <param name="str">string a ser separada</param>
    /// <returns>Uma string conforme delimitadores comuns/conhecidos</returns>
    public static string[] SplitWithDelimiters(this string str)
    {
      if (string.IsNullOrEmpty(str))
        return null;
      var splited = str.Split(new string[] { "*", "?", "|", "_", "-", ">", "<", "%", "=", "}",
        "{", "$", "@", "&", "#", "'", "!", "[", "]", "\"", "(", ")", "\\", "/", ".", " ", ";",
        ":", ",", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
      return splited;
    }

    /// <summary>
    /// Reparar as palavras conforme iniciais maiúsculas. Ex: CodColigada retorna Cod e Coligada
    /// </summary>
    /// <param name="str">String</param>
    /// <returns>Palavras conforme iniciais maiúsculas</returns>
    public static string[] PascalCaseSplit(this string str)
    {
      if (str.IsNullOrWhiteSpaceEx())
        return new string[] { };

      string pascalSeparated = str.Split((c1, c2) => char.IsDigit(c1) || char.IsDigit(c2) || (char.IsLower(c1) && char.IsUpper(c2)), " ");
      var splited = pascalSeparated.SplitWithDelimiters();
      return splited;
    }
  }
}
