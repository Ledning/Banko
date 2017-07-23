using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankoProject.Models;
using Caliburn.Micro;

namespace BankoProject.ViewModels.ConfirmationBoxes
{
  class AddNumberViewModel : Screen, IDataErrorInfo
  {
    private int _numberToAdd;
    private string _error;
    private BingoEvent Event;
    private bool _canConfirmNumber;

    public AddNumberViewModel()
    {
      DisplayName = "Indtast tal..";
      NumberToAdd = 1;
      Event = IoC.Get<BingoEvent>();
      CanConfirmNumber = false;
    }

    public void OK()
    {
      TryClose(true);
    }

    public void Cancel()
    {
      TryClose(false);
    }


    //TODO: InputRestriktion: må kun indtaste mellem 1 og 90

    public bool CanConfirmNumber
    {
      get { return _canConfirmNumber; }
      set
      {
        _canConfirmNumber = value;
        NotifyOfPropertyChange(() => CanConfirmNumber);
      }
    }

    public int NumberToAdd
    {
      get { return _numberToAdd; }
      set
      {
        _numberToAdd = value;
        NotifyOfPropertyChange(() => NumberToAdd);
      }
    }

    #region Implementation of IDataErrorInfo

    public string this[string columnName]
    {
      get
      {
        string result = null;
        if (columnName == "NumberToAdd")
        {
          if (NumberToAdd <= 0)
          {
            result = "Tallet må ikke være 0 eller under.";
            CanConfirmNumber = false;
          }
          else if (NumberToAdd > 90)
          {
            result = "Tallet må ikke være over 90.";
            CanConfirmNumber = false;
          }
          else if (!CheckIfNumberIsPicked(NumberToAdd))
          {
            result = "Tallet er allerede taget. Vælg et andet.";
            CanConfirmNumber = false;
          }
          else
          {
            CanConfirmNumber = true;
          }
        }
        return result;
      }
    }


    public bool CheckIfNumberIsPicked(int number)
    {
      foreach (BingoNumber bnum in Event.BingoNumberQueue)
      {
        if (bnum.Value == number)
        {
          return false;
        }
      }
      return true;
    }

    public string Error
    {
      get { return _error; }
    }

    #endregion
  }
}