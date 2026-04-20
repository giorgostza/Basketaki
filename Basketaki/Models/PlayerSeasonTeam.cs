using System.ComponentModel.DataAnnotations;

namespace Basketaki.Models
{
    public class PlayerSeasonTeam             
    {
        public int Id { get; set; }


        [Required]
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!; 
        

        [Required]
        [Range(0, 99)]
        public int JerseyNumber { get; set; }


        [Required]
        [DataType(DataType.Date)]
        public DateOnly JoinDate { get; set; }


        [DataType(DataType.Date)]
        public DateOnly? LeaveDate { get; set; }


        [Required]
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;   


        [Required]
        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;  


        public ICollection<PlayerStat> PlayerStats { get; set; } = new List<PlayerStat>();

    }
}
