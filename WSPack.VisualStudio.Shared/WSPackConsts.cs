using System.IO;

namespace WSPack.VisualStudio.Shared
{
  /// <summary>
  /// Constantes referentes ao WSPack
  /// </summary>
  public class WSPackConsts
  {
    /// <summary>
    /// Caminho do arquivo de configuração da Busca Changesets
    /// </summary>
    public static string SearchChangesetsConfigPath = Path.Combine(
      WSPackPackage.ParametrosGerais.WSPackConfigPath, "SearchChangesetsParams.cfg");
  }
}