using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Court
    {
        public int Id { get; set; }


        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        [Display(Name = "Court Name")]
        public string Name { get; set; } = null!;   


        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        [Display(Name = "Location")]
        public string Location { get; set; } = null!;   


        [MaxLength(300)]
        [Display(Name = "Description")]
        public string? Description { get; set; }    



       
        public ICollection<Match> Matches { get; set; } = new List<Match>();   
    }
}
