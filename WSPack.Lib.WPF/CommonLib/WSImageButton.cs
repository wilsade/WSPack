﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WSPack.Lib.WPF.CommonLib
{
  /// <summary>
  /// Botão com imagem
  /// </summary>
  public class WSImageButton : Button
  {
    string _myImage;

    /// <summary>
    /// Inicialização da classe: <see cref="WSImageButton"/>.
    /// </summary>
    public WSImageButton() : base()
    {
      if (WSPackFlexSupport.Instance?.VSStyler == null)
        return;
      SetResourceReference(ForegroundProperty,
        WSPackFlexSupport.Instance.VSStyler.ForegroundEx);
      SetResourceReference(BackgroundProperty,
        WSPackFlexSupport.Instance.VSStyler.BackgroundEx);
    }

    /// <summary>
    /// Nome da imagem (a imagem deve ser um Resource na WSPack.Lib
    /// </summary>
    public string MyImage
    {
      get => _myImage;
      set
      {
        if (string.IsNullOrWhiteSpace(value))
        {
          Content = null;
        }
        else
        {
          _myImage = value;
          Content = new Image
          {
            Source = new BitmapImage(new Uri($"pack://application:,,,/WSPack.Lib;Component/Resources/{_myImage}")),
            Width = 16,
            Height = 16
          };
        }
      }
    }
  }

  /// <summary>
  /// Definir imagem para o botão
  /// </summary>
  /// <seealso cref="Button" />
  public class WSImageButton2 : Button
  {
    static WSImageButton2()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(WSImageButton2), new FrameworkPropertyMetadata(typeof(WSImageButton2)));
    }

    /// <summary>
    /// Image property
    /// </summary>
    public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
        "Image", typeof(ImageSource), typeof(WSImageButton2), new PropertyMetadata(default(ImageSource)));

    /// <summary>
    /// Image
    /// </summary>
    public ImageSource Image
    {
      get { return (ImageSource)GetValue(ImageProperty); }
      set { SetValue(ImageProperty, value); }
    }
  }
}
