using Basketaki.Constants;
using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;  // George

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;  //Tzavellas

        public string FullName => $"{FirstName} {LastName}";

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Range(150, 250)]
        public int Height { get; set; }  // 191 cm

        [Range(50, 200)]
        public int Weight { get; set; }    //   100 kg


        [Required]
        public PlayerPosition Position { get; set; }   // Center

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }



        public ICollection<PlayerSeasonTeam> PlayerSeasonTeams { get; set; } = new List<PlayerSeasonTeam>(); // In wich Team plays per Season 

    }
}
