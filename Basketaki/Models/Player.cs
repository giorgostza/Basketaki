using Basketaki.Constants;
using Basketaki.Models;
using System.ComponentModel.DataAnnotations;

public class Player
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;  // George

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;  //Tzavellas

    [Range(150, 250)]
    public double Height { get; set; }  // 191 cm

    [Range(50, 200)]
    public int Weight { get; set; }    //   100 kg

    [Range(0, 99)]
    public int JerseyNumber { get; set; }  // 91

    [Required]
    public PlayerPosition Position { get; set; }   // Center

    [MaxLength(500)]
    public string? PlayerPhotoUrl { get; set; }  

    public ICollection<PlayerSeasonTeam> PlayerSeasonTeams { get; set; } = new List<PlayerSeasonTeam>(); // In wich Team plays per Season 
}