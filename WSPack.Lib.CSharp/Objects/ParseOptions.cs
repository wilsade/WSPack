namespace WSPack.Lib.CSharp.Objects
{
  /// <summary>
  /// Opções para o Parser
  /// </summary>
  public class ParseOptions
  {
    /// <summary>
    /// Extrair métodos?
    /// </summary>
    public bool ExtractMethods { get; set; }

    /// <summary>
    /// Extrair blocos?
    /// </summary>
    public bool ExtractBlocks { get; set; }

    /// <summary>
    /// Constantes definidas como diretiva de compilação. Ex: DEBUG, TRACE
    /// </summary>
    public string DefineConsts { get; set; }

    /// <summary>
    /// Cria uma instância com valores padrões
    /// </summary>
    /// <param name="extractMethods">Informe "true" para extrair os métodos</param>
    /// <param name="extractBlocks">Informe "true" para extrair os blocos</param>
    /// <param name="defineConsts">Constantes definidas como diretiva de compilação. Ex: DEBUG, TRACE</param>
    /// <returns>Instância</returns>
    public static ParseOptions Create(bool extractMethods = true, bool extractBlocks = true,
      string defineConsts = "")
    {
      return new ParseOptions()
      {
        ExtractMethods = extractMethods,
        ExtractBlocks = extractBlocks,
        DefineConsts = defineConsts
      };
    }
  }

}
