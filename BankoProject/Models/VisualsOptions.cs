using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;
using Caliburn.Micro;

namespace BankoProject.Models
{
  /// <summary>
  /// Refactored to handle color schemes and the like
  /// </summary>
  [Serializable]
  public class VisualsOptions : PropertyChangedBase
  {
    [XmlIgnore] private readonly ILog _log = LogManager.GetLog(typeof(BankoOptions));

    #region Fields

    //Fields for whatever the current color scheme of the application is
    private Color _baseColor; //Base color of the application

    private Color _baseTextColor; //Base text color
    private Color _buttonColor; //Button color

    private Color _buttonTextColor; //Text of the buttons (should probably be the same as basetextcolor, but if the buttoncolor dosent make the text stand out enough, this needs to be something else)

    private Color _boxAccentuationColor;


    public Color BaseColor
    {
      get => _baseColor;
      set
      {
        _baseColor = value;
        NotifyOfPropertyChange(() => BaseColor);
      }
    }

    public Color BaseTextColor
    {
      get => _baseTextColor;
      set
      {
        _baseTextColor = value;
        NotifyOfPropertyChange(() => BaseTextColor);
      }
    }

    public Color ButtonColor
    {
      get => _buttonColor;
      set
      {
        _buttonColor = value;
        NotifyOfPropertyChange(() => ButtonColor);
      }
    }

    public Color ButtonTextColor
    {
      get => _buttonTextColor;
      set
      {
        _buttonTextColor = value;
        NotifyOfPropertyChange(() => ButtonTextColor);
      }
    }

    public Color BoxAccentuationColor
    {
      get => _boxAccentuationColor;
      set
      {
        _boxAccentuationColor = value;
        NotifyOfPropertyChange(() => BoxAccentuationColor);
      }
    }

    #endregion


    #region Constructor

    public VisualsOptions()
    {
    }

    //public VisualsOptions(Color baseColor, Color baseTextColor, Color buttonColor, Color buttonTextColor, Color boxAccentuationColor)
    //{
    //  BaseColor = baseColor;
    //  BaseTextColor = baseTextColor;
    //  ButtonColor = buttonColor;
    //  ButtonTextColor = buttonTextColor;
    //  BoxAccentuationColor = boxAccentuationColor;
    //}

    #endregion


    #region Methods

    /// <summary>
    /// This method sets the original color scheme. 
    /// </summary>
    /// <remarks>
    /// I feel like this can be simplified??
    /// The  null checks have to be there though, just in case something bad happens during the conversion. Or something changes in the framework, who knows
    /// 
    /// </remarks>
    public void SetOriginalColorScheme()
    {
      object convertFromString = ColorConverter.ConvertFromString("#FF74ACFA");
      if (convertFromString != null)
        BaseColor = (Color) convertFromString;
      else
        BaseColor = Colors.Black; //this obviously indicates an error, this shouldnt happen

      convertFromString = null;
      convertFromString = ColorConverter.ConvertFromString("#FF74ACFA");
      if (convertFromString != null)
        BaseTextColor = (Color) convertFromString;
      else
        BaseTextColor = Colors.Black; //this obviously indicates an error, this shouldnt happen

      convertFromString = null;
      convertFromString = ColorConverter.ConvertFromString("#FF74ACFA");
      if (convertFromString != null)
        ButtonColor = (Color) convertFromString;
      else
        ButtonColor = Colors.Black; //this obviously indicates an error, this shouldnt happen

      convertFromString = null;
      convertFromString = ColorConverter.ConvertFromString("#FF000000");
      if (convertFromString != null)
        ButtonTextColor = (Color) convertFromString;
      else
        ButtonTextColor = Colors.Black; //this obviously indicates an error, this shouldnt happen

      convertFromString = null;
      convertFromString = ColorConverter.ConvertFromString("#FF3E8BCB");
      if (convertFromString != null)
        BoxAccentuationColor = (Color) convertFromString;
      else
        BoxAccentuationColor = Colors.Black; //this obviously indicates an error, this shouldnt happen
    }

    public void SetBwColorScheme()
    {
    }


    public void SetCrazyColorScheme()
    {
    }

    #endregion
  }
  }