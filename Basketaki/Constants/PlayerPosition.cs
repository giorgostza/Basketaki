using System.ComponentModel.DataAnnotations;

namespace Basketaki.Constants
{
    public enum PlayerPosition
    {

        [Display(Name = "Point Guard")] Point_Guard,
        [Display(Name = "Shooting Guard")] Shooting_Guard,
        [Display(Name = "Small Forward")] Small_Forward,
        [Display(Name = "Power Forward")] Power_Forward,
        [Display(Name = "Center")] Center      

    }
}
