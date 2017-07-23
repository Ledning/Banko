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
  class PlateOverlayViewModel : Conductor<Screen>.Collection.OneActive, IPresentationScreenItem
  {
    private BingoEvent _event;

    #region Constructor

    //det store boardoverview
    public PlateOverlayViewModel()
    {
    }

    public BingoEvent Event
    {
      get { return _event; }
      set
      {
        _event = value;
        NotifyOfPropertyChange(() => Event);
      }
    }

    #endregion

    #region Overrides of ViewAware

    protected override void OnViewReady(object view)
    {
      ActivateItem(new PresentationBoardViewModel());
    }

    #endregion
  }
}