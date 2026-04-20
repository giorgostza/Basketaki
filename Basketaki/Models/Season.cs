using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Season
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        [Display(Name = "Season Name")]
        public string Name { get; set; } = null!; 


        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; }  


        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; }   


     
        public ICollection<League> Leagues { get; set; } = new List<League>();   
        public ICollection<PlayerSeasonTeam> PlayerSeasonTeams { get; set; } = new List<PlayerSeasonTeam>();  
    }
}