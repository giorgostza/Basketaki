namespace Basketaki.Dtos
{
    public class TeamDto
    {

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string City { get; set; } = null!;

        public string? PhotoUrl { get; set; }



        public int? CoachId { get; set; }

        public string? CoachName { get; set; }


    }

}
