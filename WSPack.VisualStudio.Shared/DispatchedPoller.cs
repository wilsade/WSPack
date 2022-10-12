using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;

namespace WSPack.VisualStudio.Shared
{
  /// <summary>
  /// DispatchedPoller
  /// </summary>
  public class DispatchedPoller
  {
    private readonly Action _onGiveUp;
    static ProcessForm _form = null;

    /// <summary>
    /// Inicialização da classe: <see cref="DispatchedPoller"/>.
    /// </summary>
    /// <param name="maximumNumberOfAttempts">Maximum number of attempts</param>
    /// <param name="frequency">Frequency</param>
    /// <param name="condition">Condition</param>
    /// <param name="toDo">To do</param>
    /// <param name="onGiveUp">Acontece quando a condição nunca foi satisfeita</param>
    public DispatchedPoller(int maximumNumberOfAttempts, TimeSpan frequency,
      Func<bool> condition, Action toDo, Action onGiveUp)
    {
      MaximumNumberOfAttempts = maximumNumberOfAttempts;
      Condition = condition;
      ToDo = toDo;
      _onGiveUp = onGiveUp;
      Frequency = frequency;
    }

    static void CreateAndShowForm()
    {
      if (_form != null)
        return;
      _form = new ProcessForm()
      {
        TopMost = true
      };
      _form.Self.Titulo = "Selecionando o item no Source Control Explorer";
      _form.Self.Descricao = "Por favor, aguarde...";
      _form.Self.PodeSair = true;
      _form.FormClosed += (x, y) =>
      {
        _form.Dispose();
        _form = null;
      };
      _form.Show();
    }

    static void CloseForm()
    {
      if (_form == null)
        return;
      _form.Self.Close();
    }

    /// <summary>
    /// Maximum number of attempts
    /// </summary>
    public int MaximumNumberOfAttempts { get; protected set; }

    /// <summary>
    /// Condition
    /// </summary>
    public Func<bool> Condition { get; protected set; }

    /// <summary>
    /// To do
    /// </summary>
    public Action ToDo { get; protected set; }

    /// <summary>
    /// Frequency
    /// </summary>
    public TimeSpan Frequency { get; protected set; }

    /// <summary>
    /// Go
    /// </summary>
    public void Go(bool canCloseForm)
    {
      try
      {
        Loop(canCloseForm);
      }
      catch (Exception ex)
      {
        Utils.LogDebugError(ex.GetCompleteMessage());
        CloseForm();
      }
    }

    /// <summary>
    /// Loop
    /// </summary>
    protected void Loop(bool canCloseForm)
    {
      if (Condition())
      {
        CloseForm();
        ToDo();
      }
      else
      {
        int attemptsMade = 0;
        var timer = new DispatcherTimer()
        {
          Interval = Frequency,
          Tag = 0
        };
        timer.Tick += (sender, args) =>
        {
          if (attemptsMade == MaximumNumberOfAttempts)
          {
            if (canCloseForm)
              CloseForm();

            // Give up, we've tried enough times, no point in continuing
            timer.Stop();
            _onGiveUp?.Invoke();
          }
          else
          {
            if (Condition())
            {
              CloseForm();
              timer.Stop();
              ToDo();
            }
            else
            {
              CreateAndShowForm();
              // Keep the timer going and try again a few more times
              attemptsMade++;
            }
          }
        };
        timer.Start();
      }
    }
  }
}
