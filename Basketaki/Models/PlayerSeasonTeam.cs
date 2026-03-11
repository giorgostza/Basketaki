using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class PlayerSeasonTeam             //   In wich Team the Player plays in the particular Season
    {
        public int Id { get; set; }

        [Required]
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;  // George

        [Required]
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;   // Gran Camaria

        [Required]
        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;  //  "2025-2026"
    }
}
