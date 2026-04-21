using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class Team
    {
        public int Id { get; set; }


        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        [Display(Name = "Team Name")]
        public string Name { get; set; } = null!;  


        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        [Display(Name = "City")]
        public string City { get; set; } = null!;


        [MaxLength(500)]
        [Display(Name = "Photo URL")]
        public string? PhotoUrl { get; set; }

        [Display(Name = "Coach")]
        public int? CoachId { get; set; }
        public Coach? Coach { get; set; }


              
        public ICollection<TeamSeasonLeague> TeamSeasonLeagues { get; set; } = new List<TeamSeasonLeague>();   // In wich Leagues participates
        public ICollection<PlayerSeasonTeam> PlayerSeasonTeams { get; set; } = new List<PlayerSeasonTeam>();   //  The Team's Players by Season
    }
}