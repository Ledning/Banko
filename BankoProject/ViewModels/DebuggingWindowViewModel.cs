using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using BankoProject.Models;
using BankoProject.Tools;
using BankoProject.Tools.Events;
using BankoProject.ViewModels.ConfirmationBoxes;
using BankoProject.Views.ConfirmationBoxes;
using Caliburn.Micro;
using MahApps.Metro.Controls;

namespace BankoProject.ViewModels
{
  class DebuggingWindowViewModel : Screen, IHandle<CommunicationObject>
  {
    #region Fields

    private IEventAggregator _eventAggregator;
    private IWindowManager _windowManager;
    private WinSettings _winSngs;
    private BingoEvent _event;

    #endregion

    #region Constructors

    public DebuggingWindowViewModel(int width, int height, int left, int top)
    {
      WinSngs = new WinSettings();
      WinSngs.Width = width;
      WinSngs.Height = height;
      WinSngs.Left = left;
      WinSngs.Top = top;
      WinSngs.CurrentWindow = ApplicationWideEnums.MessageTypes.ChngWelcomeView.ToString();
      DisplayName = "DebuggingWindow";
    }

    #endregion

    #region Properties

    public WinSettings WinSngs
    {
      get { return _winSngs; }
      set
      {
        _winSngs = value;
        NotifyOfPropertyChange(() => WinSngs);
      }
    }

    public BingoEvent Event
    {
      get { return _event; }
      set
      {
        _event = value;
        NotifyOfPropertyChange(() => Event);
      }
    }

    #endregion

    #region Overrides of ViewAware

    protected override void OnViewReady(object view)
    {
      _eventAggregator = IoC.Get<IEventAggregator>();
      _eventAggregator.Subscribe(this);
      _windowManager = IoC.Get<IWindowManager>();
      Event = IoC.Get<BingoEvent>();
      NotifyOfPropertyChange(() => CanGeneratePlatesButton);
    }

    #endregion

    #region Methods

    public void RebootPrezScreen()
    {
      CommunicationObject rbps = new CommunicationObject(ApplicationWideEnums.MessageTypes.RbPrezScreen,
        ApplicationWideEnums.SenderTypes.DebuggingView);
      _eventAggregator.PublishOnUIThread(rbps);
    }

    public void CheckGenerate()
    {
      if (Event != null)
      {
        if (Event.SInfo != null)
        {
          NotifyOfPropertyChange(() => CanGeneratePlatesButton);
        }
      }
    }

    public void GeneratePlatesButton()
    {
      CommunicationObject gpbtn = new CommunicationObject(ApplicationWideEnums.MessageTypes.GeneratePlates,
        ApplicationWideEnums.SenderTypes.DebuggingView);
      _eventAggregator.PublishOnUIThread(gpbtn);
    }

    public bool CanGeneratePlatesButton
    {
      get
      {
        if (Event != null)
        {
          if (Event.SInfo != null)
          {
            return !string.IsNullOrEmpty(Event.SInfo.Seed);
          }
        }
        return false;
      }
    }

    public void showPresBackground()
    {
      if (Event.WindowSettings.PrsSettings.BackgroundBrush == null)
      {
        Event.WindowSettings.PrsSettings.BackgroundBrush = new SolidColorBrush(Colors.Black);
      }
      else if (Event.WindowSettings.PrsSettings.BackgroundBrush.Color == Colors.Black)
      {
        Event.WindowSettings.PrsSettings.BackgroundBrush = null;
      }
      else
      {
        Event.WindowSettings.PrsSettings.BackgroundBrush = new SolidColorBrush(Colors.Black);
      }
      //this is unused
    }

    public void ShowWelcome()
    {
      CommunicationObject chwe = new CommunicationObject(ApplicationWideEnums.MessageTypes.ChngWelcomeView,
        ApplicationWideEnums.SenderTypes.DebuggingView);
      _eventAggregator.PublishOnUIThread(chwe);
    }

    public void DialogTester()
    {
      //Jeg er ikke 100 på hvorfor this expandoshit couldnt be done 
      var dialog = new ChangePlatesUsedDialogViewModel();
      //dynamic settings = new ExpandoObject();
      //settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
      //settings.ResizeMode = ResizeMode.NoResize;
      //settings.Width = 200;
      //settings.Height = 120;
      var result = _windowManager.ShowDialog(dialog /*, null, settings*/);
      if (result == true)
      {
      }
    }

    public void exitProgram()
    {
      Environment.Exit(1);
    }

    public void ShowControl()
    {
      CommunicationObject chwe = new CommunicationObject(ApplicationWideEnums.MessageTypes.ChngControlPanelView,
        ApplicationWideEnums.SenderTypes.DebuggingView);
      _eventAggregator.PublishOnUIThread(chwe);
    }

    #endregion

    #region Implementation of IHandle<CommunicationObject>

    public void Handle(CommunicationObject message)
    {
      WinSngs.CurrentWindow = message.Message.ToString();
    }

    #endregion
  }
}