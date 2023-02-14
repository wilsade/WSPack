using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WSPack.Lib.Extensions;

namespace WSPack.Lib.Items
{
  /// <summary>
  /// Definir parâmetros de geração de ResourceString
  /// </summary>
  public class GenerateResourceParams
  {
    /// <summary>
    /// Inicialização da classe: <see cref="GenerateResourceParams"/>.
    /// </summary>
    public GenerateResourceParams()
    {

    }

    /// <summary>
    /// Cria uma instância da classe: <see cref="GenerateResourceParams"/>
    /// </summary>
    /// <param name="solutionPath">Caminho da solution</param>
    /// <param name="prefixo">Prefixo utilizado</param>
    /// <param name="recentsProjects">Projetos recentes</param>
    public GenerateResourceParams(string solutionPath, string prefixo, List<ProjectUsedResource> recentsProjects = null)
    {
      SolutionPath = solutionPath ?? "";
      Prefixo = prefixo ?? "";
      RecentsProjects = recentsProjects ?? new List<ProjectUsedResource>();
    }

    /// <summary>
    /// Caminho da solution
    /// </summary>
    public string SolutionPath { get; set; }

    /// <summary>
    /// Prefixo utilizado
    /// </summary>
    public string Prefixo { get; set; }

    /// <summary>
    /// Projetos recentes
    /// </summary>
    public List<ProjectUsedResource> RecentsProjects { get; set; }

    /// <summary>
    /// Indica se este objeto é igual a outro
    /// </summary>
    /// <param name="obj">Obj</param>
    /// <returns>true se o objeto é igual a outro</returns>
    public override bool Equals(object obj)
    {
      if (obj is GenerateResourceParams other)
        return other.SolutionPath.EqualsInsensitive(SolutionPath);
      return false;
    }

    /// <summary>
    /// Retorna um código hash para esta instância,
    /// adequado para uso em algoritmos hash e estrutura de dados como uma tabela hash.
    /// </summary>
    /// <returns>Código hash para esta instância</returns>
    public override int GetHashCode()
    {
      return SolutionPath.GetHashCode();
    }
  }
}
