using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankoProject.ViewModels
{
  //TODO: Delete this class, and refactor all the dialogs into something else. Or atleast move it to the right folder and use as a base for other DBoxes
  class dialogViewModel : Screen
  {
    #region Fields

    private bool _response = false;
    private string _text;

    #endregion

    #region Constructors

    public dialogViewModel(string text)
    {
      Text = text;
      DisplayName = "Bekræft Handling";
    }

    #endregion

    #region Properties

    public string Text
    {
      get { return _text; }

      set
      {
        _text = value;
        NotifyOfPropertyChange(() => Text);
      }
    }

    #endregion

    #region Buttons

    public void AcceptButton()
    {
      TryClose(true);
    }

    public void CancelButton()
    {
      TryClose(false);
    }

    #endregion

    //HOW TO USE THIS PIECE OF SHIT? LOOOK NO FURTHER
    //Same procedure if the result was != true
    //Og, hvis du vil bruge denne i en viewmodel, skal den have en instans af WindowManageren sendt med. 
    /*
    bool? result = winMan.ShowDialog(new dialogViewModel("Bekræft tilføjelse: " + number));
      if (result != null)
      {
        if (result.Value)
        {
          //Do some shit here
        }
      }
    */
  }
}