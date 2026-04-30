using System.ComponentModel.DataAnnotations;

namespace Basketaki.Dtos
{
    public class TeamCreateUpdateDto
    {

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Name { get; set; } = null!;


        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string City { get; set; } = null!;


        [MaxLength(500)]
        public string? PhotoUrl { get; set; }


        public int? CoachId { get; set; }

    }
}
