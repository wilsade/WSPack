namespace WSPack.VisualStudio.Shared.DocumentationObjects.Macros
{
  /// <summary>
  /// Definição de Macros para documentação
  /// </summary>
  public static class MacrosConsts
  {
    #region Environment
    /// <summary>
    /// Local onde o cursor ficará
    /// </summary>
    public const string End = "End";

    /// <summary>
    /// Nome da regra
    /// </summary>
    public const string RuleName = "RuleName";

    /// <summary>
    /// Nome do usuário
    /// </summary>
    public const string EnvironmentUserName = "Environment.UserName";

    /// <summary>
    /// Nome do computador
    /// </summary>
    public const string EnvironmentMachineName = "Environment.MachineName";

    /// <summary>
    /// Data / Hora
    /// </summary>
    public const string DateTimeNow = "DateTime.Now";

    /// <summary>
    /// Data
    /// </summary>
    public const string DateTimeNowShortDate = "DateTime.Now.ToShortDateString()";

    /// <summary>
    /// Hora
    /// </summary>
    public const string DateTimeNowShortTime = "DateTime.Now.ToShortTimeString()";
    #endregion

    #region DeclaringType
    /// <summary>
    /// Nome do tipo que declara o membro
    /// </summary>
    public const string DeclaringTypeName = "DeclaringType.Name";

    /// <summary>
    /// Nome do tipo que declara o membro com a tag See
    /// </summary>
    public const string DeclaringTypeNameSee = "DeclaringType.Name.See";

    /// <summary>
    /// Nome completo do tipo que declara o membro
    /// </summary>
    public const string DeclaringTypeFullName = "DeclaringType.FullName";

    /// <summary>
    /// Nome completo do tipo que declara o membro com a tag See
    /// </summary>
    public const string DeclaringTypeFullNameSee = "DeclaringType.FullName.See";
    #endregion

    #region Element
    /// <summary>
    /// Nome do elemento que está sendo documentado
    /// </summary>
    public const string ElementName = "Element.Name";

    /// <summary>
    /// Nome completo do elemento que está sendo documentado
    /// </summary>
    public const string ElementFullName = "Element.FullName";
    #endregion

    #region Words    
    /// <summary>
    /// Separa cada palavra do nome
    /// </summary>
    public const string AllSpaced = "All";

    /// <summary>
    /// Separa cada palavra do nome: inicial minúscula
    /// </summary>
    public const string AllLowerCase = "All.LowerCase";

    /// <summary>
    /// Separa cada palavra do nome: primeira palavra com inicial maiúscula
    /// </summary>
    public const string AllFirstUpperCase = "All.FirstUpperCase";

    /// <summary>
    /// Separa cada palavra do nome a partir da segunda palavra
    /// </summary>
    public const string AllExceptFirst = "All.ExceptFirst";

    /// <summary>
    /// Separa cada palavra do nome a partir da segunda palavra: inicial minúscula
    /// </summary>
    public const string AllExceptFirstLowerCase = "All.ExceptFirstLowerCase";

    /// <summary>
    /// Separa cada palavra do nome excetuando a última
    /// </summary>
    public const string AllExceptLast = "All.ExceptLast";

    /// <summary>
    /// Primeira palavra: inicial maiúscula
    /// </summary>
    public const string FirstUpperCase = "First.UpperCase";

    /// <summary>
    /// Primeira palavra: inicial minúscula
    /// </summary>
    public const string FirstLowerCase = "First.LowerCase";

    /// <summary>
    /// Segunda palavra: inicial maiúscula
    /// </summary>
    public const string SecondUpperCase = "Second.UpperCase";

    /// <summary>
    /// Second palavra: inicial minúscula
    /// </summary>
    public const string SecondLowerCase = "Second.LowerCase";

    /// <summary>
    /// Última palavra: inicial maiúscula
    /// </summary>
    public const string LastUpperCase = "Last.UpperCase";

    /// <summary>
    /// Última palavra: inicial minúscula
    /// </summary>
    public const string LastLowerCase = "Last.LowerCase";

    /// <summary>
    /// Entre a primeira e última palavra: inicial maiúscula
    /// </summary>
    public const string BetweenFirstAndLastUpperCase = "BetweenFirstAndLast.UpperCase";

    /// <summary>
    /// Entre a primeira e última palavra: inicial minúscula
    /// </summary>
    public const string BetweenFirstAndLastLowerCase = "BetweenFirstAndLast.LowerCase";
    #endregion

    #region MemberType
    /// <summary>
    /// Nome do Type
    /// </summary>
    public const string TypeName = "TypeName";

    /// <summary>
    /// Nome do Type com a tag: See
    /// </summary>
    public const string TypeNameSee = "TypeName.See";

    /// <summary>
    /// Nome completo do Type
    /// </summary>
    public const string TypeFullName = "TypeFullName";

    /// <summary>
    /// Nome completo do Type com a tag: See
    /// </summary>
    public const string TypeFullNameSee = "TypeFullName.See";
    #endregion
  }
}
