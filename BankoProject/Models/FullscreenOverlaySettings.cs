using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using BankoProject.ViewModels;
using Caliburn.Micro;
using Catel.Collections;

namespace BankoProject.Models
{
  public class FullscreenOverlaySettings : PropertyChangedBase
  {
    private bool _userDefinedScreen;
    private Visibility _isOverlayVisible = Visibility.Visible;
    private string _selectedBackgroundPath;
    private bool _stdScrnOL;
    [XmlIgnore] private int _selectedIndex = 0;
    [XmlIgnore] private BindableCollection<string> _customOverlayImages;
    private bool _scrnActivationRequired;
    [XmlIgnore] private const string resourceFolder = "\\BankoProject;component\\Resources\\";
    [XmlIgnore] private readonly string standardOverlay = resourceFolder + "StandardOverlay.jpg";
    [XmlIgnore] private readonly ILog _log = LogManager.GetLog(typeof(MainWindowViewModel));


    public FullscreenOverlaySettings()
    {
      var dueTime = TimeSpan.FromSeconds(10);
      var interval = TimeSpan.FromSeconds(10);
      RunPeriodicAsync(UpdateBackgrounds, dueTime, interval, CancellationToken.None);
      CustomOverlayImages = new BindableCollection<string>();
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

    public string ReturnSelectedPath()
    {
      return CustomOverlayImages[SelectedIndex];
    }

    public void UpdateBackgrounds()
    {
      DirectoryInfo info =
        new DirectoryInfo(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                          "\\BingoBankoKontrol\\Background");
      FileInfo[] fInf = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
      foreach (var background in fInf)
      {
        if (!CustomOverlayImages.Contains(background.FullName))
        {
          CustomOverlayImages.Add(background.FullName);
        }
      }
      NotifyOfPropertyChange(() => CustomOverlayImagesConverter);
    }

    #region props

    public Visibility IsOverlayVisible
    {
      get { return _isOverlayVisible; }
      set
      {
        _isOverlayVisible = value;
        NotifyOfPropertyChange(() => IsOverlayVisible);
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

    public bool StdScrnOl
    {
      get { return _stdScrnOL; }
      set
      {
        _stdScrnOL = value;
        NotifyOfPropertyChange(() => StdScrnOl);
      }
    }

    public int SelectedIndex
    {
      get { return _selectedIndex; }
      set
      {
        _selectedIndex = value;
        NotifyOfPropertyChange(() => SelectedIndex);
        _log.Info(_selectedIndex.ToString());
      }
    }

    public bool UserDefinedScreen
    {
      get { return _userDefinedScreen; }
      set
      {
        _userDefinedScreen = value;
        NotifyOfPropertyChange(() => UserDefinedScreen);
      }
    }

    public bool ScrnActivationRequired
    {
      get { return _scrnActivationRequired; }
      set
      {
        _scrnActivationRequired = value;
        NotifyOfPropertyChange(() => ScrnActivationRequired);
        if (ScrnActivationRequired && UserDefinedScreen)
        {
          if (CustomOverlayImages != null)
          {
            if (CustomOverlayImages.Count != 0)
            {
              SelectedBackgroundPath = CustomOverlayImages[SelectedIndex];
            }
          }

          ScrnActivationRequired = false;
        }
        else if (ScrnActivationRequired && StdScrnOl)
        {
          SelectedBackgroundPath = standardOverlay;
          ScrnActivationRequired = false;
        }
      }
    }

    //TODO: Når man på GUI skifter fra blank overlay til Numberbar viser den ingenting - sikker bare en variable der skal ændres

    public BindableCollection<string> CustomOverlayImages
    {
      get { return _customOverlayImages; }
      set
      {
        _customOverlayImages = value;
        NotifyOfPropertyChange(() => CustomOverlayImages);
        NotifyOfPropertyChange(() => (CustomOverlayImagesConverter));
      }
    }


    public BindableCollection<string> CustomOverlayImagesConverter
    {
      get
      {
        BindableCollection<string> resultCollection = new BindableCollection<string>();
        foreach (string item in CustomOverlayImages)
        {
          if (!(item.LastIndexOf("\\") < 0))
          {
            resultCollection.Add(item.Substring(item.LastIndexOf("\\", item.Length)));
          }
        }
        return resultCollection;
      }
    }

    #endregion
  }
}