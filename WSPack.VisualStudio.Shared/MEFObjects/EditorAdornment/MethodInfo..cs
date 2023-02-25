using System.Diagnostics;

using WSPack.Lib.CSharp.Models;

namespace WSPack.VisualStudio.Shared.MEFObjects.EditorAdornment
{
  [DebuggerDisplay("{Method.Name}")]
  internal class MethodInfo
  {
    internal MethodModel Method;
    internal int Position;
    internal MetricsModel Metric;
  }
}