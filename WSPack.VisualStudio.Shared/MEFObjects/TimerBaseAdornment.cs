using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace WSPack.VisualStudio.Shared.MEFObjects
{
  abstract class TimerBaseAdornment : BaseAdornment
  {
    /// <summary>
    /// Tempo, em milisegundos, do timer
    /// </summary>
    readonly int _timerMiliSecondsInterval;

#pragma warning disable IDE1006
    protected DispatcherTimer _timer;
#pragma warning restore IDE1006
    readonly bool _timerAlterouLayout;
    readonly bool _timerAlterouTexto;
    readonly bool _timerAlterouPosicao;

    /// <summary>
    /// Inicialização da classe <see cref="TimerBaseAdornment"/>
    /// </summary>
    public TimerBaseAdornment(ITextView view, int timerMiliSecondsInterval = 3000,
      bool timerAlterouTexto = true, bool timerAlterouLayout = true, bool timerAlterouPosicao = true) : base(view)
    {
      _timerMiliSecondsInterval = timerMiliSecondsInterval;
      _timerAlterouTexto = timerAlterouTexto;
      _timerAlterouLayout = timerAlterouLayout;
      _timerAlterouPosicao = timerAlterouPosicao;
      CriarTimer();
      view.Closed += FechouView;
    }

    private void FechouView(object sender, EventArgs e)
    {
      _timer?.Stop();
    }

    /// <summary>
    /// Operação a ser realizada no evento do timer
    /// </summary>
    /// <param name="textSnapshot">Text snapshot</param>
    protected abstract void OnTimer(ITextSnapshot textSnapshot);

    /// <summary>
    /// Acontece quando o timer está sendo iniciado
    /// </summary>
    protected abstract void OnStartTimer();

    /// <summary>
    /// Acontece quando o texto do editor é alterado
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
    protected override void AlterouTexto(object sender, TextContentChangedEventArgs eventArgs)
    {
      if (_timerAlterouTexto)
        CheckTimer();
    }

    private void CheckTimer()
    {
      if (_view == null || _view.IsClosed)
        return;
      if (_timer == null)
        CriarTimer();

      _timer.Stop();
      _timer.Start();
      OnStartTimer();
    }

    protected override void AlterouLayout(object sender, TextViewLayoutChangedEventArgs e)
    {
      if (e.NewSnapshot != e.OldSnapshot)
      {
        if (_timerAlterouLayout)
          CheckTimer();
      }
    }

    protected override void AlterouPosicao(object sender, CaretPositionChangedEventArgs e)
    {
      if (_timerAlterouPosicao)
        CheckTimer();
    }

    private void CriarTimer()
    {
      _timer = new DispatcherTimer(DispatcherPriority.Background)
      {
        Interval = TimeSpan.FromMilliseconds(_timerMiliSecondsInterval)
      };
      _timer.Tick += _timer_Tick;
    }

    private void _timer_Tick(object sender, EventArgs e)
    {
      _timer.Stop();
      OnTimer(_view.TextSnapshot);
    }
  }
}