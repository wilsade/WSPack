using System.IO;

using WSPack.VisualStudio.Shared.Options;

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

    /// <summary>
    /// Caminho do arquivo de configuração dos favoritos do TFS
    /// </summary>
    public static string TFSFavoritesConfigPath => Path.Combine(
      WSPackPackage.GetParametersPage<PageGeneral>().WSPackConfigPath,
      "TFSFavoritesParams.cfg");

    /// <summary>
    /// Caminho do arquivo de configuração de documentação
    /// </summary>
    public static string DocumentationConfigPath = Path.Combine(
      WSPackPackage.ParametrosDocumentation.DocumentationPath, "WSPackDocumentation.cfg");

    /// <summary>
    /// Caminho do arquivo de configuração da StartPage
    /// </summary>
    public static string StartPageConfigPath = Path.Combine(
      WSPackPackage.ParametrosGerais.WSPackConfigPath, "WSPackStartPage.cfg");
  }
}