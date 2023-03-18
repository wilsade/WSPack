using System.Windows.Controls;

namespace WSPack.Lib.WPF.SupportLib
{
  /// <summary>
  /// Converter ScrollBarVisibility
  /// </summary>
  public class BoolToScrollBarVisibilityConverter : BoolToBaseVisibilityConverter
  {
    /// <summary>
    /// Visible value
    /// </summary>
    public override object VisibleValue => ScrollBarVisibility.Auto;

    /// <summary>
    /// Invisible value
    /// </summary>
    public override object InvisibleValue => ScrollBarVisibility.Disabled;

    /// <summary>
    /// Verifica se pode convert back
    /// </summary>
    /// <param name="value">Value</param>
    /// <returns>
    /// "true" if this instance [can convert back] the specified value; otherwise, "false".
    /// </returns>
    public override bool CanConvertBack(object value) => value is ScrollBarVisibility && (ScrollBarVisibility)value == ScrollBarVisibility.Visible;
  }
}
