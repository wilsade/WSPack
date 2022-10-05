using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Página de opções para os MEF Objects
  /// </summary>
  public class PageMEFObjects : BaseDialogPage
  {
    #region Construtor
    /// <summary>
    /// Inicialização da classe: <see cref="PageMEFObjects"/>.
    /// </summary>
    public PageMEFObjects()
    {
      MetricsObj = new MetricsExpandableOptions();
      BlocksEndObj = new BlocksExpandableOptions();
      UseBookmarks = true;
      UseSearchWord = true;
      UseFileMargin = true;
      //UseGradientColor = true;
    }
    #endregion

    /// <summary>
    /// This method is called when the dialog page should load its default settings from the backing store.
    /// </summary>
    public override void LoadSettingsFromStorage()
    {
      base.LoadSettingsFromStorage();

      var lst = new List<Tuple<string, object>>
      {
        Tuple.Create<string, object>(nameof(BlocksEndObj), BlocksEndObj),
        Tuple.Create<string, object>(nameof(MetricsObj), MetricsObj)
      };

      LoadExpandableProperties("PageMEFObjects", lst);
    }

    /// <summary>
    /// This method does the reverse of LoadSettingsFromStorage.
    /// </summary>
    public override void SaveSettingsToStorage()
    {
      base.SaveSettingsToStorage();
      var lst = new List<Tuple<string, object>>
      {
        Tuple.Create<string, object>(nameof(BlocksEndObj), BlocksEndObj),
        Tuple.Create<string, object>(nameof(MetricsObj), MetricsObj)
      };

      SaveExpandableProperties("PageMEFObjects", lst);
    }

    /// <summary>
    /// Usar marcadores numerados para fácil navegação no código fonte
    /// </summary>
    [Category(OptionsPageConsts.Marcadores)]
    [DisplayName("Marcadores numerados")]
    [Description("Usar marcadores numerados para fácil navegação no código fonte.")]
    [DefaultValue(true)]
    public bool UseBookmarks { get; set; }

    /// <summary>
    /// Destacar palavras sobre o cursor
    /// </summary>
    [Category(OptionsPageConsts.DestacarPalavras)]
    [DisplayName("Destacar palavras sobre o cursor")]
    [Description("Ao posicionar o cursor sobre um texto, exibir na barra de rolagem e no editor todas as ocorrências do texto." +
      " Se Marcadores numerados estiver habilitado, eles serão exibidos nesta barra.")]
    [DefaultValue(true)]
    public bool UseSearchWord { get; set; }

    /// <summary>
    /// Exibir barra de texto no rodapé
    /// </summary>
    [Category(OptionsPageConsts.BarraTexto)]
    [DisplayName("Exibir barra de texto no rodapé")]
    [Description("Exibir uma barra de texto no rodapé informando o nome do arquivo e opções com o clique do Mouse.")]
    [DefaultValue(true)]
    public bool UseFileMargin { get; set; }

    /*
  /// <summary>
  /// Usar 'Cor gradiente' no texto selecionado
  /// </summary>
  [Category(OptionsPageConsts.CorGradiente)]
  [DisplayName("Usar 'Cor gradiente' no texto selecionado")]
  [Description("Destacar o texto selecionado utilizando 'Cor gradiente'.")]
  [DefaultValue(true)]
  public bool UseGradientColor { get; set; }*/

    #region Adereços no editor
    /// <summary>
    /// Definir opções para métricas
    /// </summary>
    [Category(OptionsPageConsts.AderecosEditor)]
    [DisplayName("Métricas")]
    [Description("Controlar a exibição de um pequeno bloco antes do nome de cada método contendo informações sobre métricas.")]
    [ReadOnly(false)]
    public MetricsExpandableOptions MetricsObj { get; set; }

    /// <summary>
    /// Definir opções para indicador de final de bloco
    /// </summary>
    [Category(OptionsPageConsts.AderecosEditor)]
    [DisplayName("Indicador de fim de bloco")]
    [Description("Controlar a exibição de um pequeno símbolo '<<' indicando o final de cada bloco de código.")]
    [ReadOnly(false)]
    public BlocksExpandableOptions BlocksEndObj { get; set; }

    #endregion
  }
}
