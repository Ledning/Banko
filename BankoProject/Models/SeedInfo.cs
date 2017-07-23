using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Caliburn.Micro;

namespace BankoProject.Models
{
  //TODO: I ALLE KLASSER EVERYWHERE: ORGANISER RÆKKEFØLGEN AF PROPERTIES, BACKING FIELDS, FUNCTIONS; GROUP FUNCTIONS BY FUNCTIONALITY
  // REMEMBER SERIALIZEABLETAGS HAS TO BE IN THE RIGHT PLACE


  /// <summary>
  /// Contains:
  ///  - Originalseed
  ///     The initial seed the user entered when creating the event. 
  ///  - Seed
  ///     What the seed is right now - it might have been altered due to reasons unknown. Maybe the output from that given seed was a really bad distribution. 
  ///  - SeedManipulated
  ///     has the seed been manipulated compared to the original seed?
  /// </summary>
  [Serializable]
  public class SeedInfo : PropertyChangedBase
  {
    public SeedInfo()
    {
      //TODO: MAKE up some clever way of making this not be called by anything but the serializer
    }

    public SeedInfo(string originalseed)
    {
      _originalSeed = originalseed;
      NotifyOfPropertyChange(() => OriginalSeed);
      Seed = OriginalSeed;
      _seedManipulated = false;
      NotifyOfPropertyChange(() => SeedManipulated);
      ConvertedOriginalSeed = ShortenAndParsePassphraseToInt32(originalseed);
    }

    private int _convertedOriginalSeed;
    private string _originalSeed;
    private string _seed;
    private bool _seedManipulated;

    [XmlIgnore]
    public string OriginalSeed
    {
      get { return _originalSeed; }
    }

    public string Seed
    {
      get { return _seed; }
      set
      {
        _seed = value;
        NotifyOfPropertyChange();
      }
    }

    public bool SeedManipulated
    {
      get
      {
        _seedManipulated = Seed.Equals(OriginalSeed);
        NotifyOfPropertyChange(() => SeedManipulated);
        return _seedManipulated;
      }
    }

    [XmlIgnore]
    public int ConvertedOriginalSeed
    {
      get { return _convertedOriginalSeed; }
      private set { _convertedOriginalSeed = value; }
    }

    private int ShortenAndParsePassphraseToInt32(string keyword)
    {
      // Converts user keyword into an int32 seed
      int seed;
      string temp = "";

      // Convert passphrase to UTF8 values
      keyword = ConvertTextToUTF8Value(keyword);

      while (true)
      {
        bool b = Int32.TryParse(keyword, out seed);
        if (b)
        {
          break;
        }
        else
        {
          for (int i = 0; i < keyword.Length; i += 2)
          {
            temp += keyword[i];
          }
          keyword = temp;
          temp = "";
        }
      }
      return seed;
    }

    private string ConvertTextToUTF8Value(string keyword)
    {
      byte[] arrayBytes = Encoding.UTF8.GetBytes(keyword);

      string convertedKeyword = "";

      //put values from arrayBytes into string
      foreach (byte element in arrayBytes)
      {
        convertedKeyword += Convert.ToString(element);
      }

      return convertedKeyword;
    }
  }
}