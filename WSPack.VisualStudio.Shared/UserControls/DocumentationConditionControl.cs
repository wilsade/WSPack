using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

using WSPack.Lib.DocumentationObjects;
using WSPack.Lib.Properties;

namespace WSPack.VisualStudio.Shared.UserControls
{
  /// <summary>
  /// Controle para definição de condição de regra de documentação
  /// </summary>
  /// <seealso cref="UserControl" />
  public partial class DocumentationConditionControl : UserControl
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="DocumentationConditionControl"/>
    /// </summary>
    public DocumentationConditionControl()
    {
      InitializeComponent();
      Configure();
    }
    #endregion

    private void Configure()
    {
      cbCondicoes.Items.Clear();
      cbCondicoes.Items.AddRange(Enum.GetNames(typeof(SearchConditionsEnum)));
      cbCondicoes.SelectedIndex = 0;
    }

    private void edtNome_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Space)
      {
        e.SuppressKeyPress = true;
        e.Handled = true;
      }
    }

    private void edtNome_TextChanged(object sender, EventArgs e)
    {
      var strErros = new StringBuilder();

      if (!string.IsNullOrEmpty(edtNome.Text))
      {
        if (char.IsNumber(edtNome.Text[0]))
          strErros.AppendLine("Não pode começar com número");

        if (edtNome.Text.Contains(" "))
          strErros.AppendLine(ResourcesLib.StrNaoPodeTerEspacos);
      }

      if (strErros.Length > 0)
        errorProvider1.SetError(edtNome, strErros.ToString());

      else
        errorProvider1.Clear();

      OnNameChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Condição escolhida
    /// </summary>
    SearchConditionsEnum GetNameCondition() => (SearchConditionsEnum)Enum.Parse(typeof(SearchConditionsEnum), cbCondicoes.Text);

    /// <summary>
    /// Título do Grupo
    /// </summary>
    [Description("Título que irá aparecer no grupo")]
    public string Caption
    {
      get { return groupBox1.Text; }
      set { groupBox1.Text = value; }
    }

    /// <summary>
    /// Indica se o controle possui erros
    /// </summary>
    public bool HasErros => !string.IsNullOrEmpty(errorProvider1.GetError(edtNome));

    /// <summary>
    /// Recuperar a condição definida
    /// </summary>
    public ConditionItem Condition
    {
      get { return new ConditionItem(GetNameCondition(), edtNome.Text, cbxIgnoreCase.Checked); }
      set
      {
        edtNome.Text = value.NameValue;
        cbxIgnoreCase.Checked = value.IgnoreCase;
        cbCondicoes.SelectedItem = value.SearchCondition.ToString();
      }
    }

    /// <summary>
    /// Evento disparado quando o nome é alterado
    /// </summary>
    [Description("Evento disparado quando o nome é alterado")]
    public event EventHandler<EventArgs> OnNameChanged;

    /// <summary>
    /// Evento disparado quando o tipo da condição (Any, EndsWith, etc) é alterado
    /// </summary>
    [Description("Evento disparado quando o tipo da condição (Any, EndsWith, etc) é alterado")]
    public event EventHandler<EventArgs> OnTypeChanged;

    private void cbCondicoes_SelectedIndexChanged(object sender, EventArgs e)
    {
      edtNome.Enabled = cbxIgnoreCase.Enabled = !cbCondicoes.SelectedItem.Equals(SearchConditionsEnum.Any.ToString());
      OnTypeChanged?.Invoke(sender, e);
    }
  }
}
