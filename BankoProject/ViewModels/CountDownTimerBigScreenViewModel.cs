using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.MVVM;
using System.Timers;
using System.Windows.Controls;
using Caliburn.Micro;
using BankoProject.Models;
using BankoProject.Tools;
using Action = System.Action;

namespace BankoProject.ViewModels
{
  public sealed class CountdowntimerBigScreenViewModel : Screen, IPresentationScreenItem
  {
    #region Fields

    //The colour of the button at start and finish. NSFP. Modify if different colours are wanted. cannot be made from designer. 
    string finishCoulour = "#FF3FBD5B";

    string unfinishColour = "#FFBF5050";
    private BingoEvent _event;

    //buttonsize. maybe make scale with button count? certainly possible, try scaling in control and syncing that here. SFP


    //list of buttons on the screen. set to 0 if no buttons are desired, it will now show only the timer. hopefully. helpers might be needed. SFP 

    #endregion

    #region Constructors

    public CountdowntimerBigScreenViewModel()
    {
    }

    #region Overrides of ViewAware

    protected override void OnViewReady(object view)
    {
      Event = IoC.Get<BingoEvent>();
      Event.WindowSettings.PrsSettings.DockingPlace = Dock.Top;
    }

    #endregion

    #endregion

    #region GetterSetter

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

    #region Eventstuff

    #region Overrides of Screen

    protected override void OnDeactivate(bool close)
    {
      Event.WindowSettings.PrsSettings.DockingPlace = Dock.Bottom;
    }

    #endregion

    #endregion
  }
}