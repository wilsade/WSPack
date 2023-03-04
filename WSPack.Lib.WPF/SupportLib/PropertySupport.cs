using System;
using System.Linq.Expressions;
using System.Reflection;

namespace WSPack.Lib.WPF.SupportLib
{
  /// <summary>
  /// Suporte à propriedade
  /// </summary>
  public static class PropertySupport
  {
    /// <summary>
    /// Extracts the name of the property.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyExpresssion">Property expresssion</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">propertyExpresssion</exception>
    /// <exception cref="ArgumentException">
    /// The expression is not a member access expression. - propertyExpresssion
    /// or
    /// The member access expression does not access a property. - propertyExpresssion
    /// or
    /// The referenced property is a static property. - propertyExpresssion
    /// </exception>
    public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpresssion)
    {
      if (propertyExpresssion == null)
      {
        throw new ArgumentNullException("propertyExpresssion");
      }

      if (!(propertyExpresssion.Body is MemberExpression memberExpression))
      {
        throw new ArgumentException("The expression is not a member access expression.", "propertyExpresssion");
      }

      var property = memberExpression.Member as PropertyInfo ?? throw new ArgumentException("The member access expression does not access a property.", "propertyExpresssion");
      var getMethod = property.GetGetMethod(true);
      if (getMethod.IsStatic)
        throw new ArgumentException("The referenced property is a static property.", "propertyExpresssion");

      return memberExpression.Member.Name;
    }
  }
}
