using System.IO;

using EnvDTE;

using Microsoft.TeamFoundation.Common;

using WSPack.Lib.Extensions;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Representa um item de arquivo de Resources
  /// </summary>
  public class ResourcesFileItem
  {
    #region Construtores
    /// <summary>
    /// Cria uma instância da classe: <see cref="ResourcesFileItem" /></summary>
    /// <param name="parent">Projeto que possui o arquivo</param>
    public ResourcesFileItem(ProjectItem parent)
    {
      Parent = parent;
    }

    /// <summary>
    /// Cria uma instância da classe: <see cref="ResourcesFileItem" /></summary>
    /// <param name="parent">Projeto que possui o arquivo</param>
    /// <param name="resxFileName">Caminho do arquivo Resx</param>
    public ResourcesFileItem(ProjectItem parent, string resxFileName)
      : this(parent)
    {
      ResxFileName = resxFileName;
    }
    #endregion

    /// <summary>
    /// Projeto que possui o arquivo
    /// </summary>
    public ProjectItem Parent { get; }

    /// <summary>
    /// Caminho do arquivo Resx
    /// </summary>
    public string ResxFileName { get; set; }

    /// <summary>
    /// Apenas nome do arquivo de resource Resx
    /// </summary>
    public string ResxNameOnly => ResxFileName.IsNullOrWhiteSpaceEx() ?
      string.Empty : Path.GetFileName(ResxFileName);

    /// <summary>
    /// Caminho do arquivo .designer referente ao Resx
    /// </summary>
    public string DesignerFileName => ResxFileName.IsNullOrWhiteSpaceEx() ?
      string.Empty : Path.ChangeExtension(ResxFileName, ".designer.cs");

    /// <summary>
    /// Indica se o arquivo Resx é o default esperado:
    /// Dentro de "Properties" com o nome "Resources.resx"
    /// </summary>
    public bool IsExpectedDefaultResourceFileName
    {
      get
      {
        string fullName = Parent?.ContainingProject?.FullName;
        if (!fullName.IsNullOrWhiteSpaceEx())
        {
          string resourceFileName = Path.Combine(
            Path.GetDirectoryName(fullName), "Properties", "Resources.resx");
          return resourceFileName.EqualsInsensitive(ResxFileName);
        }
        return false;
      }
    }

    /// <summary>
    /// Composição de um nome de classe conforme o nome e camiho do arquivo Resx
    /// </summary>
    public string ClassName
    {
      get
      {
        string aux = GetShortName().Replace("\\", ".");
        return Path.GetFileNameWithoutExtension(aux);
      }
    }

    #region Métodos
    /// <summary>
    /// Indica se o resource possui o arquivo: .designer.cs
    /// </summary>
    public bool HasDesignerFileName => File.Exists(DesignerFileName);

    /// <summary>
    /// Retorna uma string representando esta instância
    /// </summary>
    /// <returns>string representando esta instância</returns>
    public override string ToString()
    {
      string aux = GetShortName();
      return "$(ProjectDir)\\" + aux;
    }

    /// <summary>
    /// Indica se este objeto é igual a outro
    /// </summary>
    /// <param name="obj">Obj</param>
    /// <returns>true se o objeto é igual a outro</returns>
    public override bool Equals(object obj)
    {
      if (obj is ResourcesFileItem other)
        return other.ResxFileName.EqualsInsensitive(ResxFileName);
      return false;
    }

    /// <summary>
    /// Retorna um código hash para esta instância,
    /// adequado para uso em algoritmos hash e estrutura de dados como uma tabela hash.
    /// </summary>
    /// <returns>Código hash para esta instância</returns>
    public override int GetHashCode()
    {
      return ResxFileName.GetHashCode();
    }
    #endregion

    string GetShortName()
    {
      string basePath = Path.GetDirectoryName(Parent?.ContainingProject?.FullName);
      if (!basePath.IsNullOrEmptyEx())
      {
        string aux = ResxFileName.Replace(basePath, string.Empty);
        if (aux.StartsWith("\\"))
          return aux.Substring(1);
        return aux;
      }
      return ResxNameOnly;
    }
  }
}