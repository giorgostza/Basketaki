using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class MatchPhoto
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Url { get; set; } = null!;

        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;

    }
}
