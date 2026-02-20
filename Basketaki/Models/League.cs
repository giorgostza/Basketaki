using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basketaki.Models
{
    public class League
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = null!;



        // Season
        [Required]
        public int SeasonId { get; set; }

        [ForeignKey("SeasonId")]
        public Season Season { get; set; } = null!;



        // Navigation
        public ICollection<TeamSeasonLeague> TeamSeasonLeagues { get; set; } = new List<TeamSeasonLeague>();
        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}