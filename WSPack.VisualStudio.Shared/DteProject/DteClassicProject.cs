using System;

using EnvDTE;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using WSPack.VisualStudio.Shared.Extensions;

namespace WSPack.VisualStudio.Shared.DteProject
{
  class DteClassicProject : DteProjectHierarchy
  {
    string _defineConstants;

    public DteClassicProject(Project parent, IVsHierarchy hierarchy)
      : base(parent, hierarchy)
    {
    }

    /// <summary>
    /// Argumentos de linha de comando do projeto
    /// </summary>
    public override string CommandLineArgs
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        return Convert.ToString(CommandLineProperty.Value);
      }
      set
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        CommandLineProperty.Value = value;
      }
    }

    /// <summary>
    /// Constantes definidas como diretiva de compilação. Ex: DEBUG, TRACE
    /// </summary>
    protected override string DefineConstants
    {
      get
      {
        ThreadHelper.ThrowIfNotOnUIThread();
        if (_defineConstants == null)
        {
          ResponseItem<Property> defineConstants = _configuration.Properties.GetProperty(DEFINE_CONSTANTS);
          if (defineConstants.Success)
          {
            try
            {
              _defineConstants = Convert.ToString(defineConstants.Item.Value);
            }
            catch (Exception ex)
            {
              System.Diagnostics.Trace.WriteLine(ex.Message);
            }
          }
          else
            _defineConstants = string.Empty;
        }
        return _defineConstants;
      }
    }
  }
}