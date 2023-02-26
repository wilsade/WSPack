using System;

namespace WSPack.VisualStudio.Shared.DocumentationObjects
{
  /// <summary>
  /// Definir tipos de Type
  /// </summary>
  [Flags]
  public enum TypeTypesEnum
  {
    /// <summary>
    /// Válido para todos os tipos
    /// </summary>
    All = 1,

    /// <summary>
    /// Classes
    /// </summary>
    Classes = 2,

    /// <summary>
    /// Interfaces
    /// </summary>
    Interfaces = 4,

    /// <summary>
    /// Structs
    /// </summary>
    Structs = 8,

    /// <summary>
    /// Enums
    /// </summary>
    Enums = 16,

    /// <summary>
    /// Delegates
    /// </summary>
    Delegates = 32
  }
}
