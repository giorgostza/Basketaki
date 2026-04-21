using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Basketaki.ViewModels
{
    public class LeagueFormViewModel
    {
        public int Id { get; set; }


        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        [Display(Name = "League Name")]
        public string Name { get; set; } = null!;


        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        [Display(Name = "City")]
        public string City { get; set; } = null!;


        [Required]
        [Display(Name = "Season")]
        public int SeasonId { get; set; }



        public List<SelectListItem> Seasons { get; set; } = new();
    }
}
