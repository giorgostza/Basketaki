namespace Basketaki.Models
{
    public class TeamSeasonLeague    // In wich League participates the Team
    {
        public int Id { get; set; }


        public int TeamId { get; set; }          
        public Team Team { get; set; } = null!;   //  Gran Camaria



        public int LeagueId { get; set; }
        public League League { get; set; } = null!;    // Master League
    }
}