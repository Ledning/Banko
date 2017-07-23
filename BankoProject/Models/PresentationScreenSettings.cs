using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;
using BankoProject.Tools;
using BankoProject.ViewModels.PresentationScreen;
using Caliburn.Micro;

namespace BankoProject.Models
{
  //TODO: dw i did it yay
  //TODO: add buttons to controlpanelflyout to control overlay


  /// <summary>
  /// Represents settings for the presentation screen, where is it located, what size/resolution and so on. This class does not know which screen it is, just a central place for info about and actions relating to the presentation screen. 
  /// If the viewmodel itself has need to do anything with the functions here, maybe wrap them inside simple functions. 
  /// </summary>
  [Serializable]
  public class PresentationScreenSettings : PropertyChangedBase
  {
    private string _colorScheme;
    private int _width;
    private int _height;
    private int _left;
    private int _top;
    private int _selectedPresScreen;
    private SolidColorBrush backgroundBrush;
    private WindowState _state;
    private FullscreenOverlaySettings _fullscreenOverlaySettings;
    private bool _closeWindow = false;
    private bool _isOverLayOpen = false;

    [XmlIgnore] private readonly ILog _log = LogManager.GetLog(typeof(BingoEvent));
    private Dock _dockingPlace;

    #region Props

    public string colorScheme
    {
      get { return _colorScheme; }
      set
      {
        _colorScheme = value;
        NotifyOfPropertyChange(() => colorScheme);
      }
    }

    public int Width
    {
      get { return _width; }
      set
      {
        _width = value;
        NotifyOfPropertyChange(() => Width);
        NotifyOfPropertyChange(() => TimerWidth);
      }
    }

    public int Height
    {
      get { return _height; }
      set
      {
        _height = value;
        NotifyOfPropertyChange(() => Height);
      }
    }

    public int Left
    {
      get { return _left; }
      set
      {
        _left = value;
        NotifyOfPropertyChange(() => Left);
      }
    }

    public int Top
    {
      get { return _top; }
      set
      {
        _top = value;
        NotifyOfPropertyChange(() => Top);
      }
    }


    public PresentationScreenSettings()
    {
      OverlaySettings = new FullscreenOverlaySettings();
    }

    public int TimerWidth
    {
      get { return (int) (Width * 0.25); }
    }

    public int SelectedPresScreen
    {
      get { return _selectedPresScreen; }

      set
      {
        _selectedPresScreen = value;
        NotifyOfPropertyChange(() => SelectedPresScreen);
      }
    }


    public SolidColorBrush BackgroundBrush
    {
      get { return backgroundBrush; }
      set
      {
        backgroundBrush = value;
        NotifyOfPropertyChange(() => BackgroundBrush);
      }
    }

    public WindowState State
    {
      get { return _state; }
      set
      {
        _state = value;
        NotifyOfPropertyChange(() => State);
      }
    }


    public FullscreenOverlaySettings OverlaySettings
    {
      get { return _fullscreenOverlaySettings; }
      set
      {
        _fullscreenOverlaySettings = value;
        NotifyOfPropertyChange(() => OverlaySettings);
      }
    }

    public bool CloseWindow
    {
      get { return _closeWindow; }
      set
      {
        _closeWindow = value;
        NotifyOfPropertyChange(() => CloseWindow);
      }
    }

    public bool IsOverLayOpen
    {
      get { return _isOverLayOpen; }
      set
      {
        _isOverLayOpen = value;
        NotifyOfPropertyChange(() => IsOverLayOpen);
      }
    }

    public Dock DockingPlace
    {
      get { return _dockingPlace; }
      set
      {
        _dockingPlace = value;
        NotifyOfPropertyChange(() => DockingPlace);
      }
    }

    #endregion
  }
}