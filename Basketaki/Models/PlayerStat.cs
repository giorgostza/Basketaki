namespace Basketaki.Models
{
    public class PlayerStat
    {
        public int Id { get; set; }


        public int PlayerSeasonTeamId { get; set; }
        public PlayerSeasonTeam PlayerSeasonTeam { get; set; } = null!;



        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;



        public int Points { get; set; }
        public int Rebounds { get; set; }
        public int Assists { get; set; }
        public int Steals { get; set; }
        public int Blocks { get; set; }
        public int Fouls { get; set; }

    }
}