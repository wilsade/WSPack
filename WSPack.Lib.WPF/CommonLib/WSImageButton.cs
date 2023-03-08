using System;
using System.Windows.Controls;
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
      if (VisualSutioFlexStylerController.Instance?.Controller == null)
        return;
      SetResourceReference(ForegroundProperty,
        VisualSutioFlexStylerController.Instance.Controller.GetForeground());
      SetResourceReference(BackgroundProperty,
        VisualSutioFlexStylerController.Instance.Controller.GetBackground());
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
}
