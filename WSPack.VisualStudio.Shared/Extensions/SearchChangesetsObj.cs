using Microsoft.TeamFoundation.VersionControl.Client;
using System.Collections.Generic;
using System;
using WSPack.Lib.Extensions;
using WSPack.Lib.Items;
using WSPack.Lib;
using System.Linq;

namespace WSPack.VisualStudio.Shared.Extensions
{
  /// <summary>
  /// Classe auxiliar para buscar Changesets
  /// </summary>
  public static class SearchChangesetsObj
  {
    private struct TSearchChangesetsParams
    {
      public VersionSpec SpecFrom;
      public VersionSpec SpecTo;
      public string User;
      public string Comentario;
      public int NumMaxRegistros;
      public bool IncludeChanges;
    }

    static TSearchChangesetsParams GetSearchChangesetsParams(DateTime? dataInicio, DateTime? dataTermino,
      string user, string comentario, int numMaxRegistros, bool includeChanges, List<string> lstArquivos)
    {
      var instance = new TSearchChangesetsParams()
      {
        SpecFrom = dataInicio.HasValue ? new DateVersionSpec(dataInicio.Value) : null,
        SpecTo = dataTermino.HasValue ? new DateVersionSpec(dataTermino.Value) : null,
        User = string.IsNullOrEmpty(user) ? null : user,
        Comentario = comentario ?? string.Empty,
        NumMaxRegistros = numMaxRegistros == 0 ? int.MaxValue : numMaxRegistros,

        // Se vai filtrar arquivos, precisamos incluir os changes
        IncludeChanges = includeChanges || (lstArquivos != null && lstArquivos.Count > 0)
      };
      return instance;
    }

    /// <summary>
    /// Buscar Changesets (sempre a versão mais atual)
    /// </summary>
    /// <param name="vcServer">VersionControlServer</param>
    /// <param name="serverPath">Caminho do servidor. Pode ser uma pasta ou item específico</param>
    /// <param name="includeChanges">Incluir ou não os arquivos do Check In</param>
    /// <param name="numMaxRegistros">Nº máximo de registros a serem retornados</param>
    /// <param name="user">Mostrar Changesets apenas deste usuário</param>
    /// <param name="comentario">Filtrar por este comentário</param>
    /// <param name="dataInicio">Filtrar nesta data de início</param>
    /// <param name="dataTermino">Filtrar nesta data de término</param>
    /// <param name="lstFiltroNotas">Apenas Changesets com estas notas serão listados</param>
    /// <param name="lstArquivos">Filtrar arquivos específicos</param>
    /// <returns>Lista de changeSet</returns>
    public static List<Changeset> SearchChangesets(this VersionControlServer vcServer, string serverPath, bool includeChanges,
      int numMaxRegistros, string user, string comentario, DateTime? dataInicio, DateTime? dataTermino,
      List<CheckInNotesItem> lstFiltroNotas, List<string> lstArquivos)
    {
      TSearchChangesetsParams searchParams = GetSearchChangesetsParams(dataInicio, dataTermino, user,
        comentario, numMaxRegistros, includeChanges, lstArquivos);

      // Faz a procura no TFS
      List<Changeset> lstQueryHistory = vcServer.QueryHistory(
        serverPath,
        VersionSpec.Latest,
        deletionId: 0,
        RecursionType.Full,
        searchParams.User,
        searchParams.SpecFrom,
        searchParams.SpecTo,
        searchParams.NumMaxRegistros,
        includeChanges: false,
        slotMode: false
        ).Cast<Changeset>().Where(x => x.Comment.ContainsInsensitive(searchParams.Comentario)).ToList();
      if (searchParams.IncludeChanges)
      {
        foreach (var esteCS in lstQueryHistory)
        {
          esteCS.Changes = ChangeSetDetails(vcServer, esteCS.ChangesetId).Changes;
        }
      }

      List<Changeset> lstRetorno = FiltrarArquivos(lstArquivos, lstQueryHistory);
      FiltrarCheckInNotes(lstFiltroNotas, lstQueryHistory, lstRetorno);

      return lstRetorno;
    }

