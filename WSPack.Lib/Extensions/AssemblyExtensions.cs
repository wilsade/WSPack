using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.Lib.Extensions
{
  /// <summary>
  /// Métodos estendidos para Assembly
  /// </summary>
  public static class AssemblyExtensions
  {
    /// <summary>
    /// Recuperar o título do Assembly
    /// </summary>
    /// <param name="asm">Assembly</param>
    /// <returns>Título do Assembly</returns>
    public static string GetTitle(this Assembly assembly)
    {
      object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
      if (attributes.Length > 0)
      {
        AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
        if (!string.IsNullOrEmpty(titleAttribute.Title))
          return titleAttribute.Title;
      }
      return System.IO.Path.GetFileNameWithoutExtension(assembly.CodeBase);
    }

    /// <summary>
    /// Recuperar o produto
    /// </summary>
    /// <param name="asm">Assembly</param>
    /// <returns>Produto; string.Empty caso não exista definido</returns>
    public static string GetProduct(this Assembly asm)
    {
      object[] attributes = asm.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
      if (attributes.Length == 0)
        return string.Empty;
      return ((AssemblyProductAttribute)attributes[0]).Product;
    }

    /// <summary>
    /// Retornar a versão do Assembly
    /// </summary>
    /// <param name="asm">Assembly</param>
    /// <returns>Versão do Assembly</returns>
    public static string GetVersion(this Assembly asm) => asm.GetName().Version.ToString();

    /// <summary>
    /// Recuperar os direitos autorias
    /// </summary>
    /// <param name="asm">Assembly</param>
    /// <returns>Direitos autorais; string.Empty caso não exista definido</returns>
    public static string GetCopyright(this Assembly asm)
    {
      object[] attributes = asm.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
      if (attributes.Length == 0)
        return string.Empty;
      return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
    }

    /// <summary>
    /// Recuperar a companhia do Assembly
    /// </summary>
    /// <param name="asm">Assembly</param>
    /// <returns>Companhia; string.Empty caso não exista definido</returns>
    public static string GetCompany(this Assembly asm)
    {
      object[] attributes = asm.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
      if (attributes.Length == 0)
        return "";
      return ((AssemblyCompanyAttribute)attributes[0]).Company;
    }

    /// <summary>
    /// Recuperar a descrição do Assembly
    /// </summary>
    /// <param name="asm">Assembly</param>
    /// <returns>Descrição; string.Empty caso não exista definido</returns>
    public static string GetDescription(this Assembly asm)
    {
      object[] attributes = asm.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
      if (attributes.Length == 0)
        return string.Empty;
      return ((AssemblyDescriptionAttribute)attributes[0]).Description;
    }
  }
}
