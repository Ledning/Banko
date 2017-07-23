using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankoProject.Models;
using Caliburn.Micro;

namespace BankoProject.ViewModels.ConfirmationBoxes
{
  class ChangePlatesUsedDialogViewModel : Screen
  {
    #region Fields

    private int _antalPlader;

    #endregion

    #region Constructors

    public ChangePlatesUsedDialogViewModel()
    {
      DisplayName = "Skift antal brugte plader..";
    }

    #endregion

    #region Properties

    public int AntalPlader
    {
      get { return _antalPlader; }
      set
      {
        _antalPlader = value;
        NotifyOfPropertyChange(() => AntalPlader);
      }
    }

    #endregion

    #region Methods

    //TODO: Input restriction. We need to restrict what can be added and what cannot. probably like 1-100k plates is fine
    public void OK()
    {
      TryClose(true);
    }

    public void Cancel()
    {
      TryClose(false);
    }

    #endregion
  }
}