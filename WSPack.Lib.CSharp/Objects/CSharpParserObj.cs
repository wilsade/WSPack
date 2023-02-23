using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using WSPack.Lib.CSharp.Models;
using WSPack.Lib.CSharp.Walkers;

namespace WSPack.Lib.CSharp.Objects
{
  /// <summary>
  /// Classe responsável por fazer parser de código C#
  /// </summary>
  public class CSharpParserObj
  {
    SyntaxTree _tree;
    SyntaxNode _root;

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="CSharpParserObj"/>
    /// </summary>
    internal CSharpParserObj()
    {

    }
    #endregion

    static async Task<SyntaxTree> InternalParseAsync(string sourceCode, ParseOptions options)
    {
      Task<SyntaxTree> task = Task.Run(() =>
      {
        CSharpParseOptions csharpOptions = CSharpParseOptions.Default;
        if (options?.DefineConsts != null)
          csharpOptions = csharpOptions.WithPreprocessorSymbols(options.DefineConsts.Split(';'));

        var tree = CSharpSyntaxTree.ParseText(sourceCode, csharpOptions);
        return tree;
      });
      return await task.ConfigureAwait(false);
    }

    /// <summary>
    /// Extrair informações de um código fonte C#
    /// </summary>
    /// <param name="sourceCode">Source code</param>
    /// <param name="options">Opções para o Parser</param>
    public static async Task<CSharpParserObj> ParseAsync(string sourceCode, ParseOptions options = null)
    {
      var arvore = await InternalParseAsync(sourceCode, options).ConfigureAwait(false);

      var instancia = new CSharpParserObj()
      {
        _tree = arvore
      };

      IEnumerable<Diagnostic> lstDiags = instancia._tree.GetDiagnostics();
      instancia.HasErrors = lstDiags.Any(x => x.Severity == DiagnosticSeverity.Error);
      if (instancia.HasErrors)
      {
        instancia.BlocksList = new List<BlockModel>();
        instancia.MethodsList = new List<MethodModel>();
        return instancia;
      }

      if (options != null)
      {
        if (options.ExtractBlocks || options.ExtractMethods)
          instancia._root = await instancia._tree.GetRootAsync().ConfigureAwait(false);

        var lstTasks = new List<Task>();

        // Extrair métodos
        if (options.ExtractMethods)
        {
          lstTasks.Add(Task.Run(() =>
          {
            instancia.MethodsList = MethodsWalker.ExtractMethods(instancia._root);
          }));
        }

        // Extrair blocos
        if (options.ExtractBlocks)
        {
          lstTasks.Add(Task.Run(() =>
          {
            var lstBlocos = BlocksWalker.Calculate(instancia._root);
            instancia.BlocksList = lstBlocos;
          }));
        }

        Task.WaitAll(lstTasks.ToArray());
      }

      if (instancia.BlocksList == null)
        instancia.BlocksList = new List<BlockModel>();
      if (instancia.MethodsList == null)
        instancia.MethodsList = new List<MethodModel>();

      return instancia;
    }

    /// <summary>
    /// Extrair informações de um código fonte C#
    /// </summary>
    /// <param name="sourceCode">Source code</param>
    /// <param name="options">Opções para o Parser</param>
    public static CSharpParserObj Parse(string sourceCode, ParseOptions options = null)
    {
      var parser = ParseAsync(sourceCode, options).Result;
      return parser;
    }

    /// <summary>
    /// Indica se existem erros de sintaxe
    /// </summary>
    public bool HasErrors { get; private set; }

    /// <summary>
    /// Lista de métodos
    /// </summary>
    public List<MethodModel> MethodsList { get; private set; }

    /// <summary>
    /// Lista de blocos
    /// </summary>
    public List<BlockModel> BlocksList { get; private set; }
  }

}
