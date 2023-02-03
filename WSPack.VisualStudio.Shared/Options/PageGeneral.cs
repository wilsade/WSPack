using System.ComponentModel;

using Microsoft.VisualStudio.Shell;

using WSPack.Lib;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Classe base para os parâmetros do WSPack
  /// </summary>
  public sealed class PageGeneral : DialogPage
  {
    /// <summary>
    /// Inicialização da classe: <see cref="PageGeneral"/>.
    /// </summary>
    public PageGeneral()
    {
      WSPackConfigPath = BaseDialogPage.BaseConfigPath;
      EnableDebugMode = false;
      //OnGuardarOpcoes += PageGeneral_OnGuardarOpcoes;
      //FormatOnSaveOptions = new FormatOnSaveExpandableOptions();
      //ForceUTF8OnSaveOptions = new ForceUTF8OnSaveExpandableOptions();
      //ShowGitPullInSolutionExplorerToolbar = true;
    }

    /// <summary>
    /// Handles "apply" messages from the Visual Studio environment.
    /// </summary>
    /// <param name="e">Parâmetros</param>
    /// <devdoc>
    /// This method is called when VS wants to save the user's
    /// changes then the dialog is dismissed.
    /// </devdoc>
    protected override void OnApply(PageApplyEventArgs e)
    {
      if (string.IsNullOrEmpty(WSPackConfigPath))
        WSPackConfigPath = BaseDialogPage.BaseConfigPath;

      base.OnApply(e);
    }

    #region Editor
    /// <summary>
    /// Editor de texto que deverá ser utilizado abertura de arquivos
    /// </summary>
    [Category(OptionsPageConsts.Editor)]
    [DisplayName("Editor de texto")]
    [Description("Editor de texto que deverá ser utilizado para abertura de arquivos. Informe o caminho completo do executável.")]
    [Editor(typeof(PathExeEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [ReadOnly(false)]
    public string EditorTexto { get; set; }
    #endregion

    #region Outros
    /// <summary>
    /// Caminho onde serão salvos os arquivos de configuração gerados
    /// </summary>
    [Category(OptionsPageConsts.Outros)]
    [DisplayName("Arquivos de configuração")]
    [Description("Informe o caminho onde serão salvos os arquivos de configuração gerados.")]
    [Editor(typeof(FolderPathEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [ReadOnly(false)]
    public string WSPackConfigPath { get; set; }

    /// <summary>
    /// Habilitar modo Debug
    /// </summary>
    [Category(OptionsPageConsts.Outros)]
    [DisplayName("Habilitar modo de Depuração")]
    [Description("Habilitar o modo de Depuração (Debug) para maior rastreabilidade do WSPack.")]
    [DefaultValue(false)]
    public bool EnableDebugMode { get; set; }
    #endregion

    #region TFS
    /// <summary>
    /// Um valor indicando se a tela de merge será aberta após o Check In
    /// </summary>
    [Category(OptionsPageConsts.TFS)]
    [DisplayName("Abrir tela de merge após Check In")]
    [Description("Exibir um tela para escolha da branch de destino para efetuar operação de merge logo após o Check In")]
    [DefaultValue(false)]
    public bool AbrirTelaMerge { get; set; }
    #endregion
  }
}