using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Court
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string Location { get; set; } = null!;

        // Navigation property
        public ICollection<Match> Matches { get; set; } = new List<Match>();
    }
}
