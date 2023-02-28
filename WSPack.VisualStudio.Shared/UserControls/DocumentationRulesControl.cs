using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using WSPack.Lib;
using WSPack.Lib.DocumentationObjects;
using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Forms;
using WSPack.VisualStudio.Shared.Options;

namespace WSPack.VisualStudio.Shared.UserControls
{
  /// <summary>
  /// Controle para definição de regras de documentação
  /// </summary>
  public partial class DocumentationRulesControl : UserControl
  {
    TreeNode _rootTipos, _rootMembros;

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="DocumentationRulesControl" />.
    /// </summary>
    public DocumentationRulesControl()
    {
      InitializeComponent();
      Configurar();
    }
    #endregion

    private void Configurar()
    {
      imageList1.Images.Add(TiposImagem.Type.ToString(), ResourcesLib.pngTypeDefinition_16x);
      imageList1.Images.Add(TiposImagem.Member.ToString(), ResourcesLib.pngCode_16x);
      imageList1.Images.Add(TiposImagem.Class.ToString(), ResourcesLib.pngClass_16x);
      imageList1.Images.Add(TiposImagem.Interface.ToString(), ResourcesLib.pngInterface_16xLG);
      imageList1.Images.Add(TiposImagem.Struct.ToString(), ResourcesLib.pngStructure_grey_16x);
      imageList1.Images.Add(TiposImagem.Enum.ToString(), ResourcesLib.pngEnum_16xLG);
      imageList1.Images.Add(TiposImagem.Delegate.ToString(), ResourcesLib.pngDelegate_16x);
      imageList1.Images.Add(TiposImagem.Field.ToString(), ResourcesLib.pngField_16xLG);
      imageList1.Images.Add(TiposImagem.Property.ToString(), ResourcesLib.pngProperty_16x);
      imageList1.Images.Add(TiposImagem.Event.ToString(), ResourcesLib.pngEvent_16x);
      imageList1.Images.Add(TiposImagem.Method.ToString(), ResourcesLib.pngMethod_16x);
      imageList1.Images.Add(TiposImagem.Constructor.ToString(), ResourcesLib.pngMethodInstance_16x);
      imageList1.Images.Add(TiposImagem.Parameter.ToString(), ResourcesLib.pngParameter_16x);

      _rootTipos = new TreeNode("Tipos")
      {
        Name = "nodoTipos",
        SelectedImageIndex = TiposImagem.Type.GetHashCode(),
        ImageIndex = TiposImagem.Type.GetHashCode()
      };
      _rootMembros = new TreeNode("Membros")
      {
        Name = "nodoMembros",
        SelectedImageIndex = TiposImagem.Member.GetHashCode(),
        ImageIndex = TiposImagem.Member.GetHashCode()
      };
      viewRegras.Nodes.Add(_rootTipos);
      viewRegras.Nodes.Add(_rootMembros);
    }

    #region Propriedades
    /// <summary>
    /// Gets or sets the reference to the underlying OptionsPage object.
    /// </summary>
    public PageDocumentationRules OptionsPage { get; set; }

    /// <summary>
    /// Indica se o nodo de types está expandido
    /// </summary>
    public bool IsTypeNodeExpanded => _rootTipos.IsExpanded;

    /// <summary>
    /// Indica se o nodo de membros está expandido
    /// </summary>
    public bool IsMemberNodeExpanded => _rootMembros.IsExpanded;
    #endregion

    #region Métodos
    /// <summary>
    /// Carregar a TreeView com as regras definidas
    /// </summary>
    /// <param name="ruleList">Lista de regras</param>
    public void Bind(List<RuleBaseItem> ruleList)
    {
      void AddRules(TreeNode root, IEnumerable<RuleBaseItem> lst)
      {
        foreach (var estaRegra in lst.OrderBy(x => x.Id))
        {
          BaseNode nodo = BaseNode.Create(estaRegra);
          root.Nodes.Add(nodo);
        }
      }

      viewRegras.BeginUpdate();
      try
      {
        _rootTipos.Nodes.Clear();
        _rootMembros.Nodes.Clear();

        // Recuperar as regras de cada tipo
        AddRules(_rootTipos, ruleList.OfType<TypeRuleItem>());
        AddRules(_rootMembros, ruleList.OfType<MemberRuleItem>());
      }
      finally
      {
        viewRegras.EndUpdate();
        _rootTipos.ExpandAll();
        _rootMembros.ExpandAll();
      }
    }

