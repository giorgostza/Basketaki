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

       
        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        public int? CoachId { get; set; }
        public Coach? Coach { get; set; }


        // Navigation
        public ICollection<TeamSeasonLeague> TeamSeasonLeagues { get; set; } = new List<TeamSeasonLeague>();   // In wich Leagues participates
        public ICollection<PlayerSeasonTeam> PlayerSeasonTeams { get; set; } = new List<PlayerSeasonTeam>();   //  The Team's Players by Season
    }
}