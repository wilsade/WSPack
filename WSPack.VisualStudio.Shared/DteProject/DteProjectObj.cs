using System;

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
    string _name, _fullName, _uniqueName;
    DteProjectHierarchy _properties;

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="DteProject"/>
    /// </summary>
    /// <param name="project"></param>
    public DteProjectObj(Project project)
    {
      _project = project;
    }
    #endregion

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
          _fullName = _project.FullName;
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