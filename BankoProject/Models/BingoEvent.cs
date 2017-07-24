using System;
using System.Collections;
using System.Collections.Generic;
using Caliburn.Micro;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing.Text;
using System.Windows.Controls.Primitives;
using System.Xml.Serialization;
using BingoCardGenerator;


namespace BankoProject.Models
{
  [Serializable]
  public class BingoEvent : PropertyChangedBase
  {
    private int _queueLength = 0;
    private string _eventTitle;
    private DateTime _creationTime;
    private WinSettings _winSettings;
    private VisualsOptions _visualsOptions;
    private TimerOptions _timeOpt;
    private bool _initialised = false;
    private bool _generating = false;
    private bool _isBingoRunning = false;
    private bool _isResat = false;
    private DateTime _resetTime;


    [XmlIgnore] private readonly ILog _log = LogManager.GetLog(typeof(BingoEvent));

    private BankoOptions _bnkOptions;
    private CompetitionOptions _cmpOptions;
    private SeedInfo _seedInfo;
    private string _seedInfoText;
    private PlateInfo _plateInfo;
    private string _plateInfoText;
    private BingoNumberBoard _numberBoard;

    //any aggregated objects; settings object(general/specific), lists of objects for the competitions held during the event,
    [XmlArray("CompetitionList")] [XmlArrayItem("Competition")] private BindableCollection<CompetitionObject>
      _competitionList; //A list of all the competitions during the game

    [XmlArray("BingoNumberQueue")] [XmlArrayItem("BingoNumber")] private BindableCollection<BingoNumber>
      _bingoNumberQueue; //the numbers picked in the game, input into this list as they come in. 

    [XmlArray("AvailableNumbersQueue")] [XmlArrayItem("AvailableNumbers")] private BindableCollection<BingoNumber>
      _availableNumbersQueue; //The numbers available to be picked.

    [XmlArray("LatestNumbersQueue")] [XmlArrayItem("LatestNumbers")]
    private BindableCollection<string> _latestNumbersQueue;

    /// <summary>
    /// The latest 10 nubers to be picked.
    /// </summary>

    #region GetterSetter

    public string EventTitle
    {
      get { return _eventTitle; }
      set
      {
        _eventTitle = value;
        NotifyOfPropertyChange(()=>EventTitle);
      }
    }

    public BindableCollection<string> RecentFiles { get; set; }

    public DateTime CreationTime
    {
      get { return _creationTime; }
    }

    public BingoNumberBoard NumberBoard
    {
      get { return _numberBoard; }
      set
      {
        _numberBoard = value;
        NotifyOfPropertyChange(() => NumberBoard);
      }
    }

    [XmlArray("CompetitionList")]
    [XmlArrayItem(Type = typeof(CompetitionObject))]
    public BindableCollection<CompetitionObject> CompetitionList
    {
      get { return _competitionList; }
      set
      {
        _competitionList = value;
        NotifyOfPropertyChange(() => CompetitionList);
      }
    }

    [XmlArray("BingoNumberQueue")]
    [XmlArrayItem(Type = typeof(BingoNumber))]
    public BindableCollection<BingoNumber> BingoNumberQueue
    {
      get { return _bingoNumberQueue; }
      set
      {
        _bingoNumberQueue = value;
        QueueLength = _bingoNumberQueue.Count;
        NotifyOfPropertyChange(() => BingoNumberQueue);
      }
    }

    [XmlArray("AvailableNumbersQueue")]
    [XmlArrayItem(Type = typeof(BingoNumber))]
    public BindableCollection<BingoNumber> AvailableNumbersQueue
    {
      get { return _availableNumbersQueue; }
      set
      {
        _availableNumbersQueue = value;
        QueueLength = _availableNumbersQueue.Count;
        NotifyOfPropertyChange(() => AvailableNumbersQueue);
      }
    }

    [XmlArray("LatestNumbersQueue")]
    [XmlArrayItem(Type = typeof(string))]
    public BindableCollection<string> LatestNumbersQueue
    {
      get { return _latestNumbersQueue; }
      set
      {
        _latestNumbersQueue = value;
        NotifyOfPropertyChange(() => LatestNumbersQueue);
      }
    }


    public bool Generating
    {
      get { return _generating; }
      set
      {
        _generating = value;
        NotifyOfPropertyChange(() => Generating);
      }
    }

    public BankoOptions BnkOptions
    {
      get { return _bnkOptions; }
      set
      {
        _bnkOptions = value;
        NotifyOfPropertyChange(() => BnkOptions);
      }
    }

    public CompetitionOptions CmpOptions
    {
      get { return _cmpOptions; }
      set
      {
        _cmpOptions = value;
        NotifyOfPropertyChange(() => CmpOptions);
      }
    }

    public SeedInfo SInfo
    {
      get { return _seedInfo; }
      set
      {
        _seedInfo = value;
        NotifyOfPropertyChange(() => SInfo);
      }
    }

    public string SInfoText
    {
      get { return _seedInfoText; }
      set
      {
        _seedInfoText = value;
        NotifyOfPropertyChange(() => SInfoText);
      }
    }

    public PlateInfo PInfo
    {
      get { return _plateInfo; }
      set
      {
        _plateInfo = value;
        NotifyOfPropertyChange(() => PInfo);
      }
    }

