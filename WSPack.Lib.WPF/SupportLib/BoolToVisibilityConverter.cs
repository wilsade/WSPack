using System.Windows;

namespace WSPack.Lib.WPF.SupportLib
{
  /// <summary>
  /// Converter bool para Visibility
  /// </summary>
  public class BoolToVisibilityConverter : BoolToBaseVisibilityConverter
  {
    /// <summary>
    /// Visible value
    /// </summary>
    public override object VisibleValue => Visibility.Visible;

    /// <summary>
    /// Invisible value
    /// </summary>
    public override object InvisibleValue => Visibility.Collapsed;

    /// <summary>
    /// Verifica se pode convert back
    /// </summary>
    /// <param name="value">Value</param>
    /// <returns>
    /// "true" if this instance [can convert back] the specified value; otherwise, "false".
    /// </returns>
    public override bool CanConvertBack(object value) =>
      value is Visibility visibility && visibility == Visibility.Visible;
  }
}
