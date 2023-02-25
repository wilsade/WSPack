using System;
using System.Collections.Generic;
using System.IO;
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
    public const string NumeroVersao = "4.0.0.8";

    /// <summary>
    /// WSPack
    /// </summary>
    public const string WSPack = "WSPack";

    /// <summary>
    /// Meu nome
    /// </summary>
    public const string WilliamSadeDePaiva = "William Sade de Paiva";

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
    public static readonly string GitHubWSPackReleaseNotes = $"{GitHubWSPackWiki}/Novidades-da-versão";

    /// <summary>
    /// string representando: [Gerenciador...]
    /// </summary>
    public static readonly string GerenciadorFavoritos = "[Gerenciador...]";
  }
}
