using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankoProject.Models;
using BankoProject.Tools;
using Caliburn.Micro;

namespace BankoProject.ViewModels.PresentationScreen
{
  class FixedNumberBarViewModel : Screen, IPresentationScreenItem
  {
    private IWindowManager _winMan;
    private IEventAggregator _events;
    private BingoEvent _bingoEvent;
    private readonly ILog _log = LogManager.GetLog(typeof(WelcomeViewModel));

    #region Overrides of ViewAware

    protected override void OnViewReady(object view)
    {
      _winMan = IoC.Get<IWindowManager>();
      _events = IoC.Get<IEventAggregator>();
      Event = IoC.Get<BingoEvent>();
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

    #endregion
  }
}