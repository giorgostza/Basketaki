using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Range(0, 99)]
        public int JerseyNumber { get; set; }

        [MaxLength(20)]
        public string? Position { get; set; }



        // Navigation
        public ICollection<PlayerSeasonTeam> PlayerSeasonTeams { get; set; } = new List<PlayerSeasonTeam>();
        public ICollection<PlayerStat> PlayerStats { get; set; } = new List<PlayerStat>();
    }
}