using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class TeamStanding
    {

        public int Id { get; set; }


        [Required]
        public int TeamSeasonLeagueId { get; set; }
        public TeamSeasonLeague TeamSeasonLeague { get; set; } = null!;


        [Range(0, int.MaxValue)]
        public int Played { get; set; }   
        

        [Range(0, int.MaxValue)]
        public int Wins { get; set; }  
        

        [Range(0, int.MaxValue)]
        public int Losses { get; set; }  
        

        [Range(0, int.MaxValue)]
        public int PointsFor { get; set; } 
        

        [Range(0, int.MaxValue)]
        public int PointsAgainst { get; set; } 


        public int PointDifference => PointsFor - PointsAgainst;


        [Range(0, int.MaxValue)]
        public int LeaguePoints { get; set; }  


        [Range(0, int.MaxValue)]
        public int NoShow { get; set; }      

        
        public int CurrentStreak { get; set; } 

    }
}
