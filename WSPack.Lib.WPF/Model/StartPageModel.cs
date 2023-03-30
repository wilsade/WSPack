namespace WSPack.Lib.WPF.Model
{
  /// <summary>
  /// Modelo da StartPage
  /// </summary>
  public class StartPageModel
  {
    internal const double ComprimentoDefaultMinimoProjectContainer = 275;

    /// <summary>
    /// ComprimentoMinimoProjectContainer
    /// </summary>
    public const double ComprimentoMinimoProjectContainer = 150;

    /// <summary>
    /// Indica se a página está em modo de edição
    /// </summary>
    public bool IsInEditMode { get; set; }

    /// <summary>
    /// Exibir ou não os diretórios de cada projeto
    /// </summary>
    public bool ShowProjectsDirectory { get; set; }

    /// <summary>
    /// Indica se a janela está recolhida
    /// </summary>
    public bool IsExpanded { get; set; } = true;

    /// <summary>
    /// Altura máxima do painel de projetos
    /// </summary>
    public double ProjectContainerWidth { get; set; } = ComprimentoDefaultMinimoProjectContainer;

    /// <summary>
    /// Altura máxima do painel de projetos
    /// </summary>
    public double ProjectContainerMaxHeight { get; set; } = 500;

    /// <summary>
    /// Indica se a barra de rolagem horizontal será exibida
    /// </summary>
    public bool ProjectHorizontalScrollVisible { get; set; }
  }
}
