using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper
{
  /// <summary>
  /// Definir características de um método
  /// </summary>
  public interface IMethodElementEx : IBaseElementEx
  {
    /// <summary>
    /// Indica se o método é o Get/Set de uma propriedade
    /// </summary>
    bool IsGetOrSet { get; }

    /// <summary>
    /// Indica se a função é 'void'
    /// </summary>
    bool IsVoid { get; }

    /// <summary>
    /// Indica se é o método construtor
    /// </summary>
    bool IsConstructor { get; }
  }
}
