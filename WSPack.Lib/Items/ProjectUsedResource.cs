namespace WSPack.Lib.Items
{
  /// <summary>
  /// Uso de Resource em projetos
  /// </summary>
  public class ProjectUsedResource
  {
    /// <summary>
    /// Cria uma instância da classe: <see cref="ProjectUsedResource"/>
    /// </summary>
    /// <param name="projectFullName">Caminho do projeto</param>
    /// <param name="lastResxFileUsed">Caminho do resource utilizado</param>
    public ProjectUsedResource(string projectFullName, string lastResxFileUsed)
    {
      ProjectFullName = projectFullName;
      LastResxFileUsed = lastResxFileUsed;
    }

    /// <summary>
    /// Caminho do projeto
    /// </summary>
    public string ProjectFullName { get; set; }

    /// <summary>
    /// Caminho do resource utilizado
    /// </summary>
    public string LastResxFileUsed { get; set; }
  }
}
