using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(100)]
        public string? CoachName { get; set; }




        // Navigation
        public ICollection<TeamSeasonLeague> TeamSeasonLeagues { get; set; } = new List<TeamSeasonLeague>();
        public ICollection<PlayerSeasonTeam> PlayerSeasonTeams { get; set; } = new List<PlayerSeasonTeam>();
    }
}