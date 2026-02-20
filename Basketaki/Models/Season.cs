using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Season
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = null!; // "2025-2026"

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }



        // Navigation
        public ICollection<League> Leagues { get; set; } = new List<League>();
        public ICollection<PlayerSeasonTeam> PlayerSeasonTeams { get; set; } = new List<PlayerSeasonTeam>();
    }
}