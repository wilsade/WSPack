using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Microsoft;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Classe base para os parâmetros do WSPack
  /// </summary>
  public abstract class BaseDialogPage : DialogPage
  {
    static readonly Type _typeOfThis = typeof(BaseDialogPage);

    bool _guardouOpcoes = false;

    static readonly string Settings_Store_Base_Name =
      $"ApplicationPrivateSettings\\{_typeOfThis.Namespace.Replace(".", "\\")}\\";

    IEnumerable<PropertyInfo> GetPropertiesFromObject(object obj) =>
      obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
      .Where(x => x.PropertyType.IsSerializable);

    /// <summary>
    /// Flag para controlar a exibição de uma mensagem para reiniciar o Visual Studio
    /// </summary>
    protected static bool MostrarMensagemReiniciar = false;

    /// <summary>
    /// The show message on close
    /// </summary>
    protected static Action ShowMessageOnClose = null;

    /// <summary>
    /// Handles "apply" messages from the Visual Studio environment.
    /// </summary>
    /// <param name="e">Parâmetros</param>
    /// <devdoc>
    /// This method is called when VS wants to save the user's
    /// changes then the dialog is dismissed.
    /// </devdoc>
    protected override void OnApply(PageApplyEventArgs e)
    {
      base.OnApply(e);
    }

    /// <summary>
    /// Handles Close messages from the Visual Studio environment.
    /// </summary>
    /// <param name="e">[in] Arguments to event handler.</param>
    protected override void OnClosed(EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (MostrarMensagemReiniciar)
      {
        bool ok = MessageBoxUtils.ShowWarningYesNo(ResourcesLib.StrDesejaReiniciarVisualStudio, MessageBoxDefaultButton.Button2);
        if (ok)
        {
          var shell = (IVsShell4)WSPackPackage.ServiceProvider.GetService(typeof(SVsShell));
          Assumes.Present(shell);
          shell.Restart((uint)__VSRESTARTTYPE.RESTART_Normal);
        }
      }

      ShowMessageOnClose?.Invoke();

      MostrarMensagemReiniciar = false;
      ShowMessageOnClose = null;

      _guardouOpcoes = false;
      base.OnClosed(e);
    }

    /// <summary>
    /// Handles Windows Activate messages from the Visual Studio environment.
    /// </summary>
    /// <param name="e">[in] Arguments to event handler.</param>
    protected override void OnActivate(CancelEventArgs e)
    {
      if (!_guardouOpcoes)
      {
        OnGuardarOpcoes?.Invoke(this, EventArgs.Empty);
        _guardouOpcoes = true;
      }
      base.OnActivate(e);
    }

    /// <summary>
    /// Acontece quando é necessário guardar as opções que foram carregadas a primeira vez
    /// </summary>
    protected event EventHandler<EventArgs> OnGuardarOpcoes;

    /// <summary>
    /// Carregar propriedades de um objeto expansível
    /// </summary>
    /// <param name="pageName">Nome da página de opções</param>
    /// <param name="lstExpandableObjects">Lista de propriedades: Nome da propriedade + Object expansivel</param>
    protected void LoadExpandableProperties(string pageName, List<(string PropName, object ExpObject)> lstExpandableObjects)
    {
      try
      {
        SettingsStore settingsStore = WSPackPackage.Instance.GetReadOnlyUserSettingsStorage();
        if (settingsStore == null)
          return;

        string collectionName = Settings_Store_Base_Name + pageName;
        foreach (var estaTupla in lstExpandableObjects)
        {
          IEnumerable<PropertyInfo> lstProps = GetPropertiesFromObject(estaTupla.ExpObject);
          foreach (var estaProp in lstProps)
          {
            object valor = null;
            string fullPropName = $"{estaTupla.PropName}.{estaProp.Name}";
            if (settingsStore.PropertyExists(collectionName, fullPropName))
            {
              if (estaProp.PropertyType == typeof(int))
                valor = settingsStore.GetInt32(collectionName, fullPropName);
              else if (estaProp.PropertyType == typeof(bool))
                valor = settingsStore.GetBoolean(collectionName, fullPropName);
              else if (estaProp.PropertyType.IsEnum)
                valor = settingsStore.GetInt32(collectionName, fullPropName);
              else
                valor = settingsStore.GetString(collectionName, fullPropName);
            }
            else
            {
              var defaultValueAtt = estaProp.GetCustomAttribute<DefaultValueAttribute>();
              if (defaultValueAtt != null)
              {
                if (estaProp.PropertyType.IsEnum)
                  valor = (int)defaultValueAtt.Value;
                else
                  valor = defaultValueAtt.Value;
              }
#if DEBUG
              else
              {
                System.Diagnostics.Debug.WriteLine("Verificar valor default nulo");
              }
#endif
            }

#if DEBUG
            if (valor == null)
            {
              System.Diagnostics.Debug.WriteLine("Valor nulo");
            }
#endif

            estaProp.SetValue(estaTupla.ExpObject, valor);
          }
        }
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"LoadExpandableProperties [{pageName}]: {ex.Message}");
      }
    }


    /// <summary>
    /// Carrgar propriedades de um objeto expansível
    /// </summary>
    /// <param name="pageName">Nome da página de opções</param>
    /// <param name="lstExpandableObjects">Lista de propriedades: Nome da propriedade + Object expansivel</param>
    protected void SaveExpandableProperties(string pageName, List<(string PropName, object ExpObject)> lstExpandableObjects)
    {
      try
      {
        WritableSettingsStore settingsStore = WSPackPackage.Instance.GetWritableUserSettingsStorage();
        if (settingsStore == null)
          return;

        string collectionName = Settings_Store_Base_Name + pageName;
        if (!settingsStore.CollectionExists(collectionName))
          return;

        foreach (var estaTupla in lstExpandableObjects)
        {
          IEnumerable<PropertyInfo> lstProps = GetPropertiesFromObject(estaTupla.ExpObject);
          foreach (var estaProp in lstProps)
          {
            object valor = estaProp.GetValue(estaTupla.ExpObject);
            string fullPropName = $"{estaTupla.PropName}.{estaProp.Name}";
            if (estaProp.PropertyType == typeof(int))
              settingsStore.SetInt32(collectionName, fullPropName, Convert.ToInt32(valor));
            else if (estaProp.PropertyType == typeof(bool))
              settingsStore.SetBoolean(collectionName, fullPropName, Convert.ToBoolean(valor));
            else if (estaProp.PropertyType.IsEnum)
              settingsStore.SetInt32(collectionName, fullPropName, (int)valor);
            else
              settingsStore.SetString(collectionName, fullPropName, Convert.ToString(valor));
          }
        }
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"{nameof(SaveExpandableProperties)} [{pageName}]: {ex.Message}");
      }
    }

    /// <summary>
    /// Pasta base onde os arquivos de configuração serão salvos
    /// </summary>
    public static readonly string BaseConfigPath = Path.Combine(
      Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
      _typeOfThis.Assembly.GetCompany(),
      _typeOfThis.Assembly.GetProduct());

  }
} 