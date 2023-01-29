using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using EnvDTE;

using Microsoft.VisualStudio;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

namespace WSPack.VisualStudio.Shared.DteProject
{
  /// <summary>
  /// Fornecer mecanismo para manipular projetos específicos
  /// </summary>
  abstract class DteProjectHierarchy
  {
    public const string DEFINE_CONSTANTS = "DefineConstants";
    public const string OUTPUT_PATH = "OutputPath";
    public const string OUTPUT_FILENAME = "OutputFileName";

    private static readonly string[] _knownStartupProperties = { "CommandArguments", "StartArguments" };
    DteConfiguration _dteConfiguration;
    protected Configuration _configuration;
    Properties _properties;
    protected string _targetFullPathName;

    /// <summary>
    /// Propriedade do projeto que contém os argumentos de linha de comando
    /// </summary>
    protected Property CommandLineProperty { get; private set; }

    /// <summary>
    /// Projeto pai
    /// </summary>
    protected readonly Project Parent;

    /// <summary>
    /// Hierarchy
    /// </summary>
    protected readonly IVsHierarchy Hierarchy;

    /// <summary>
    /// Cria uma instância da classe <see cref="DteProjectHierarchy"/>
    /// </summary>
    /// <param name="parent">Projeto</param>
    /// <param name="hierarchy"></param>
    protected DteProjectHierarchy(Project parent, IVsHierarchy hierarchy)
    {
      Parent = parent;
      Hierarchy = hierarchy;
    }

    ResponseItem<Configuration> GetActiveConfiguration()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var response = new ResponseItem<Configuration>();
      ConfigurationManager configurationManager = Parent.ConfigurationManager;
      if (configurationManager == null)
      {
        response.ErrorMessage = ResourcesLib.StrProjetoNaoPossuiGerenciadorConfiguracao;
        return response;
      }

      response.Item = configurationManager.ActiveConfiguration;
      response.Success = response.Item != null;
      if (!response.Success)
        response.ErrorMessage = ResourcesLib.StrProjetoNaoPossuiConfiguracaoAtiva;
      return response;
    }

    ResponseItem<Properties> GetProperties()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (_properties != null)
        return new ResponseItem<Properties>()
        {
          Item = _properties,
          Success = true
        };

      var response = new ResponseItem<Properties>();
      try
      {
        var acheiConfiguracao = GetActiveConfiguration();
        if (!acheiConfiguracao.Success)
        {
          response.ErrorMessage = acheiConfiguracao.ErrorMessage;
          return response;
        }

        response.Item = acheiConfiguracao.Item.Properties;
        _properties = acheiConfiguracao.Item.Properties;
        response.Success = response.Item != null;
        if (!response.Success)
          response.ErrorMessage = ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao;
        return response;
      }
      catch (Exception ex)
      {
        if (ErrorHandler.IsCriticalException(ex))
          throw;
        return response;
      }
    }

    ResponseItem<Property> GetCommandLineProperty()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var properties = GetProperties();

      var retorno = new ResponseItem<Property>();
      if (properties.Success)
      {
        retorno = properties.Item.GetProperty(_knownStartupProperties);
        if (!retorno.Success)
          retorno.ErrorMessage = ResourcesLib.StrPropriedadeLilnhaComandoNaoEncontrada.FormatWith(Parent.FullName);
      }
      else
        retorno.ErrorMessage = properties.ErrorMessage;
      return retorno;
    }

    /// <summary>
    /// Devolve uma insância da classe
    /// </summary>
    /// <param name="parent">Projeto pai</param>
    /// <param name="hierarchy">Hiearchy</param>
    /// <returns>Instância da classe</returns>
    public static DteProjectHierarchy CreateInstance(Project parent, IVsHierarchy hierarchy)
    {
      if (hierarchy.IsDotNetCoreStandardShared())
        return new DteDotNetCoreStandardProject(parent, hierarchy);
      else
        return new DteClassicProject(parent, hierarchy);
    }

    /// <summary>
    /// Argumentos de linha de comando do projeto
    /// </summary>
    public abstract string CommandLineArgs { get; set; }

    /// <summary>
    /// Constantes definidas como diretiva de compilação. Ex: DEBUG, TRACE
    /// </summary>
    protected abstract string DefineConstants { get; }

    /// <summary>
    /// Caminho completo do assembly. Ex: c:\pasta\subpasta\bin\projeto.dll
    /// </summary>
    protected virtual string TargetFullPathName
    {
      get
      {
        if (_targetFullPathName != null)
          return _targetFullPathName;

        ResponseItem<Property> defineOutput = _configuration.Properties.GetProperty(OUTPUT_PATH);
        if (defineOutput.Success)
        {
          try
          {
            var outputPath = Convert.ToString(defineOutput.Item.Value);
            var dirInfo = new DirectoryInfo(this.Parent.FullName);
            var nameOnly = Parent.Properties.GetPropertyStringValue(DteProjectHierarchy.OUTPUT_FILENAME);
            _targetFullPathName = Path.Combine(dirInfo.Parent.FullName, outputPath, nameOnly);
          }
          catch (Exception ex)
          {
            System.Diagnostics.Trace.WriteLine(ex.Message);
          }
        }
        else
          _targetFullPathName = string.Empty;
        return _targetFullPathName;
      }
    }

    /// <summary>
    /// Configuração ativa
    /// </summary>
    public DteConfiguration ActiveConfiguration
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        if (_dteConfiguration == null)
        {
          ResponseItem<Configuration> config = GetActiveConfiguration();
          if (config.Success)
          {
            _configuration = config.Item;
            _dteConfiguration = new DteConfiguration(config.Item.ConfigurationName,
              config.Item.PlatformName, DefineConstants, TargetFullPathName);
          }
          else
            _ = Utils.ExecuteInMainThreadAsync(() => Utils.LogDebugMessage(config.ErrorMessage));
        }
        return _dteConfiguration;
      }
    }

    /// <summary>
    /// Indica se o projeto possui uma propriedade para armazenar parâmetros de linha de comando
    /// </summary>
    public bool HasCommandLineProperty(out string msgErro)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      msgErro = null;
      if (CommandLineProperty == null)
      {
        var resposta = GetCommandLineProperty();
        CommandLineProperty = resposta.Item;
        msgErro = resposta.ErrorMessage;
      }
      return CommandLineProperty != null;
    }
  }
}