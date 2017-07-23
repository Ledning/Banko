using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace BankoProject.Models
{
  [Serializable]
  public class CompetitionObject : PropertyChangedBase
  {
    //TODO: MÅSKE VIRKER SERIALIZE IKKE PÅ COLLECTIONS: ARRAYS ER EN LØSNING ISTEDET.
    //Serialization kræver åbenbart parameterless constructors, hvis vi skal achieve det for vores, er vi nødt til at sørge for en mere konsolideret form for inputinitialisering der skal ske før et loaded objekt bliver brugt

    //TODO: Samme deal her, der skal laves så man på en eller anden skør måde kan få det her til at virke, men med parameterless constructor. måske er det faktisk en slags mønster?
    public CompetitionObject()
    {
    }

    public CompetitionObject(int numberOfParticipants, int numberOfTeams, int competitionDuration, int startValue)
    {
      int localstartValue = startValue;
      this.AllTeams = new List<Team>();
      for (int i = 0; i < numberOfTeams; i++)
      {
        Team team = new Team(numberOfParticipants);
        this.AllTeams.Add(team);

        team.TeamNumber = localstartValue;
        localstartValue++;
      }
      this.CompetitionDuration = CompetitionDuration;
    }

    public int NumberOfParticipants { get; set; }
    public int CompetitionDuration { get; set; }
    public List<Team> AllTeams { get; set; }
  }
}