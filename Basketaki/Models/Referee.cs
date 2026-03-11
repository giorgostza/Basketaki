using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Referee
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;


        [Range(18, 70)]
        public int Age { get; set; }

        [Range(150, 250)]
        public double Height { get; set; }


        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        public ICollection<MatchReferee> MatchReferees { get; set; } = new List<MatchReferee>();



    }
}
