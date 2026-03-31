using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class TeamSeasonLeague    // In wich League participates the Team
    {
        public int Id { get; set; }

        [Required]
        public int TeamId { get; set; }          
        public Team Team { get; set; } = null!;   //  Gran Camaria


        [Required]
        public int LeagueId { get; set; }
        public League League { get; set; } = null!;    // Master League

        [Required]
        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;


        public ICollection<Match> HomeMatches { get; set; } = new List<Match>();
        public ICollection<Match> AwayMatches { get; set; } = new List<Match>();
    }
}