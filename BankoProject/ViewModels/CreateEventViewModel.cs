using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BankoProject.Models;
using Caliburn.Micro;

namespace BankoProject.ViewModels
{
  class CreateEventViewModel : Caliburn.Micro.Screen, INotifyDataErrorInfo
  {
    //TODO: Make this window a totally borderless window
    /*Den skal være helt kantløs, og man behøver ikke kunne flytte den. 
     * Den bør ikke låse musen til programvinduet, bare man ikke kan klikke andet i programmet.
     * Derudover skal det overvejes om der skal være inputrestriktion i dette vindue  */

    //TODO: Husk at lave vinduet om så alle tingene er synlige

    //TODO: LAv Skærmindstillinger dropdownen
    //Den kan bare bruge en enum der betegner alle de forskellige normale aspect ratios, lav enumen i ApplicationWideEnums, så vi kan reffe den otherwhere
    //Det valgte skal så være bundet til en prop på BingoEvent objektet, under PresentationScreen objektet, aka lissom begivenhedsnavnet eller whatev

    //TODO: Make this window great again
    //det ligner lidt lårt gør det ikk? Make it better or summin, idunno

    #region Fields

    private BingoEvent _bingoEvent;
    private string _phTitle;
    private string _seed;
    private int _phPladetal;

    private readonly ILog _log = LogManager.GetLog(typeof(CreateEventViewModel));

    #endregion

    #region Constructors

    public CreateEventViewModel()
    {
      this._phPladetal = 5;
      this._seed = "DefaultValue";
      this._phTitle = "Default";
    }

    #endregion

    #region Properties

    public BingoEvent Event
    {
      get { return _bingoEvent; }
      set
      {
        _bingoEvent = value;
        NotifyOfPropertyChange(() => Event);
      }
    }

    public int PhPladetal
    {
      get { return _phPladetal; }
      set
      {
        _phPladetal = value;
        NotifyOfPropertyChange(() => PhPladetal);
        IsAmountPlatesValid(value, nameof(this.PhPladetal));
        NotifyOfPropertyChange(() => CanAcceptButton);
      }
    }

    public string PhSeed
    {
      get { return _seed; }
      set
      {
        _seed = value;
        NotifyOfPropertyChange(() => PhSeed);
      }
    }

    public string PhTitle
    {
      get { return _phTitle; }
      set
      {
        _phTitle = value;
        NotifyOfPropertyChange(() => PhTitle);
        IsValid(value, nameof(this.PhTitle));
        NotifyOfPropertyChange(() => CanAcceptButton);
      }
    }

    #endregion

    #region Overrides of ViewAware

    protected override void OnViewReady(object view)
    {
      Event = IoC.Get<BingoEvent>();
      DisplayName = "Nyt Event";
    }

    #endregion

    #region Buttons

    public void AcceptButton()
    {
      Event.Initialize(PhSeed, PhTitle, PhPladetal); //skal fjernes når vi kan åbne et event rigtigt
      _log.Info("Event created, createeventviewmodel");
      TryClose(true);
    }

    public bool CanAcceptButton
    {
      get { return !HasErrors; }
    }

    public void CancelButton()
    {
      TryClose(false);
    }

    #endregion

    //this should be moved to a baseclass, in case more views need inputvalidation

    #region Implementation of INotifyDataErrorInfo

    //contains all errors, for a given property, where the property is the key accessor
    private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public IEnumerable GetErrors(string propertyName)
    {
      if (string.IsNullOrEmpty(propertyName) || !_errors.ContainsKey(propertyName))
      {
        return null;
      }
      return _errors[propertyName];
    }

    public bool HasErrors
    {
      get { return _errors.Count > 0; }
    }

    //error message defined as a constant
    private const string GENERAL_ERROR = "Dette felt må ikke være tomt.";

    private const string TOOMANYPLATES_ERROR = "Angiv mindre en 10,000 plader";

    private const string TOOFEWPLATES_ERROR = "Angiv mindst 1 plade";
    //basically need a bunch of methods, that validate a certain property each.
    //for example a method that validates a name property

    public bool IsValid(string value, string propertyName)
    {
      bool isValid = true;
      if (value == "")
      {
        AddError(propertyName, GENERAL_ERROR, false);
        isValid = false;
      }
      else
      {
        RemoveError(propertyName, GENERAL_ERROR);
        isValid = true;
      }
      return isValid;
    }

    public bool IsAmountPlatesValid(int value, string propertyName)
    {
      bool isValid = true;

      if (value > 10000)
      {
        AddError(propertyName, TOOMANYPLATES_ERROR, false);
        isValid = false;
      }
      else if (value < 1)
      {
        AddError(propertyName, TOOFEWPLATES_ERROR, false);
        isValid = false;
      }
      else
      {
        RemoveError(propertyName, TOOMANYPLATES_ERROR);
        RemoveError(propertyName, TOOFEWPLATES_ERROR);
        isValid = true;
      }

      return isValid;
    }


    public void RaiseErrorsChanged(string propertyName)
    {
      ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    //add error to dictionary, with propertyname as key
    public void AddError(string propertyName, string error, bool isWarning)
    {
      if (!_errors.ContainsKey(propertyName))
      {
        _errors[propertyName] = new List<string>();
      }
      if (!_errors[propertyName].Contains(error))
      {
        if (isWarning)
        {
          _errors[propertyName].Add(error);
        }
        else
        {
          _errors[propertyName].Insert(0, error);
        }
      }
      RaiseErrorsChanged(propertyName);
    }

    public void RemoveError(string propertyName, string error)
    {
      if (_errors.ContainsKey(propertyName) && _errors[propertyName].Contains(error))
      {
        _errors[propertyName].Remove(error);
        if (_errors[propertyName].Count == 0)
        {
          _errors.Remove(propertyName);
        }
      }
      RaiseErrorsChanged(propertyName);
    }

    #endregion
  }
}