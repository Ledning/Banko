using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using BankoProject.Models;
using BankoProject.Tools;
using BankoProject.Tools.Events;
using BankoProject.ViewModels.ConfirmationBoxes;
using Caliburn.Micro;
using ScrnHelper = WpfScreenHelper.Screen;

namespace BankoProject.ViewModels.Flyout
{
  class ControlPanelFlyoutViewModel : Screen, IFlyoutItem
  {
    #region Fields

    private readonly ILog _log = LogManager.GetLog(typeof(MainWindowViewModel));
    private BingoEvent _bingoEvent;
    private bool _isOpen = false;
    private string _startStopText = "Stop BingoBanko";
    private IEventAggregator _events;
    private IWindowManager _windowManager;
    private int _selectedScreen;

    #endregion

    //IMPORTANT
    //For once in this apps lifetime, these things have to be loaded in here. since this is a flyout, OnViewReady is not called/not called at the appropriate time, so that does not work.
    //On the other hand, this one is 100% called before the flyout is shown, so this works for these. (not for anything else afaik.)

    #region Constructor

    public ControlPanelFlyoutViewModel()
    {
      Event = IoC.Get<BingoEvent>();
      _events = IoC.Get<IEventAggregator>();
      _windowManager = IoC.Get<IWindowManager>();
      DisplayName = "";
      _log.Info("ControlFlyoutTriggered");
      var dueTime = TimeSpan.FromSeconds(5);
      var interval = TimeSpan.FromSeconds(5);
      RunPeriodicAsync(UpdateScreensAvailable, dueTime, interval, CancellationToken.None);
      SelectedScreen = Event.WindowSettings.ChoosenPresentationScreen;
    }

    #endregion

    #region props

    public BingoEvent Event
    {
      get { return _bingoEvent; }
      set
      {
        _bingoEvent = value;
        NotifyOfPropertyChange(() => Event);
      }
    }

    public string StartStopText
    {
      get { return _startStopText; }
      set
      {
        _startStopText = value;
        NotifyOfPropertyChange(() => StartStopText);
      }
    }

    public bool IsOpen
    {
      get { return _isOpen; }
      set
      {
        _isOpen = value;
        NotifyOfPropertyChange(() => IsOpen);
      }
    }

    #endregion

    #region Overrides of ViewAware

    protected override void OnViewReady(object view)
    {
    }

    #endregion

    #region Async stuff

    private void UpdateScreensAvailable()
    {
      List<WpfScreenHelper.Screen> result = ScrnHelper.AllScreens.ToList();
      BindableCollection<WpfScreenHelper.Screen> bindableresult = new BindableCollection<ScrnHelper>();
      foreach (var screen in result)
      {
        bindableresult.Add(screen);
      }
      Event.WindowSettings.Screens = bindableresult;
    }

    private static async Task RunPeriodicAsync(System.Action onTick,
      TimeSpan dueTime,
      TimeSpan interval,
      CancellationToken token)
    {
      // Initial wait time before we begin the periodic loop.
      if (dueTime > TimeSpan.Zero)
        await Task.Delay(dueTime, token);

      // Repeat this loop until cancelled.
      while (!token.IsCancellationRequested)
      {
        // Call our onTick function.
        onTick?.Invoke();

        // Wait to repeat again.
        if (interval > TimeSpan.Zero)
          await Task.Delay(interval, token);
      }
    }

    #endregion

    #region Methods

    public void ToggleBingo()
    {
      if (Event != null)
      {
        if (Event.IsBingoRunning)
        {
          Event.IsBingoRunning = false;
          StartStopText = "Start BingoBanko";
        }
        else if (!Event.IsBingoRunning)
        {
          Event.IsBingoRunning = true;
          StartStopText = "Stop BingoBanko";
        }
        else
        {
          NotifyOfPropertyChange(() => StartStopText);
        }
      }
    }

    public void ChangePlatesUsed()
    {
      var dialog = new ChangePlatesUsedDialogViewModel();

      var result = _windowManager.ShowDialog(dialog);
      if (result == true)
      {
        Event.PInfo.PlatesUsed = dialog.AntalPlader;
      }
    }

    public void GeneratePlates()
    {
      _events.PublishOnBackgroundThread(new CommunicationObject(ApplicationWideEnums.MessageTypes.GeneratePlates,
        ApplicationWideEnums.SenderTypes.ControlPanelView));
    }


    public bool CanGeneratePlates()
    {
      if (
        File.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\BingoBankoKontrol\\" +
                    Event.EventTitle + "Plader" + ".pdf"))
      {
        return false;
      }
      return true;
    }

    public List<string> ScreenConverter
    {
      get
      {
        List<string> result = new List<string>();

        foreach (var screenItem in ScrnHelper.AllScreens)
        {
          if (screenItem.Primary)
          {
            result.Add("PRIMARY");
          }
          else
          {
            result.Add("SECONDARY" + screenItem.DeviceName);
          }
        }
        return result;
      }
    }

    public int SelectedScreen
    {
      get { return _selectedScreen; }
      set
      {
        _selectedScreen = value;
        NotifyOfPropertyChange(() => SelectedScreen);
      }
    }


    public void ChangeBigScreen()
    {
      Event.WindowSettings.PrsSettings.State = WindowState.Normal;
      Event.WindowSettings.ChoosenPresentationScreen = SelectedScreen;
      Event.WindowSettings.PrsSettings.Width = (int) Event.WindowSettings
        .Screens[Event.WindowSettings.ChoosenPresentationScreen].WorkingArea.Width;
      Event.WindowSettings.PrsSettings.Height = (int) Event.WindowSettings
        .Screens[Event.WindowSettings.ChoosenPresentationScreen].WorkingArea.Height;
      Event.WindowSettings.PrsSettings.Left = (int) Event.WindowSettings
        .Screens[Event.WindowSettings.ChoosenPresentationScreen].WorkingArea.Left;
      Event.WindowSettings.PrsSettings.Top = (int) Event.WindowSettings
        .Screens[Event.WindowSettings.ChoosenPresentationScreen].WorkingArea.Top;
      Event.WindowSettings.PrsSettings.State = WindowState.Maximized;
    }

    public void BgColor()
    {
      Event.WindowSettings.ToggleBG();
    }

    #endregion
  }
}