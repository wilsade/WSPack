using System;
using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.VisualStudio.Shell;

using WSPack.Lib;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Classe base para os parâmetros do WSPack
  /// </summary>
  public sealed class PageGeneral : BaseDialogPage
  {
    bool _oldUseSolutionNavigatorGraphProvider;
    string _oldWSPackConfigPath;

    /// <summary>
    /// Inicialização da classe: <see cref="PageGeneral"/>.
    /// </summary>
    public PageGeneral()
    {
      WSPackConfigPath = BaseDialogPage.BaseConfigPath;
      EnableDebugMode = false;
      OnGuardarOpcoes += PageGeneral_OnGuardarOpcoes;
      FormatOnSaveOptions = new FormatOnSaveExpandableOptions();
      ForceUTF8OnSaveOptions = new ForceUTF8OnSaveExpandableOptions();
      ShowGitPullInSolutionExplorerToolbar = true;
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

      if (e.ApplyBehavior == ApplyKind.Apply)
      {
        bool alterou = _oldWSPackConfigPath != WSPackConfigPath || VerificaRegistro();
        if (alterou)
          MostrarMensagemReiniciar = true;
      }
      base.OnApply(e);
    }

    private void PageGeneral_OnGuardarOpcoes(object sender, EventArgs e)
    {
      _oldWSPackConfigPath = WSPackConfigPath;
      try
      {
        DisableSolutionExplorerMembersNavigation = !RegistroVisualStudioObj.Instance.UseSolutionMemberNavigator;
        _oldUseSolutionNavigatorGraphProvider = DisableSolutionExplorerMembersNavigation;
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"PageGeneral_OnGuardarOpcoes: {ex.Message}");
        DisableSolutionExplorerMembersNavigation = false;
      }
    }

    /// <summary>
    /// Verificas se houve alteração em uma das opções do registro
    /// </summary>
    /// <returns>true se houve alteração em uma das opções do registro</returns>
    private bool VerificaRegistro()
    {
      bool alterou = false;
      try
      {
        if (_oldUseSolutionNavigatorGraphProvider != DisableSolutionExplorerMembersNavigation)
        {
          RegistroVisualStudioObj.Instance.UseSolutionMemberNavigator = !DisableSolutionExplorerMembersNavigation;
          alterou = true;
        }
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"VerificaRegistro: {ex.Message}");
      }

      return alterou;
    }

    /// <summary>
    /// This method is called when the dialog page should load its default settings from the backing store.
    /// </summary>
    public override void LoadSettingsFromStorage()
    {
      base.LoadSettingsFromStorage();

      var lst = new List<(string PropName, object ExpObject)>
      {
        (nameof(FormatOnSaveOptions), FormatOnSaveOptions)
      };

      _ = Utils.ExecuteInMainThreadAsync(() =>
      {
        LoadExpandableProperties("PageGeneral", lst);

        lst = new List<(string PropName, object ExpObject)>
        {
          (nameof(ForceUTF8OnSaveOptions), ForceUTF8OnSaveOptions)
        };
        LoadExpandableProperties("PageGeneral", lst);
      });

    }

    /// <summary>
    /// This method does the reverse of LoadSettingsFromStorage.
    /// </summary>
    public override void SaveSettingsToStorage()
    {
      base.SaveSettingsToStorage();
      var lst = new List<(string PropName, object ExpObject)>
      {
        (nameof(FormatOnSaveOptions), FormatOnSaveOptions)
      };

      SaveExpandableProperties("PageGeneral", lst);

      lst = new List<(string PropName, object ExpObject)>
      {
        (nameof(ForceUTF8OnSaveOptions), ForceUTF8OnSaveOptions)
      };

      SaveExpandableProperties("PageGeneral", lst);
    }

    /// <summary>
    /// Um valor indicando se a reflexão de objetos no Solution Explorer deverá ser desabilitada
    /// </summary>
    [Category(OptionsPageConsts.AlteracoesRegistro)]
    [DisplayName("Desabilitar visualização de membros no Solution Explorer")]
    [Description("Por padrão, no Solution Explorer, é possível navegar pelas classes e membros através de (>) nodos expansíveis. " +
      "Marque esta opção para desabilitar este recurso.")]
    [DefaultValue(false)]
    [ReadOnly(false)]
    public bool DisableSolutionExplorerMembersNavigation { get; set; }

    #region Editor
    /// <summary>
    /// Opções para formatação do documento ao salvar
    /// </summary>
    [DisplayName("Formatação do documento ao salvar")]
    [Description("Opções para formatação do documento ao salvar.")]
    [Category(OptionsPageConsts.Editor)]
    [ReadOnly(false)]
    public FormatOnSaveExpandableOptions FormatOnSaveOptions { get; set; }

    /// <summary>
    /// Opções para conversão UTF-8 do documento ao salvar
    /// </summary>
    [DisplayName("Forçar codificação UTF-8 do documento ao salvar")]
    [Description("Opções para conversão UTF-8 do documento ao salvar.")]
    [Category(OptionsPageConsts.Editor)]
    [ReadOnly(false)]
    public ForceUTF8OnSaveExpandableOptions ForceUTF8OnSaveOptions { get; set; }

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

    #region GIT
    /// <summary>
    /// Fazer Pull ao abrir Solution
    /// </summary>
    [Category(OptionsPageConsts.GIT)]
    [DisplayName("Fazer Pull ao abrir Solution")]
    [Description("Ao abrir uma solution, caso ela esteja em um repositório GIT, " +
      "faz um Pull conforme a opção de intervalo escolhida.")]
    [DefaultValue(GitPullOnOpenSolutionOptions.Nao)]
    public GitPullOnOpenSolutionOptions FazerPullAoAbrirSolution { get; set; }

    /// <summary>
    /// Fazer Pull ao abrir Solution
    /// </summary>
    [Category(OptionsPageConsts.GIT)]
    [DisplayName("Exibir Pull no Solution Explorer")]
    [Description("Exibir um botão de Pull no Barra de Ferramentas do Solution Explorer.")]
    [DefaultValue(true)]
    public bool ShowGitPullInSolutionExplorerToolbar { get; set; }
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

    #region Página inicial
    /// <summary>
    /// Exibir / Esconder a página inicial ao Fechar / Abrir uma Solution
    /// </summary>
    [Category(OptionsPageConsts.PaginaInicial)]
    [DisplayName("Exibir / Esconder automaticamente")]
    [Description("Exibir / Esconder a página inicial ao Fechar / Abrir uma Solution")]
    [DefaultValue(false)]
    public bool AutoShowHideStartPageOnSolutionCloseOpen { get; set; }
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

    /// <summary>
    /// Exibir / Esconder a página inicial ao Fechar / Abrir uma Solution
    /// </summary>
    [Category(OptionsPageConsts.TFS)]
    [DisplayName("Usar menu na Barra de ferramentas do Source Control Explorer")]
    [Description("Utilizar um controlador de menus para agrupar vários botões da " +
      "Barra de ferramentas do Source Control Explorer.")]
    [DefaultValue(false)]
    public bool UseMenuControllerToolbarSCE { get; set; }
    #endregion
  }
}