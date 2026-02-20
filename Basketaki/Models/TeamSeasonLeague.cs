namespace Basketaki.Models
{
    public class TeamSeasonLeague
    {
        public int Id { get; set; }


        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;



        public int LeagueId { get; set; }
        public League League { get; set; } = null!;
    }
}