﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankoProject.Tools;
using Caliburn.Micro;

namespace BankoProject.ViewModels.PresentationScreen
{
  class PlateOverlayViewModel: Conductor<Screen>.Collection.OneActive, IPresentationScreenItem
  {
    #region Constructor
    //det store boardoverview
    public PlateOverlayViewModel()
    {
    } 
    #endregion

    #region Overrides of ViewAware

    protected override void OnViewReady(object view)
    {
      ActivateItem(new BoardViewModel());
    }

    #endregion
  }
}