    private static void FiltrarCheckInNotes(List<CheckInNotesItem> lstFiltroNotas, List<Changeset> lstQueryHistory, List<Changeset> lstRetorno)
    {
      if (lstFiltroNotas == null)
        return;

      bool func_FiltrouNota(string valorNota, OperatorTypes operador, string valorFiltro)
      {
        switch (operador)
        {
          case OperatorTypes.Equals:
            return string.Equals(valorNota, valorFiltro);
          case OperatorTypes.NotEquals:
            return !string.Equals(valorNota, valorFiltro);
          case OperatorTypes.StartsWith:
            return valorNota.ToLower().StartsWith(valorFiltro.ToLower());
          case OperatorTypes.Like:
            return valorNota.ContainsInsensitive(valorFiltro);
          case OperatorTypes.NotLike:
            return !valorNota.ContainsInsensitive(valorFiltro);
          case OperatorTypes.IsNull:
            return string.IsNullOrEmpty(valorNota);
          case OperatorTypes.IsNotNull:
            return !string.IsNullOrEmpty(valorNota);
          default:
            return false;
        }
      }

      foreach (CheckInNotesItem esteFiltroNota in lstFiltroNotas)
      {
        foreach (Changeset esteChange in lstQueryHistory)
        {
          bool achou = false;
          for (int i = 0; i < esteChange.CheckinNote.Values.Length; i++)
          {
            if (esteChange.CheckinNote.Values[i].Name == esteFiltroNota.CheckInNoteName &&
              func_FiltrouNota(esteChange.CheckinNote.Values[i].Value, esteFiltroNota.Operador, esteFiltroNota.CheckInNoteValue))
            {
              achou = true;
              break;
            }
          }
          if (!achou)
            lstRetorno.Remove(esteChange);
        }
      }

    }

    private static List<Changeset> FiltrarArquivos(List<string> lstArquivos, List<Changeset> lstQueryHistory)
    {
      var lstRetorno = new List<Changeset>();

      // Filtrar aquivos
      if (lstArquivos != null && lstArquivos.Count > 0)
      {
        bool gotoNextChangeset;
        foreach (Changeset esteItem in lstQueryHistory)
        {
          gotoNextChangeset = false;
          foreach (Change esteChange in esteItem.Changes)
          {
            foreach (string esteArquivo in lstArquivos)
            {
              if (esteChange.Item.ServerItem.FileNameOnly().ContainsInsensitive(esteArquivo))
              {
                lstRetorno.Add(esteItem);
                gotoNextChangeset = true;
                break;
              }
            }
            if (gotoNextChangeset)
              break;
          }
        }
      }
      else
        lstRetorno.AddRange(lstQueryHistory);
      return lstRetorno;
    }

    /// <summary>
    /// Recuperar os detalhes de um ChangeSet
    /// </summary>
    /// <param name="vcServer">VersionControlServer</param>
    /// <param name="changeSetId">ChangeSetId</param>
    /// <returns>Lista de changeSet</returns>
    /// <param name="includeChanges">true para incluir os arquivos alterados</param>
    public static Changeset ChangeSetDetails(this VersionControlServer vcServer, int changeSetId, bool includeChanges = true)
    {
      int deletionId = 0;
      RecursionType tipoRecursao = RecursionType.Full;
      string serverItem = "$/";

      // Para pesquisar por qualquer usuário, temos que passar null
      string user = null;

      Changeset cs = vcServer.QueryHistory(
        serverItem,
        VersionSpec.Latest, deletionId, tipoRecursao, user,
        VersionSpec.ParseSingleSpec(changeSetId.ToString(), vcServer.AuthorizedUser),
        VersionSpec.ParseSingleSpec(changeSetId.ToString(), vcServer.AuthorizedUser),
        int.MaxValue, includeChanges, false).Cast<Changeset>().FirstOrDefault();
      return cs;
    }
  }
}