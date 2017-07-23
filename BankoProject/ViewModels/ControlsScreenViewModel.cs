using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.MVVM;
using Caliburn.Micro;
using BankoProject.Tools;
using BankoProject.Models;
using System.Windows.Media;

namespace BankoProject.ViewModels
{
  public sealed class ControlsScreenViewModel : Screen, IMainViewItem
  {
    #region Fields

    private OptionsFlyoutViewModel _OFVM;
    private ControlOptions _COptions;
    private BingoNumberBoard _BNBoard;

    private Random randomNumber
      ; //use to pick new random number. Maybe make into seperate class that adjusts according to prev numbers being picked.

    private readonly SolidColorBrush _defaultBrush = Brushes.Black;

    #endregion

    #region Constructors

    public ControlsScreenViewModel()
    {
      OFVM = new OptionsFlyoutViewModel();
      _BNBoard = new BingoNumberBoard();
    }

    #endregion

    #region Wrappers

    /*public void Pick(int num)
    {
      BNBoard.PickNumber(num);
    }
    public void UnPick(int num)
    {
      BNBoard.UnPickNumber(num);
    }
    public void Reset(BingoNumberBoard Board)
    {
      BNBoard.ResetBoard();
    }*/

    #endregion

    #region Properties

    public OptionsFlyoutViewModel OFVM
    {
      get { return _OFVM; }

      set
      {
        _OFVM = value;
        NotifyOfPropertyChange(() => OFVM);
      }
    }

    public ControlOptions COptions
    {
      get { return _COptions; }

      set
      {
        _COptions = value;
        NotifyOfPropertyChange(() => COptions);
      }
    }

    public BingoNumberBoard BNBoard
    {
      get { return _BNBoard; }

      set
      {
        _BNBoard = value;
        NotifyOfPropertyChange(() => BNBoard);
      }
    }

    public SolidColorBrush DefaultBrush
    {
      get { return _defaultBrush; }
    }

    #endregion
  }
}