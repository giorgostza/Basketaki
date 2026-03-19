using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class TeamStanding
    {

        public int Id { get; set; }

        public int TeamSeasonLeagueId { get; set; }
        public TeamSeasonLeague TeamSeasonLeague { get; set; } = null!;


        [Range(0, int.MaxValue)]
        public int Played { get; set; }         // How many matches played

        [Range(0, int.MaxValue)]
        public int Wins { get; set; }           // How many matches won

        [Range(0, int.MaxValue)]
        public int Losses { get; set; }         // How many matches lost

        [Range(0, int.MaxValue)]
        public int PointsFor { get; set; }     // How many points scored 

        [Range(0, int.MaxValue)]
        public int PointsAgainst { get; set; } // How many points conceded

        [Range(0, int.MaxValue)]
        public int LeaguePoints { get; set; }  // How many league points (2 for win, 1 for loss , 0 for not shown up)

        [Range(0, int.MaxValue)]
        public int NoShow { get; set; }       // How many matches not shown up (0, 1 or 2)

        
        public int CurrentStreak { get; set; } //   How many consecutive wins (positive) or losses (negative) the team has.




    }
}
