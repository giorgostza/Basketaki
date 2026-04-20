using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Basketaki.ViewModels
{
    public class MatchPhotoFormViewModel
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(500)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = null!;


        [Required]
        [Display(Name = "Match")]
        public int MatchId { get; set; }



        public List<SelectListItem> Matches { get; set; } = new();

    }
}
