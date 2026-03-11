using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basketaki.Models
{
    public class League
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; } = null!;  // Master League

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string City { get; set; } = null!;  //  Athens



        // Season
        [Required]
        public int SeasonId { get; set; }   

        [ForeignKey("SeasonId")]
        public Season Season { get; set; } = null!;   //   Each League belongs to one Season.



        // Navigation
        public ICollection<TeamSeasonLeague> TeamSeasonLeagues { get; set; } = new List<TeamSeasonLeague>();   // One League has a lot of Teams
        public ICollection<Match> Matches { get; set; } = new List<Match>();  //  One League has a lot of Matches
    }
}