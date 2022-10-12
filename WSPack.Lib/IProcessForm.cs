using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.Lib
{
  /// <summary>
  /// Expor membros do form de processamento
  /// </summary>
  public interface IProcessForm
  {
    /// <summary>
    /// Título da tela
    /// </summary>
    string Titulo { get; set; }

    /// <summary>
    /// Descrição
    /// </summary>
    string Descricao { get; set; }

    /// <summary>
    /// Flag para permitir que o form seja fechado
    /// </summary>
    bool PodeSair { get; set; }

    /// <summary>
    /// Fechar o form
    /// </summary>
    void Close();
  }
}
