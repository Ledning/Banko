using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using BankoProject.Models;
using BankoProject.Tools;
using BankoProject.Tools.Events;
using Caliburn.Micro;

namespace BankoProject.ViewModels.PresentationScreen
{
  class FullscreenImageViewModel : Screen, IPresentationScreenItem, IHandle<CommunicationObject>
  {
    #region Fields

    private BingoEvent _event;
    private bool isBlank;

    #endregion

    #region Constructor

    //Der burde ikke være problemer med at køre IOC her, da den bliver automatisk kaldt ved instantation af viewet
    public FullscreenImageViewModel()
    {
      Event = IoC.Get<BingoEvent>();
      ShowStandard();
      SaveDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\BingoBankoKontrol" +
                      "\\Backgrounds";
    }

    public FullscreenImageViewModel(bool blank)
    {
      isBlank = blank;
      Event = IoC.Get<BingoEvent>();
      Event.WindowSettings.PrsSettings.OverlaySettings.IsOverlayVisible = Visibility.Hidden;
      SaveDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\BingoBankoKontrol" +
                      "\\Backgrounds";
    }

    #endregion

    #region Overrides of ViewAware

    protected override void OnViewReady(object view)
    {
      //ShowStandard(); If bugs appear; consider reenabling this
    }

    #endregion

    #region Background stuff

    private const string resourceFolder = "\\BankoProject;component\\Resources\\";
    private readonly string standardOverlay = resourceFolder + "StandardOverlay.jpg";
    private readonly string bingoOverlay = resourceFolder + "BingoBanko2016.jpg";
    private Visibility _isOverlayVisible = Visibility.Visible;
    private string _selectedBackgroundPath; //for whatever is shown on the overlay
    private string _saveDirectory;
    private string _dropdownSelectedBackground;
    private readonly ILog _log = LogManager.GetLog(typeof(MainWindowViewModel));


    public void ShowStandard()
    {
      Event.WindowSettings.PrsSettings.OverlaySettings.IsOverlayVisible = Visibility.Visible;
      _log.Info(standardOverlay);
      Event.WindowSettings.PrsSettings.OverlaySettings.SelectedBackgroundPath = standardOverlay;
    }

    public void ShowCustom()
    {
      Event.WindowSettings.PrsSettings.OverlaySettings.SelectedBackgroundPath =
        SelectedBackgroundPath = Event.WindowSettings.PrsSettings.OverlaySettings.ReturnSelectedPath();
      Event.WindowSettings.PrsSettings.OverlaySettings.IsOverlayVisible = Visibility.Visible;
    }

    public void ShowBingo()
    {
      Event.WindowSettings.PrsSettings.OverlaySettings.IsOverlayVisible = Visibility.Visible;
      _log.Info(bingoOverlay);
      Event.WindowSettings.PrsSettings.OverlaySettings.SelectedBackgroundPath = bingoOverlay;
    }

    #endregion

    #region props

    public string SaveDirectory
    {
      get { return _saveDirectory; }
      set
      {
        _saveDirectory = value;
        NotifyOfPropertyChange(() => SaveDirectory);
      }
    }

    public string DropdownSelectedBackground
    {
      get { return _dropdownSelectedBackground; }
      set
      {
        _dropdownSelectedBackground = value;
        NotifyOfPropertyChange(() => DropdownSelectedBackground);
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

    public string SelectedBackgroundPath
    {
      get { return _selectedBackgroundPath; }
      set
      {
        _selectedBackgroundPath = value;
        NotifyOfPropertyChange(() => SelectedBackgroundPath);
      }
    }

    public bool IsBlank
    {
      get { return isBlank; }
      set
      {
        isBlank = value;
        NotifyOfPropertyChange(() => IsBlank);
      }
    }

    #endregion

    #region Implementation of IHandle<CommunicationObject>

    public void Handle(CommunicationObject message)
    {
      switch (message.Message)
      {
        case ApplicationWideEnums.MessageTypes.ScrnActivationTriggered:
          if (Event.WindowSettings.PrsSettings.OverlaySettings.UserDefinedScreen)
          {
            ShowCustom();
          }
          else
          {
            ShowStandard();
          }
          break;
      }
    }

    #endregion
  }
}