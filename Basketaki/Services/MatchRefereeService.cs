using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class MatchRefereeService : IMatchRefereeService
    {
        private readonly ApplicationDbContext _context;

        public MatchRefereeService(ApplicationDbContext context)
        {

            _context = context;

        }

        public async Task<List<MatchReferee>> GetByMatchIdAsync(int matchId) 
        {

            return await _context.MatchReferees
                .Include(mr => mr.Referee)
                .Where(mr => mr.MatchId == matchId)
                .OrderBy(mr => mr.Referee.LastName)
                .ThenBy(mr => mr.Referee.FirstName)
                .ToListAsync();                      // Retrieve all Referees assigned to a specific Match, sorted by Last Name and then First Name

        }

        public async Task<bool> AssignRefereeAsync(int matchId, int refereeId)
        {
            
            var match = await _context.Matches.FirstOrDefaultAsync(m => m.Id == matchId);  // check if Match exists and bring it 

            if (match == null)
            {

                return false;

            }

           
            if (!await _context.Referees.AnyAsync(r => r.Id == refereeId))  //  Check if Referee exists by his Id
            {

                return false;

            }

            
            if (await ExistsAsync(matchId, refereeId))  
            {

                return false;

            }


            var conflict = await _context.MatchReferees
                .Include(mr => mr.Match)
                .AnyAsync(mr => mr.RefereeId == refereeId && mr.Match.MatchDate == match!.MatchDate && mr.Match.StartTime == match.StartTime);
            // Check for scheduling conflicts: Ensure the referee is not assigned to another match at the same date and time

            if (conflict)
            {

                return false;

            }

            var matchReferee = new MatchReferee  // Create a new MatchReferee entity with the provided MatchId and RefereeId
            {
                MatchId = matchId,
                RefereeId = refereeId
            };

            _context.MatchReferees.Add(matchReferee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefereeAsync(int matchId, int refereeId)
        {

            var entity = await _context.MatchReferees.FirstOrDefaultAsync(mr => mr.MatchId == matchId && mr.RefereeId == refereeId);
            // Find the MatchReferee entity based on the provided MatchId and RefereeId

            if (entity == null)
            {

                return false;

            }

            _context.MatchReferees.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int matchId, int refereeId)
        {

            return await _context.MatchReferees.AnyAsync(mr => mr.MatchId == matchId && mr.RefereeId == refereeId);  // Check if a specific Match-Referee assignment exists 
            // Check if a specific Match-Referee assignment exists 

        }
    }
}
