using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class PlayerStat       
    {
        public int Id { get; set; }


        [Required]
        public int PlayerSeasonTeamId { get; set; }
        public PlayerSeasonTeam PlayerSeasonTeam { get; set; } = null!;


        [Required]
        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;   


        [Range(0, 200)]
        public int Points { get; set; }    


        [Range(0, 200)]
        public int OffensiveRebounds { get; set; }  


        [Range(0, 200)]
        public int DefensiveRebounds { get; set; }  


        public int TotalRebounds => OffensiveRebounds + DefensiveRebounds;


        [Range(0, 200)]
        public int Assists { get; set; }   


        [Range(0, 200)]
        public int Steals { get; set; }   


        [Range(0, 200)]
        public int Blocks { get; set; }   


        [Range(0, 5)]
        public int Fouls { get; set; }   


        [Range(0, 80)]
        public int MinutesPlayed { get; set; }  


        [Range(0, 100)]
        public int FreeThrowsMade { get; set; }      


        [Range(0, 200)]
        public int FreeThrowsAttempted { get; set; } 


        [Range(0, 100)]
        public int TwoPointsMade { get; set; }    


        [Range(0, 200)]
        public int TwoPointsAttempted { get; set; }  


        [Range(0, 100)]
        public int ThreePointsMade { get; set; }    


        [Range(0, 200)]
        public int ThreePointsAttempted { get; set; }  


        public bool IsMVP { get; set; }   


        [Range(0, 3)]
        public int SuspensionGames { get; set; } 

    }

}
