namespace Basketaki.Dtos
{
    public class MatchDto
    {
        public int Id { get; set; }

        public DateOnly MatchDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public int CourtId { get; set; }

        public string CourtName { get; set; } = null!;

        public string CourtLocation { get; set; } = null!;

        public int LeagueId { get; set; }

        public string LeagueName { get; set; } = null!;

        public string LeagueSeasonName { get; set; } = null!;

        public int HomeTeamSeasonLeagueId { get; set; }

        public string HomeTeamName { get; set; } = null!;

        public int AwayTeamSeasonLeagueId { get; set; }

        public string AwayTeamName { get; set; } = null!;

        public int? HomeScore { get; set; }

        public int? AwayScore { get; set; }

        public bool IsPlayed { get; set; }
    }
}
