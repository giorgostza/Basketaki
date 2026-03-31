using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Coach
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        public string FullName => $"{FirstName} {LastName}";

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Range(150, 250)]
        public int Height { get; set; }

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

       
    }
}
