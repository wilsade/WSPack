using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using EnvDTE;

using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Debug;
using Microsoft.VisualStudio.ProjectSystem.Properties;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using WSPack.Lib.Extensions;

namespace WSPack.VisualStudio.Shared.DteProject
{
  class DteDotNetCoreStandardProject : DteProjectHierarchy
  {
    ILaunchProfile _launchProfile;
    IProjectServices _projectServices;
    ILaunchSettingsProvider _launchSettingsProvider;
    string _defineConstants;

    class WritableLaunchProfile : ILaunchProfile
    {
      public string Name { set; get; }
      public string CommandName { set; get; }
      public string ExecutablePath { set; get; }
      public string CommandLineArgs { set; get; }
      public string WorkingDirectory { set; get; }
      public bool LaunchBrowser { set; get; }
      public string LaunchUrl { set; get; }
      public ImmutableDictionary<string, string> EnvironmentVariables { set; get; }
      public ImmutableDictionary<string, object> OtherSettings { set; get; }

      public WritableLaunchProfile(ILaunchProfile launchProfile)
      {
        Name = launchProfile.Name;
        ExecutablePath = launchProfile.ExecutablePath;
        CommandName = launchProfile.CommandName;
        CommandLineArgs = launchProfile.CommandLineArgs;
        WorkingDirectory = launchProfile.WorkingDirectory;
        LaunchBrowser = launchProfile.LaunchBrowser;
        LaunchUrl = launchProfile.LaunchUrl;
        EnvironmentVariables = launchProfile.EnvironmentVariables;
        OtherSettings = launchProfile.OtherSettings;
      }
    }

    /// <summary>
    /// Cria uma instância da classe <see cref="DteDotNetCoreStandardProject"/>
    /// </summary>
    /// <param name="parent">Projeto pai</param>
    /// <param name="hierarchy">Hiearchy</param>
    public DteDotNetCoreStandardProject(Project parent, IVsHierarchy hierarchy)
      : base(parent, hierarchy)
    {

    }

    void LoadLaunchProfile()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (_launchProfile == null)
      {
        if (TryGetProjectServices(Parent, out var unconfiguredProjectServices,
          out var projectServices))
        {
          _launchSettingsProvider = unconfiguredProjectServices.ExportProvider.GetExportedValue<ILaunchSettingsProvider>();
          _launchProfile = _launchSettingsProvider?.ActiveProfile;
          _projectServices = projectServices;
        }
      }
    }

    private static bool TryGetProjectServices(Project project,
      out IUnconfiguredProjectServices unconfiguredProjectServices,
      out IProjectServices projectServices)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      IVsBrowseObjectContext context = project as IVsBrowseObjectContext;
      if (context == null && project != null)
      {
        // VC implements this on their DTE.Project.Object
        context = project.Object as IVsBrowseObjectContext;
      }

      if (context == null)
      {
        unconfiguredProjectServices = null;
        projectServices = null;

        return false;
      }
      else
      {
        UnconfiguredProject unconfiguredProject = context.UnconfiguredProject;

        // VS2017 returns the interface types of the services classes but VS2019 returns the classes directly.
        // Hence, we need to obtain the object via reflection to avoid MissingMethodExceptions.
        object services = typeof(UnconfiguredProject).GetProperty("Services").GetValue(unconfiguredProject);
        object prjServices = typeof(IProjectService).GetProperty("Services").GetValue(unconfiguredProject.ProjectService);

        unconfiguredProjectServices = services as IUnconfiguredProjectServices;
        projectServices = prjServices as IProjectServices;

        return unconfiguredProjectServices != null && project != null;
      }
    }

    /// <summary>
    /// Argumentos de linha de comando do projeto
    /// </summary>
    public override string CommandLineArgs
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        LoadLaunchProfile();
        if (_launchProfile == null)
          return null;

        string argsAtual = _launchProfile.CommandLineArgs;
        return argsAtual;
      }
      set
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        var writableLaunchProfile = new WritableLaunchProfile(_launchProfile)
        {
          CommandLineArgs = value
        };

        // Does not work on VS2015, which should be okay ...
        // We don't hold references for VS2015, where the interface is called IThreadHandling
        IProjectThreadingService projectThreadingService = _projectServices.ThreadingPolicy;
        projectThreadingService.ExecuteSynchronously(() =>
        {
          return _launchSettingsProvider.AddOrUpdateProfileAsync(writableLaunchProfile, addToFront: false);
        });
      }
    }

    protected override string DefineConstants
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        if (_defineConstants == null)
        {
          try
          {
            string fullName = Parent.FullName;
            if (File.Exists(fullName))
            {
              _defineConstants = GetDefineConstants(fullName);
            }
          }
          catch (Exception ex)
          {
            Utils.LogDebugError($"Erro ao recuperar DefineConstants: {ex}");
          }
        }
        return _defineConstants;
      }
    }

    private string GetDefineConstants(string fullName)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      XDocument doc = XDocument.Load(fullName);

      var platform = _configuration.PlatformName.EqualsInsensitive("Any CPU") ?
        "AnyCPU" : _configuration.PlatformName;

      var lst = from nodos in doc.Descendants("PropertyGroup")
                let attCondition = nodos.Attribute("Condition")?.Value
                where !string.IsNullOrEmpty(attCondition) &&
                  attCondition.Contains(_configuration.ConfigurationName) &&
                  attCondition.Contains(platform)
                select nodos;
      var lstDefine = lst.Descendants(DEFINE_CONSTANTS).Select(x => x.Value);
      return lstDefine.FirstOrDefault();
    }
  }
}