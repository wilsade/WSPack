using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WSPack.Lib
{
  /// <summary>
  /// Classe utilitária para Xml
  /// </summary>
  public static class XmlUtils
  {
    /// <summary>
    /// Ler um XML de parâmetros
    /// </summary>
    /// <typeparam name="T">Classe presente no XML</typeparam>
    /// <param name="path">Caminho do arquivo</param>
    /// <returns>Classe</returns>
    public static T ReadXMLParams<T>(string path) where T : class
    {
      string directory = Path.GetDirectoryName(path);
      if (!Directory.Exists(directory))
        Directory.CreateDirectory(directory);

      if (!File.Exists(path))
        return Activator.CreateInstance(typeof(T)) as T;

      var serializer = new XmlSerializer(typeof(T));
      using (var reader = new StreamReader(path))
      {
        object o = serializer.Deserialize(reader);
        return o as T;
      }
    }

    /// <summary>
    /// Salvar um XML de parâmetros
    /// </summary>
    /// <typeparam name="T">Classe de parâmetros</typeparam>
    /// <param name="instance">Instância da classe</param>
    /// <param name="path">Caminho do arquivo</param>
    public static void SaveXMLParams<T>(T instance, string path)
    {
      string directory = Path.GetDirectoryName(path);
      if (!Directory.Exists(directory))
        Directory.CreateDirectory(directory);

      var serializer = new XmlSerializer(typeof(T));
      using (var writer = new StreamWriter(path, false, Encoding.UTF8))
      {
        serializer.Serialize(writer, instance);
      }
    }
  }
}
