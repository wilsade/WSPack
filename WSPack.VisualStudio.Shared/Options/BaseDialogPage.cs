using System;
using System.IO;

using Microsoft.VisualStudio.Shell;

using WSPack.Lib.Extensions;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Classe base para os parâmetros do WSPack
  /// </summary>
  public abstract class BaseDialogPage : DialogPage
  {
    static readonly Type _typeOfThis = typeof(BaseDialogPage);

    /// <summary>
    /// Pasta base onde os arquivos de configuração serão salvos
    /// </summary>
    public static readonly string BaseConfigPath = Path.Combine(
      Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
      _typeOfThis.Assembly.GetCompany(),
      _typeOfThis.Assembly.GetProduct());

  }
}