using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WSPack.Lib;
using WSPack.Lib.Properties;

namespace WSPack.Lib.Forms
{
  /// <summary>
  /// Exibir um form para testar um MessageBox
  /// </summary>
  /// <seealso cref="Form" />
  public partial class MessageBoxTesterForm : Form
  {
    private readonly Action<string> _copyToClipboardHandler;

    /// <summary>
    /// Cria uma instância da classe <see cref="MessageBoxTesterForm"/>
    /// </summary>
    public MessageBoxTesterForm(Action<string> copyToClipboardHandler)
    {
      InitializeComponent();
      _copyToClipboardHandler = copyToClipboardHandler;
    }

    MessageBoxButtons Botao
    {
      get { return (MessageBoxButtons)cbBotoes.SelectedItem; }
    }

    MessageBoxIcon Icone
    {
      get { return (MessageBoxIcon)Enum.Parse(typeof(MessageBoxIcon), cbIcone.Text); }
    }

    MessageBoxDefaultButton DefaultButton
    {
      get { return (MessageBoxDefaultButton)cbBotaoDefault.SelectedItem; }
    }

    /// <summary>
    /// Titulo
    /// </summary>
    public string Titulo
    {
      get { return edtTitulo.Text; }
      set { edtTitulo.Text = value; }
    }

    /// <summary>
    /// Texto
    /// </summary>
    public string Texto
    {
      get { return memoTexto.Text; }
      set { memoTexto.Text = value; }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(edtTitulo.Text))
        edtTitulo.Text = ResourcesLib.StrInformacao;
      cbBotoes.DataSource = Enum.GetValues(typeof(MessageBoxButtons));
      cbIcone.DataSource = Enum.GetNames(typeof(MessageBoxIcon));
      cbBotaoDefault.DataSource = Enum.GetValues(typeof(MessageBoxDefaultButton));

      cbIcone.SelectedIndex = cbIcone.Items.IndexOf(MessageBoxIcon.Information.ToString());
    }

    private void btnTestarMessage_Click(object sender, EventArgs e)
    {
      MessageBox.Show(memoTexto.Text, edtTitulo.Text, Botao, Icone, DefaultButton);
    }

    private void MessageBoxTesterForm_Shown(object sender, EventArgs e)
    {
      memoTexto.SelectAll();
      memoTexto.Focus();
    }

    private void MessageBoxTesterForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
        Close();
    }

    private void btnCodigo_Click(object sender, EventArgs e)
    {
      try
      {
        StringBuilder str = new StringBuilder();
        str.Append("System.Windows.Forms.MessageBox.Show(").AppendLine("")
          .Append("  ").Append(memoTexto.Text.Contains("\n") ? "@" : "").Append("\"").Append(memoTexto.Text).Append("\",").AppendLine("")
          .Append("  \"").Append(edtTitulo.Text).Append("\",").AppendLine("")
          .Append("  System.Windows.Forms.MessageBoxButtons.").Append(Botao).Append(",").AppendLine("")
          .Append("  System.Windows.Forms.MessageBoxIcon.").Append(Icone).Append(",").AppendLine("")
          .Append("  System.Windows.Forms.MessageBoxDefaultButton.").Append(DefaultButton).Append(");");
        string aux = str.ToString().Replace("\r\n", "\n").Replace("\n", "\r\n");
        if (!cbxIncluirNamespace.Checked)
          aux = aux.Replace("System.Windows.Forms.", string.Empty);
        _copyToClipboardHandler(aux);

        MessageBoxUtils.ShowInformation(ResourcesLib.StrCopiadoAreaTransferencia + Environment.NewLine +
          ResourcesLib.StrConsidereCriacaoResourceString, ResourcesLib.StrInformacao);
      }
      catch (Exception ex)
      {
        MessageBoxUtils.ShowError(ex.Message);
      }
    }

    private void edtTitulo_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
        btnTestarMessage_Click(sender, e);
    }
  }
}
