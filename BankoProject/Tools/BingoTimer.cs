using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml.Serialization;
using BankoProject.Models;
using Caliburn.Micro;

namespace BankoProject.Tools
{
  public class BingoTimer : PropertyChangedBase
  {
    private bool _countUp; //this property decides if the timer counts up or down
    private int _countDownInput; //should prolly enter seconds
    private Stopwatch _stopWatch;
    private DispatcherTimer _timer;
    private string _currentTime = "00:00:00";
    private string _localTime = "00:00:00";
    private BingoEvent Event;

    private bool _isTimerStarted = false;

    //dispatcherTimer_Tick
    private TimeSpan _currentTimeSpan;

    private TimeSpan _localTimeSpan;
    private TimeSpan _emptyTimeSpan;
    private TimeSpan _targetTimeSpan;

    #region Constructors

    public BingoTimer(int seconds)
    {
      Event = IoC.Get<BingoEvent>();
      this._currentTimeSpan = new TimeSpan();
      this._localTimeSpan = new TimeSpan();
      this._emptyTimeSpan = new TimeSpan();

      //setting up dispatcher and stopwatch
      this._stopWatch = new Stopwatch();
      this._timer = new DispatcherTimer();
      this._timer.Tick += new EventHandler(dispatcherTimer_Tick);
      this._timer.Interval = new TimeSpan(0, 0, 0, 0, milliseconds: 10);
      this._countUp = true;


      if (seconds > 0)
      {
        this._currentTimeSpan = this._currentTimeSpan.Add(new TimeSpan(0, 0, 0, seconds));
        this.CountDownInput = seconds;
        this._countUp = false;
      }
    }

    #endregion

    #region Properties

    public int CountDownInput
    {
      get { return _countDownInput; }
      set
      {
        _countDownInput = value;
        NotifyOfPropertyChange(() => CountDownInput);
      }
    }

    public string CurrentTime
    {
      get
      {
        if (IsTimerStarted)
        {
          return _currentTime;
        }
        else
        {
          return LocalTime;
        }
      }
      set
      {
        _currentTime = value;
        NotifyOfPropertyChange(() => CurrentTime);
      }
    }

    public bool IsTimerStarted
    {
      get { return _isTimerStarted; }
      set
      {
        _isTimerStarted = value;
        NotifyOfPropertyChange(() => IsTimerStarted);
      }
    }

    public string LocalTime
    {
      get { return _localTime; }
      set
      {
        _localTime = value;
        NotifyOfPropertyChange(() => LocalTime);
      }
    }

    #endregion

    #region Methods

    private void dispatcherTimer_Tick(object sender, EventArgs e)
    {
      if (_countUp)
      {
        _targetTimeSpan = _currentTimeSpan.Add(TimeSpan.Parse("0:" + Event.TimeOpt.TextTime));
        this.CurrentTime = FormatString(_stopWatch.Elapsed);
        this._localTimeSpan = this._currentTimeSpan - this._stopWatch.Elapsed;

        if (this._localTimeSpan.Negate() > this._targetTimeSpan)
        {
          CurrentTime = this.CurrentTime = FormatString(_targetTimeSpan);
          this.TimerStop();
        }
      }

      else
      {
        this._localTimeSpan = this._currentTimeSpan - this._stopWatch.Elapsed;
        this.CurrentTime = FormatString(this._localTimeSpan);

        if (this._localTimeSpan < this._emptyTimeSpan)
        {
          CurrentTime = this.CurrentTime = FormatString(new TimeSpan(0, 0, 0, 0));
          this.TimerStop();
        }
      }
    }

    private string FormatString(TimeSpan timespan)
    {
      string currentTime = String.Format("{0:00}:{1:00}:{2:00}",
        timespan.Minutes, timespan.Seconds, timespan.Milliseconds / 10);
      return currentTime;
    }

    public void InitializeNewCountDownTimer()
    {
      if (this.CountDownInput > 0 && !this._timer.IsEnabled)
      {
        this.TimerReset();
        this.CurrentTime = FormatString(new TimeSpan(0, 0, 0, this.CountDownInput));
        this._currentTimeSpan = new TimeSpan(0, 0, 0, this.CountDownInput);
        this._countUp = false;
        this.CountDownInput = 0;
      }
    }

    #endregion

    public void TimerStart()
    {
      if (!this._timer.IsEnabled)
      {
        this._timer.Start();
        this._stopWatch.Start();
      }
    }

    public void TimerStop()
    {
      if (this._timer.IsEnabled)
      {
        this._timer.Stop();
        this._stopWatch.Stop();
      }
    }

    public void TimerReset()
    {
      if (!this._timer.IsEnabled)
      {
        this.CurrentTime = FormatString(new TimeSpan()); //reset it
        this._stopWatch.Reset();
        this._countUp = true;
      }
    }
  }
}