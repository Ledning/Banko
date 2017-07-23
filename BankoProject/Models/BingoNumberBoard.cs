using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankoProject.Models;
using Caliburn.Micro;
using System.IO;
using BankoProject.ViewModels;
using System.Windows.Media;
using System.Xml.Serialization;

namespace BankoProject.Models
{
  [Serializable]
  public class BingoNumberBoard : PropertyChangedBase
  {
    //supposed to implement a large array and all the needed functionality for controlling the numbers 1-90. 
    //this class could technically just be a list of 90 numbers being removed/added as the game went along, but with extraction of that functionality into seperate classes, it allows for extensions and checks to be made. 

    private BindableCollection<BingoNumber> _board;
    private int _boardSize = 90;
    [XmlIgnore] private readonly ILog _log = LogManager.GetLog(typeof(BingoNumberBoard));
    private int _selectedIndex = 0;


    private IWindowManager _winMan;

    public BingoNumberBoard()
    {
      //_winMan = IoC.Get<IWindowManager>();
    }

    //TODO: Consider this function. should it even be here?=
    public void Initialize()
    {
      _log.Info("Initialising bingo-board with 90 numbers...");
      _board = new BindableCollection<BingoNumber>();
      for (int i = 0; i < _boardSize; i++)
      {
        Board.Add(new BingoNumber(i + 1));
      }
      NotifyOfPropertyChange(() => Board);
      _log.Info("Initialization of board done.");
    }

    [XmlArray("Board")]
    [XmlArrayItem(Type = typeof(BingoNumber))]
    public BindableCollection<BingoNumber> Board
    {
      get { return _board; }
      set
      {
        _board = value;
        NotifyOfPropertyChange(() => Board);
      }
    }
  }
}