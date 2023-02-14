using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSPack.Lib.Extensions
{
  /// <summary>
  /// Fornecer métodos estendidos para enumerados
  /// </summary>
  public static class EnumerableExtensions
  {
    /// <summary>
    /// Percorrer o enumerado e executar uma ação
    /// </summary>
    /// <typeparam name="T">Tipo do item</typeparam>
    /// <param name="self">Enumerado</param>
    /// <param name="action">Ação a ser executado no laço</param>
    public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
    {
      if (self != null)
      {
        foreach (var item in self)
        {
          action(item);
        }
      }
    }

    /// <summary>
    /// Ordenar uma ObservableCollection
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <param name="collection">Collection</param>
    /// <param name="keySelector">Funcao</param>
    /// <param name="onSorting">Acontece quando o item está sendo ordenado</param>
    /// <param name="isDescending">Indica se a ordenação será decrescente</param>
    public static void Sort<TSource, TKey>(this ObservableCollection<TSource> collection, Func<TSource, TKey> keySelector,
      Action<TSource, int> onSorting = null, bool isDescending = false)
    {
      List<TSource> sorted = isDescending ?
        collection.OrderByDescending(keySelector).ToList() :
        collection.OrderBy(keySelector).ToList();

      int total = sorted.Count;
      for (int i = 0; i < total; i++)
      {
        onSorting?.Invoke(sorted[i], i);
        collection.Move(collection.IndexOf(sorted[i]), i);
      }
    }

    /// <summary>
    /// Recuperar o segundo item
    /// </summary>
    /// <typeparam name="T">Tipo do item</typeparam>
    /// <param name="self">Lista de itens</param>
    /// <returns>Segundo item; primeiro item se a lista tem apenas um elemento</returns>
    public static T SecondOrFirst<T>(this IEnumerable<T> self)
    {
      var second = self.Skip(1).Take(1).FirstOrDefault();
      if (second == null)
        second = self.FirstOrDefault();
      return second;
    }

    /// <summary>
    /// Recuperar todos os itens, excetuando o primeiro e o último
    /// </summary>
    /// <typeparam name="T">Tipo do item</typeparam>
    /// <param name="self">Lista de itens</param>
    /// <returns>todos os itens, excetuando o primeiro e o último</returns>
    public static IEnumerable<T> BetweenFirstAndLast<T>(this IEnumerable<T> self)
    {
      var lst = new List<T>();
      int total = self.Count();

      if (total <= 2)
        return self;

      for (int i = 1; i < total - 1; i++)
      {
        lst.Add(self.ElementAt(i));
      }

      return lst;
    }

    /// <summary>
    /// Recuperar todos os itens, excetuando o primeiro
    /// </summary>
    /// <typeparam name="T">Tipo do item</typeparam>
    /// <param name="self">Lista de itens</param>
    /// <returns>Segundo item; primeiro item se a lista tem apenas um elemento</returns>
    public static IEnumerable<T> ExceptFirst<T>(this IEnumerable<T> self)
    {
      if (self == null || !self.Any())
        return null;

      var except = self.Skip(1);
      if (!except.Any())
        return self;
      return except;
    }

    /// <summary>
    /// Verifica existência do item e o insere um item na primeira posição
    /// </summary>
    /// <typeparam name="T">Tipo do item</typeparam>
    /// <param name="lista">Lista</param>
    /// <param name="item">Item</param>
    public static void MakeItemFirstInList<T>(this IList<T> lista, T item)
    {
      int posicao = lista.IndexOf(item);
      if (posicao >= 0)
      {
        lista.RemoveAt(posicao);
      }
      lista.Insert(0, item);
    }

    /// <summary>
    /// Recuperar todos os itens, excetuando o último
    /// </summary>
    /// <typeparam name="T">Tipo do item</typeparam>
    /// <param name="self">Lista de itens</param>
    /// <returns>todos os itens, excetuando o primeiro e o último</returns>
    public static IEnumerable<T> ExceptLast<T>(this IEnumerable<T> self)
    {
      var lst = new List<T>();
      int total = self.Count();

      if (total <= 2)
        return self.Take(1);

      for (int i = 0; i < total - 1; i++)
      {
        lst.Add(self.ElementAt(i));
      }

      return lst;
    }
  }
}
