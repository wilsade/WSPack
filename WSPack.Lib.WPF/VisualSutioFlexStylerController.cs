using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace WSPack.Lib.WPF
{
  public class VisualSutioFlexStylerController
  {
    static VisualSutioFlexStylerController _instance;

    /// <summary>
    /// Inicialização da classe: <see cref="VisualSutioFlexStylerController"/>.
    /// </summary>
    public VisualSutioFlexStylerController()
    {
      //if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
      //  return;

      //string location = typeof(VisualSutioFlexStylerController).Assembly.Location;
      //string path = Path.GetDirectoryName(location);

      //var regex = new Regex("WSPack\\d+\\.dll", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      //string file = Directory.EnumerateFiles(path, "WSPack*.dll", SearchOption.TopDirectoryOnly)
      //  .FirstOrDefault(x => regex.IsMatch(x));

      //Assembly asm = Assembly.LoadFile(file);
      //Type tipo = asm.GetType("WSPack.VisualStudio.Shared.WPFs.VisualSutioStylerController");
      //Controller = Activator.CreateInstance(tipo) as IVisualSutioStylerController;
    }

    public IVisualSutioStylerController Controller { get; set; }

    /// <summary>
    /// Devolve a instãncia da classe: <see cref="VisualSutioFlexStylerController"/>
    /// </summary>
    /// <value>Instância da classe: <see cref="VisualSutioFlexStylerController"/></value>
    public static VisualSutioFlexStylerController Instance
      => _instance ?? (_instance = new VisualSutioFlexStylerController());
  }
}
