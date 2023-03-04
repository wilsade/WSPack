namespace WSPack.Lib.WPF.Model
{
  /// <summary>
  /// Representa um item básico na StartPage
  /// </summary>
  public abstract class BaseModel
  {
    /// <summary>
    /// Identificador do item
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Título do item
    /// </summary>
    public string Caption { get; set; }
  }
}
