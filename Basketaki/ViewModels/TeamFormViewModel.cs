using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Basketaki.ViewModels
{
    public class TeamFormViewModel
    {
        public int Id { get; set; }


        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Name { get; set; } = null!;


        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string City { get; set; } = null!;


        [MaxLength(500)]
        [Display(Name = "Photo URL")]
        public string? PhotoUrl { get; set; }


        [Display(Name = "Coach")]
        public int? CoachId { get; set; }



        public List<SelectListItem> Coaches { get; set; } = new();

    }
}