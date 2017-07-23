using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankoProject.ViewModels
{
  class ContestViewModel
  {
    #region Constructors

    public ContestViewModel()
    {
    }

    #endregion

    #region Properties

    private CountdownTimerControlViewModel TimerControl { get; set; }
    private CountdowntimerBigScreenViewModel Timer { get; set; }

    #endregion
  }
}