using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankoProject.Models;
using BankoProject.Tools;

namespace BankoProject.ViewModels
{
  class BoardViewModel : Screen, IMainViewItem
  {
    #region Fields

    private BingoEvent _event;
    private IWindowManager _winMan;
    private readonly ILog _log = LogManager.GetLog(typeof(BingoNumberBoard));
    private int _selectedIndex = 0;

    #endregion

    #region Constructor

    public BoardViewModel()
    {
    }

    #endregion

    #region Overrides of ViewAware

    protected override void OnViewReady(object view)
    {
      Event = IoC.Get<BingoEvent>();
      _winMan = IoC.Get<WindowManager>();
    }

    #endregion

    #region Properties

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
  }
}