using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Basketaki.ViewModels
{
    public class PlayerSeasonTeamFormViewModel
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "Player")]
        public int PlayerId { get; set; }


        [Required]
        [Display(Name = "Team")]
        public int TeamId { get; set; }


        [Required]
        [Display(Name = "Season")]
        public int SeasonId { get; set; }


        [Required]
        [Range(0, 99)]
        [Display(Name = "Jersey Number")]
        public int JerseyNumber { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Join Date")]
        public DateOnly JoinDate { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Leave Date")]
        public DateOnly? LeaveDate { get; set; }



        public List<SelectListItem> Players { get; set; } = new();
        public List<SelectListItem> Teams { get; set; } = new();
        public List<SelectListItem> Seasons { get; set; } = new();

    }
}
