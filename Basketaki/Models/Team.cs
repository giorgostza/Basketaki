using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Name { get; set; } = null!;  // Gran Camaria

        [MaxLength(100)]
        public string? CoachName { get; set; }   //  Athanasiou

        [MaxLength(500)]
        public string? LogoPhotoUrl { get; set; }

        // Navigation
        public ICollection<TeamSeasonLeague> TeamSeasonLeagues { get; set; } = new List<TeamSeasonLeague>();   // In wich Leagues participates
        public ICollection<PlayerSeasonTeam> PlayerSeasonTeams { get; set; } = new List<PlayerSeasonTeam>();   //  The Team's Players by Season
    }
}