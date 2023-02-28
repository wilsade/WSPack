using System;
using System.Collections.Generic;
using System.Windows.Forms;

using WSPack.Lib.DocumentationObjects;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.DocumentationObjects.Macros;

namespace WSPack.VisualStudio.Shared.Forms
{
  /// <summary>
  /// Definir características para o form de documentação
  /// </summary>
  public interface IDocumentationRuleBaseForm
  {
    /// <summary>
    /// Nome da regra
    /// </summary>
    string NomeRegra { get; set; }

    /// <summary>
    /// Título do controle de condição
    /// </summary>
    string CaptionControleCondicao { get; set; }

    /// <summary>
    /// Texto do rótulo: Returns
    /// </summary>
    string CaptionReturns { get; set; }

    /// <summary>
    /// Condição escolhida no controle
    /// </summary>
    ConditionItem Condition { get; set; }

    /// <summary>
    /// Estrutura de Summary
    /// </summary>
    string Summary { get; set; }

    /// <summary>
    /// Estrutura de Returns
    /// </summary>
    string Returns { get; set; }

    /// <summary>
    /// Estrutura de Remarks
    /// </summary>
    string Remarks { get; set; }

    /// <summary>
    /// Inicializar os controles do form conforme a regra
    /// </summary>
    /// <param name="ruleBaseItem">Rule base item</param>
    void Initialize(RuleBaseItem ruleBaseItem);

    /// <summary>
    /// Recuperar para qual item a regra será válida
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum.</typeparam>
    TEnum GetValidoPara<TEnum>();

    /// <summary>
    /// Indica para qual item a regra será válida
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum.</typeparam>
    void SetValidoPara<TEnum>(TEnum validoPara);

    /// <summary>
    /// Recuperar o próximo Id de regras
    /// </summary>
    /// <returns>próximo Id de regras</returns>
    int GetNextRuleId();
  }

  /// <summary>
  /// Form básico para definição de regra de documentação
  /// </summary>
  /// <seealso cref="Form" />
  public partial class DocumentationRuleBaseForm : Form, IDocumentationRuleBaseForm
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="DocumentationRuleBaseForm"/>
    /// </summary>
    public DocumentationRuleBaseForm()
    {
      InitializeComponent();
      PreencherValidoPara(cbValidoPara);
      Self = this;
    }
    #endregion

    /// <summary>
    /// Criar uma instância do form conforme tipo de regra
    /// </summary>
    /// <param name="ruleBaseItem">Rule base item</param>
    /// <returns>Instância do form conforme tipo de regra</returns>
    public static DocumentationRuleBaseForm Create(RuleBaseItem ruleBaseItem)
    {
      if (ruleBaseItem is TypeRuleItem)
        return new DocumentationRuleTypeForm();
      else if (ruleBaseItem is MemberRuleItem)
        return new DocumentationRuleMemberForm();
      else
        throw new NotImplementedException();
    }

    /// <summary>
    /// Preencher o comboBox: ValidoPara
    /// </summary>
    /// <param name="comboBox">Combo box</param>
    protected virtual void PreencherValidoPara(ComboBox comboBox) { }

    /// <summary>
    /// Botao ok
    /// </summary>
    protected Button BotaoOK => btnOK;

    /// <summary>
    /// Evento disparado quando o valor do ComboBox é alterado
    /// </summary>
    protected event EventHandler<EventArgs> AlterouValidoPara;

    /// <summary>
    /// Habilitar a estrutura de retorno
    /// </summary>
    /// <param name="valor">Informe "true" para valor</param>
    protected void HabilitarReturns(bool valor)
    {
      lbReturns.Enabled = edtReturns.Enabled = btnMacroReturns.Enabled = valor;
    }

    /// <summary>
    /// Habilitar a estrutura de remarks
    /// </summary>
    /// <param name="valor">Informe "true" para valor</param>
    protected void HabilitarRemarks(bool valor)
    {
      lbRemarks.Enabled = edtRemarks.Enabled = btnMacroRemarks.Enabled = valor;
    }

    /// <summary>
    /// LoadMacrosForTypes
    /// </summary>
    /// <param name="typesEnum">TypesEnum</param>
    protected virtual bool LoadMacrosForTypes(out TypeTypesEnum typesEnum)
    {
      typesEnum = TypeTypesEnum.All;
      return false;
    }

    /// <summary>
    /// LoadMacrosForMembers
    /// </summary>
    /// <param name="membersEnum">TypesEnum</param>
    protected virtual bool LoadMacrosForMembers(out MemberTypesEnum membersEnum)
    {
      membersEnum = MemberTypesEnum.All;
      return false;
    }

    /// <summary>
    /// Definir características para o form de documentação
    /// </summary>
    public IDocumentationRuleBaseForm Self { get; }

    /// <summary>
    /// Recuperar a condição escolhida no controle
    /// </summary>
    public ConditionItem Condition
    {
      get { return controleCondicao.Condition; }
      set { controleCondicao.Condition = value; }
    }

    /// <summary>
    /// Inicializar os controles do form conforme a regra
    /// </summary>
    /// <param name="ruleBaseItem">Rule base item</param>
    public virtual void Initialize(RuleBaseItem ruleBaseItem)
    {
      Self.Condition = ruleBaseItem.NameCondition;
      Self.Summary = ruleBaseItem.Summary;
      Self.NomeRegra = ruleBaseItem.ItemName;
      Self.Returns = ruleBaseItem.Returns;
      Self.Remarks = ruleBaseItem.Remarks;
    }

