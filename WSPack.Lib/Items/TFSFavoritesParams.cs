using System.Diagnostics;

using WSPack.Lib.Extensions;

namespace WSPack.Lib.Items
{
  /// <summary>
  /// Parâmetros para os favoritos do TFS
  /// </summary>
  [DebuggerDisplay("{ServerItem}")]
  public class TFSFavoritesParams
  {
    /// <summary>
    /// Url do servidor do TFS
    /// </summary>
    public string TFSUrl { get; set; }

    /// <summary>
    /// Caminho do item no servidor
    /// </summary>
    public string ServerItem { get; set; }

    /// <summary>
    /// Título do item
    /// </summary>
    public string Caption { get; set; }

    /// <summary>
    /// Índice do item (ordenação em lista)
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Determina se o <see cref="object" /> especificado é igual ao desta instância
    /// </summary>
    /// <param name="obj">Objeto a ser comparado com o desta instância</param>
    /// <returns>
    /// true se o objeto especificado é igual ao desta instância
    /// </returns>
    public override bool Equals(object obj)
    {
      if (obj is TFSFavoritesParams other)
        return other.TFSUrl.EqualsInsensitive(TFSUrl) &&
          other.ServerItem.EqualsInsensitive(ServerItem);
      return false;
    }

    /// <summary>
    /// Retorna um código hash para esta instância
    /// </summary>
    /// <returns>
    /// Um código hash para esta instância, adequado para uso em algoritmos hash e estrutura de dados como uma tabela hash
    /// </returns>
    public override int GetHashCode()
    {
      return TFSUrl.ToLower().GetHashCode() + ServerItem.ToLower().GetHashCode();
    }
  }
}
