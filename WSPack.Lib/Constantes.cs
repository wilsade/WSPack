using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.Lib
{
  /// <summary>
  /// Definição de constantes
  /// </summary>
  public static class Constantes
  {
    /// <summary>
    /// Nº de versão do Package e Assembly
    /// </summary>
    public const string NumeroVersao = "4.0.0.1";

    /// <summary>
    /// Caminho do projeto WSPack no github
    /// </summary>
    public const string GitHubWSPack = "https://github.com/wilsade/WSPack";

    /// <summary>
    /// Caminho do projeto WSPack no github
    /// </summary>
    public static readonly string GitHubWSPackWiki = $"{GitHubWSPack}/wiki";

    /// <summary>
    /// Release Notes Url
    /// </summary>
    public static readonly string GitHubWSPackReleaseNotes = $"{GitHubWSPackWiki}/Release-Notes";
  }
}
