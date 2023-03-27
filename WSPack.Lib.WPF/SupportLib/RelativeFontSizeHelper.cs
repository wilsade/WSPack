using System;
using System.Windows;
using System.Windows.Controls;

namespace WSPack.Lib.WPF.SupportLib
{
  /// <summary>
  /// RelativeFontSizeHelper
  /// </summary>
  public class RelativeFontSizeHelper
  {
    /// <summary>
    /// Font size scale property
    /// </summary>
    public static readonly DependencyProperty FontSizeScaleProperty = DependencyProperty.RegisterAttached(
        "FontSizeScale", typeof(double), typeof(RelativeFontSizeHelper), new PropertyMetadata(default(double), OnPropertyChanged));

    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (d is TextBlock block)
      {
        block.FontSize = Math.Max(1, block.FontSize * GetFontSizeScale(d));
        return;
      }

      if (d is Control control)
      {
        control.FontSize = Math.Max(1, control.FontSize * GetFontSizeScale(d));
      }
    }

    /// <summary>
    /// Sets the font size scale.
    /// </summary>
    /// <param name="element">Element</param>
    /// <param name="value">Value</param>
    public static void SetFontSizeScale(DependencyObject element, double value)
    {
      element.SetValue(FontSizeScaleProperty, value);
    }

    /// <summary>
    /// Gets the font size scale.
    /// </summary>
    /// <param name="element">Element</param>
    /// <returns></returns>
    public static double GetFontSizeScale(DependencyObject element)
    {
      return (double)element.GetValue(FontSizeScaleProperty);
    }
  }
}
