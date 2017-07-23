using BankoProject.Models;
using BankoProject.Tools;
using Caliburn.Micro;

namespace BankoProject.ViewModels.PresentationScreen
{
  class BingoScreenViewModel : Screen, IPresentationScreenItem
  {
    private BingoEvent _event;

    private readonly ILog _log = LogManager.GetLog(typeof(WelcomeViewModel));

    public BingoEvent Event
    {
      get { return _event; }
      set
      {
        _event = value;
        NotifyOfPropertyChange(() => Event);
      }
    }

    #region Overrides of ViewAware

    protected override void OnViewReady(object view)
    {
      Event = IoC.Get<BingoEvent>();
    }

    #endregion
  }
}