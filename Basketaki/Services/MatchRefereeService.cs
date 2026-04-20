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

            return await _context.MatchReferees.AsNoTracking().Include(mr => mr.Referee)
                                                              .Where(mr => mr.MatchId == matchId)
                                                              .OrderBy(mr => mr.Referee.LastName)
                                                              .ThenBy(mr => mr.Referee.FirstName)
                                                              .ToListAsync();

        }

        public async Task<SimpleResult> CreateAsync(int matchId, int refereeId)
        {
            var match = await _context.Matches.AsNoTracking().FirstOrDefaultAsync(m => m.Id == matchId);

            if (match == null)
            {

                return SimpleResult.Fail("Match not found.");

            }


            var refereeExists = await _context.Referees.AnyAsync(r => r.Id == refereeId);

            if (!refereeExists)
            {

                return SimpleResult.Fail("Referee not found.");

            }


            var alreadyAssigned = await _context.MatchReferees.AnyAsync(mr => mr.MatchId == matchId && mr.RefereeId == refereeId);

            if (alreadyAssigned)
            {

                return SimpleResult.Fail("Referee already assigned to this match.");

            }



            var conflict = await _context.MatchReferees.Include(mr => mr.Match).AnyAsync(mr => mr.RefereeId == refereeId &&
                                                                                               mr.Match.MatchDate == match.MatchDate &&
                                                                                               match.StartTime < mr.Match.EndTime &&
                                                                                               match.EndTime > mr.Match.StartTime);

            if (conflict)
            {

                return SimpleResult.Fail("Referee has another match during this time.");

            }



            var matchReferee = new MatchReferee
            {
                MatchId = matchId,
                RefereeId = refereeId
            };

            _context.MatchReferees.Add(matchReferee);



            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Referee assigned successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to assign referee to the match.");

            }

        }



        public async Task<SimpleResult> DeleteAsync(int matchId, int refereeId)
        {
            var entity = await _context.MatchReferees.FirstOrDefaultAsync(mr => mr.MatchId == matchId && mr.RefereeId == refereeId);

            if (entity == null)
            {

                return SimpleResult.Fail("Assignment not found.");

            }



            try
            {

                _context.MatchReferees.Remove(entity);
                await _context.SaveChangesAsync();

                return SimpleResult.Ok("Referee removed successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to remove referee from the match.");

            }


        }

    }
}