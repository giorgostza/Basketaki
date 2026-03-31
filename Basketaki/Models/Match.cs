using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Match
    {
        public int Id { get; set; }


        [Required]
        public DateOnly MatchDate { get; set; }  // 10/1/2026 

        [Required]
        public TimeOnly StartTime { get; set; }   // 18:00

        [Required]
        public TimeOnly EndTime { get; set; }     // 19:30


        public int CourtId { get; set; }
        public Court Court { get; set; } = null!;  // Basketaki Arena

        public int LeagueId { get; set; }
        public League League { get; set; } = null!;   // Master League

        public int HomeTeamSeasonLeagueId { get; set; }
        public TeamSeasonLeague HomeTeamSeasonLeague { get; set; } = null!;   // Gran Camaria

        public int AwayTeamSeasonLeagueId { get; set; }    
        public TeamSeasonLeague AwayTeamSeasonLeague { get; set; } = null!;   // Rejects

        [Range(0, 300)]
        public int? HomeScore { get; set; }   //  75-69 

        [Range(0, 300)]
        public int? AwayScore { get; set; }   // 59-65

        public bool IsPlayed { get; set; } = false; 



        public ICollection<PlayerStat> PlayerStats { get; set; } = new List<PlayerStat>();

        public ICollection<MatchPhoto> Photos { get; set; } = new List<MatchPhoto>();


    }
}