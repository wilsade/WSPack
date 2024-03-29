﻿using System.Windows;
using System.Windows.Controls;

namespace WSPack.Lib.WPF.CommonLib
{
  /// <summary>
  /// TextBlock com trigger
  /// </summary>
  public class WSUnderlineTextBlock : WSTextBlock
  {
    /// <summary>
    /// Cria uma instância da classe <see cref="WSTextBlock"/>
    /// </summary>
    public WSUnderlineTextBlock()
      : base()
    {
      SetTrigger();
    }

    private void SetTrigger()
    {
      Style style = new Style(typeof(TextBlock));
      Trigger trigger = new Trigger()
      {
        Property = Control.IsMouseOverProperty,
        Value = true
      };

      Setter setter = new Setter()
      {
        Property = TextBlock.TextDecorationsProperty,
        Value = System.Windows.TextDecorations.Underline
      };
      trigger.Setters.Add(setter);

      style.Triggers.Add(trigger);
      Style = style;
    }
  }
}
