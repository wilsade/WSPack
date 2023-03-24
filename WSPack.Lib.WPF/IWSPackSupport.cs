using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.Lib.WPF
{
  /// <summary>
  /// Suporte a funcionalidades dependentes de classes do WSPack
  /// </summary>
  public interface IWSPackSupport
  {
    /// <summary>
    /// Caminho do arquivo de configuração da StartPage
    /// </summary>
    string StartPageConfigPath { get; }

    /// <summary>
    /// Acontece em caso de erro
    /// </summary>
    void LogError(string errorMessage);

    /// <summary>
    /// Localizar o projeto no TFS ou efetuar GET
    /// </summary>
    /// <param name="localItem">Projeto</param>
    /// <returns>true se a operação foi executada com sucesso</returns>
    bool Locate_OR_Get(string localItem, OperationTFSTypes tipoOperacao);

    /// <summary>
    /// Abrir um arquivo de solution (sln)
    /// </summary>
    /// <param name="solutionFullPath">Caminho completo da solution</param>
    /// <returns>true se a solution foi aberta com sucesso</returns>
    bool OpenSolutionFile(string solutionFullPath);

    /// <summary>
    /// Abrir um arquivo de projeto (csproj)
    /// </summary>
    /// <param name="projectFullPath">Caminho completo do projeto</param>
    /// <returns>true se o projeto foi aberto com sucesso</returns>
    bool OpenProjectFile(string projectFullPath);

    /// <summary>
    /// Localizar um item no windows
    /// </summary>
    /// <param name="itemPath">Caminho completo do item</param>
    void LocateInWindows(string itemPath);

    /// <summary>
    /// Indica se o projeto/solution é do Git
    /// </summary>
    /// <param name="projectFullPath">Caminho completo do projeto</param>
    /// <returns>true se é do GIT</returns>
    bool IsGit(string projectFullPath);

    /// <summary>
    /// Recuperar o nome do projeto ativo no controle do TFS da IDE
    /// </summary>
    /// <returns>Nome do projeto</returns>
    string GetTFSActiveProjectName();

    /// <summary>
    /// Recuperar o nome do workspace/serverItem para um localItem
    /// </summary>
    /// <param name="projectFullPath">Caminho completo do projeto</param>
    /// <returns>workspace/serverItem</returns>
    (bool OK, string WsName, string ServerItem) GetWorkspaceForLocalItem(string projectFullPath);
  }
}
