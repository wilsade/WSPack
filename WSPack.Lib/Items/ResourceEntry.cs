using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Resources;

namespace WSPack.Lib.Items
{
  /// <summary>
  /// Representa um item do arquivo de ResourceString
  /// </summary>
  [DebuggerDisplay("{Name, nq}")]
  public class ResourceEntry
  {
    #region Construtores
    /// <summary>
    /// Cria uma instância da classe: <see cref="ResourceEntry"/>
    /// </summary>
    public ResourceEntry()
    {

    }

    /// <summary>
    /// Cria uma instância da classe: <see cref="ResourceEntry"/>
    /// </summary>
    /// <param name="node">Nodo do arquivo de Resource</param>
    public ResourceEntry(ResXDataNode node)
    {
      Name = node.Name;
      Value = Convert.ToString(node.GetValue((ITypeResolutionService)null));
      Comment = node.Comment;
    }

    /// <summary>
    /// Cria uma instância da classe: <see cref="ResourceEntry"/>
    /// </summary>
    /// <param name="name">Nome</param>
    /// <param name="value">Valor</param>
    /// <param name="comment">Comentário</param>
    public ResourceEntry(string name, string value, string comment)
    {
      Name = name;
      Value = value;
      Comment = comment;
    }
    #endregion

    /// <summary>
    /// Nome
    /// </summary>
    [DisplayName("Nome")]
    public string Name { get; set; }

    /// <summary>
    /// Valor
    /// </summary>
    [DisplayName("Valor")]
    public string Value { get; set; }

    /// <summary>
    /// Comentário
    /// </summary>
    [DisplayName("Comentário")]
    public string Comment { get; set; }
  }
}
