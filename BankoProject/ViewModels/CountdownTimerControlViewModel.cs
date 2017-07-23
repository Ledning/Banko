using BankoProject.Models;
using BankoProject.Tools;
using Caliburn.Micro;
using Catel.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BankoProject.ViewModels
{
  public class CountdownTimerControlViewModel : Screen, IMainViewItem
  {
    #region Fields

    public CountdowntimerBigScreenViewModel CTBSVM { get; set; }
    private WindowManager _winMan;

    private bool _countUp; //this property decides if the timer counts up or down
    private int _countDownInput; //should prolly enter seconds
    private BindableCollection<Team> _allTeams;
    private Stopwatch _stopWatch;
    private DispatcherTimer _timer;
    private string _currentTime;

    //dispatcherTimer_Tick
    private TimeSpan _currentTimeSpan;

    private TimeSpan _localTimeSpan;
    private TimeSpan _emptyTimeSpan;

    #endregion

    #region Constructors

    public CountdownTimerControlViewModel(BindableCollection<Team> allTeams, int seconds)
    {
      CTBSVM = new CountdowntimerBigScreenViewModel();
      this.AllTeams = new BindableCollection<Team>(allTeams);
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

    public Team SelectedTeam { get; set; }

    public BindableCollection<Team> AllTeams
    {
      get { return _allTeams; }
      set
      {
        _allTeams = value;
        NotifyOfPropertyChange(() => _allTeams);
      }
    }

    public string CurrentTime
    {
      get { return _currentTime; }
      set
      {
        _currentTime = value;
        NotifyOfPropertyChange(() => CurrentTime);
      }
    }

    #endregion

    #region Methods

    private void dispatcherTimer_Tick(object sender, EventArgs e)
    {
      if (_countUp)
      {
        this.CurrentTime = FormatString(_stopWatch.Elapsed);
      }

      else
      {
        this._localTimeSpan = this._currentTimeSpan - this._stopWatch.Elapsed;
        this.CurrentTime = FormatString(this._localTimeSpan);

        if (this._localTimeSpan < this._emptyTimeSpan)
        {
          this.TimerStop();
          this.TimerReset();
        }
      }
    }

    private string FormatString(TimeSpan timespan)
    {
      string currentTime = String.Format("{0:00}:{1:00}:{2:00}",
        timespan.Minutes, timespan.Seconds, timespan.Milliseconds / 10);
      return currentTime;
    }

    public void RemoveSelectedTeamFromCompetition()
    {
      if (this.SelectedTeam != null)
      {
        this.SelectedTeam.IsTeamActive = false;
      }
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

    #region TimerMethods

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

    #endregion
  }
}