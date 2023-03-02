using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace WSPack.Lib.WPF
{
  class VisualSutioFlexStylerController
  {
    static VisualSutioFlexStylerController _instance;
    static readonly object _locker = new object();

    /// <summary>
    /// Inicialização da classe: <see cref="VisualSutioFlexStylerController"/>.
    /// </summary>
    public VisualSutioFlexStylerController()
    {
      if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        return;

      string location = typeof(VisualSutioFlexStylerController).Assembly.Location;
      string path = Path.GetDirectoryName(location);

      var regex = new Regex("WSPack\\d+\\.dll", RegexOptions.IgnoreCase | RegexOptions.Compiled);
      string file = Directory.EnumerateFiles(path, "WSPack*.dll", SearchOption.TopDirectoryOnly)
        .FirstOrDefault(x => regex.IsMatch(x));

      Assembly asm = Assembly.LoadFile(file);
      Type tipo = asm.GetType("WSPack.VisualStudio.Shared.WPFs.VisualSutioStylerController");
      Controller = Activator.CreateInstance(tipo) as IVisualSutioStylerController;
    }

    public IVisualSutioStylerController Controller { get; }

    /// <summary>
    /// Devolve a instãncia da classe: <see cref="VisualSutioFlexStylerController"/>
    /// </summary>
    /// <value>Instância da classe: <see cref="VisualSutioFlexStylerController"/></value>
    public static VisualSutioFlexStylerController Instance
    {
      get
      {
        lock (_locker)
        {
          if (_instance == null)
            _instance = new VisualSutioFlexStylerController();
          return _instance;
        }
      }
    }
  }
}
