using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.TeamFoundation.VersionControl.Client;

using WSPack.Lib.Forms;
using WSPack.Lib.Properties;

namespace WSPack.VisualStudio.Shared.Forms
{
  /// <summary>
  /// Tela para escolher um Workspace e o item local
  /// </summary>
  public partial class ChooseWorkspaceLocalItemForm : BaseForm
  {
    /// <summary>
    /// Inicialização da classe: <see cref="ChooseWorkspaceLocalItemForm"/>.
    /// </summary>
    public ChooseWorkspaceLocalItemForm()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Preencher a lista com os itens
    /// </summary>
    /// <param name="lstItens">Lista de itens</param>
    public void BindList(IEnumerable<(Workspace ws, string LocalItem)> lstItens)
    {
      viewItens.BeginUpdate();
      try
      {
        viewItens.Items.Clear();
        foreach (var esteItem in lstItens)
        {
          var item = new ListViewItem(new string[] { esteItem.ws.Name, esteItem.LocalItem })
          {
            Tag = esteItem.ws
          };
          viewItens.Items.Add(item);
        }
      }
      finally
      {
        viewItens.EndUpdate();
      }
    }

    /// <summary>
    /// Retornar o Item selecionado
    /// </summary>
    public (Workspace Workspace, string LocalItem) ItemSelecionado
    {
      get
      {
        if (viewItens.SelectedItems.Count > 0 && viewItens.SelectedItems[0] is ListViewItem item)
        {
          return ((Workspace)item.Tag, item.SubItems[1].Text);
        }
        return (null, null);
      }
    }

    private void ChooseWorkspaceLocalItemForm_Load(object sender, EventArgs e)
    {
      Icon = ResourcesLib.icoChoose_32;
    }

    private void ChooseWorkspaceLocalItemForm_Shown(object sender, EventArgs e)
    {
      BotaoOKHabilitado = false;
      if (viewItens.Items.Count > 0)
      {
        if (viewItens.CanFocus)
          viewItens.Focus();
        viewItens.Items[0].Selected = true;
      }

    }

    private void viewItens_SelectedIndexChanged(object sender, EventArgs e)
    {
      BotaoOKHabilitado = viewItens.SelectedItems.Count == 1;
    }

    private void viewItens_DoubleClick(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
      Close();
    }
  }
}
