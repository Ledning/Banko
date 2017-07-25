using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.MVVM;
using Orchestra.Views;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using System.Windows;
using BankoProject.Models;
using Caliburn.Micro;
using BankoProject.Tools;
using BankoProject.Tools.Events;
using BingoCardGenerator;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using BankoProject.ViewModels.Flyout;
using MahApps.Metro;
using FormScrn = System.Windows.Forms.Screen;
using WpfScreenHelper;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using ScrnHelpScrn = WpfScreenHelper.Screen;

namespace BankoProject.ViewModels
{
  /*
   HOW TO DO EVENTS AND USE WINDOWMANAGER:

    In order to use anything contained in the IOC container this project has, it is as simple as creating a property to contain it, and then assigning that propert to the corresponding value from IOC:
    someproperty = IoC.Get<IWindowManager>();

    As of right now, it has 3(4) things:
    WindowManager  (Use IWindowManager)
    EventAggregator (Use IEventAggregator)
    BingoEvent (Use BingoEvent)

    Windowmanager is the only one which is not a singleton (meaning it wont be the same instance passed around)
    BingoEvent has all the info needed to run and show an event.

    To use WindowManager to show a dialog:
    _winMan.ShowDialog(new dialogViewModel("dialog text"))


    PH means placeholder
  */

  //TODO: Hvis vi skal have noget applicationwide adjustment of contrast and shit, skal der bindes mange forskellige steder eller noget i den stil. 
  //it might be possible be easier
  //TODO: work on prez scren is not done; in particular there is missing a lot of functions for handling blank screen stuff

  class MainWindowViewModel : Conductor<IMainViewItem>.Collection.OneActive, IShell, IHandle<CommunicationObject>
  {
    #region Fields

    private IWindowManager _winMan;
    private IEventAggregator _eventAggregator;
    private BingoEvent _bingoEvent;
    private readonly ILog _log = LogManager.GetLog(typeof(MainWindowViewModel));
    private string _saveDirectory;
    private bool _directoriesInitialised;
    private IFlyoutItem _flyoutViewModel;

    #endregion

    #region Constructors

    public MainWindowViewModel()
    {
      ActivateItem(new WelcomeViewModel());
      Tuple<AppTheme, Accent> appstyle = ThemeManager.DetectAppStyle(Application.Current);
      ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Green"), ThemeManager.GetAppTheme("BaseDark"));
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

    public string SaveDirectory
    {
      get { return _saveDirectory; }
      set { _saveDirectory = value; }
    }

    public bool DirectoriesInitialised
    {
      get { return _directoriesInitialised; }
    }

    public IFlyoutItem FlyoutViewModel
    {
      get { return _flyoutViewModel; }
      set
      {
        _flyoutViewModel = value;
        NotifyOfPropertyChange(() => FlyoutViewModel);
      }
    }

    #endregion

    #region Overrides of ViewAware

    //The function below can be used as a constructor for the view. Everything in it will happen after the view is loaded.
    protected override void OnViewReady(object view)
    {
      _winMan = IoC.Get<IWindowManager>();
      _winMan.ShowWindow(new DebuggingWindowViewModel(490, 490, 2300, 200));
      _eventAggregator = IoC.Get<IEventAggregator>();
      Event = IoC.Get<BingoEvent>();
      _eventAggregator.Subscribe(this);
      DisplayName = "Bingo Kontrol";
      _log.Info("Main View loaded");
      worker.DoWork += worker_DoWork;
      worker.RunWorkerCompleted += worker_RunWorkerCompleted;
      SaveDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      CreateApplicationDirectories();
      _directoriesInitialised = true;
      FlyoutViewModel = /*new VisibilityOptionsViewModel();*/new WelcomeScreenFlyoutViewModel();
    }

    #endregion

    #region Methods

    public void OnApplicationExit()
    {
      SaveSession();
      Environment.Exit(1);
    }

    public void Show()
    {
    }

    private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
    {
      return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
    }

    public void Handle(CommunicationObject message)
    {
      switch (message.Message)
      {
        case ApplicationWideEnums.MessageTypes.ChngWelcomeView:
          _log.Info("Changing to WelcomeView...");
          DisplayName = "Bingo Kontrol";
          GoToWelcomeView();
          break;
        case ApplicationWideEnums.MessageTypes.ChngControlPanelView:
          _log.Info("Changing to ControlPanelView...");
          DisplayName = DisplayName + ": " + Event.EventTitle;
          GoToControlPanel();
          break;

        case ApplicationWideEnums.MessageTypes.Save:
          _log.Info("Saving...");
          SaveSession();
          break;

        case ApplicationWideEnums.MessageTypes.Load:
          _log.Info("Loading...");
          LoadSession(message.SessionName);
          break;

        case ApplicationWideEnums.MessageTypes.GeneratePlates:
          _log.Info("Generate-plates message recieved...");
          GeneratePlates();
          break;

        case ApplicationWideEnums.MessageTypes.RbPrezScreen:
          _log.Info("method moved. NOT TO BE USED");
          break;

        case ApplicationWideEnums.MessageTypes.CreateApplicationDirectories:
          _log.Info("Creating directories...");
          CreateApplicationDirectories();
          break;
      }
    }

    #endregion

    #region async plate generation

    private readonly BackgroundWorker worker = new BackgroundWorker();


    private void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      if (File.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                      "\\BingoBankoKontrol\\" +
                      Event.EventTitle + "Plader.pdf"))
      {
        _log.Info("Plates has already been generated.");
        Event.PInfo.HasPlatesBeenGenerated = true;
        return;
      }
      _log.Info("Async plate-generation running...");
      Event.Generating = true;
      Tools.PDFMaker maker = new Tools.PDFMaker();
      string outputDir = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                         "\\BingoBankoKontrol\\" +
                         Event.EventTitle + "Plader";
      Event.PInfo.CardList = maker.MakePDF(Event.PInfo.CardGenerator.GenerateCard(Event.PInfo.PlatesGenerated),
        outputDir); //Takes care of generating the plates, and returning the used array. Should be secure enough.
      Event.PInfo.HasPlatesBeenGenerated = true;
    }


