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
    /// <summary>
    /// A partir de uma posição inicial, ao fazer uma procura em uma string, vamos procurar em quais direções?
    /// </summary>
    enum SentidoProcura
    {
      /// <summary>
      /// Procurar para o lado esquerdo
      /// </summary>
      Esquerdo,
      /// <summary>
      /// Procurar para o lado direito
      /// </summary>
      Direito,
      /// <summary>
      /// Procurar nos dois sentidos: direito e esquerdo
      /// </summary>
      Ambos
    };

    /// <summary>
    /// Retorna a palavra sobre a posição da procura.
    /// Letras, números ou o símbolo "_" são considerados parte da palavra
    /// Qualquer caractere diferente destes são considerados delimitadores
    /// </summary>
    /// <param name="texto">Porção do texto onde vamos procurar uma palavra</param>
    /// <param name="posicaoProcura">Posição inicial</param>
    /// <param name="sentidoProcura">Em quais sentidos a procura deverá ser feita: diretia, esquerda ou ambos</param>
    /// <param name="ignorarEspacos">true para não encontrar espaços; false, caso contrário
    /// <remarks>Informe true para o método continuar procurando palavras mesmo se encontrar espaços contínuos</remarks></param>
    /// <returns>Informações da palavra</returns>
    private static string InternalGetWordAt(string texto, int posicaoProcura, SentidoProcura sentidoProcura, bool ignorarEspacos)
    {
      if (string.IsNullOrEmpty(texto))
        return string.Empty;
      else if (sentidoProcura == SentidoProcura.Direito && string.IsNullOrEmpty(texto.Substring(posicaoProcura)))
        return string.Empty;

      int inicioPalavra, fimPalavra;

      switch (sentidoProcura)
      {
        case SentidoProcura.Esquerdo:
          // Vamos voltar uma posição e ver se temos um espaço
          if (ignorarEspacos && texto.Length > 1 && posicaoProcura >= 1 && texto.Substring(posicaoProcura - 1, 1) == " ")
            return InternalGetWordAt(texto, posicaoProcura - 1, sentidoProcura, ignorarEspacos);

          inicioPalavra = FindWordStart(texto, posicaoProcura);
          fimPalavra = FindWordEnd(texto, posicaoProcura);
          break;
        case SentidoProcura.Direito:
          // A próxima posição é um espaço?
          if (ignorarEspacos && texto.Length > 1 && posicaoProcura < texto.Length - 1 && texto.Substring(posicaoProcura, 1) == " ")
            return InternalGetWordAt(texto, posicaoProcura + 1, sentidoProcura, ignorarEspacos);

          //inicioPalavra = posicaoProcura;
          inicioPalavra = FindWordStart(texto, posicaoProcura, sentidoProcura);
          fimPalavra = posicaoProcura == texto.Length ? texto.Length : FindWordEnd(texto, posicaoProcura, sentidoProcura);
          break;
        case SentidoProcura.Ambos:
          string palavra = InternalGetWordAt(texto, posicaoProcura, SentidoProcura.Esquerdo, ignorarEspacos);
          if (string.IsNullOrEmpty(palavra))
            palavra = InternalGetWordAt(texto, posicaoProcura, SentidoProcura.Direito, ignorarEspacos);
          return palavra;

        //inicioPalavra = posicaoProcura > 0 ? FindWordStart(texto, posicaoProcura - 1) : 0;
        //fimPalavra = posicaoProcura == texto.Length ? texto.Length : FindWordEnd(texto, posicaoProcura);
        //break;
        default:
          inicioPalavra = fimPalavra = posicaoProcura;
          break;
      }

      //int inicioDaPalavra = posicaoProcura > 0 ? FindWordStart(s, posicaoProcura-1) : 0;
      //int fimDaPalavra = posicaoProcura == s.Length ? s.Length : FindWordEnd(s, posicaoProcura);
      string retorno = texto.Substring(inicioPalavra, fimPalavra - inicioPalavra);
      return retorno;
    }

    /// <summary>
    /// A partir da posição inicial, procura o início da palavra
    /// Letras, números ou o símbolo "_" são considerados parte da palavra
    /// Qualquer caractere diferente destes são considerados delimitadores
    /// <remarks>A forma de procurar será da posição inicial (inclusive) à esquerda</remarks>
    /// </summary>
    /// <param name="s">Texto</param>
    /// <param name="start">Posição inicial</param>
    /// <returns>O índice que indica o início da palavra</returns>
    private static int FindWordStart(string s, int start)
    {
      return FindWordStart(s, start, SentidoProcura.Esquerdo);
    }

    /// <summary>
    /// A partir da posição inicial, procura o início da palavra
    /// Letras, números ou o símbolo "_" são considerados parte da palavra
    /// Qualquer caractere diferente destes são considerados delimitadores
    /// </summary>
    /// <param name="s">Texto</param>
    /// <param name="start">Posição inicial</param>
    /// <param name="sentidoProcura">A partir da posição inicial, de qual lado iremos procurar a palavra?</param>
    /// <returns>O índice que indica o início da palavra</returns>
    private static int FindWordStart(string s, int start, SentidoProcura sentidoProcura)
    {
      if (start == 0)
      {
        // Se o início da palavra não for Letra, dígito ou '_', é porque a palavra começa com algum delimitador
        if (IsLetterDigitOrUnderscore(Convert.ToChar(s.Substring(start, 1))))
          return start;

        else if (sentidoProcura == SentidoProcura.Direito)
          return FindWordStart(s, start + 1, SentidoProcura.Esquerdo);

        else
          return start;
      }
      else
      {
        if (start >= s.Length)
        {
          start--;

          // O cursor estava no início da palavra
          string aux = s.Substring(start, 1);
          if (!IsLetterDigitOrUnderscore(Convert.ToChar(aux)) || aux == " ")
            return start + 1;
        }

        if (start == 0)
          return start;

        string letraStringAtual = s.Substring(start, 1);
        string letraStringAnterior = s.Substring(start - 1, 1);

        // Se a posição anterior náo for uma letra/numero/_, possivelmente o cursor já está no início da palavra
        // Vamos simplesmente retornar
        if (!IsLetterDigitOrUnderscore(Convert.ToChar(letraStringAnterior)))
          return start;

        // Se a posição atual for uma letra/numero/_, possivelmente o cursor está sobre a palavra.
        // Vamos voltar a posição
        else if (IsLetterDigitOrUnderscore(Convert.ToChar(letraStringAtual)))
          return FindWordStart(s, start - 1, sentidoProcura);

        else if (sentidoProcura == SentidoProcura.Esquerdo)
          return FindWordStart(s, start - 1, sentidoProcura);

        else
          return start + 1;
      }
    }

    /// <summary>
    /// A partir da posição inicial, procura o final da palavra
    /// Letras, números ou o símbolo "_" são considerados parte da palavra
    /// Qualquer caractere diferente destes são considerados delimitadores
    /// <remarks>A forma de procurar será da posição inicial (inclusive) à esquerda</remarks>
    /// </summary>
    /// <param name="s">Texto</param>
    /// <param name="start">Posição inicial</param>
    /// <returns>O índice que indica o final da palavra</returns>
    private static int FindWordEnd(string s, int start)
    {
      return FindWordEnd(s, start, SentidoProcura.Esquerdo);
    }


    /// <summary>
    /// A partir da posição inicial, procura o final da palavra
    /// Letras, números ou o símbolo "_" são considerados parte da palavra
    /// Qualquer caractere diferente destes são considerados delimitadores
    /// </summary>
    /// <param name="s">Texto</param>
    /// <param name="start">Posição inicial</param>
    /// <param name="sentidoProcura">A partir da posição inicial, de qual lado iremos procurar a palavra?</param>
    /// <returns>O índice que indica o final da palavra</returns>
    private static int FindWordEnd(string s, int start, SentidoProcura sentidoProcura)
    {
      if (start == s.Length)
        return start;
      else
      {
        string letraStringAtual = s.Substring(start, 1);
        if (!IsLetterDigitOrUnderscore(Convert.ToChar(letraStringAtual)) && sentidoProcura == SentidoProcura.Esquerdo)
          return start;
        else
          return FindWordEnd(s, start + 1, SentidoProcura.Esquerdo);
      }
    }


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
    /// Verifica se o caractere é um número, uma letra ou o símboro "_"
    /// </summary>
    /// <param name="c">O caractere a ser verificado</param>
    /// <returns>true ou false, caso o caractere seja ou não uma letra, número ou o símbodo "_"</returns>
    public static bool IsLetterDigitOrUnderscore(this char c)
    {
      bool isTecladoNumerico =
        c >= (int)System.Windows.Forms.Keys.NumPad0 &&
        c <= (int)System.Windows.Forms.Keys.NumPad9;

      return isTecladoNumerico ||
        char.IsLetterOrDigit(c) ||
        (c == '_');
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

    /// <summary>
    /// Recuperar a palavra sobre o cursor
    /// </summary>
    /// <param name="source">Texto completo</param>
    /// <param name="position">Posição onde o cursor se encontra</param>
    /// <returns>Palavra; string.Empty caso não seja encontrada</returns>
    public static string GetWordAt(this string source, int position)
    {
      // Evitar index out of range
      if (position > source.Length)
        position = source.Length;

      // Tratar ;
      if (position > 0 && position == source.Length && source[position - 1] == ';')
        position--;

      string palavra = InternalGetWordAt(source, position, SentidoProcura.Ambos, true);
      return palavra;
    }
  }
}
