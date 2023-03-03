using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

using WSPack.Lib.Extensions;

namespace WSPack.Lib.DocumentationObjects
{
  /// <summary>
  /// Auxiliar na manipulação dos nodos XML de um summary
  /// </summary>
  [DebuggerDisplay("{XMLContent}")]
  public class XMLSummaryDocObj
  {
    XmlDocument _xmlDocument;
    XmlNode _docNode, _summaryNode, _returnsXmlNode, _remarksXmlNode, _valueXmlNode;

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="XMLSummaryDocObj"/>
    /// </summary>
    /// <param name="xmlContent">Conteúdo XML do summamry</param>
    private XMLSummaryDocObj(string xmlContent)
    {
      LoadDocument(xmlContent);
    }
    #endregion

    void LoadDocument(string xmlContent)
    {
      _xmlDocument = new XmlDocument();
      if (!xmlContent.IsNullOrEmptyEx() && xmlContent.Contains($"<{SummaryNodesNames.Doc}"))
      {
        _xmlDocument.LoadXml(xmlContent);

        _docNode = _xmlDocument.DocumentElement;
        _summaryNode = _docNode[SummaryNodesNames.Sumamry];
        _returnsXmlNode = _docNode[SummaryNodesNames.Returns];
        _remarksXmlNode = _docNode[SummaryNodesNames.Remarks];
        _valueXmlNode = _docNode[SummaryNodesNames.Value];
      }
      else
      {
        _docNode = _xmlDocument.CreateNode(XmlNodeType.Element, SummaryNodesNames.Doc, "");
        _xmlDocument.AppendChild(_docNode);
      }
    }

    /// <summary>
    /// Nodo Summary
    /// </summary>
    XmlNode SummaryXmlNode
    {
      get
      {
        if (_summaryNode == null)
        {
          _summaryNode = _xmlDocument.CreateNode(XmlNodeType.Element, SummaryNodesNames.Sumamry, "");
          _docNode.InsertBefore(_summaryNode, _docNode.FirstChild);
        }
        return _summaryNode;
      }
    }

    /// <summary>
    /// Nodo Returns
    /// </summary>
    XmlNode ReturnsXmlNode
    {
      get
      {
        if (_returnsXmlNode == null)
        {
          _returnsXmlNode = _xmlDocument.CreateNode(XmlNodeType.Element, SummaryNodesNames.Returns, "");
          _docNode.AppendChild(_returnsXmlNode);
        }
        return _returnsXmlNode;
      }
    }

    /// <summary>
    /// Nodo value
    /// </summary>
    XmlNode ValueXmlNode
    {
      get
      {
        if (_valueXmlNode == null)
        {
          _valueXmlNode = _xmlDocument.CreateNode(XmlNodeType.Element, SummaryNodesNames.Value, "");
          _docNode.AppendChild(_valueXmlNode);
        }
        return _valueXmlNode;
      }
    }

    /// <summary>
    /// Nodo Returns
    /// </summary>
    XmlNode RemarksXmlNode
    {
      get
      {
        if (_remarksXmlNode == null)
        {
          _remarksXmlNode = _xmlDocument.CreateNode(XmlNodeType.Element, SummaryNodesNames.Remarks, "");
          _docNode.AppendChild(_remarksXmlNode);
        }
        return _remarksXmlNode;
      }
    }

    /// <summary>
    /// Recuperar a lista de nodos, conforme o nome do nodo
    /// </summary>
    /// <param name="nodeName">Nome do nodo, conforme <see cref="SummaryNodesNames"/></param>
    /// <returns>Lista de nodos</returns>
    IEnumerable<XmlNode> GetNodeList(string nodeName)
    {
      var lstNodos = _docNode.ChildNodes.OfType<XmlNode>().Where(x => x.Name == nodeName);
      return lstNodos;
    }

    /// <summary>
    /// Atribuir uma lista de nodos de parâmetros no XML
    /// </summary>
    /// <param name="lstToAdd">Lista para adicionar</param>
    /// <param name="nodeName">Nome do nodo, conforme <see cref="SummaryNodesNames"/></param>
    /// <param name="insertAfter">Referência do nodo para inserção logo após</param>
    void SetNodeList(IEnumerable<XMLParamNode> lstToAdd, string nodeName, XmlNode insertAfter)
    {
      var lstToRemove = _docNode.ChildNodes.OfType<XmlNode>().Where(x => x.Name == nodeName);
      while (lstToRemove.Any())
      {
        _docNode.RemoveChild(lstToRemove.ElementAt(0));
      }

      foreach (var item in lstToAdd)
      {
        var novoParametro = _xmlDocument.CreateNode(XmlNodeType.Element, nodeName, "");
        novoParametro.InnerText = item.ParamSummary;

        var atribulo = _xmlDocument.CreateAttribute(SummaryNodesNames.Name);
        atribulo.Value = item.ParamName;

        novoParametro.Attributes.Append(atribulo);
        _docNode.InsertAfter(novoParametro, insertAfter);
        insertAfter = novoParametro;
      }
    }

