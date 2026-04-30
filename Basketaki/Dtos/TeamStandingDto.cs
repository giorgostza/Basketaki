namespace Basketaki.Dtos
{
    public class TeamStandingDto
    {
        public int Id { get; set; }

        public int TeamSeasonLeagueId { get; set; }

        public string TeamName { get; set; } = null!;

        public string LeagueName { get; set; } = null!;

        public int Played { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public int PointsFor { get; set; }

        public int PointsAgainst { get; set; }

        public int PointDifference { get; set; }

        public int LeaguePoints { get; set; }

        public int NoShow { get; set; }

        public int CurrentStreak { get; set; }
    }
}