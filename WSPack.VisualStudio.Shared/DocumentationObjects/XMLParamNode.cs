using System.Xml;

namespace WSPack.VisualStudio.Shared.DocumentationObjects
{
  /// <summary>
  /// Representa um nodo de parâmetro
  /// </summary>
  public class XMLParamNode
  {
    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="XMLParamNode"/>
    /// </summary>
    /// <param name="xmlParamNode">Nodo XML de parâmetro</param>
    public XMLParamNode(XmlNode xmlParamNode)
    {
      ParamName = xmlParamNode.Attributes[SummaryNodesNames.Name].Value;
      ParamSummary = xmlParamNode.InnerText;
      ParamSummaryAsXML = xmlParamNode.InnerXml;
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="XMLParamNode"/>
    /// </summary>
    /// <param name="paramName">Nome do parâmetro</param>
    /// <param name="paramSummary">Summary do parâmetro</param>
    public XMLParamNode(string paramName, string paramSummary)
      : this(paramName, paramSummary, paramSummary)
    {

    }

    /// <summary>
    /// Cria uma instância da classe <see cref="XMLParamNode"/>
    /// </summary>
    /// <param name="paramName">Nome do parâmetro</param>
    /// <param name="paramSummary">Summary do parâmetro</param>
    /// <param name="paramSummaryXML">Summary XML do parâmetro</param>
    public XMLParamNode(string paramName, string paramSummary, string paramSummaryXML)
    {
      ParamName = paramName;
      ParamSummary = paramSummary ?? "";
      ParamSummaryAsXML = paramSummaryXML ?? "";
    }
    #endregion

    /// <summary>
    /// Nome do parâmetro
    /// </summary>
    public string ParamName { get; }

    /// <summary>
    /// Summary texto do parâmetro
    /// </summary>
    public string ParamSummary { get; set; }

    /// <summary>
    /// Summary XML do parâmetro
    /// </summary>
    public string ParamSummaryAsXML { get; set; }

    /// <summary>
    /// Nome da regra que documentou o parâmetro
    /// </summary>
    public string QuemDocumentou { get; set; }
  }
}
