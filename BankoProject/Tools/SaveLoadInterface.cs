using BankoProject.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankoProject.Tools
{
  public interface ISave
  {
    bool
      SaveSession(
        ref BingoEvent bingoEvent); // Save all these to a text file or a database or something. dosent matter that much, nobody is gonna be snoopin around
  }

  public interface ILoad
  {
    bool
      LoadSession(
        ref BingoEvent bingoEvent); //Load based on the key. Make function in flyout to select from the different savefiles found automatically. This relies on coptions and bnboard being refs to the master viewmodel
  }
}