using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// PropertyOrder
  /// </summary>
  /// <seealso cref="System.Attribute" />
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class PropertyOrderAttribute : Attribute
  {
    /// <summary>
    /// Cria uma instância da classe <see cref="PropertyOrderAttribute"/>
    /// </summary>
    /// <param name="order">Order</param>
    public PropertyOrderAttribute(int order)
    {
      Order = order;
    }

    /// <summary>
    /// Order
    /// </summary>
    public int Order { get; }
  }
}
