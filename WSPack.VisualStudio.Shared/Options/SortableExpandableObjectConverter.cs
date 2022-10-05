using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Permitir ordenar propriedades de um objeto expansível
  /// </summary>
  /// <seealso cref="System.ComponentModel.ExpandableObjectConverter" />
  public class SortableExpandableObjectConverter : ExpandableObjectConverter
  {
    IEnumerable<string> _lstDecoratedProps;

    /// <summary>
    /// Gets a collection of properties for the type of object specified by the value parameter.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
    /// <param name="value">An <see cref="T:System.Object" /> that specifies the type of object to get the properties for.</param>
    /// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that will be used as a filter.</param>
    /// <returns>
    /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> with the properties that are exposed for the component, or null if there are no properties.
    /// </returns>
    public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
    {
      PropertyDescriptorCollection lstProps = base.GetProperties(context, value, attributes);
      IEnumerable<string> lstWithOrder = GetDecoratedProps(lstProps);
      var lstOrdered = lstProps.Sort(lstWithOrder.ToArray());
      return lstOrdered;
    }

    private IEnumerable<string> GetDecoratedProps(PropertyDescriptorCollection lstProps)
    {
      if (_lstDecoratedProps == null)
        _lstDecoratedProps = from estaProp in lstProps.OfType<PropertyDescriptor>()
                             let tem = estaProp.Attributes.OfType<PropertyOrderAttribute>().FirstOrDefault()
                             where tem != null
                             orderby tem.Order
                             select estaProp.Name;
      return _lstDecoratedProps;
    }
  }

}
