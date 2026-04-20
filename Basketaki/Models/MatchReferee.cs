namespace Basketaki.Models
{
    public class MatchReferee
    {

        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;


        public int RefereeId { get; set; }
        public Referee Referee { get; set; } = null!;


    }
}
