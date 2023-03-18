using System;
using System.Windows;

namespace WSPack.Lib.WPF.SupportLib
{
  /// <summary>
  /// FocusExtension
  /// </summary>
  public static class FocusExtension
  {
    /// <summary>
    /// Is focused property
    /// </summary>
    public static readonly DependencyProperty IsFocusedProperty =
        DependencyProperty.RegisterAttached("IsFocused", typeof(bool?), typeof(FocusExtension), new FrameworkPropertyMetadata(IsFocusedChanged));

    /// <summary>
    /// Gets the is focused.
    /// </summary>
    /// <param name="element">Element</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">element</exception>
    public static bool? GetIsFocused(DependencyObject element)
    {
      if (element == null)
      {
        throw new ArgumentNullException("element");
      }

      return (bool?)element.GetValue(IsFocusedProperty);
    }

    /// <summary>
    /// Sets the is focused.
    /// </summary>
    /// <param name="element">Element</param>
    /// <param name="value">Value</param>
    /// <exception cref="ArgumentNullException">element</exception>
    public static void SetIsFocused(DependencyObject element, bool? value)
    {
      if (element == null)
      {
        throw new ArgumentNullException("element");
      }

      element.SetValue(IsFocusedProperty, value);
    }

    private static void IsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var fe = (FrameworkElement)d;

      if (e.OldValue == null)
      {
        fe.GotFocus += FrameworkElement_GotFocus;
        fe.LostFocus += FrameworkElement_LostFocus;
      }

      if (!fe.IsVisible)
      {
        fe.IsVisibleChanged += new DependencyPropertyChangedEventHandler(fe_IsVisibleChanged);
      }

      if (e.NewValue != null && (bool)e.NewValue)
      {
        try
        {
          fe.Focus();
        }
        catch (Exception ex)
        {
          System.Diagnostics.Trace.WriteLine(ex.Message);
        }
      }
    }

    private static void fe_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      var fe = (FrameworkElement)sender;
      if (fe.IsVisible && (bool)((FrameworkElement)sender).GetValue(IsFocusedProperty))
      {
        fe.IsVisibleChanged -= fe_IsVisibleChanged;
        fe.Focus();
      }
    }

    private static void FrameworkElement_GotFocus(object sender, RoutedEventArgs e)
    {
      ((FrameworkElement)sender).SetValue(IsFocusedProperty, true);
    }

    private static void FrameworkElement_LostFocus(object sender, RoutedEventArgs e)
    {
      ((FrameworkElement)sender).SetValue(IsFocusedProperty, false);
    }
  }
}
