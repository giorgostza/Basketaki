using System.ComponentModel.DataAnnotations;

namespace Basketaki.Dtos
{
    public class TeamStandingCreateUpdateDto
    {
        [Required]
        public int TeamSeasonLeagueId { get; set; }

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

        [Range(0, int.MaxValue)]
        public int LeaguePoints { get; set; }

        [Range(0, int.MaxValue)]
        public int NoShow { get; set; }

        public int CurrentStreak { get; set; }
    }
}