    /// <summary>
    /// Retornar as regras definidas
    /// </summary>
    public List<RuleBaseItem> GetRules()
    {
      var lst = new List<RuleBaseItem>();
      foreach (BaseNode esteNodo in viewRegras.Nodes.OfType<TreeNode>().SelectMany(x => x.Nodes.OfType<BaseNode>()))
      {
        esteNodo.Rule.Id = esteNodo.Index;
        lst.Add(esteNodo.Rule);
      }
      return lst;
    }

    internal void ExpandNodes(bool typeNodeExpanded, bool memberNodeExpanded)
    {
      if (typeNodeExpanded)
        _rootTipos.ExpandAll();
      else
        _rootTipos.Collapse(true);

      if (memberNodeExpanded)
        _rootMembros.ExpandAll();
      else
        _rootMembros.Collapse(true);
    }
    #endregion

    private void viewRegras_MouseDown(object sender, MouseEventArgs e)
    {
      TreeNode nodo = viewRegras.GetNodeAt(e.Location);
      if (nodo != null)
        viewRegras.SelectedNode = nodo;
    }

    private void viewRegras_AfterSelect(object sender, TreeViewEventArgs e)
    {
      btnAdd.Enabled = viewRegras.SelectedNode != null;
      btnEditar.Enabled = btnExcluir.Enabled = viewRegras.SelectedNode != null &&
        !_rootMembros.IsSelected && !_rootTipos.IsSelected;

      btnUp.Enabled = viewRegras.SelectedNode?.Parent != null &&
        viewRegras.SelectedNode?.Index > 0;
      btnDown.Enabled = viewRegras.SelectedNode?.Parent != null &&
        viewRegras.SelectedNode?.Index < viewRegras.SelectedNode?.Parent.Nodes.Count - 1;
    }

    private void btnEditar_Click(object sender, EventArgs e)
    {
      if (viewRegras.SelectedNode is BaseNode nodo)
      {
        using (var baseForm = DocumentationRuleBaseForm.Create(nodo.Rule))
        {
          baseForm.Self.Initialize(nodo.Rule);
          if (baseForm.ShowDialog() == DialogResult.OK)
          {
            var rule = baseForm.CreateRule(nodo.Rule.Id);
            nodo.Rule = rule;
            nodo.Text = baseForm.Self.NomeRegra;
          }
          viewRegras.Focus();
        }
      }
    }

    private void viewRegras_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      if (e.Node is BaseNode nodo)
      {
        btnEditar_Click(nodo, e);
      }
    }

    private void btnExcluir_Click(object sender, EventArgs e)
    {
      if (viewRegras.SelectedNode is BaseNode nodo)
      {
        if (MessageBoxUtils.ShowConfirmationYesNo(ResourcesLib.StrConfirmaExclusaoItem.FormatWith(nodo.Text)))
        {
          var outro = nodo.PrevNode;
          viewRegras.Nodes.Remove(nodo);
          if (outro != null)
          {
            viewRegras.SelectedNode = outro;
          }
        }
        viewRegras.Focus();
      }
    }

