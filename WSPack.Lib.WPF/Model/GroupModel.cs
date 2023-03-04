using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.Lib.WPF.Model
{
  /// <summary>
  /// Representa um grupo da StartPage
  /// </summary>
  public class GroupModel : BaseModel
  {
    #region Construtores
    /// <summary>
    /// Cria uma instância da classe <see cref="GroupModel"/>
    /// </summary>
    public GroupModel()
    {

    }

    /// <summary>
    /// Cria uma instância da classe <see cref="GroupModel"/>
    /// </summary>
    /// <param name="id">Identificador do grupo</param>
    /// <param name="caption">Título do grupo</param>
    public GroupModel(int id, string caption)
      : this()
    {
      Id = id;
      Caption = caption;
    }
    #endregion
  }
}
