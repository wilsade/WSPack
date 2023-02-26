
using WSPack.Lib;

namespace WSPack.VisualStudio.Shared.DocumentationObjects
{
  /// <summary>
  /// DocumentationUtils
  /// </summary>
  public static class DocumentationUtils
  {
    static DocumentationParams _documentationParams;
    static readonly object _locker = new object();

    /// <summary>
    /// Recuperar os parâmetros de documentação
    /// </summary>
    public static DocumentationParams ReadDocumentationParams()
    {
      lock (_locker)
      {
        if (_documentationParams == null)
          _documentationParams = XmlUtils.ReadXMLParams<DocumentationParams>(WSPackConsts.DocumentationConfigPath);
        return _documentationParams;
      }
    }

    /// <summary>
    /// Salvar os parâmetros no arquivo de documentação
    /// </summary>
    /// <param name="documentation">Documentation</param>
    public static void SaveDocumentationParams(DocumentationParams documentation)
    {
      lock (_locker)
      {
        XmlUtils.SaveXMLParams(documentation, WSPackConsts.DocumentationConfigPath);
        _documentationParams = documentation;
      }
    }
  }
}
