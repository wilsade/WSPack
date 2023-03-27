using System;
using System.Diagnostics;

using EnvDTE;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using WSPack.VisualStudio.Shared.Extensions;

namespace WSPack.VisualStudio.Shared.DteProject
{
  /// <summary>
  /// Wrapper para um projeto do Visual Studio
  /// </summary>
  class DteProjectObj
  {
    readonly Project _project;
    IVsSolution2 _solutionService;
    string _name, _fullName, _uniqueName, _outputFileName;
    DteProjectHierarchy _properties;

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="DteProject"/>
    /// </summary>
    /// <param name="project">Projeto</param>
    public DteProjectObj(Project project)
    {
      _project = project;
    }
    #endregion

    /// <summary>
    /// Cria uma instância da classe <see cref="DteProject"/>
    /// </summary>
    /// <param name="project">Project</param>
    /// <returns>Instancia da classe</returns>
    public static DteProjectObj Create(Project project)
    {
      if (project != null)
        return new DteProjectObj(project);
      return null;
    }

    /// <summary>
    /// Criar um projeto conforme o projeto ativo na solution
    /// </summary>
    /// <param name="instance">Instância</param>
    /// <returns>true se o projeto foi criado</returns>
    public static bool TryCreateFromActiveDocument(out DteProjectObj instance)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      instance = null;

      try
      {
        Project project = WSPackPackage.Dte?.ActiveDocument?.ProjectItem?.ContainingProject ??
            WSPackPackage.Dte?.GetSolutionExplorerActiveProject();

        if (project != null)
          instance = new DteProjectObj(project);
        return instance != null;
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"Erro ao recuperar projeto com base no documento: {ex}");
        return false;
      }
    }

    void LoadSolutionService()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (_solutionService == null)
      {
        _solutionService = Package.GetGlobalService(typeof(SVsSolution)) as IVsSolution2;
      }
    }

    DteProjectHierarchy GetProjectHierarchy()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      LoadSolutionService();
      int resposta = _solutionService.GetProjectOfUniqueName(UniqueName, out var hierarchy);
      if (resposta == 0)
        return DteProjectHierarchy.CreateInstance(_project, hierarchy);
      return null;
    }

    /// <summary>
    /// Nome do projeto
    /// </summary>
    public string Name
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        if (_name == null)
          _name = _project.Name;
        return _name;
      }
    }

    /// <summary>
    /// Nome completo do projeto
    /// </summary>
    public string FullName
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        if (_fullName == null)
          try
          {
            _fullName = _project.FullName;
          }
          catch (Exception ex)
          {
            Trace.WriteLine(ex.Message);
          }
        return _fullName;
      }
    }

    /// <summary>
    /// Nome do único do projeto na solution
    /// </summary>
    public string UniqueName
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        if (_uniqueName == null)
          _uniqueName = _project.UniqueName;
        return _uniqueName;
      }
    }

    /// <summary>
    /// Nome do assembly. Ex: projeto.exe  projeto.dll
    /// </summary>
    public string OutputFileName
    {
      get
      {
        if (_outputFileName == null)
        {
          var response = _project.Properties.GetProperty(DteProjectHierarchy.OUTPUT_FILENAME);
          if (response.Success)
            _outputFileName = Convert.ToString(response.Item.Value);
        }
        return _outputFileName;
      }
    }

    /// <summary>
    /// Indica se é Shared project
    /// </summary>
    public bool IsSharedProject => FullName.EndsWith(".shproj", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Propriedades específicas conforme o tipo de projeto
    /// </summary>
    public DteProjectHierarchy Properties
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        if (_properties == null)
          _properties = GetProjectHierarchy();
        return _properties;
      }
    }
  }
}