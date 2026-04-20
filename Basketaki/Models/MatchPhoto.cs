using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class MatchPhoto
    {

        public int Id { get; set; }


        [Required]
        [MaxLength(500)]
        public string ImageUrl { get; set; } = null!;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;

    }
}
