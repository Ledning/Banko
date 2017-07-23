using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using BankoProject.Tools;
using Caliburn.Micro;
using ScrnHelper = WpfScreenHelper.Screen;

namespace BankoProject.Models
{
  /// <summary>
  /// Settings / info for the control window. DOES not include settings related to the presentation window, except in the capacity that it shows what screen is chosen to keep the presentation on.
  /// What it does do is populate the list of screens that are available, and calculate basic info about them. 
  /// This is a one-off class. Anything related to a particular screen should be in PresentationScreenSettings. 
  /// </summary>
  /// 
  [Serializable]
  public class WinSettings : PropertyChangedBase
  {
    //These again refer to the controlpanelWindow, not the presentationwindow. 
    private int _width;

    private int _height;
    private int _left;
    private int _top;
    private string currentWindow;
    private int _choosenPresentationScreen;
    private string _bgColor;
    private string _bgColorText;

    //This object holds reference to the current presentation screen. 
    private PresentationScreenSettings _prsSettings;

    /// <summary>
    /// List of all the available screens.
    /// </summary>
    [XmlIgnore] private BindableCollection<ScrnHelper> _screens;


    public WinSettings()
    {
      Screens = new BindableCollection<ScrnHelper>(ScrnHelper.AllScreens);
      PrsSettings = new PresentationScreenSettings();
      PrsSettings.State = WindowState.Normal;
      ChoosenPresentationScreen = 0;

      ToggleBG();
    }

    public void ToggleBG()
    {
      if (BgColor == null)
      {
        //BgColor = "#ffffff";
        BgColor = "#FF5AFF27";
        BgColorText = "Klik for at skifte \n til gennemsigtig.";
      }

      else
      {
        BgColor = null;
        BgColorText = "Klik for at skifte til hvid";
      }

      //sæt baggrunden til at være = BgColor her :)))))
    }


    #region props

    public int Width
    {
      get { return _width; }
      set
      {
        _width = value;
        NotifyOfPropertyChange(() => Width);
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

    [XmlIgnore]
    [_screens: NonSerialized]
    public BindableCollection<ScrnHelper> Screens
    {
      get { return _screens; }
      set
      {
        _screens = value;
        NotifyOfPropertyChange(() => Screens);
      }
    }

    public PresentationScreenSettings PrsSettings
    {
      get { return _prsSettings; }
      set
      {
        _prsSettings = value;
        NotifyOfPropertyChange(() => PrsSettings);
      }
    }

    public string CurrentWindow
    {
      get { return currentWindow; }
      set
      {
        currentWindow = value;
        NotifyOfPropertyChange(() => CurrentWindow);
      }
    }

    public int ChoosenPresentationScreen
    {
      get { return _choosenPresentationScreen; }
      set
      {
        _choosenPresentationScreen = value;
        NotifyOfPropertyChange(() => ChoosenPresentationScreen);
      }
    }

    public string BgColor
    {
      get { return _bgColor; }
      set
      {
        _bgColor = value;
        NotifyOfPropertyChange(() => BgColor);
      }
    }

    public string BgColorText
    {
      get { return _bgColorText; }
      set
      {
        _bgColorText = value;
        NotifyOfPropertyChange(() => BgColorText);
      }
    }
  }

  #endregion
}