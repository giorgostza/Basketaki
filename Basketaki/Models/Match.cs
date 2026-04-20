using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Match
    {
        public int Id { get; set; }


        [Required]
        public DateOnly MatchDate { get; set; }  


        [Required]
        public TimeOnly StartTime { get; set; } 
        

        [Required]
        public TimeOnly EndTime { get; set; }    


        public int CourtId { get; set; }
        public Court Court { get; set; } = null!;  


        public int LeagueId { get; set; }
        public League League { get; set; } = null!;  


        public int HomeTeamSeasonLeagueId { get; set; }
        public TeamSeasonLeague HomeTeamSeasonLeague { get; set; } = null!;  
        

        public int AwayTeamSeasonLeagueId { get; set; }    
        public TeamSeasonLeague AwayTeamSeasonLeague { get; set; } = null!;   


        [Range(0, 300)]
        public int? HomeScore { get; set; }   


        [Range(0, 300)]
        public int? AwayScore { get; set; }   


        public bool IsPlayed { get; set; } = false;


        
        public ICollection<PlayerStat> PlayerStats { get; set; } = new List<PlayerStat>();

        public ICollection<MatchPhoto> Photos { get; set; } = new List<MatchPhoto>();

        public ICollection<MatchReferee> MatchReferees { get; set; } = new List<MatchReferee>();


    }
}