    /// <summary>
    /// Conteúdo texto do Nodo Summary
    /// </summary>
    public string SummaryNode
    {
      get { return SummaryXmlNode.InnerText; }
      set { SummaryXmlNode.InnerText = Environment.NewLine + (value.IsNullOrEmptyEx() ? "" : value.Trim()) + Environment.NewLine; }
    }

    /// <summary>
    /// Conteúdo xml do Nodo Summary
    /// </summary>
    public string SummaryNodeAsXML => SummaryXmlNode.InnerXml;

    /// <summary>
    /// Conteúdo texto do Nodo Returns
    /// </summary>
    public string ReturnsNode
    {
      get { return ReturnsXmlNode.InnerText; }
      set { ReturnsXmlNode.InnerText = value; }
    }

    /// <summary>
    /// Conteúdo XML do Nodo Returns
    /// </summary>
    public string ReturnsNodeAsXML => ReturnsXmlNode.InnerXml;

    /// <summary>
    /// Conteúdo texto do Nodo Value
    /// </summary>
    public string ValueNode
    {
      get { return ValueXmlNode.InnerText; }
      set { ValueXmlNode.InnerText = value; }
    }

    /// <summary>
    /// Conteúdo XML do Nodo Value
    /// </summary>
    public string ValueNodeAsXML => ValueXmlNode.InnerXml;

    /// <summary>
    /// Conteúdo texto do Nodo Remarks
    /// </summary>
    public string RemarksNode
    {
      get { return RemarksXmlNode.InnerText; }
      set { RemarksXmlNode.InnerText = value; }
    }

    /// <summary>
    /// Conteúdo XML do Nodo Remarks
    /// </summary>
    public string RemarksNodeAsXML => RemarksXmlNode.InnerXml;

    /// <summary>
    /// Lista de nodos de parâmetros
    /// </summary>
    public List<XMLParamNode> GetParamNodeList()
    {
      var lstNodos = GetNodeList(SummaryNodesNames.Param).Select(x => new XMLParamNode(x)).ToList();
      return lstNodos;
    }

    /// <summary>
    /// Lista de nodos de argumentos genéricos
    /// </summary>
    public List<XMLParamNode> GetGenericArgumentsNodeList()
    {
      var lstNodos = GetNodeList(SummaryNodesNames.TypeParam).Select(x => new XMLParamNode(x)).ToList();
      return lstNodos;
    }

    /// <summary>
    /// Atribuir uma lista de nodos de parâmetros no XML
    /// </summary>
    /// <param name="lstToAdd">Lista para adicionar</param>
    public void SetParamNodeList(IEnumerable<XMLParamNode> lstToAdd)
    {
      XmlNode ultimoParametro = _docNode.ChildNodes.OfType<XmlNode>().LastOrDefault(x => x.Name == SummaryNodesNames.TypeParam);
      if (ultimoParametro == null)
        ultimoParametro = SummaryXmlNode;
      SetNodeList(lstToAdd, SummaryNodesNames.Param, ultimoParametro);
    }

    /// <summary>
    /// Atribuir uma lista de nodos de argumentos genéricos no XML
    /// </summary>
    /// <param name="lstToAdd">Lista para adicionar</param>
    public void SetGenericArgumentsNodeList(IEnumerable<XMLParamNode> lstToAdd)
    {
      SetNodeList(lstToAdd, SummaryNodesNames.TypeParam, SummaryXmlNode);
    }

    /// <summary>
    /// Conteúdo XML do summary
    /// </summary>
    public string XMLContent => _xmlDocument.OuterXml;

    /// <summary>
    /// Verifica se existe nodo
    /// </summary>
    /// <param name="nodeName">Nome do nodo, conforme <see cref="SummaryNodesNames"/></param>
    /// <returns>true se node</returns>
    public bool HasNode(string nodeName) => _docNode[nodeName] != null;

    /// <summary>
    /// Excluir um nodo
    /// </summary>
    /// <param name="nodeName">Nome do nodo, conforme <see cref="SummaryNodesNames"/></param>
    public void DeleteNode(string nodeName)
    {
      var nodeToExclude = _docNode[nodeName];
      if (nodeToExclude != null)
      {
        _docNode.RemoveChild(nodeToExclude);
      }
    }

    /// <summary>
    /// Carregar o XML do summary
    /// </summary>
    /// <param name="xmlContent">Conteúdo XML do summary</param>
    /// <returns>Objeto para manipulação do summary</returns>
    public static XMLSummaryDocObj LoadOrCreate(string xmlContent)
    {
      try
      {
        var obj = new XMLSummaryDocObj(xmlContent);
        return obj;
      }
      catch (Exception ex)
      {
        throw new Exception("Não foi possível ler a estrutura XML do summary." + Environment.NewLine +
          "Favor verificar se existem caracteres XML especials no conteúdo do comentário.", ex);
      }
    }
  }
}
