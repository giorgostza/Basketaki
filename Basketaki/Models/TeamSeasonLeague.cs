using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class TeamSeasonLeague    
    {
        public int Id { get; set; }


        [Required]
        public int TeamId { get; set; }          
        public Team Team { get; set; } = null!;   


        [Required]
        public int LeagueId { get; set; }
        public League League { get; set; } = null!;    

       

        public ICollection<Match> HomeMatches { get; set; } = new List<Match>();
        public ICollection<Match> AwayMatches { get; set; } = new List<Match>();
    }
}