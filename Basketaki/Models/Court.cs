using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Court
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; } = null!;   // Basketaki Arena

        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string Location { get; set; } = null!;   // Menidi

        [MaxLength(300)]
        public string? Description { get; set; }    // Description of the Court for example if it is Indoor/Outdoor


        // Navigation property
        public ICollection<Match> Matches { get; set; } = new List<Match>();   // The games that take place in this Court
    }
}
