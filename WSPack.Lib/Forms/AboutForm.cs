using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Diagnostics;
using WSPack.Lib;
using WSPack.Lib.Properties;
using WSPack.Lib.Extensions;

namespace WSPack2019.Forms
{
  /// <summary>
  /// Form: Sobre
  /// </summary>
  public partial class AboutForm : Form
  {
    /// <summary>
    /// Cria uma instância da classe <see cref="AboutForm"/>
    /// </summary>
    public AboutForm(Assembly sender)
    {
      InitializeComponent();
      Text = sender.GetTitle();
      linkNomeProduto.Text = sender.GetProduct();
      lbNumeroVersao.Text = sender.GetVersion();
      lbDireitos.Text = sender.GetCopyright();
      lbEmpresa.Text = sender.GetCompany();
      textBoxDescription.Text = sender.GetDescription() + DescricoesComplementares;
    }

    string DescricoesComplementares
    {
      get
      {
        return $@"Desenvolvido por: William Sade de Paiva
Histórico de versões
----------------------------------------------

{Constantes.GitHubWSPackReleaseNotes}";
      }
    }

    private void AboutForm_Load(object sender, EventArgs e)
    {
      logoPictureBox.Image = ResourcesLib.icoWSPackLogo.ToBitmap();
      linkNomeProduto.LinkClicked += LabelProductName_LinkClicked;
      textBoxDescription.LinkClicked += TextBoxDescription_LinkClicked;
    }

    private void TextBoxDescription_LinkClicked(object sender, LinkClickedEventArgs e)
    {
      _ = Process.Start(e.LinkText);
    }

    private void LabelProductName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      _ = Process.Start(Constantes.GitHubWSPackWiki);
    }

    private void AboutForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
        Close();
    }
  }
}
