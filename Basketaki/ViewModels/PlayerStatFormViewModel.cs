using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Basketaki.ViewModels
{
    public class PlayerStatFormViewModel
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "Player Assignment")]
        public int PlayerSeasonTeamId { get; set; }


        [Required]
        [Display(Name = "Match")]
        public int MatchId { get; set; }


        [Range(0, 200)]
        public int Points { get; set; }


        [Range(0, 200)]
        [Display(Name = "Offensive Rebounds")]
        public int OffensiveRebounds { get; set; }


        [Range(0, 200)]
        [Display(Name = "Defensive Rebounds")]
        public int DefensiveRebounds { get; set; }


        [Range(0, 200)]
        public int Assists { get; set; }


        [Range(0, 200)]
        public int Steals { get; set; }


        [Range(0, 200)]
        public int Blocks { get; set; }


        [Range(0, 5)]
        public int Fouls { get; set; }


        [Range(0, 80)]
        [Display(Name = "Minutes Played")]
        public int MinutesPlayed { get; set; }


        [Range(0, 100)]
        [Display(Name = "Free Throws Made")]
        public int FreeThrowsMade { get; set; }


        [Range(0, 200)]
        [Display(Name = "Free Throws Attempted")]
        public int FreeThrowsAttempted { get; set; }


        [Range(0, 100)]
        [Display(Name = "Two Points Made")]
        public int TwoPointsMade { get; set; }


        [Range(0, 200)]
        [Display(Name = "Two Points Attempted")]
        public int TwoPointsAttempted { get; set; }


        [Range(0, 100)]
        [Display(Name = "Three Points Made")]
        public int ThreePointsMade { get; set; }


        [Range(0, 200)]
        [Display(Name = "Three Points Attempted")]
        public int ThreePointsAttempted { get; set; }


        [Display(Name = "MVP")]
        public bool IsMVP { get; set; }


        [Range(0, 3)]
        [Display(Name = "Suspension Games")]
        public int SuspensionGames { get; set; }



        public List<SelectListItem> Matches { get; set; } = new();
        public List<SelectListItem> PlayerSeasonTeams { get; set; } = new();

    }
}
