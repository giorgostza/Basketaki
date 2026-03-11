using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class PlayerStat       //  A Player's Statistics in the Match
    {
        public int Id { get; set; }


        public int PlayerSeasonTeamId { get; set; }
        public PlayerSeasonTeam PlayerSeasonTeam { get; set; } = null!;



        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;   // 10/1/2026 - Basketaki Arena


        [Range(0, 200)]
        public int Points { get; set; }    // 19

        [Range(0, 200)]
        public int OffensiveRebounds { get; set; }  // 5

        [Range(0, 200)]
        public int DefensiveRebounds { get; set; }  // 11

        [Range(0, 200)]
        public int Assists { get; set; }   // 4

        [Range(0, 200)]
        public int Steals { get; set; }   //  0

        [Range(0, 200)]
        public int Blocks { get; set; }   //  2

        [Range(0, 6)]
        public int Fouls { get; set; }   //  3

        [Range(0, 80)]
        public int MinutesPlayed { get; set; }  // 32'

        [Range(0, 100)]
        public int FreeThrowsMade { get; set; }       // 7

        [Range(0, 200)]
        public int FreeThrowsAttempted { get; set; }  // 12

        [Range(0, 100)]
        public int TwoPointsMade { get; set; }    //  8

        [Range(0, 200)]
        public int TwoPointsAttempted { get; set; }  // 15

        [Range(0, 100)]
        public int ThreePointsMade { get; set; }     // 0

        [Range(0, 200)]
        public int ThreePointsAttempted { get; set; }  // 0


        public bool IsMVP { get; set; }   //  True

        public int SuspensionGames { get; set; } // 1 match
    }


}
