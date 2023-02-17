using System.Collections.Generic;

using WSPack.Lib.Items;

namespace WSPack.Lib
{
  /// <summary>
  /// Serviços para o gerenciador de favoritos
  /// </summary>
  public interface ITFSFavoritesManagerForm
  {
    /// <summary>
    /// Url do servidor do TFS
    /// </summary>
    string DomainUri { get; }

    /// <summary>
    /// Carregar os favoritos salvos
    /// </summary>
    /// <returns>favoritos salvos</returns>
    List<TFSFavoritesParams> LoadFrom();

    /// <summary>
    /// Salvar os favoritos
    /// </summary>
    /// <param name="lst">Lista de favoritos a serem salvos</param>
    void Save(List<TFSFavoritesParams> lst);

    /// <summary>
    /// Indica se um item do TFS existe
    /// </summary>
    /// <param name="serverItem">Item do TFS</param>
    /// <returns>true se um item do TFS existe</returns>
    bool ItemExists(string serverItem);

    /// <summary>
    /// Abre um diálogo para escolher um item do TFS
    /// </summary>
    /// <param name="preSelectServerItem">Pré-selecionar um item (pode ser null)</param>
    /// <returns>Item escolhido; null, se nenhum item foi escolhido</returns>
    string ShowChooseTFSItemDialog(string preSelectServerItem);
  }
}
