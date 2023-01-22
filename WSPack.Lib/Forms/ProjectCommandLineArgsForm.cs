using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSPack.Lib.Forms
{
  /// <summary>
  /// Tela para definir parâmetros de linha de comando de um projeto
  /// </summary>
  public partial class ProjectCommandLineArgsForm : Form
  {
    /// <summary>
    /// Inicialização da classe: <see cref="ProjectCommandLineArgsForm"/>.
    /// </summary>
    public ProjectCommandLineArgsForm()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Título do form
    /// </summary>
    public string ProjectName
    {
      get { return statusProjeto.Text; }
      set
      {
        statusProjeto.Text = value;
      }
    }

    /// <summary>
    /// Command line args definido
    /// </summary>
    public string CommandLineArgs
    {
      get { return memoArgs.Text; }
      set { memoArgs.Text = value; }
    }

    /// <summary>
    /// Indica se vai quebrar linhas
    /// </summary>
    public bool QuebrarLinhas
    {
      get { return cbxQuebraLinhas.Checked; }
      set { cbxQuebraLinhas.Checked = value; }
    }

    private void cbxQuebraLinhas_CheckedChanged(object sender, EventArgs e)
    {
      memoArgs.WordWrap = cbxQuebraLinhas.Checked;
    }

    private void ProjectCommandLineArgsForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
        Close();
    }
  }
}