    /// <summary>
    /// Criar uma regra conforme parâmetros do form
    /// </summary>
    /// <param name="id">Id da regra</param>
    /// <returns>Regra</returns>
    public virtual RuleBaseItem CreateRule(int id)
    {
      return null;
    }

    /// <summary>
    /// Recuperar o próximo Id de regras
    /// </summary>
    /// <returns>próximo Id de regras</returns>
    public virtual int GetNextRuleId()
    {
      throw new NotSupportedException();
    }

    /// <summary>
    /// Recuperar para qual item a regra será válida
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum.</typeparam>
    TEnum IDocumentationRuleBaseForm.GetValidoPara<TEnum>()
    {
      object obj = Enum.Parse(typeof(TEnum), cbValidoPara.Text);
      return (TEnum)obj;
    }

    /// <summary>
    /// Sets the valido para.
    /// </summary>
    /// <typeparam name="TValidoPara">The type of the valido para.</typeparam>
    /// <param name="validoPara">Valido para</param>
    void IDocumentationRuleBaseForm.SetValidoPara<TValidoPara>(TValidoPara validoPara)
    {
      cbValidoPara.SelectedItem = validoPara.ToString();
    }

    /// <summary>
    /// Nome da regra
    /// </summary>
    string IDocumentationRuleBaseForm.NomeRegra
    {
      get { return edtNomeRegra.Text; }
      set { edtNomeRegra.Text = value; }
    }

    /// <summary>
    /// Título do controle de condição
    /// </summary>
    string IDocumentationRuleBaseForm.CaptionControleCondicao
    {
      get { return controleCondicao.Caption; }
      set { controleCondicao.Caption = value; }
    }

    /// <summary>
    /// Texto do rótulo: Returns
    /// </summary>
    string IDocumentationRuleBaseForm.CaptionReturns
    {
      get { return lbReturns.Text; }
      set { lbReturns.Text = value; }
    }

    /// <summary>
    /// Estrutura de Summary
    /// </summary>
    string IDocumentationRuleBaseForm.Summary
    {
      get { return edtSummary.Text; }
      set { edtSummary.Text = value; }
    }

    /// <summary>
    /// Estrutura de Returns
    /// </summary>
    string IDocumentationRuleBaseForm.Returns
    {
      get { return edtReturns.Text; }
      set { edtReturns.Text = value; }
    }

    /// <summary>
    /// Estrutura de Remarks
    /// </summary>
    string IDocumentationRuleBaseForm.Remarks
    {
      get { return edtRemarks.Text; }
      set { edtRemarks.Text = value; }
    }

    private void DocumentationRuleBaseForm_Load(object sender, EventArgs e)
    {
      toolTip1.SetToolTip(btnMacroSummary, ResourcesLib.StrAbrirTelaParaInsercaoMacro);
      toolTip1.SetToolTip(btnMacroReturns, ResourcesLib.StrAbrirTelaParaInsercaoMacro);
      toolTip1.SetToolTip(btnMacroRemarks, ResourcesLib.StrAbrirTelaParaInsercaoMacro);
      controleCondicao.OnNameChanged += DocumentationConditionControl1_OnNameChanged;
      controleCondicao.OnTypeChanged += ControleCondicao_OnTypeChanged;
    }

    private void ControleCondicao_OnTypeChanged(object sender, EventArgs e)
    {
      BotaoOKHabilitado();
    }

    private void DocumentationConditionControl1_OnNameChanged(object sender, EventArgs e)
    {
      BotaoOKHabilitado();
    }

    /// <summary>
    /// Habilitar o botão OK mediante condição
    /// </summary>
    protected virtual void BotaoOKHabilitado()
    {
      btnOK.Enabled = !controleCondicao.HasErros && !string.IsNullOrEmpty(edtNomeRegra.Text);
    }

    private void DocumentationRuleBaseForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
        Close();
    }

    private void edtNomeRegra_TextChanged(object sender, EventArgs e)
    {
      BotaoOKHabilitado();
    }

    private void cbValidoPara_SelectedIndexChanged(object sender, EventArgs e)
    {
      AlterouValidoPara?.Invoke(sender, e);
    }

    private void btnMacroSummary_Click(object sender, EventArgs e)
    {
      ShowMacroForm(edtSummary);
    }

    private void ShowMacroForm(TextBox textBox)
    {
      using (var form = new DocumentationMacrosForm())
      {
        if (this is DocumentationRuleMemberForm)
        {
          List<MacroGroupItems> lst = MacroMemberItems.CreateGroup();
          lst.ForEach(x => form.LoadMacros(x));
        }

        form.EditText = textBox.Text;
        if (form.ShowDialog() == DialogResult.OK)
        {
          textBox.Text = form.EditText;
          textBox.Focus();
          textBox.SelectionStart = textBox.Text.Length;
        }
      }
    }

    private void btnMacroReturns_Click(object sender, EventArgs e)
    {
      ShowMacroForm(edtReturns);
    }

    private void btnMacroRemarks_Click(object sender, EventArgs e)
    {
      ShowMacroForm(edtRemarks);
    }
  }
}
