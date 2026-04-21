using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Referee
    {

        public int Id { get; set; }


        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;


        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;


        public string FullName => $"{FirstName} {LastName}";


        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateOnly DateOfBirth { get; set; }


        [Range(150, 250)]
        [Display(Name = "Height (cm)")]
        public int Height { get; set; }


        [MaxLength(500)]
        [Display(Name = "Photo URL")]
        public string? PhotoUrl { get; set; }



        public ICollection<MatchReferee> MatchReferees { get; set; } = new List<MatchReferee>();

    }
}
