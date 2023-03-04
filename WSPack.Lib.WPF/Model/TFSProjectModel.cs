using WSPack.Lib.Extensions;

namespace WSPack.Lib.WPF.Model
{
  /// <summary>
  /// Representa um item de Projeto no TFS
  /// </summary>
  public class TFSProjectModel
  {
    /// <summary>
    /// Endereço do Team Project no TFS
    /// </summary>
    public string TeamProjectUrl { get; set; }

    /// <summary>
    /// Caminho local completo do projeto no Windows
    /// </summary>
    public string ProjectFullLocalPath { get; set; }

    /// <summary>
    /// Indica se Projeto está no TFS
    /// </summary>
    public bool IsInTFS { get; set; }

    /// <summary>
    /// Retorna um código hash para esta instância,
    /// adequado para uso em algoritmos hash e estrutura de dados como uma tabela hash.
    /// </summary>
    /// <returns>Código hash para esta instância</returns>
    public override int GetHashCode()
    {
      return ProjectFullLocalPath.ToLower().GetHashCode();
    }

    /// <summary>
    /// Indica se este objeto é igual a outro
    /// </summary>
    /// <param name="obj">Obj</param>
    /// <returns>true se o objeto é igual a outro</returns>
    public override bool Equals(object obj)
    {
      if (!(obj is TFSProjectModel other))
        return false;
      return ProjectFullLocalPath.EqualsInsensitive(other.ProjectFullLocalPath);
    }
  }
}
