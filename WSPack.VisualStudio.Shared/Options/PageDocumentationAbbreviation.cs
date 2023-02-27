using System;
using System.Collections.Generic;

using WSPack.Lib.DocumentationObjects;

namespace WSPack.VisualStudio.Shared.Options
{
  /// <summary>
  /// Página de opções de abreviações de documentação
  /// </summary>
  public class PageDocumentationAbbreviation : PageDocumentationBaseGridDictionary
  {
    /// <summary>
    /// Recuperar a lista de itens default
    /// </summary>
    /// <returns>Default itens</returns>
    protected override List<DictionaryItem> GetDefaultItens()
    {
      return new List<DictionaryItem>()
      {
        new DictionaryItem("cod", "cód.", true),
        new DictionaryItem("Cod", "Cód.", true),
        new DictionaryItem("codigo", "código", true),
        new DictionaryItem("Codigo", "Código", true),
        new DictionaryItem("num", "número", true),
        new DictionaryItem("Num", "Número", true)
      };
    }

    /// <summary>
    /// Texto contendo informações sobre a tela
    /// </summary>
    /// <returns>Texto contendo informações sobre a tela</returns>
    protected override string GetInformationText()
    {
      return "Utilize as abreviações para documentar as siglas presentes dentro de uma string." + Environment.NewLine +
        "Ex: 'cod' => 'cód.' ou 'Num' => 'Número'" + Environment.NewLine +
        "Os itens de abreviações fazem distinção de Maiúsculas / Minúsculas e verificam a sigla inteira." + Environment.NewLine +
        "Para uma melhor experiência de uso, defina Regras utilizando uma das macros 'Words'.";
    }

    /// <summary>
    /// Ler a lista de itens do XML de parâmetros de documentação
    /// </summary>
    /// <param name="documentationParams">Parâmetros de documentação</param>
    /// <returns>lista de itens do XML de parâmetros de documentação</returns>
    protected override List<DictionaryItem> ReadList(DocumentationParams documentationParams)
    {
      return documentationParams.AbbreviationList;
    }

    /// <summary>
    /// Atualizar a lista com os valores do controle
    /// </summary>
    /// <param name="documentationParams">Parâmetros de documentação</param>
    /// <param name="dictionaryItems">Itens do controle</param>
    protected override void UpdateList(DocumentationParams documentationParams, List<DictionaryItem> dictionaryItems)
    {
      documentationParams.AbbreviationList = dictionaryItems;
    }
  }
}

