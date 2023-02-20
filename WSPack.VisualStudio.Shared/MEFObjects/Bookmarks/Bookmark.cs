using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;

namespace WSPack.VisualStudio.Shared.MEFObjects.Bookmarks
{
  /// <summary>
  /// Objeto para armazenar os marcadores
  /// </summary>
  public sealed class Bookmark : IEquatable<Bookmark>, INotifyPropertyChanged
  {
    #region Construtores
    /// <summary>
    /// Inicialização da classe: <see cref="Bookmark"/>.
    /// </summary>
    public Bookmark()
    {

    }

    /// <summary>
    /// Cria uma instância da classe <see cref="Bookmark"/>
    /// </summary>
    /// <param name="name">Nome do marcador</param>
    /// <param name="number">Nº 0 a 9 do marcador</param>
    /// <param name="line">Nº da linha onde o marcador se encontra</param>
    /// <param name="column">Nº da coluna onde o marcador está</param>
    /// <param name="fullName">Nome completo da classe onde o marcador está</param>
    /// <param name="offSet">Posição absoluta de onde o cursor está no texto</param>
    public Bookmark(string name, int number, int line, int column, string fullName, int offSet)
    {
      Name = name;
      Number = number;
      Line = line;
      Column = column;
      FullName = fullName;
      IsEnabled = true;
      Offset = offSet;
    }
    #endregion

    /// <summary>
    /// Nº 0 a 9 do marcador
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Nome do marcador
    /// </summary>
    public string Name
    {
      get { return _name; }
      set
      {
        if (value != _name)
        {
          _name = value;
          OnPropertyChanged(nameof(Name));
        }
      }
    }
    string _name;

    /// <summary>
    /// Nome completo da classe onde o marcador está
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Nº da linha onde o marcador se encontra
    /// </summary>
    public int Line { get; set; }

    /// <summary>
    /// Nº da coluna onde o marcador está
    /// </summary>
    public int Column { get; set; }

    /// <summary>
    /// Um valor indicando se Bookmark está habilitado ou não
    /// </summary>
    public bool IsEnabled
    {
      get { return _isEnabled; }
      set
      {
        if (value != _isEnabled)
        {
          _isEnabled = value;
          OnPropertyChanged(nameof(IsEnabled));
        }
      }
    }
    bool _isEnabled;

    /// <summary>
    /// Posição absoluta de onde o cursor está no texto
    /// </summary>
    public int Offset { get; set; }

    /// <summary>
    /// Retorna uma string representando esta instância
    /// </summary>
    /// <returns>string representando esta instância</returns>
    public override string ToString()
    {
      string nome = string.Empty;
      if (!string.IsNullOrEmpty(Name))
        nome = " " + Name;
      string s = string.Format(ResourcesLib.StrMarcador, nome, Number, Line, Column, FullName);
      return s;
    }

    /// <summary>
    /// Retorna um código hash para esta instância,
    /// adequado para uso em algoritmos hash e estrutura de dados como uma tabela hash.
    /// </summary>
    /// <returns>Código hash para esta instância</returns>
    public override int GetHashCode()
    {
      return Number.GetHashCode();
    }

    /// <summary>
    /// Indica se este objeto é igual a outro
    /// </summary>
    /// <param name="obj">Obj</param>
    /// <returns>true se o objeto é igual a outro</returns>
    public override bool Equals(object obj)
    {
      return Equals(obj as Bookmark);
    }

    #region IEquatable<Bookmark> Members

    /// <summary>
    /// Indica se este objeto é igual a outro
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>true se o objeto é igual a outro</returns>
    public bool Equals(Bookmark other)
    {
      return other != null &&
        Number == other.Number &&
        FullName.EqualsInsensitive(other.FullName) &&
        Line == other.Line;
    }
    #endregion

    void OnPropertyChanged(string name)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    #region INotifyPropertyChanged Members

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
  }
}