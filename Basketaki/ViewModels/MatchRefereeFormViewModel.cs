using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Basketaki.ViewModels
{
    public class MatchRefereeFormViewModel
    {
        [Required]
        public int MatchId { get; set; }


        [Required]
        [Display(Name = "Referee")]
        public int RefereeId { get; set; }


        public string MatchDisplayName { get; set; } = null!;



        public List<SelectListItem> Referees { get; set; } = new();

    }
}
