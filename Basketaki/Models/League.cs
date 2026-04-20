using System.ComponentModel.DataAnnotations;


namespace Basketaki.Models
{
    public class League
    {
        public int Id { get; set; }


        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; } = null!;  


        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string City { get; set; } = null!;  

                
        public int SeasonId { get; set; }   
        public Season Season { get; set; } = null!;   


        
        public ICollection<TeamSeasonLeague> TeamSeasonLeagues { get; set; } = new List<TeamSeasonLeague>();   
        public ICollection<Match> Matches { get; set; } = new List<Match>();  
    }
}