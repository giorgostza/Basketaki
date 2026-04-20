using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Referee
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


        [MaxLength(500)]
        public string? PhotoUrl { get; set; }



        public ICollection<MatchReferee> MatchReferees { get; set; } = new List<MatchReferee>();

    }
}
