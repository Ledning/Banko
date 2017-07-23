using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankoProject.Models;
using Caliburn.Micro;

namespace BankoProject.ViewModels.PresentationScreen
{
  class PresentationBoardViewModel : Screen
  {
    private BingoEvent _event;

    public PresentationBoardViewModel()
    {
      Event = IoC.Get<BingoEvent>();
    }

    #region Overrides of ViewAware

    protected override void OnViewReady(object view)
    {
      Event = IoC.Get<BingoEvent>();
    }

    #endregion

    public BingoEvent Event
    {
      get { return _event; }
      set
      {
        _event = value;
        NotifyOfPropertyChange(() => Event);
      }
    }
  }
}