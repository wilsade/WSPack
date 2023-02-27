using System;

namespace WSPack.Lib.DocumentationObjects
{
  /// <summary>
  /// Definir tipos de membros
  /// </summary>
  [Flags]
  public enum MemberTypesEnum
  {
    /// <summary>
    /// Todos os membros
    /// </summary>
    All = 1,

    /// <summary>
    /// Propriedades
    /// </summary>
    Property = 2,

    /// <summary>
    /// Parâmetros
    /// </summary>
    Parameters = 4,

    /// <summary>
    /// Métodos
    /// </summary>
    Method = 8,

    /// <summary>
    /// Construtores
    /// </summary>
    Constructor = 16,

    /// <summary>
    /// Campos
    /// </summary>
    Field = 32,

    /// <summary>
    /// Eventos
    /// </summary>
    Event = 64,
  }

}
