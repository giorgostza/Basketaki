using System.ComponentModel.DataAnnotations;

namespace Basketaki.Dtos
{
    public class MatchCreateUpdateDto
    {
        [Required]
        public DateOnly MatchDate { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        public int CourtId { get; set; }

        [Required]
        public int LeagueId { get; set; }

        [Required]
        public int HomeTeamSeasonLeagueId { get; set; }

        [Required]
        public int AwayTeamSeasonLeagueId { get; set; }

        [Range(0, 300)]
        public int? HomeScore { get; set; }

        [Range(0, 300)]
        public int? AwayScore { get; set; }

        public bool IsPlayed { get; set; }
    }
}
