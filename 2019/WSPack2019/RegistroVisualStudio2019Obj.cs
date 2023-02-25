using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Settings;

using WSPack;
using WSPack.VisualStudio.Shared;

namespace WSPack2019
{
  /// <summary>
  /// Gerenciar operações do Visual Studio 2019
  /// </summary>
  class RegistroVisualStudio2019Obj : RegistroVisualStudioObj
  {
    #region Construtor
    /// <summary>
    /// Criar uma instância da classe <see cref="RegistroVisualStudio2019Obj"/>
    /// </summary>
    private RegistroVisualStudio2019Obj()
    {

    }
    #endregion

    /// <summary>
    /// Versão do Visual Studio. Ex: 2019 / 2022
    /// </summary>
    public override string Version => "2019";

    /// <summary>
    /// Devolve a instância da classe <see cref="RegistroVisualStudio2019Obj"/>
    /// </summary>
    public static RegistroVisualStudio2019Obj CreateInstance() => new RegistroVisualStudio2019Obj();
  }
}
