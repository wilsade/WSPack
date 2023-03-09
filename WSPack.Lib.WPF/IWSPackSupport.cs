using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.Lib.WPF
{
  /// <summary>
  /// Suporte a funcionalidades dependentes de classes do WSPack
  /// </summary>
  public interface IWSPackSupport
  {
    /// <summary>
    /// Caminho do arquivo de configuração da StartPage
    /// </summary>
    string StartPageConfigPath { get; }

    /// <summary>
    /// Acontece em caso de erro
    /// </summary>
    void LogError(string errorMessage);
  }
}
