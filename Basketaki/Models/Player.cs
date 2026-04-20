using Basketaki.Constants;
using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Player
    {
        public int Id { get; set; }


        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;  


        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string LastName { get; set; } = null!; 


        public string FullName => $"{FirstName} {LastName}";


        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }


        [Range(150, 250)]
        public int Height { get; set; }  


        [Range(50, 200)]
        public int Weight { get; set; }    


        [Required]
        public PlayerPosition Position { get; set; }   


        [MaxLength(500)]
        public string? PhotoUrl { get; set; }



        public ICollection<PlayerSeasonTeam> PlayerSeasonTeams { get; set; } = new List<PlayerSeasonTeam>(); 

    }
}
