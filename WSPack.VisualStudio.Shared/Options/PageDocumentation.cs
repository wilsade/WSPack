using System;
using System.ComponentModel;

using WSPack.Lib.Extensions;
using WSPack.VisualStudio.Shared.DocumentationObjects.Macros;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Página de opções para criação de summary
  /// </summary>
  public class PageDocumentation : BaseDialogPage
  {
    string _oldDocPath;
    string _defaultSummaryForTypes, _defaultSummaryForMembers;

    #region Construtor
    /// <summary>
    /// Inicialização da classe <see cref="PageDocumentation"/>
    /// </summary>
    public PageDocumentation()
    {
      GenerateForTripleSlash = true;
      SearchBaseDocumentation = true;
      SearchSelfDocumentation = true;
      GenerateForGenericTypesInTypes = true;
      GenerateForGenericTypesInMembers = true;
      DefaultSummaryForTypes = MacroBaseItems.GetMacroText(MacrosConsts.ElementName);
      DefaultSummaryForMembers = MacroBaseItems.GetMacroText(MacrosConsts.ElementName);
      DocumentationPath = WSPackPackage.ParametrosGerais.WSPackConfigPath;
      OnGuardarOpcoes += PageDocumentation_OnGuardarOpcoes;
    }
    #endregion

    private void PageDocumentation_OnGuardarOpcoes(object sender, EventArgs e)
    {
      _oldDocPath = DocumentationPath;
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
      if (DocumentationPath.IsNullOrEmptyEx())
        DocumentationPath = WSPackPackage.ParametrosGerais.WSPackConfigPath;

      if (e.ApplyBehavior == ApplyKind.Apply)
      {
        MostrarMensagemReiniciar = _oldDocPath != DocumentationPath;
      }
      base.OnApply(e);
    }

    /// <summary>
    /// Indica se o comando de documentação está habilitado
    /// </summary>
    [DisplayName("Gerar documentação ao digitar ///")]
    [Description("Habilitar a geração de documentação (criação de summary) ao digitar ///.")]
    [DefaultValue(true)]
    [Category(OptionsPageConsts.Geral)]
    public bool GenerateForTripleSlash { get; set; }

    /// <summary>
    /// Caminho do arquivo de configuração
    /// </summary>
    [DisplayName("Local do arquivo")]
    [Description("Informe o caminho onde será salvo o arquivo de documentação.")]
    [DefaultValue(null)]
    [Editor(typeof(FolderPathEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [Category(OptionsPageConsts.Geral)]
    public string DocumentationPath { get; set; }

    /// <summary>
    /// Procurar summary nos Tipos base
    /// </summary>
    [DisplayName("Procurar documentação nos Tipos 'bases'")]
    [Description("Ao documentar um Tipo ou Membro, procurar por summary definido nas Classes ancestrais e Interfaces.")]
    [DefaultValue(true)]
    [Category(OptionsPageConsts.Geral)]
    public bool SearchBaseDocumentation { get; set; }

    /// <summary>
    /// Procurar summary no próprio arquivo
    /// </summary>
    [DisplayName("Procurar documentação no próprio arquivo")]
    [Description("Ao documentar um Tipo ou Membro, procurar por summary definido nas Classes e Membros definidos no próprio arquivo .cs.")]
    [DefaultValue(true)]
    [Category(OptionsPageConsts.Geral)]
    public bool SearchSelfDocumentation { get; set; }

    /// <summary>
    /// Exibir aviso ao documentar um elemento
    /// </summary>
    [DisplayName("Exibir aviso ao documentar um elemento")]
    [Description("Ao documentar um Tipo ou Membro, exibir informações no painel Output do 'WSPack' sobre qual regra documentou o elemento.")]
    [DefaultValue(false)]
    [Category(OptionsPageConsts.Geral)]
    public bool ShowOutputDocumentationWaring { get; set; }

    /// <summary>
    /// Summary padrão para Tipos
    /// </summary>
    [DisplayName("Summary padrão para Tipos")]
    [Description("Informe o summary para Tipos caso nenhuma regra tenha sido definida para ele.")]
    [DefaultValue("$(Element.Name)")]
    [Category(OptionsPageConsts.SummaryDefault)]
    [Editor(typeof(MacroSummaryEditor), typeof(System.Drawing.Design.UITypeEditor))]
    public string DefaultSummaryForTypes
    {
      get => _defaultSummaryForTypes;
      set
      {
        if (value.IsNullOrWhiteSpaceEx())
          _defaultSummaryForTypes = MacroBaseItems.GetMacroText(MacrosConsts.ElementName);
        _defaultSummaryForTypes = value;
      }
    }

    /// <summary>
    /// Summary padrão para Membros
    /// </summary>
    [DisplayName("Summary padrão para Membros")]
    [Description("Informe o summary para Membros caso nenhuma regra tenha sido definida para ele.")]
    [DefaultValue("$(Element.Name)")]
    [Category(OptionsPageConsts.SummaryDefault)]
    [Editor(typeof(MacroSummaryEditor), typeof(System.Drawing.Design.UITypeEditor))]
    public string DefaultSummaryForMembers
    {
      get => _defaultSummaryForMembers;
      set
      {
        if (value.IsNullOrWhiteSpaceEx())
          _defaultSummaryForMembers = MacroBaseItems.GetMacroText(MacrosConsts.ElementName);
        _defaultSummaryForMembers = value;
      }
    }

    /// <summary>
    /// Gerar summary para Argumentos genéricos presentes em Types
    /// </summary>
    [DisplayName("Presentes em Tipos")]
    [Description("Ao documentar um tipo, gerar summary 'typeparam' para os Argumentos genéricos.")]
    [DefaultValue(true)]
    [Category(OptionsPageConsts.GenericTypes)]
    public bool GenerateForGenericTypesInTypes { get; set; }

    /// <summary>
    /// Gerar summary para Tipos genéricos presentes em Membros
    /// </summary>
    [DisplayName("Presentes em Membros")]
    [Description("Ao documentar um Membro, gerar summary 'typeparam' para os Argumentos genéricos.")]
    [DefaultValue(true)]
    [Category(OptionsPageConsts.GenericTypes)]
    public bool GenerateForGenericTypesInMembers { get; set; }
  }
}