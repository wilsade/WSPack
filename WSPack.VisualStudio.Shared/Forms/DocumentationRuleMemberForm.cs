using System;
using System.Linq;
using System.Windows.Forms;

using WSPack.Lib.DocumentationObjects;

namespace WSPack.VisualStudio.Shared.Forms
{
  /// <summary>
  /// Form para definição de regra de documentação de membros
  /// </summary>
  public partial class DocumentationRuleMemberForm : DocumentationRuleBaseForm
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="DocumentationRuleMemberForm"/>
    /// </summary>
    public DocumentationRuleMemberForm()
    {
      InitializeComponent();
      HabilitarReturns(false);
      AlterouValidoPara += cbValidoPara_SelectedIndexChanged;
      controleCondicaoTipo.OnTypeChanged += ControleCondicaoTipo_OnTypeChanged;
    }
    #endregion

    private void ControleCondicaoTipo_OnTypeChanged(object sender, EventArgs e)
    {
      BotaoOKHabilitado();
    }

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="memberRule">Member rule</param>
    public override void Initialize(RuleBaseItem memberRule)
    {
      base.Initialize(memberRule);
      ConditionType = ((MemberRuleItem)memberRule).TypeCondition;
      Self.SetValidoPara(((MemberRuleItem)memberRule).MemberType);
    }

    /// <summary>
    /// Criar uma regra conforme parâmetros do form
    /// </summary>
    /// <param name="id">Id da regra</param>
    /// <returns>Regra</returns>
    public override RuleBaseItem CreateRule(int id)
    {
      var validoPara = Self.GetValidoPara<MemberTypesEnum>();
      var rule = new MemberRuleItem(id, Self.NomeRegra, Condition,
        ConditionType, validoPara, Self.Summary)
      {
        Returns = PodeTerReturns(validoPara) ? Self.Returns : "",
        Remarks = PodeTerRemarks(validoPara) ? Self.Remarks : ""
      };
      return rule;
    }

    /// <summary>
    /// Recuperar a condição de type escolhida no controle
    /// </summary>
    public ConditionItem ConditionType
    {
      get { return controleCondicaoTipo.Condition; }
      set { controleCondicaoTipo.Condition = value; }
    }

    /// <summary>
    /// Preencher o comboBox: ValidoPara
    /// </summary>
    /// <param name="comboBox"></param>
    protected override void PreencherValidoPara(ComboBox comboBox)
    {
      comboBox.Items.Clear();
      comboBox.Items.AddRange(Enum.GetNames(typeof(MemberTypesEnum)));
      comboBox.SelectedIndex = 0;
    }

    /// <summary>
    /// Habilitar o botão OK mediante condição
    /// </summary>
    protected override void BotaoOKHabilitado()
    {
      errorProvider1.Clear();
      base.BotaoOKHabilitado();
      BotaoOK.Enabled &= !controleCondicaoTipo.HasErros;

      var tipoMembro = Self.GetValidoPara<MemberTypesEnum>();
      if (tipoMembro == MemberTypesEnum.All && Self.Condition.SearchCondition == SearchConditionsEnum.Any &&
        controleCondicaoTipo.Condition.SearchCondition == SearchConditionsEnum.Any)
      {
        errorProvider1.SetError(BotaoOK, "Regra válida para qualquer membro (All) não pode ter a condição 'Any'" +
          " definida nas duas condições.");
        BotaoOK.Enabled = false;
      }
    }

    /// <summary>
    /// LoadMacrosForMembers
    /// </summary>
    /// <param name="membersEnum">MembersEnum</param>
    /// <returns></returns>
    protected override bool LoadMacrosForMembers(out MemberTypesEnum membersEnum)
    {
      membersEnum = Self.GetValidoPara<MemberTypesEnum>();
      return true;
    }

    /// <summary>
    /// Recuperar o próximo Id de regras
    /// </summary>
    /// <returns>próximo Id de regras</returns>
    public override int GetNextRuleId()
    {
      var docParams = DocumentationUtils.ReadDocumentationParams(WSPackConsts.DocumentationConfigPath);
      if (docParams.RuleList.OfType<MemberRuleItem>().Any())
        return docParams.RuleList.OfType<MemberRuleItem>().Max(x => x.Id) + 1;
      else
        return 1;
    }

    private void DocumentationRuleMemberForm_Load(object sender, EventArgs e)
    {
      controleCondicaoTipo.OnNameChanged += ControleCondicaoTipo_OnNameChanged;
      HabilitarReturns(PodeTerReturns(Self.GetValidoPara<MemberTypesEnum>()));
      BotaoOKHabilitado();
    }

    private void ControleCondicaoTipo_OnNameChanged(object sender, EventArgs e)
    {
      BotaoOKHabilitado();
    }

    private void cbValidoPara_SelectedIndexChanged(object sender, EventArgs e)
    {
      MemberTypesEnum validoPara = Self.GetValidoPara<MemberTypesEnum>();

      // Tratar caption de método
      if (validoPara == MemberTypesEnum.Method)
        controleCondicaoTipo.Caption = "Definição da condição para o tipo de retorno:";
      else
        controleCondicaoTipo.Caption = "Definição da condição para o tipo:";

      // Tratar caption de construtor
      if (validoPara == MemberTypesEnum.Constructor)
        Self.CaptionControleCondicao = "Definição da condição para o tipo do construtor (DeclaringType):";
      else
        Self.CaptionControleCondicao = "Definição da condição para o nome:";

      // Construtor não tem condição para nome
      controleCondicaoTipo.Enabled = validoPara != MemberTypesEnum.Constructor;

      HabilitarReturns(PodeTerReturns(validoPara));
      HabilitarRemarks(PodeTerRemarks(validoPara));

      // Tratar caption de propriedade
      if (validoPara == MemberTypesEnum.Property)
        Self.CaptionReturns = "<value>";
      else
        Self.CaptionReturns = "<returns>";

      BotaoOKHabilitado();
    }

    private static bool PodeTerReturns(MemberTypesEnum validoPara)
    {
      return validoPara == MemberTypesEnum.Property ||
        validoPara == MemberTypesEnum.Method ||
        validoPara == MemberTypesEnum.All;
    }

    private static bool PodeTerRemarks(MemberTypesEnum validoPara)
    {
      return validoPara != MemberTypesEnum.Parameters;
    }
  }
}
