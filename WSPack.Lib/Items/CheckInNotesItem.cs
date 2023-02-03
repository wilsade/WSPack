using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.Lib.Items
{
  /// <summary>
  /// Classe para representar um Check In Note e seu respectivo valor
  /// </summary>
  [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
  public class CheckInNotesItem
  {
    /// <summary>
    /// Nome do Check In Note
    /// </summary>
    public string CheckInNoteName { get; set; }

    /// <summary>
    /// Operador
    /// </summary>
    public OperatorTypes Operador { get; set; }

    /// <summary>
    /// Valor do Check In Note
    /// </summary>
    public string CheckInNoteValue { get; set; }

    private string GetDebuggerDisplay()
    {
      return $"{CheckInNoteName}: {CheckInNoteValue}";
    }
  }
}
