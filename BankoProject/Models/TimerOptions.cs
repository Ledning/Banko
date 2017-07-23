using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BankoProject.Tools;
using Caliburn.Micro;

namespace BankoProject.Models
{
  [Serializable]
  public class TimerOptions : PropertyChangedBase, IDataErrorInfo
  {
    private const string START = "Start";
    private const string STOP = "Stop";

    #region Fields

    private TimeSpan _timerTime;
    private string _textTime;
    private string _error;
    private bool _canShow;
    private string _toggleTimerText;

    [XmlIgnore] private BingoTimer _bTimer;
    private bool _countUp;
    private bool _resetTimer;
    private bool Start_StopWatch = true;

    #endregion

    #region Constructors

    public TimerOptions()
    {
      _timerTime = new TimeSpan(0, 0, 0);
      NotifyOfPropertyChange(() => TimerTime);
      TextTime = "05:00";
      CanShow = true;
      ToggleTimerText = START;
      CountUp = false;
      ResetTimer = true;
      BTimer = new BingoTimer(0);
      BTimer.LocalTime = "00:00:00";
    }

    #endregion


    #region Methods

    public void ToggleTimer()
    {
      if (Start_StopWatch)
      {
        StartTimer();
        Start_StopWatch = false;
      }
      else
      {
        StopTimer();
        Start_StopWatch = true;
      }
    }

    public void StartTimer()
    {
      if (ResetTimer)
      {
        if (CountUp)
        {
          BTimer = new BingoTimer(0);
        }
        else
        {
          BTimer = new BingoTimer((int) TimerTime.TotalSeconds);
        }
        BTimer.InitializeNewCountDownTimer();
        BTimer.TimerStart();
        ResetTimer = false;
        BTimer.IsTimerStarted = true;
      }
      else
      {
        BTimer.TimerStart();
        BTimer.IsTimerStarted = true;
      }
      ToggleTimerText = STOP;
    }

    public void StopTimer()
    {
      BTimer.TimerStop();
      BTimer.IsTimerStarted = false;
      ToggleTimerText = START;
    }

    #endregion

    #region Props

    public TimeSpan TimerTime
    {
      //TODO: Kan crashes nemt ved at skrive ulovlige input.
      get { return TimeSpan.Parse("0:" + TextTime); }
    }

    public string TextTime
    {
      get { return _textTime; }
      set
      {
        _textTime = value;
        NotifyOfPropertyChange(() => TextTime);
        if (BTimer != null)
        {
          BTimer.LocalTime = _textTime;
        }
      }
    }

    #endregion

    #region Implementation of IDataErrorInfo

    public string this[string columnName]
    {
      get
      {
        string result = null;
        if (columnName == "TextTime")
        {
          TimeSpan? convertedTimespan = null;
          try
          {
            var temp = DateTime.Parse(TextTime);
            convertedTimespan =
              TimeSpan.Parse("0:" + TextTime); //this thinks its in 24 hr format, while it is actually in mm:ss
          }
          catch (Exception)
          {
            //TODO: RN this throws exceptions whenever anything is wrong 
            CanShow = false;
            result = "Invalid time-format";
          }
          if (convertedTimespan.HasValue)
          {
            if (convertedTimespan <= TimeSpan.Zero)
            {
              result = "The Interval is 0 or less.";
              CanShow = false;
            }
            if (convertedTimespan >= new TimeSpan(1, 0, 0))
            {
              result = "The interval is larger than an hour.";
              CanShow = false;
            }
          }
          if (TextTime.Contains("-"))
          {
            result = "No negative values.";
            CanShow = false;
          }
        }


        if (result == null)
        {
          CanShow = true;
        }
        return result;
      }
    }


    public string Error
    {
      get { return _error; }
    }

    public bool CanShow
    {
      get { return _canShow; }
      set
      {
        _canShow = value;
        NotifyOfPropertyChange(() => CanShow);
      }
    }

    public string ToggleTimerText
    {
      get { return _toggleTimerText; }
      set
      {
        _toggleTimerText = value;
        NotifyOfPropertyChange(() => ToggleTimerText);
      }
    }

    public bool CountUp
    {
      get { return _countUp; }
      set
      {
        _countUp = value;
        NotifyOfPropertyChange(() => CountUp);
      }
    }

    [XmlIgnore]
    public BingoTimer BTimer
    {
      get { return _bTimer; }
      set
      {
        _bTimer = value;
        NotifyOfPropertyChange(() => BTimer);
      }
    }

    public bool ResetTimer
    {
      get { return _resetTimer; }
      set
      {
        _resetTimer = value;
        NotifyOfPropertyChange(() => ResetTimer);
      }
    }

    #endregion
  }
}