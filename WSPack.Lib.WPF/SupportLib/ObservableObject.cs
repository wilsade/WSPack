using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace WSPack.Lib.WPF.SupportLib
{
  /// <summary>
  /// ObservableObject
  /// </summary>
  /// <seealso cref="INotifyPropertyChanged" />
  [Serializable]
  public abstract class ObservableObject : INotifyPropertyChanged
  {
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    [field: NonSerialized]
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Raises the <see cref="E:PropertyChanged" /> event.
    /// </summary>
    /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      PropertyChanged?.Invoke(this, e);
    }

    /// <summary>
    /// Raises the property changed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyExpresssion">Property expresssion</param>
    protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpresssion)
    {
      var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
      RaisePropertyChanged(propertyName);
    }

    /// <summary>
    /// Raises the property changed.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    protected void RaisePropertyChanged(string propertyName)
    {
      VerifyPropertyName(propertyName);
      OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Warns the developer if this Object does not have a public property with
    /// the specified name. This method does not exist in a Release build.
    /// </summary>
    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public void VerifyPropertyName(string propertyName)
    {
      // verify that the property name matches a real,  
      // public, instance property on this Object.
      if (TypeDescriptor.GetProperties(this)[propertyName] == null)
      {
        Debug.Fail("Invalid property name: " + propertyName);
      }
    }
  }
}
