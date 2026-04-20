using Basketaki.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Basketaki.ViewModels
{
    public class PlayerFormViewModel
    {
        public int Id { get; set; }


        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;


        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;


        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateOnly DateOfBirth { get; set; }


        [Range(150, 250)]
        public int Height { get; set; }


        [Range(50, 200)]
        public int Weight { get; set; }


        [Required]
        public PlayerPosition Position { get; set; }


        [MaxLength(500)]
        [Display(Name = "Photo URL")]
        public string? PhotoUrl { get; set; }



        public List<SelectListItem> Positions { get; set; } = new();

    }
}
