namespace WSPack.Lib.WPF.Model
{
  /// <summary>
  /// Representa um item de projeto da StartPage
  /// </summary>
  public class ProjectModel : BaseModel
  {
    #region Construtores
    /// <summary>
    /// Cria uma instância da classe <see cref="ProjectModel"/>
    /// </summary>
    public ProjectModel()
    {

    }

    /// <summary>
    /// Cria uma instância da classe <see cref="ProjectModel"/>
    /// </summary>
    /// <param name="id">Identificador</param>
    /// <param name="caption">Título</param>
    /// <param name="fullPath">Caminho completo</param>
    public ProjectModel(int id, string caption, string fullPath)
    {
      Id = id;
      Caption = caption;
      FullPath = fullPath;
    }
    #endregion

    /// <summary>
    /// Caminho completo do item
    /// </summary>
    public string FullPath { get; set; }

    /// <summary>
    /// Nome do diretório onde o projeto se encontra
    /// </summary>
    public string DirectoryName => !string.IsNullOrEmpty(FullPath) ? System.IO.Path.GetDirectoryName(FullPath) : string.Empty;
  }
}
