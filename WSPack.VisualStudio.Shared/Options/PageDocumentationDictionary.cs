using System.Collections.Generic;

using WSPack.Lib.DocumentationObjects;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Página de opções do dicionário de documentação
  /// </summary>
  public class PageDocumentationDictionary : PageDocumentationBaseGridDictionary
  {
    /// <summary>
    /// Recuperar a lista de itens default
    /// </summary>
    /// <returns>Default itens</returns>
    protected override List<DictionaryItem> GetDefaultItens()
    {
      return new List<DictionaryItem>()
      {
        new DictionaryItem("codVersao", "Cód. Versão", true),
        new DictionaryItem("descricao", "Descrição.", true),
        new DictionaryItem("Dispose", "Liberar os recursos alocados pelo objeto", true),
        new DictionaryItem("id", "Identificador", true),
        new DictionaryItem("logFileName", "Caminho do arquivo de log", true)
      };
    }

    /// <summary>
    /// Texto contendo informações sobre a tela
    /// </summary>
    /// <returns>Texto contendo informações sobre a tela</returns>
    protected override string GetInformationText()
    {
      return "Utilize o dicionário para documentar um Tipo ou um Membro. Os itens do dicionário" +
        " têm prioridade sobre as regras, fazem distinção de Maiúsculas / Minúsculas e verificam a palavra inteira.";
    }

    /// <summary>
    /// Ler a lista de itens do XML de parâmetros de documentação
    /// </summary>
    /// <param name="documentationParams">Parâmetros de documentação</param>
    /// <returns>lista de itens do XML de parâmetros de documentação</returns>
    protected override List<DictionaryItem> ReadList(DocumentationParams documentationParams)
    {
      return documentationParams.DictionaryList;
    }

    /// <summary>
    /// Atualizar a lista com os valores do controle
    /// </summary>
    /// <param name="documentationParams">Parâmetros de documentação</param>
    /// <param name="dictionaryItems">Itens do controle</param>
    protected override void UpdateList(DocumentationParams documentationParams, List<DictionaryItem> dictionaryItems)
    {
      documentationParams.DictionaryList = dictionaryItems;
    }
  }
}