    private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      Event.Generating = false;
      _log.Info("Generation done.");
    }

    public void GeneratePlates()
    {
      worker.RunWorkerAsync();
    }

    #endregion

    #region eventbusMethods

    private void GoToWelcomeView()
    {
      FlyoutViewModel = new WelcomeScreenFlyoutViewModel();
      ActivateItem(new WelcomeViewModel());
      DisplayName = "Bingo Banko";
    }

    private void GoToControlPanel()
    {
      FlyoutViewModel = new WelcomeScreenFlyoutViewModel();
      FlyoutViewModel = new ControlPanelFlyoutViewModel();
      ActivateItem(new ControlPanelViewModel());
      DisplayName = "Bingo Bango: " + Event.EventTitle;
      Event.IsBingoRunning = true;
      Event.PInfo.PlatesUsed =
        Event.PInfo
          .PlatesGenerated; //Just a standard value for platesused, vi antager bare at man bruger dem alle sammen. 
    }

    #endregion

    #region DirectoryStuff

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

    #region SerializingCode

    /// <summary>
    /// Serializes an object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serializableObject"></param>
    /// <param name="fileName"></param>
    public void SerializeObject<T>(T serializableObject, string fileName)
    {
      if (serializableObject == null)
      {
        return;
      }

      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
        using (MemoryStream stream = new MemoryStream())
        {
          serializer.Serialize(stream, serializableObject);
          stream.Position = 0;
          xmlDocument.Load(stream);
          xmlDocument.Save(fileName);
          stream.Close();
        }
      }
      catch (Exception ex)
      {
        _log.Error(ex);
      }
    }


    /// <summary>
    /// Deserializes an xml file into an object list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public T DeSerializeObject<T>(string fileName)
    {
      if (string.IsNullOrEmpty(fileName))
      {
        return default(T);
      }

      T objectOut = default(T);

      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(fileName);
        string xmlString = xmlDocument.OuterXml;

        using (StringReader read = new StringReader(xmlString))
        {
          Type outType = typeof(T);

          XmlSerializer serializer = new XmlSerializer(outType);
          using (XmlReader reader = new XmlTextReader(read))
          {
            objectOut = (T) serializer.Deserialize(reader);
            reader.Close();
          }

          read.Close();
        }
      }
      catch (Exception ex)
      {
        _log.Error(ex);
      }

      return objectOut;
    }

    #endregion

    #region loadsave

    public bool LoadSession(string sessionName)
    {
      if (!DirectoriesInitialised)
      {
        CreateApplicationDirectories();
      }
      _log.Info("LOADSESSION");
      BingoEvent ev = new BingoEvent();
      ev = DeSerializeObject<BingoEvent>(SaveDirectory + "\\BingoBankoKontrol" + "\\LatestEvents" + "\\" + sessionName +
                                         ".xml");
      CopyEvent(ev, Event);
      Event.WindowSettings.PrsSettings.IsOverLayOpen = false;
      //TODO: There should be no errors at this point, provided that no files has been corrupted or anything. if it has, the application will crash
      //We should come up with some way of avoiding this, might be a buch of valuechecks or something. 
      return true;
    }


    /// <summary>
    /// SO. This is a fucking disgusting function
    /// When we use DeSerializeObject, it returns an entirely new object which the one in our simplecontainer is then replaced with.
    /// SimpleContainer does not like it when we do this, in fact it resets the object entirely. 
    /// So for now we have copied over all the values manually. it is disgusting but it works. 
    /// </summary>
    /// <param name="fr"></param>
    /// <param name="to"></param>
    private void CopyEvent(BingoEvent fr, BingoEvent to)
    {
      to.PInfo = fr.PInfo;
      to.Initialize(fr.SInfo.Seed, fr.EventTitle, fr.PInfo.PlatesGenerated);
      to.BingoNumberQueue = fr.BingoNumberQueue;
      to.BnkOptions = fr.BnkOptions;
      to.CmpOptions = fr.CmpOptions;
      to.CompetitionList = fr.CompetitionList;
      to.EventTitle = fr.EventTitle;
      to.Generating = fr.Generating;
      to.NumberBoard = fr.NumberBoard;
      to.VisOptions = fr.VisOptions;
      to.RecentFiles = fr.RecentFiles;
      to.WindowSettings = fr.WindowSettings;
      to.SInfo = fr.SInfo;
      Event.PInfo.CardList = Event.PInfo.CardGenerator.GenerateCard(Event.PInfo.PlatesGenerated);
      Event.PInfo.HasPlatesBeenGenerated = true;
    }


    public bool SaveSession()
    {
      if (!DirectoriesInitialised)
      {
        CreateApplicationDirectories();
      }
      _log.Info("SAVESESSION");
      SerializeObject<BingoEvent>(Event,
        SaveDirectory + "\\BingoBankoKontrol" + "\\LatestEvents" + "\\" + Event.EventTitle + ".xml");
      return true;
    }

    #endregion

    #region Consolidated async events

    //TODO: Hopefully we can consolidate these calls in here. it is not very clear or readable as it is rn

    #endregion

    //TODO: Somebody needs to look through the output log, and fix all the errors there. this is not easy, it takes a lot of time.
  }
}