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
                .AsNoTracking()
                .Include(mr => mr.Referee)
                .Where(mr => mr.MatchId == matchId)
                .OrderBy(mr => mr.Referee.LastName)
                .ThenBy(mr => mr.Referee.FirstName)
                .ToListAsync();
        }

        public async Task<SimpleResult> AssignRefereeAsync(int matchId, int refereeId)
        {
            var match = await _context.Matches
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == matchId);

            if (match == null)
            {
                return new SimpleResult { Success = false, Message = "Match not found" };
            }

            var refereeExists = await _context.Referees.AnyAsync(r => r.Id == refereeId);

            if (!refereeExists)
            {
                return new SimpleResult { Success = false, Message = "Referee not found" };
            }

            if (await ExistsAsync(matchId, refereeId))
            {
                return new SimpleResult { Success = false, Message = "Referee already assigned to this match" };
            }

            var conflict = await _context.MatchReferees
                .Include(mr => mr.Match)
                .AnyAsync(mr =>
                    mr.RefereeId == refereeId &&
                    mr.Match.MatchDate == match.MatchDate &&
                    mr.Match.StartTime == match.StartTime);

            if (conflict)
            {
                return new SimpleResult { Success = false, Message = "Referee has another match at this time" };
            }

            var matchReferee = new MatchReferee
            {
                MatchId = matchId,
                RefereeId = refereeId
            };

            _context.MatchReferees.Add(matchReferee);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<SimpleResult> RemoveRefereeAsync(int matchId, int refereeId)
        {
            if (!await ExistsAsync(matchId, refereeId))
            {
                return new SimpleResult { Success = false, Message = "Assignment not found" };
            }

            var entity = await _context.MatchReferees
                .FirstAsync(mr => mr.MatchId == matchId && mr.RefereeId == refereeId);

            _context.MatchReferees.Remove(entity);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<bool> ExistsAsync(int matchId, int refereeId)
        {
            return await _context.MatchReferees
                .AnyAsync(mr => mr.MatchId == matchId && mr.RefereeId == refereeId);
        }
    }
}