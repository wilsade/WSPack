namespace WSPack.VisualStudio.Shared.DteProject
{
  class DteConfiguration
  {
    /// <summary>
    /// Cria uma instância da classe <see cref="DteConfiguration" /></summary>
    /// <param name="name">Nome da configuração. Ex: Debug, Release</param>
    /// <param name="platform">Plataforma. Ex: Any CPU</param>
    /// <param name="defineConstants">Constantes definidas como diretiva de compilação. Ex: DEBUG, TRACE</param>
    /// <param name="targetFullPathName">Caminho completo do assembly. Ex: c:\pasta\subpasta\bin\projeto.dll</param>
    public DteConfiguration(string name, string platform, string defineConstants, string targetFullPathName)
    {
      Name = name;
      Platform = platform;
      DefineConstants = defineConstants;
      TargetFullPathName = targetFullPathName;
    }

    /// <summary>
    /// Nome da configuração. Ex: Debug, Release
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Plataforma. Ex: Any CPU
    /// </summary>
    public string Platform { get; }

    /// <summary>
    /// Constantes definidas como diretiva de compilação. Ex: DEBUG, TRACE
    /// </summary>
    public string DefineConstants { get; }

    /// <summary>
    /// Caminho completo do assembly. Ex: c:\pasta\subpasta\bin\projeto.dll
    /// </summary>
    public string TargetFullPathName { get; set; }
  }
}