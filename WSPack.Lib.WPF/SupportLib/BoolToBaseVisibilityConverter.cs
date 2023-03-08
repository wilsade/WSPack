using System;
using System.Windows.Data;

namespace WSPack.Lib.WPF.SupportLib
{
  /// <summary>
  /// Converter base para booleanos
  /// </summary>
  /// <seealso cref="IValueConverter" />
  public abstract class BoolToBaseVisibilityConverter : IValueConverter
  {
    enum Parameters
    {
      Normal, Inverted
    }

    /// <summary>
    /// Visible value
    /// </summary>
    public abstract object VisibleValue { get; }

    /// <summary>
    /// Invisible value
    /// </summary>
    public abstract object InvisibleValue { get; }

    /// <summary>
    /// Verifica se pode convert back
    /// </summary>
    /// <param name="value">Value</param>
    /// <returns>
    /// "true" if this instance [can convert back] the specified value; otherwise, "false".
    /// </returns>
    public abstract bool CanConvertBack(object value);

    /// <summary>
    /// Converts a value.
    /// </summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>
    /// A converted value. If the method returns null, the valid null value is used.
    /// </returns>
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      Parameters direction = Parameters.Normal;
      if (parameter != null)
        Enum.TryParse(parameter.ToString(), true, out direction);

      if (value is bool boolean && boolean)
      {
        if (direction == Parameters.Normal)
          return VisibleValue;

        else
          return InvisibleValue;
      }

      if (direction == Parameters.Normal)
        return InvisibleValue;
      else
        return VisibleValue;
    }

    /// <summary>
    /// Converts a value.
    /// </summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>
    /// A converted value. If the method returns null, the valid null value is used.
    /// </returns>
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (CanConvertBack(value))
      {
        return true;
      }
      return false;
    }

  }
}
