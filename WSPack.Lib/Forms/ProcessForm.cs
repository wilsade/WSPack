using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSPack.Lib.Forms
{
  /// <summary>
  /// Form para processamento
  /// </summary>
  public sealed partial class ProcessForm : Form, IProcessForm
  {
    /// <summary>
    /// Form para processamento
    /// </summary>
    public ProcessForm()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Expor membros do form de processamento
    /// </summary>
    public IProcessForm Self
    {
      get { return this; }
    }

    private void ProcessForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      e.Cancel = !Self.PodeSair;
    }

    private void ProcessForm_Load(object sender, EventArgs e)
    {
      Self.PodeSair = false;
    }

    void IProcessForm.Close()
    {
      Self.PodeSair = true;
      if (InvokeRequired)
        Invoke(new MethodInvoker(() =>
          {
            Close();
          }));
      else
        Close();
    }

    string IProcessForm.Titulo
    {
      get
      {
        return Text;
      }
      set
      {
        Text = value;
        Update();
      }
    }

    string IProcessForm.Descricao
    {
      get
      {
        return lbDescricao.Text;
      }
      set
      {
        lbDescricao.Text = value;
        Update();
      }
    }

    bool IProcessForm.PodeSair { get; set; }

    private void ProcessForm_Activated(object sender, EventArgs e)
    {
      Update();
    }
  }
}
