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
        public DateOnly StartDate { get; set; }  //   Start: 01/09/2025

        [Required]
        public DateOnly EndDate { get; set; }   //   End: 30/06/2026



        // Navigation
        public ICollection<League> Leagues { get; set; } = new List<League>();   // One Season has a lot of Leagues 
        public ICollection<PlayerSeasonTeam> PlayerSeasonTeams { get; set; } = new List<PlayerSeasonTeam>();  //  It shows in which Team each player plays for this particular Season.
    }
}