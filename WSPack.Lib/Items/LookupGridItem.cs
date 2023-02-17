using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.Lib.Items
{
  /// <summary>
  /// Representa um item de grid
  /// </summary>
  public class LookupGridItem
  {
    /// <summary>
    /// Inicialização da classe: <see cref="LookupGridItem"/>.
    /// </summary>
    /// <param name="nome">Nome</param>
    /// <param name="caminho">Caminho</param>
    public LookupGridItem(string nome, string caminho)
    {
      Nome = nome;
      Caminho = caminho;
      if (string.IsNullOrWhiteSpace(Nome))
        Nome = caminho;
    }

    /// <summary>
    /// Nome do item
    /// </summary>
    public string Nome { get; set; }

    /// <summary>
    /// Caminho completo do item
    /// </summary>
    public string Caminho { get; set; }

    /// <summary>
    /// OwnerData
    /// </summary>
    public object OwnerData;
  }
}
