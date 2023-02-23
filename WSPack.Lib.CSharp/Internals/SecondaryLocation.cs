using Microsoft.CodeAnalysis;

namespace WSPack.Lib.CSharp.Internals
{
  class SecondaryLocation
  {
    public SecondaryLocation(Location location, string message)
    {
      Location = location;
      Message = message;
    }

    public Location Location { get; }
    public string Message { get; }
  }
}
