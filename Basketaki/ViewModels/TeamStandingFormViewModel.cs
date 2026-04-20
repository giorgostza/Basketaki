using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Basketaki.ViewModels
{
    public class TeamStandingFormViewModel
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "Team League Assignment")]
        public int TeamSeasonLeagueId { get; set; }


        [Range(0, int.MaxValue)]
        public int Played { get; set; }


        [Range(0, int.MaxValue)]
        public int Wins { get; set; }


        [Range(0, int.MaxValue)]
        public int Losses { get; set; }


        [Range(0, int.MaxValue)]
        [Display(Name = "Points For")]
        public int PointsFor { get; set; }


        [Range(0, int.MaxValue)]
        [Display(Name = "Points Against")]
        public int PointsAgainst { get; set; }


        [Range(0, int.MaxValue)]
        [Display(Name = "League Points")]
        public int LeaguePoints { get; set; }


        [Range(0, int.MaxValue)]
        [Display(Name = "No Show")]
        public int NoShow { get; set; }


        [Display(Name = "Current Streak")]
        public int CurrentStreak { get; set; }


        public List<SelectListItem> TeamSeasonLeagues { get; set; } = new();

    }
}