    public string PInfoText
    {
      get { return _plateInfoText; }
      set
      {
        _plateInfoText = value;
        NotifyOfPropertyChange(() => PInfoText);
      }
    }

    public WinSettings WindowSettings
    {
      get { return _winSettings; }
      set
      {
        _winSettings = value;
        NotifyOfPropertyChange(() => WindowSettings);
      }
    }

    public int QueueLength
    {
      get { return _queueLength; }
      set
      {
        _queueLength = value;
        NotifyOfPropertyChange(() => QueueLength);
      }
    }

    public bool IsBingoRunning
    {
      get { return _isBingoRunning; }
      set
      {
        _isBingoRunning = value;
        NotifyOfPropertyChange(() => IsBingoRunning);
      }
    }

    //Not intended to be used on interface
    public bool IsResat
    {
      get { return _isResat; }
      set
      {
        _log.Warn("Not intended for display");
        _isResat = value;
      }
    }

    //Not intended to be used on interface
    public DateTime ResetTime
    {
      get { return _resetTime; }
      set
      {
        _log.Warn("Not intended for display");
        _resetTime = value;
      }
    }

    public TimerOptions TimeOpt
    {
      get { return _timeOpt; }
      set
      {
        _timeOpt = value;
        NotifyOfPropertyChange(() => TimeOpt);
      }
    }

    public VisualsOptions VisOptions
    {
      get { return _visualsOptions; }
      set { _visualsOptions = value; NotifyOfPropertyChange(()=>VisOptions);}
    }

    #endregion

    public void Initialize(string seed, string title, int pladetal)
    {
      _log.Info("Starting event object initialization...");
      NumberBoard = new BingoNumberBoard();
      NumberBoard.Initialize();
      CompetitionList = new BindableCollection<CompetitionObject>();
      BingoNumberQueue = new BindableCollection<BingoNumber>();
      BnkOptions = new BankoOptions();
      CmpOptions = new CompetitionOptions();
      SInfo = new SeedInfo(seed);
      PInfo = new PlateInfo();
      WindowSettings = new WinSettings();
      VisOptions = new VisualsOptions();
      VisOptions.SetOriginalColorScheme();
      EventTitle = title;
      PInfo.PlatesGenerated = pladetal;
      PInfoText = "Antal plader: " + pladetal;
      SInfoText = "Seed: " + SInfo.Seed;
      _creationTime = DateTime.Now;
      NotifyOfPropertyChange(() => CreationTime);
      _initialised = true;
      IsBingoRunning = false;
      _log.Info("Event object initialization done.");
      PInfo.CardGenerator = new Generator(SInfo.Seed);
      TimeOpt = new TimerOptions();
      LatestNumbersQueue = new BindableCollection<string>();
      for (int i = 0; i < 10; i++)
      {
        LatestNumbersQueue.Add("");
      }
      AvailableNumbersQueue = new BindableCollection<BingoNumber>();
      for (int i = 1; i <= 90; i++)
      {
        BingoNumber j = new BingoNumber();
        j.Value = i;
        AvailableNumbersQueue.Add(j);
      }
    }

    /// <summary>
    /// Intended to be used when resetting a game, and only then. Lets us distinguish the files from each other when the backup is made. 
    /// </summary>
    /// <param name="seed"></param>
    /// <param name="title"></param>
    /// <param name="pladetal"></param>
    /// <param name="resetTime"></param>
    public void Initialize(string seed, string title, int pladetal, DateTime resetTime)
    {
      //TODO: Refactor the contents of the intialize functions out into seperate functions for types of things that has to be initted, make it more clear
      _log.Info("Starting event object initialization...");
      NumberBoard = new BingoNumberBoard();
      NumberBoard.Initialize();
      CompetitionList = new BindableCollection<CompetitionObject>();
      BingoNumberQueue = new BindableCollection<BingoNumber>();
      BnkOptions = new BankoOptions();
      CmpOptions = new CompetitionOptions();
      SInfo = new SeedInfo(seed);
      PInfo = new PlateInfo();
      WindowSettings = new WinSettings();
      EventTitle = title;
      PInfo.PlatesGenerated = pladetal;
      WindowSettings = new WinSettings();
      VisOptions = new VisualsOptions();
      VisOptions.SetOriginalColorScheme();
      _creationTime = DateTime.Now;
      NotifyOfPropertyChange(() => CreationTime);
      _initialised = true;
      IsBingoRunning = false;
      IsResat = true;
      ResetTime = resetTime;
      _log.Info("Event object initialization done.");
      PInfo.CardGenerator = new Generator(SInfo.Seed);
      TimeOpt = new TimerOptions();
      LatestNumbersQueue = new BindableCollection<string>();
      for (int i = 0; i < 10; i++)
      {
        LatestNumbersQueue.Add("");
      }
      AvailableNumbersQueue = new BindableCollection<BingoNumber>();
      for (int i = 1; i <= 90; i++)
      {
        BingoNumber j = new BingoNumber();
        j.Value = i;
        AvailableNumbersQueue.Add(j);
      }
    }


    private string GenerateSeedFromKeyword(string keyword)
    {
      return keyword;
    }
  }
}