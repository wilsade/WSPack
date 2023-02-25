using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Settings;

namespace WSPack.VisualStudio.Shared
{
  abstract class RegistroVisualStudioObj
  {
    string _directoryFullPath;
    static RegistroVisualStudioObj _instance;

    /// <summary>
    /// Nome da chave: UseSolutionNavigatorGraphProvider
    /// </summary>
    protected const string UseSolutionNavigatorGraphProvider = "UseSolutionNavigatorGraphProvider";

    /// <summary>
    /// Versão do Visual Studio. Ex: 2019 / 2022
    /// </summary>
    public abstract string Version { get; }

    /// <summary>
    /// Devolve uma instância especializada classe <see cref="RegistroVisualStudioObj"/>
    /// </summary>
    public static RegistroVisualStudioObj Instance
    {
      get {
        if (_instance == null)
        {
          var tipos = Assembly.GetExecutingAssembly().GetTypes();
          var achei = tipos.FirstOrDefault(x => x.IsSubclassOf(typeof(RegistroVisualStudioObj)));
          var metodo = achei.GetMethod("CreateInstance", BindingFlags.Public | BindingFlags.Static);
          _instance = metodo.Invoke(null, null) as RegistroVisualStudioObj;
        }
        return _instance;
      }
    }

    /// <summary>
    /// Diretório completo do Devenv.exe
    /// </summary>
    public string DirectoryFullPath
    {
      get {
        if (_directoryFullPath == null)
        {
          Process p = Process.GetCurrentProcess();
          _directoryFullPath = Path.GetDirectoryName(p.MainModule.FileName);
        }
        return _directoryFullPath;
      }
    }

    /// <summary>
    /// Criar um ProcessStartInfo para o tf.exe
    /// </summary>
    /// <returns>ProcessStartInfo para o tf.exe</returns>
    public virtual ProcessStartInfo TFProcessStartInfo()
    {
      var pInfo = new ProcessStartInfo
      {
        FileName = Path.Combine(DirectoryFullPath,
           "CommonExtensions", "Microsoft", "TeamFoundation", "Team Explorer", "tf.exe"),
        UseShellExecute = false,
        CreateNoWindow = true,
        WorkingDirectory = DirectoryFullPath
      };
      return pInfo;
    }

    /// <summary>
    /// Criar um ProcessStartInfo para o git.exe
    /// </summary>
    /// <returns>ProcessStartInfo para o tf.exe</returns>
    public virtual ProcessStartInfo GitProcessStartInfo()
    {
      var gitExe = Path.Combine(DirectoryFullPath,
        "CommonExtensions", "Microsoft", "TeamFoundation", "Team Explorer",
        "Git", "cmd", "git.exe");
      if (!File.Exists(gitExe))
        Directory.EnumerateFiles(DirectoryFullPath, "git.exe", SearchOption.AllDirectories).FirstOrDefault();

      var pInfo = new ProcessStartInfo
      {
        FileName = gitExe,
        UseShellExecute = false,
        CreateNoWindow = true,
        WorkingDirectory = DirectoryFullPath,
        RedirectStandardOutput = true,
        RedirectStandardError = true
      };
      return pInfo;
    }

    /// <summary>
    /// Recuperar o caminho da .bat do Developer prompt command
    /// </summary>
    /// <returns>Caminho da .bat do Developer prompt command</returns>
    public virtual string GetDeveloperCommandPromptPath()
    {
      string batFile = DirectoryFullPath + "\\..\\Tools\\" + "VsDevCmd.bat";
      return batFile;
    }

    /// <summary>
    /// Indica se o Visual Studio está configurado para usar a navegação por membros no Solution Explorer
    /// </summary>
    public virtual bool UseSolutionMemberNavigator
    {
      get {
        SettingsStore store = WSPackPackage.Instance.GetReadOnlyUserSettingsStorage();
        if (store != null && store.CollectionExists(""))
        {
          uint valor = store.GetUInt32("", UseSolutionNavigatorGraphProvider, 1);
          return valor == 1;
        }
        return true;
      }

      set {
        WritableSettingsStore store = WSPackPackage.Instance.GetWritableUserSettingsStorage();
        if (store != null && store.CollectionExists(""))
        {
          store.SetUInt32("", UseSolutionNavigatorGraphProvider, value ? (uint)1 : 0);
        }
      }
    }

    /// <summary>
    /// Efetuar uma operação de Merge de um Changeset
    /// </summary>
    /// <param name="changesetId">Nº do Changeset</param>
    /// <param name="sourceLocalPath">Caminho local da Branch de origem</param>
    /// <param name="targetLocalPath">Caminho local da Branch de destino</param>
    /// <param name="onError">Acontece quando há erros</param>
    public void Merge(int changesetId, string sourceLocalPath, string targetLocalPath,
      Action<string> onError)
    {
      ProcessStartInfo pInfo = TFProcessStartInfo();
      pInfo.Arguments = string.Format("merge /recursive /lock:none /version:c{0}~c{0} \"{1}\" \"{2}\"",
        changesetId, sourceLocalPath, targetLocalPath);
      pInfo.RedirectStandardError = true;

      Process p = Process.Start(pInfo);

      if (onError != null)
      {
        //Task<string> t = p.StandardError.ReadToEndAsync();
        //string x = t.Result;
        string x = p.StandardError.ReadToEnd();
        if (!string.IsNullOrEmpty(x))
          onError(x);
      }
    }

  }
}