    private void btnUp_Click(object sender, EventArgs e)
    {
      if (viewRegras.SelectedNode != null)
      {
        var oldNode = viewRegras.SelectedNode;
        var oldIndex = oldNode.Index;
        var oldParent = oldNode.Parent;

        viewRegras.Nodes.Remove(oldNode);
        if (sender == btnUp)

          oldParent.Nodes.Insert(oldIndex - 1, oldNode);
        else
          oldParent.Nodes.Insert(oldIndex + 1, oldNode);
        viewRegras.SelectedNode = oldNode;
        oldNode.EnsureVisible();
        viewRegras.Focus();
      }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      int id = viewRegras.SelectedNode.Parent == null ?
        viewRegras.SelectedNode.Nodes.Count :
        viewRegras.SelectedNode.Index + 1;

      DocumentationRuleBaseForm CreateForm(out TreeNode root)
      {
        if (viewRegras.SelectedNode == _rootTipos || viewRegras.SelectedNode?.Parent == _rootTipos)
        {
          root = _rootTipos;
          return new DocumentationRuleTypeForm();
        }
        else if (viewRegras.SelectedNode == _rootMembros || viewRegras.SelectedNode?.Parent == _rootMembros)
        {
          root = _rootMembros;
          return new DocumentationRuleMemberForm();
        }
        else
          throw new NotImplementedException();
      }

      using (var baseForm = CreateForm(out TreeNode root))
      {
        if (baseForm.ShowDialog() == DialogResult.OK)
        {
          var rule = baseForm.CreateRule(id);
          var nodo = BaseNode.Create(rule);
          root.Nodes.Insert(id, nodo);
          viewRegras.SelectedNode = nodo;
        }
        viewRegras.Focus();
      }
    }
  }

  enum TiposImagem
  {
    Type = 0,
    Member = 1,
    Class = 2,
    Interface = 3,
    Struct = 4,
    Enum = 5,
    Delegate = 6,
    Field = 7,
    Property = 8,
    Event = 9,
    Method = 10,
    Constructor = 11,
    Parameter = 12
  }

  /// <summary>
  /// Nodo base
  /// </summary>
  public abstract class BaseNode : TreeNode
  {
    private RuleBaseItem _rule;

    /// <summary>
    /// Sets the index of the image.
    /// </summary>
    protected abstract void SetImageIndex();

    /// <summary>
    /// Regra deste nodo
    /// </summary>
    public RuleBaseItem Rule
    {
      get => _rule;
      set
      {
        _rule = value;
        ToolTipText = _rule.ToString();
        SetImageIndex();
      }
    }

    /// <summary>
    /// Criar um nodo conforme uma regra base
    /// </summary>
    /// <param name="ruleBase">Rule base</param>
    public static BaseNode Create(RuleBaseItem ruleBase)
    {
      if (ruleBase is TypeRuleItem typeRule)
        return new TypeNode(typeRule);
      else if (ruleBase is MemberRuleItem memberRule)
        return new MemberNode(memberRule);
      else
        throw new NotImplementedException();
    }
  }

  /// <summary>
  /// Nodo para tipos
  /// </summary>
  public class TypeNode : BaseNode
  {
    /// <summary>
    /// Cria uma instância da classe <see cref="TypeNode"/>
    /// </summary>
    public TypeNode(TypeRuleItem ruleItem)
      : base()
    {
      Text = ruleItem.ItemName;
      Rule = ruleItem;
    }

    /// <summary>
    /// Sets the index of the image.
    /// </summary>
    protected override void SetImageIndex()
    {
      switch (((TypeRuleItem)Rule).TypeType)
      {
        case TypeTypesEnum.All:
          ImageIndex = SelectedImageIndex = TiposImagem.Type.GetHashCode();
          break;
        case TypeTypesEnum.Classes:
          ImageIndex = SelectedImageIndex = TiposImagem.Class.GetHashCode();
          break;
        case TypeTypesEnum.Interfaces:
          ImageIndex = SelectedImageIndex = TiposImagem.Interface.GetHashCode();
          break;
        case TypeTypesEnum.Structs:
          ImageIndex = SelectedImageIndex = TiposImagem.Struct.GetHashCode();
          break;
        case TypeTypesEnum.Enums:
          ImageIndex = SelectedImageIndex = TiposImagem.Enum.GetHashCode();
          break;
        case TypeTypesEnum.Delegates:
          ImageIndex = SelectedImageIndex = TiposImagem.Delegate.GetHashCode();
          break;
        default:
          break;
      }
    }
  }

  /// <summary>
  /// Nodo para membros
  /// </summary>
  public class MemberNode : BaseNode
  {
    /// <summary>
    /// Cria uma instância da classe <see cref="MemberNode"/>
    /// </summary>
    public MemberNode(MemberRuleItem memberRule)
    {
      Text = memberRule.ItemName;
      Rule = memberRule;
    }

    /// <summary>
    /// Sets the index of the image.
    /// </summary>
    protected override void SetImageIndex()
    {
      switch (((MemberRuleItem)Rule).MemberType)
      {
        case MemberTypesEnum.All:
          ImageIndex = SelectedImageIndex = TiposImagem.Member.GetHashCode();
          break;
        case MemberTypesEnum.Property:
          ImageIndex = SelectedImageIndex = TiposImagem.Property.GetHashCode();
          break;
        case MemberTypesEnum.Parameters:
          ImageIndex = SelectedImageIndex = TiposImagem.Parameter.GetHashCode();
          break;
        case MemberTypesEnum.Method:
          ImageIndex = SelectedImageIndex = TiposImagem.Method.GetHashCode();
          break;
        case MemberTypesEnum.Constructor:
          ImageIndex = SelectedImageIndex = TiposImagem.Constructor.GetHashCode();
          break;
        case MemberTypesEnum.Field:
          ImageIndex = SelectedImageIndex = TiposImagem.Field.GetHashCode();
          break;
        case MemberTypesEnum.Event:
          ImageIndex = SelectedImageIndex = TiposImagem.Event.GetHashCode();
          break;
        default:
          break;
      }
    }
  }
}

