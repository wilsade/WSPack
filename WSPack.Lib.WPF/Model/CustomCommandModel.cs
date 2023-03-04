namespace WSPack.Lib.WPF.Model
{
  /// <summary>
  /// Definir comandos personalizados
  /// </summary>
  public class CustomCommandModel : BaseModel
  {
    #region Construtores
    /// <summary>
    /// Cria uma instância da classe <see cref="CustomCommandModel"/>
    /// </summary>
    public CustomCommandModel()
    {

    }

    /// <summary>
    /// Cria uma instância da classe <see cref="CustomCommandModel" />
    /// </summary>
    /// <param name="id">Identificador do comando</param>
    /// <param name="caption">Título do comando</param>
    /// <param name="commandDef">Definição do comando (é o que será executado)</param>
    /// <param name="args">Argumentos do parãmetro</param>
    public CustomCommandModel(int id, string caption, string commandDef, string args = "")
    {
      Id = id;
      Caption = caption;
      CommandDef = commandDef;
      CommandArgs = args;
    }
    #endregion

    /// <summary>
    /// Definição do comando (é o que será executado)
    /// </summary>
    public string CommandDef { get; set; }

    /// <summary>
    /// Argumentos do parãmetro
    /// </summary>
    public string CommandArgs { get; set; }
  }
}
