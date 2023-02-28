using System;
using System.Linq;
using System.Windows.Forms;

using WSPack.Lib.DocumentationObjects;

namespace WSPack.VisualStudio.Shared.Forms
{
  /// <summary>
  /// Form para definição de regra de documentação de 'Tipos'
  /// </summary>
  public partial class DocumentationRuleTypeForm : DocumentationRuleBaseForm
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="DocumentationRuleTypeForm"/>
    /// </summary>
    public DocumentationRuleTypeForm()
    {
      InitializeComponent();
      HabilitarReturns(false);
      AlterouValidoPara += DocumentationRuleTypeForm_AlterouValidoPara;
    }

    private void DocumentationRuleTypeForm_AlterouValidoPara(object sender, EventArgs e)
    {
      TypeTypesEnum validoPara = Self.GetValidoPara<TypeTypesEnum>();
      HabilitarReturns(validoPara == TypeTypesEnum.Delegates);
    }
    #endregion

    /// <summary>
    /// Inicializar os controles do form conforme a regra
    /// </summary>
    /// <param name="ruleBaseItem">Rule base item</param>
    public override void Initialize(RuleBaseItem ruleBaseItem)
    {
      base.Initialize(ruleBaseItem);
      Self.SetValidoPara(((TypeRuleItem)ruleBaseItem).TypeType);
    }

    /// <summary>
    /// Criar uma regra conforme parâmetros do form
    /// </summary>
    /// <param name="id">Id da regra</param>
    /// <returns>Regra</returns>
    public override RuleBaseItem CreateRule(int id)
    {
      var validoPara = Self.GetValidoPara<TypeTypesEnum>();
      var rule = new TypeRuleItem(id, Self.NomeRegra, Condition)
      {
        Summary = Self.Summary,
        TypeType = validoPara,
        Returns = validoPara == TypeTypesEnum.Delegates ? Self.Returns : "",
        Remarks = Self.Remarks
      };
      return rule;
    }

    /// <summary>
    /// Preencher o comboBox: ValidoPara
    /// </summary>
    /// <param name="comboBox"></param>
    protected override void PreencherValidoPara(ComboBox comboBox)
    {
      comboBox.Items.Clear();
      comboBox.Items.AddRange(Enum.GetNames(typeof(TypeTypesEnum)));
      comboBox.SelectedIndex = 0;
    }

    /// <summary>
    /// LoadMacrosForTypes
    /// </summary>
    /// <param name="typesEnum">TypesEnum</param>
    /// <returns></returns>
    protected override bool LoadMacrosForTypes(out TypeTypesEnum typesEnum)
    {
      typesEnum = Self.GetValidoPara<TypeTypesEnum>();
      return true;
    }

    /// <summary>
    /// Recuperar o próximo Id de regras
    /// </summary>
    /// <returns>próximo Id de regras</returns>
    public override int GetNextRuleId()
    {
      var docParams = DocumentationUtils.ReadDocumentationParams(WSPackConsts.DocumentationConfigPath);
      if (docParams.RuleList.OfType<TypeRuleItem>().Any())
        return docParams.RuleList.OfType<TypeRuleItem>().Max(x => x.Id) + 1;
      else
        return 1;
    }
  }
}
