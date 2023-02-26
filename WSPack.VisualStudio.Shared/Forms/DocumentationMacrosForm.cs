using System;
using System.Windows.Forms;

using WSPack.VisualStudio.Shared.DocumentationObjects.Macros;

namespace WSPack.VisualStudio.Shared.Forms
{
  /// <summary>
  /// Tela para definição de macros em um texto
  /// </summary>
  public partial class DocumentationMacrosForm : Form
  {
    const int Image_Index_Group = 0;
    internal const int _imageIndexMacro = 1;

    #region Construtor
    /// <summary>
    /// Cria uma instância desta classe
    /// </summary>
    public DocumentationMacrosForm()
    {
      InitializeComponent();
      LoadMacros(MacroEnvironmentItems.CreateGroup());
      LoadMacros(MacroElementItems.CreateGroup());
    }
    #endregion

    /// <summary>
    /// Texto da tela
    /// </summary>
    public string EditText
    {
      get { return edtTexto.Text; }
      set { edtTexto.Text = value; }
    }

    /// <summary>
    /// Carregar Macros
    /// </summary>
    /// <param name="macroGroup">MacroGroup</param>
    public void LoadMacros(MacroGroupItems macroGroup)
    {
      treeMacros.BeginUpdate();
      try
      {
        var nodoPadrao = treeMacros.Nodes.Add(macroGroup.GroupName, macroGroup.GroupName, Image_Index_Group);
        nodoPadrao.ToolTipText = macroGroup.ToolTip;
        CreateNodes(nodoPadrao, macroGroup);
      }
      finally
      {
        treeMacros.EndUpdate();
      }
    }

    void CreateNodes(TreeNode root, MacroGroupItems grupo)
    {
      // Criar os nodos de macro deste grupo
      foreach (var esteMacro in grupo.MacroList)
      {
        root.Nodes.Add(new MacroNode(esteMacro));
      }

      foreach (var esteSubGrupo in grupo.SubGroupsList)
      {
        var novoGrupo = root.Nodes.Add(esteSubGrupo.GroupName);
        novoGrupo.ToolTipText = esteSubGrupo.ToolTip;
        CreateNodes(novoGrupo, esteSubGrupo);
      }
    }

    private void DocumentationMacrosForm_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
        Close();
    }

    private void DocumentationMacrosForm_Shown(object sender, EventArgs e)
    {
      edtTexto.SelectionStart = edtTexto.Text.Length;
      //foreach (TreeNode esteRaiz in treeMacros.Nodes)
      //{
      //  esteRaiz.Expand();
      //}
    }

    private void cbxQuebrarLinhas_CheckedChanged(object sender, EventArgs e)
    {
      edtTexto.WordWrap = cbxQuebrarLinhas.Checked;
    }

    private void treeMacros_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      if (treeMacros.SelectedNode is MacroNode macroNode)
      {
        edtTexto.SelectedText = macroNode.MacroItem.MacroText;
        edtTexto.Focus();
        edtTexto.SelectionLength = 0;
      }
    }
  }

  /// <summary>
  /// Nodo de macro
  /// </summary>
  class MacroNode : TreeNode
  {
    /// <summary>
    /// Cria uma instância desta classe
    /// </summary>
    /// <param name="macro">Macro</param>
    public MacroNode(MacroBaseItems macro)
          : base()
    {
      Text = macro.Code;
      ToolTipText = macro.Description;
      MacroItem = macro;
      ImageIndex = SelectedImageIndex = DocumentationMacrosForm._imageIndexMacro;
    }

    /// <summary>
    /// Macro
    /// </summary>
    public MacroBaseItems MacroItem { get; set; }
  }
}
