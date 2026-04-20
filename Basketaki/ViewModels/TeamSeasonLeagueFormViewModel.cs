using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Basketaki.ViewModels
{
    public class TeamSeasonLeagueFormViewModel
    {

        [Required]
        [Display(Name = "Team")]
        public int TeamId { get; set; }


        [Required]
        [Display(Name = "League")]
        public int LeagueId { get; set; }



        public List<SelectListItem> Teams { get; set; } = new();
        public List<SelectListItem> Leagues { get; set; } = new();
    }
}
