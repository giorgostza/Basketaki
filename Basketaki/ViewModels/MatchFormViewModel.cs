using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Basketaki.ViewModels
{
    public class MatchFormViewModel
    {
        public int Id { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Match Date")]
        public DateOnly MatchDate { get; set; }


        [Required]
        [Display(Name = "Start Time")]
        public TimeOnly StartTime { get; set; }


        [Required]
        [Display(Name = "End Time")]
        public TimeOnly EndTime { get; set; }


        [Required]
        [Display(Name = "Court")]
        public int CourtId { get; set; }


        [Required]
        [Display(Name = "League")]
        public int LeagueId { get; set; }


        [Required]
        [Display(Name = "Home Team")]
        public int HomeTeamSeasonLeagueId { get; set; }


        [Required]
        [Display(Name = "Away Team")]
        public int AwayTeamSeasonLeagueId { get; set; }


        [Range(0, 300)]
        [Display(Name = "Home Score")]
        public int? HomeScore { get; set; }


        [Range(0, 300)]
        [Display(Name = "Away Score")]
        public int? AwayScore { get; set; }


        [Display(Name = "Played")]
        public bool IsPlayed { get; set; }



        public List<SelectListItem> Courts { get; set; } = new();
        public List<SelectListItem> Leagues { get; set; } = new();
        public List<SelectListItem> HomeTeams { get; set; } = new();
        public List<SelectListItem> AwayTeams { get; set; } = new();
    }
}
