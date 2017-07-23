using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Windows.Media;

namespace BankoProject.ViewModels.ConfirmationBoxes
{
  class PlateHasBingoViewModel : Screen
  {
    #region Fields

    private string _answerText = "Error";
    private List<List<int>> _missingNumbersInRows;
    private int[,] _chosenPlate;
    private string brush;
    private string _bingo = "#FF3DCD4A";
    private string _noBingo = "#FF850000";

    #endregion


    public PlateHasBingoViewModel(int plateNum, int[,] chosenPlate, List<List<int>> missingNumbersInRows, bool hasBingo)
    {
      MissingNumbersInRows = missingNumbersInRows;

      //determine if bingo or np
      if (hasBingo)
      {
        DisplayName = "Plade nr. " + plateNum + " har BANKO!";
        AnswerText = "Pladen har BANKO!";
        Brush = _bingo;
      }
      else
      {
        DisplayName = "Plade nr. " + plateNum + " har ikke banko.";
        AnswerText = "Pladen har ikke banko. Der mangler nedenstående numre:\n ";
        int i = 1;
        foreach (var missingNumbers in MissingNumbersInRows)
        {
          AnswerText = AnswerText + "Række " + i + ":";
          foreach (var number in missingNumbers)
          {
            if (AnswerText.Contains(":"))
            {
              if (number != 0)
              {
                AnswerText = AnswerText + " " + number;
              }
            }
          }
          i++;
          AnswerText = AnswerText + "\n";
        }
        if (chosenPlate[0, 0] == -1)
        {
          AnswerText =
            "Pladen har ikke banko, da det er en ugyldig/frafiltreret plade.\n Ydermere, hvis der findes en person med dette pladenummer, er der noget seriøst galt.";
        }
        Brush = _noBingo;
      }
    }

    public void OK()
    {
      TryClose(true);
    }

    #region Props

    public string AnswerText
    {
      get { return _answerText; }
      set
      {
        _answerText = value;
        NotifyOfPropertyChange(() => AnswerText);
      }
    }

    public List<List<int>> MissingNumbersInRows
    {
      get { return _missingNumbersInRows; }
      set
      {
        _missingNumbersInRows = value;
        NotifyOfPropertyChange(() => MissingNumbersInRows);
      }
    }

    public string Brush
    {
      get { return brush; }

      set
      {
        brush = value;
        NotifyOfPropertyChange(() => Brush);
      }
    }

    #endregion
  }
}