using BankoProject.Tools;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using BankoProject.Models;
using BankoProject.Tools.Events;
using BankoProject.ViewModels.PresentationScreen;

namespace BankoProject.ViewModels
{
  class WelcomeViewModel : Screen, IMainViewItem
  {
    #region Fields

    private IWindowManager _winMan;
    private IEventAggregator _events;
    private BingoEvent _bingoEvent;
    private readonly ILog _log = LogManager.GetLog(typeof(WelcomeViewModel));
    private KeyValuePair<string, string> _selectedEvent;
    private BindableCollection<KeyValuePair<string, string>> _latestEvents;
    private string _title;

    #endregion

    #region Constructors

    public WelcomeViewModel()
    {
      Title = "BingoBanko Kontrol";
      SaveDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      CreateApplicationDirectories();
      _latestEvents = new BindableCollection<KeyValuePair<string, string>>();
      UpdateLatestEvents();


      //Run the update on latestevents
      var dueTime = TimeSpan.FromSeconds(10);
      var interval = TimeSpan.FromSeconds(10);
      RunPeriodicAsync(UpdateLatestEvents, dueTime, interval, CancellationToken.None);
    }

    #endregion

    #region Properties

    public BingoEvent Event
    {
      get { return _bingoEvent; }
      set
      {
        _bingoEvent = value;
        NotifyOfPropertyChange(() => Event);
      }
    }

    public string Title
    {
      get { return _title; }
      set
      {
        _title = value;
        NotifyOfPropertyChange(() => Title);
      }
    }

    public string SaveDirectory
    {
      get { return _saveDirectory; }
      set { _saveDirectory = value; }
    }

    public KeyValuePair<string, string> SelectedEvent
    {
      get { return _selectedEvent; }

      set
      {
        _selectedEvent = value;
        NotifyOfPropertyChange(() => SelectedEvent);
      }
    }

    public BindableCollection<KeyValuePair<string, string>> LatestEvents
    {
      get { return _latestEvents; }
    }

    #endregion

    #region Overrides for ViewAware

