using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BingoCardGenerator;
using Caliburn.Micro;

namespace BankoProject.Models
{
  /// <summary>
  /// A class that holds all the information about the plates, how many did we print how many were actually used and so on
  /// </summary>
  [Serializable]
  public class PlateInfo : PropertyChangedBase
  {
    private int _platesGenerated;
    private int _platesUsed;
    private bool _hasPlatesBeenGenerated = false;
    private string _saveDirectory;

    [XmlIgnore] private Generator _cardGenerator;

    [XmlIgnore] public List<int[,]> CardList;

    public PlateInfo()
    {
      CardList = new List<int[,]>();
      SaveDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      if (!DoesDirectoryExist())
      {
        Directory.CreateDirectory(SaveDirectory + "\\BingoBankoKontrol");
      }
    }


    public bool DoesDirectoryExist()
    {
      if (Directory.Exists(SaveDirectory + "\\BingoBankoKontrol"))
      {
        return true;
      }


      return false;
    }


    public string SaveDirectory
    {
      get { return _saveDirectory; }
      set { _saveDirectory = value; }
    }

    public int PlatesGenerated
    {
      get { return _platesGenerated; }
      set
      {
        _platesGenerated = value;
        NotifyOfPropertyChange(() => PlatesGenerated);
      }
    }

    public int PlatesUsed
    {
      get { return _platesUsed; }
      set
      {
        _platesUsed = value;
        NotifyOfPropertyChange(() => PlatesUsed);
      }
    }

    [XmlIgnore]
    public Generator CardGenerator
    {
      get { return _cardGenerator; }
      set { _cardGenerator = value; }
    }

    public bool HasPlatesBeenGenerated
    {
      get { return _hasPlatesBeenGenerated; }
      set
      {
        _hasPlatesBeenGenerated = value;
        NotifyOfPropertyChange(() => HasPlatesBeenGenerated);
        NotifyOfPropertyChange(() => CanGeneratePlates);
      }
    }

    public bool CanGeneratePlates
    {
      get { return !HasPlatesBeenGenerated; }
    }
  }
}