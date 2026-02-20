namespace Basketaki.Models
{
    public class Match
    {
        public int Id { get; set; }

        public DateTime MatchDate { get; set; }



        public int LeagueId { get; set; }
        public League League { get; set; } = null!;



        public int HomeTeamSeasonLeagueId { get; set; }
        public TeamSeasonLeague HomeTeamSeasonLeague { get; set; } = null!;



        public int AwayTeamSeasonLeagueId { get; set; }
        public TeamSeasonLeague AwayTeamSeasonLeague { get; set; } = null!;



        public int HomeScore { get; set; }
        public int AwayScore { get; set; }



        // Navigation
        public ICollection<PlayerStat> PlayerStats { get; set; } = new List<PlayerStat>();
    }
}