    protected override void OnViewReady(object view)
    {
      SaveDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      _winMan = IoC.Get<IWindowManager>();
      _events = IoC.Get<IEventAggregator>();
      Event = IoC.Get<BingoEvent>();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Used for doing things when doubleclicking latestevents
    /// </summary>
    public void DoubleClickAction()
    {
      if (SelectedEvent.Key != null)
      {
        _events.PublishOnCurrentThread(new CommunicationObject(ApplicationWideEnums.MessageTypes.Load,
          ApplicationWideEnums.SenderTypes.WelcomeView, SelectedEvent.Key.Replace(".xml", "")));
        _events.PublishOnUIThread(new CommunicationObject(ApplicationWideEnums.MessageTypes.ChngControlPanelView,
          ApplicationWideEnums.SenderTypes.WelcomeView));
      }
    }

    #endregion

    #region BindableCollectionManipulation

    //So some of these might not be entirely needed, but i was not 100% sure on waht the built in ones for keyvaluepair did, so i thought this was better
    public bool ContainsKey(BindableCollection<KeyValuePair<string, string>> inputCollection, string key)
    {
      foreach (KeyValuePair<string, string> pair in inputCollection)
      {
        if (pair.Key == key)
        {
          return true;
        }
      }
      return false;
    }

    public void RemoveKeyPair(BindableCollection<KeyValuePair<string, string>> inputCollection, string targetkey)
    {
      foreach (KeyValuePair<string, string> pair in inputCollection)
      {
        if (pair.Key == targetkey)
        {
          inputCollection.Remove(pair);
        }
      }
    }

    #endregion

    #region asynctask latestevents

    public void UpdateLatestEvents()
    {
      bool changeHappened = false;
      DirectoryInfo info =
        new DirectoryInfo(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                          "\\BingoBankoKontrol\\LatestEvents");
      FileInfo[] fInf = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();


      foreach (var file in fInf)
      {
        KeyValuePair<string, string> targetKeyValuePair = new KeyValuePair<string, string>(file.Name,
          file.LastAccessTime.ToString());

        //Corresponds to the key not being in there already
        if (!ContainsKey(LatestEvents, targetKeyValuePair.Key))
        {
          _latestEvents.Add(targetKeyValuePair);
          changeHappened = true;
        }
        else
        {
          //Corresponds to the key being in there, but not the right value(date)
          if (!LatestEvents.Contains(targetKeyValuePair))
          {
            RemoveKeyPair(_latestEvents, targetKeyValuePair.Key);
            _latestEvents.Add(targetKeyValuePair);
            changeHappened = true;
          }
        }
      }
      if (changeHappened)
      {
        //TODO: MAke it sorted
        NotifyOfPropertyChange(() => LatestEvents);
      }
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

    #region Loading/creating

    public void CreateEvent()
    {
      bool? result = _winMan.ShowDialog(new CreateEventViewModel());
      if (result.HasValue)
      {
        if (result.Value)
        {
          _log.Info("exit on event created, welcomeview");
          _events.PublishOnBackgroundThread(new CommunicationObject(ApplicationWideEnums.MessageTypes.Save,
            ApplicationWideEnums.SenderTypes.WelcomeView));
          _log.Info("SAVEMESSAGE");
          _events.PublishOnUIThread(new CommunicationObject(ApplicationWideEnums.MessageTypes.ChngControlPanelView,
            ApplicationWideEnums.SenderTypes.WelcomeView));
        }
      }
    }

    //TODO: Ordentlig boks til titlen i WelcomewView.
    //Den ligner sku en sæk lort lige pt

    public void OpenFileDialog()
    {
      var ofd = new Microsoft.Win32.OpenFileDialog()
      {
        Filter = "Event filer (*.xml)|*xml"
      };
      var d = "NOFILE";
      if (ofd.ShowDialog() ?? false)
      {
        d = ofd.FileName;
      }
      if (d != "NOFILE")
      {
        d = Path.GetFileNameWithoutExtension(d);
        _events.PublishOnUIThread(new CommunicationObject(ApplicationWideEnums.MessageTypes.Load,
          ApplicationWideEnums.SenderTypes.WelcomeView, d));
        _events.PublishOnUIThread(new CommunicationObject(ApplicationWideEnums.MessageTypes.ChngControlPanelView,
          ApplicationWideEnums.SenderTypes.WelcomeView));
      }
    }

    #endregion

    #region DirectoryStuff

    private string _saveDirectory;

    //TODO: Make these use a string for each of the subdirectories and the main directory, no chance of spelling error
    private bool ApplicationDirectoryExists()
    {
      if (Directory.Exists(SaveDirectory + "\\BingoBankoKontrol"))
      {
        return true;
      }
      return false;
    }

    private bool ApplicationSubDirectoriesExists()
    {
      bool result = false;
      if (Directory.Exists(SaveDirectory + "\\BingoBankoKontrol" + "\\LatestEvents"))
      {
        if (Directory.Exists(SaveDirectory + "\\BingoBankoKontrol" + "\\Settings"))
        {
          if (Directory.Exists(SaveDirectory + "\\BingoBankoKontrol" + "\\Background"))
          {
            result = true;
          }
        }
      }
      else
        result = false;
      return result;
    }

    private bool ApplicationDirectoriesExist()
    {
      if (ApplicationDirectoryExists())
      {
        if (ApplicationSubDirectoriesExists())
        {
          return true;
        }
      }
      return false;
    }

    private void CreateApplicationDirectories()
    {
      Directory.CreateDirectory(SaveDirectory + "\\BingoBankoKontrol");
      CreateLatestDirectory();
      CreateSettingsDirectory();
      CreateBackgroundsDirectories();
    }

    private void CreateLatestDirectory()
    {
      Directory.CreateDirectory(SaveDirectory + "\\BingoBankoKontrol" + "\\LatestEvents");
    }

    private void CreateSettingsDirectory()
    {
      Directory.CreateDirectory(SaveDirectory + "\\BingoBankoKontrol" + "\\Settings");
    }

    private void CreateBackgroundsDirectories()
    {
      Directory.CreateDirectory(SaveDirectory + "\\BingoBankoKontrol" + "\\Background");
    }

    #endregion
  }
}