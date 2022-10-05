using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WSPack.Lib.Properties;

namespace WSPack.Lib
{
  /// <summary>
  /// Utilitários para exibição de mensagens
  /// </summary>
  public static class MessageBoxUtils
  {
    /// <summary>
    /// Mostrar mensagem de erro
    /// </summary>
    /// <param name="msg">Mensagem a ser exibida</param>
    /// <param name="parameters">Parâmetros da string</param>
    public static void ShowError(string msg, params object[] parameters)
    {
      MessageBox.Show(string.Format(msg, parameters), ResourcesLib.StrErro, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    /// Mostrar mensagem de aviso
    /// </summary>
    /// <param name="msg">Mensagem a ser exibida</param>
    /// <param name="parameters">Parâmetros da string</param>
    public static void ShowWarning(string msg, params object[] parameters)
    {
      MessageBox.Show(string.Format(msg, parameters), ResourcesLib.StrAviso, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    /// <summary>
    /// Mostrar mensagem de aviso
    /// </summary>
    /// <param name="msg">Mensagem a ser exibida</param>
    /// <param name="defaultButton">Botão default selecionado</param>
    /// <returns>true se a resposta foi 'Yes'</returns>
    public static bool ShowWarningYesNo(string msg, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
    {
      return MessageBox.Show(msg, ResourcesLib.StrAviso, MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning, defaultButton) == DialogResult.Yes;
    }

    /// <summary>
    /// Mostrar mensagem de aviso
    /// </summary>
    /// <param name="msg">Mensagem a ser exibida</param>
    /// <param name="parameters">Parâmetros da string</param>
    /// <returns>true se a resposta foi 'Yes'</returns>
    public static bool ShowWarningYesNo(string msg, params object[] parameters)
    {
      return MessageBox.Show(
        string.Format(msg, parameters), ResourcesLib.StrAviso,
        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
    }

    /// <summary>
    /// Exibir uma mensagem de confirmação Sim/Não com ícone Question
    /// </summary>
    /// <param name="msg">Mensagem a ser exibida</param>
    /// <param name="defaultButton">Botão default</param>
    /// <returns>true se o usuário escolheu: Sim</returns>
    public static bool ShowConfirmationYesNo(string msg, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
    {
      return MessageBox.Show(msg, ResourcesLib.StrConfirmacao, MessageBoxButtons.YesNo, MessageBoxIcon.Question, defaultButton) == DialogResult.Yes;
    }

    /// <summary>
    /// Mostrar mensagem de informação
    /// </summary>
    /// <param name="msg">Mensagem a ser exibida</param>
    /// <param name="parameters">Parâmetros da string</param>
    public static void ShowInformation(string msg, params object[] parameters)
    {
      MessageBox.Show(string.Format(msg, parameters), ResourcesLib.StrInformacao, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    /// <summary>
    /// Exibir um diálogo para digitação de alguma informação
    /// </summary>
    /// <param name="label">Rótulo da caixa de texto</param>
    /// <param name="title">Título do diálogo</param>
    /// <param name="response">Texto digitado pelo usuário</param>
    /// <param name="defaultMessage">Texto padrão que irá aparecer na caixa de texto</param>
    /// <returns>true se o usuário digitou alguma mensagem</returns>
    public static bool InputBox(string label, string title, out string response, string defaultMessage = "")
    {
      response = Microsoft.VisualBasic.Interaction.InputBox(
        label, title, defaultMessage);
      return !string.IsNullOrEmpty(response);
    }
  }
}
