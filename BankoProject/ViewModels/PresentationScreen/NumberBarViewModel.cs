using BankoProject.Tools;
using BankoProject.Models;
using Caliburn.Micro;

namespace BankoProject.ViewModels.PresentationScreen
{
  class NumberBarViewModel : Screen, IPresentationScreenItem
  {
    #region Fields

    private IWindowManager _winMan;
    private IEventAggregator _events;
    private BingoEvent _bingoEvent;
    private readonly ILog _log = LogManager.GetLog(typeof(WelcomeViewModel));

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

    #endregion

    #region Overrides of ViewAware

    protected override void OnViewReady(object view)
    {
      _winMan = IoC.Get<IWindowManager>();
      _events = IoC.Get<IEventAggregator>();
      Event = IoC.Get<BingoEvent>();
    }

    #endregion

    #region Methods

    public void FontSizeConverter()
    {
    }

    #endregion
  }
}