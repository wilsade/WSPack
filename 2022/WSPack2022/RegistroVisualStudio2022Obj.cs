using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WSPack.VisualStudio.Shared;

namespace WSPack2022
{
  internal class RegistroVisualStudio2022Obj : RegistroVisualStudioObj
  {
    /// <summary>
    /// Cria uma instância da classe <see cref="RegistroVisualStudio2022Obj"/>
    /// </summary>
    private RegistroVisualStudio2022Obj()
    {

    }

    /// <summary>
    /// Devolve a instância da classe <see cref="RegistroVisualStudio2022Obj"/>
    /// </summary>
    public static RegistroVisualStudio2022Obj CreateInstance() => new RegistroVisualStudio2022Obj();
    
  }
}
