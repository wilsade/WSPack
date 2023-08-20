using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using WSPack.Lib.Extensions;

namespace WSPack.Lib.WPF.SupportLib
{
  /// <summary>
  /// Descobrir o ícone associado a um caminho
  /// </summary>
  /// <seealso cref="IValueConverter" />
  public class PathToSystemIconConverter : IValueConverter
  {
    private static readonly Dictionary<string, ImageSource> _imageCache;

    /// <summary>
    /// Inicialização da classe: <see cref="PathToSystemIconConverter"/>.
    /// </summary>
    static PathToSystemIconConverter()
    {
      _imageCache = new Dictionary<string, ImageSource>();
    }

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
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is string fileName))
        return null;

      return ConvertFromCache(fileName);
    }

    static bool IsHttp(string fileName)
    {
      return fileName.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
              || fileName.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
    }

    private object ConvertFromCache(string fileName)
    {
      if (IsHttp(fileName))
      {
        fileName = "test.url";
      }

      var extension = Path.GetExtension(fileName);
      if (extension.EqualsInsensitive(".sln"))
      {
        try
        {
          if (File.Exists(fileName))
          {
            using (var reader = new StreamReader(fileName))
            {
              string linha = reader.ReadLine();
              while (linha != null)
              {
                if (!string.IsNullOrEmpty(linha))
                {
                  var splited = linha.Split(' ');
                  if (splited.Length > 0)
                  {
                    string versao = splited[splited.Length - 1];
                    extension += versao;
                  }
                  break;
                }
                linha = reader.ReadLine();
              }
            }

          }
        }
        catch (Exception ex)
        {
          System.Diagnostics.Trace.WriteLine(ex.Message);
        }
      }

      if (!_imageCache.ContainsKey(extension))
      {
        if (extension.IsNullOrWhiteSpaceEx() || Directory.Exists(fileName))
          _imageCache[extension] = new BitmapImage(new Uri(
            $"pack://application:,,,/WSPack.Lib;Component/Resources/FolderClosed.png", UriKind.RelativeOrAbsolute));
        else
        {
          _imageCache.Add(extension, GetFileIcon(fileName));
        }
      }

      return _imageCache[extension];
    }

    static ImageSource GetFileIcon(string fileName)
    {
      // if file does not exist, create a temp file with the same file extension
      var isTemp = false;
      if (!File.Exists(fileName) && !Directory.Exists(fileName))
      {
        isTemp = true;
        fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + Path.GetExtension(fileName));
        File.WriteAllText(fileName, string.Empty);
      }

      var shinfo = new SHFILEINFO();

      var flags = SHGFI_SYSICONINDEX;
      if (fileName.IndexOf(":", StringComparison.Ordinal) == -1)
        flags |= SHGFI_USEFILEATTRIBUTES;
      flags = flags | SHGFI_ICON | SHGFI_SMALLICON;

      SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);
      var icon = Icon.FromHandle(shinfo.hIcon);
      var bitmap = icon.ToBitmap();

      IntPtr hBitmap = bitmap.GetHbitmap();
      try
      {
        return Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());
      }
      finally
      {
        DeleteObject(hBitmap);
        if (isTemp)
        {
          File.Delete(fileName);
        }
      }
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
    /// <exception cref="NotImplementedException"></exception>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    #region PInvoke

    // ReSharper disable InconsistentNaming
    // ReSharper disable FieldCanBeMadeReadOnly.Local
    // ReSharper disable MemberCanBePrivate.Local

    private const uint SHGFI_ICON = 0x100;
    private const uint SHGFI_SMALLICON = 0x1;
    private const uint SHGFI_SYSICONINDEX = 16384;
    private const uint SHGFI_USEFILEATTRIBUTES = 16;

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes,
                                              ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr hObject);

    [StructLayout(LayoutKind.Sequential)]
    private struct SHFILEINFO
    {
      /// <summary>
      /// hIcon
      /// </summary>
      public IntPtr hIcon;

      /// <summary>
      /// iIcon
      /// </summary>
      public IntPtr iIcon;

      /// <summary>
      /// dwAttributes
      /// </summary>
      public uint dwAttributes;

      /// <summary>
      /// szDisplayName
      /// </summary>
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
      public string szDisplayName;

      /// <summary>
      /// szTypeName
      /// </summary>
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
      public string szTypeName;
    };
    #endregion
  }
}
