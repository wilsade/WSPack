using System;

namespace WSPack.Lib.Items
{
  /// <summary>
  /// Definir parâmetros para a Busca de Changesets
  /// </summary>
  public class SearchChangesetsParams
  {
    /// <summary>
    /// Local da procura. Ex: $Root/Folder/...
    /// </summary>
    public string LocalProcura { get; set; }

    /// <summary>
    /// Filtrar por data?
    /// </summary>
    public bool FiltrarData { get; set; }

    /// <summary>
    /// Data inicial
    /// </summary>
    public DateTime DataInicial { get; set; }

    /// <summary>
    /// Data final
    /// </summary>
    public DateTime DataFinal { get; set; }

    /// <summary>
    /// Filtrar por Usuário?
    /// </summary>
    public bool FiltrarUsuario { get; set; }

    /// <summary>
    /// Nome do Usuário. Ex: dominio\usuario
    /// </summary>
    public string Usuario { get; set; }

    /// <summary>
    /// Filtrar por Comentário?
    /// </summary>
    public bool FiltrarComentario { get; set; }

    /// <summary>
    /// Comentário
    /// </summary>
    public string Comentario { get; set; }

    /// <summary>
    /// Filtrar por Arquivos?
    /// </summary>
    public bool FiltrarArquivos { get; set; }

    /// <summary>
    /// Arquivos a serem procurados. Separe os arquivos por vírgulda. Ex: arquivo01.cs, arquivo02.cs
    /// </summary>
    public string Arquivos { get; set; }

    /// <summary>
    /// Último changeset procurado
    /// </summary>
    public int? LastChangeset { get; set; }
  }
}
