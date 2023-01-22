namespace WSPack.VisualStudio.Shared
{
  /// <summary>
  /// Representa um item de resposta
  /// </summary>
  /// <typeparam name="T">Tipo do item a ser retornado</typeparam>
  public class ResponseItem<T>
  {
    /// <summary>
    /// Indica sucesso
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Item de retorno em caso de sucesso
    /// </summary>
    public T Item { get; set; }

    /// <summary>
    /// Mensagem de erro
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;
  }
}