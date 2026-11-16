using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WSPack.Lib
{
  public static class LibSerializerObj
  {
    /// <summary>
    /// Carregar um arquivo json de configuração
    /// </summary>
    /// <typeparam name="T">Tipo de classe do json</typeparam>
    /// <param name="configFile">Caminho do arquivo</param>
    /// <returns>Instância da classe</returns>
    /// <exception cref="InvalidDataException"></exception>
    public static T Load<T>(string configFile) where T : new()
    {
      if (!File.Exists(configFile))
        return new T();

      try
      {
        using (var stream = File.OpenRead(configFile))
        {
          var serializer = new DataContractJsonSerializer(typeof(T));
          var obj = serializer.ReadObject(stream);

          return obj == null ? new T() : (T)obj;
        }
      }
      catch (SerializationException ex)
      {
        throw new InvalidDataException($"Configuration '{configFile}' is corrupted.", ex);
      }
    }

    /// <summary>
    /// Salvar um arquivo de configuração json
    /// </summary>
    /// <typeparam name="T">Tipo de classe do json</typeparam>
    /// <param name="file">Caminho do arquivo</param>
    /// <param name="instance">Instância da classe</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void SaveConfiguration<T>(string file, T instance)
    {
      if (instance == null)
        throw new ArgumentNullException(nameof(instance));

      var tempFile = file + ".tmp";

      try
      {
        using (var stream = File.Create(tempFile))
        using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, false, true))
        {
          var serializer = new DataContractJsonSerializer(typeof(T));
          serializer.WriteObject(writer, instance);
          writer.Flush();
        }

        if (File.Exists(file))
          File.Replace(tempFile, file, null);
        else
          File.Move(tempFile, file);
      }
      catch
      {
        if (File.Exists(tempFile))
        {
          try
          {
            File.Delete(tempFile);
          }
          catch (Exception ex)
          {
            /* best effort */
            Trace.WriteLine(ex.Message);
          }
        }
        throw;
      }
    }
  }
}
