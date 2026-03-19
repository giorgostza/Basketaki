using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext _context;

        public MatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Match>> GetAllAsync()
        {
            return await _context.Matches                  // Eagerly load related entities to avoid N+1 query issues
                .Include(m => m.Court)                     // Load the Court where the Match is played
                .Include(m => m.League)                    // Load the League to which the Match belongs
                .Include(m => m.HomeTeamSeasonLeague)      // Load the HomeTeam Season League details
                        .ThenInclude(tsl => tsl.Team)      // Load the HomeTeam details
                .Include(m => m.AwayTeamSeasonLeague)      // Load the AwayTeam Season League details
                        .ThenInclude(tsl => tsl.Team)      // Load the AwayTeam details
                .ToListAsync();                            // Execute the query and return the list of Matches with all related data
        }

        public async Task<Match?> GetByIdAsync(int id)
        {
            return await _context.Matches               // Eagerly load related entities to avoid N+1 query issues
                .Include(m => m.Court)                  // Load the Court where the Match is played
                .Include(m => m.League)                 // Load the League to which the Match belongs
                .Include(m => m.HomeTeamSeasonLeague)   // Load the HomeTeam Season League details
                       .ThenInclude(tsl => tsl.Team)    // Load the HomeTeam details
                .Include(m => m.AwayTeamSeasonLeague)   // Load the AwayTeam Season League details
                       .ThenInclude(tsl => tsl.Team)    // Load the AwayTeam details
                .Include(m => m.PlayerStats)            // Load the PlayerStats for the Match
                .Include(m => m.Photos)                 // Load the Match Photos
                .FirstOrDefaultAsync(m => m.Id == id);  // Execute the query and return the Match with all related data, or null if not found
        }

        public async Task<bool> CreateAsync(Match match)
        {

            if (match.HomeTeamSeasonLeagueId == match.AwayTeamSeasonLeagueId)  // Ensure that the Home and Away teams are not the same
            {

                return false;

            }
                

            
            if (match.StartTime >= match.EndTime)  // Ensure that the StartTime is before the EndTime
            {

                return false;

            }


            if (!await _context.Courts.AnyAsync(c => c.Id == match.CourtId))  // Ensure that the specified Court exists
            {

                return false;

            }


            if (!await _context.Leagues.AnyAsync(l => l.Id == match.LeagueId))  // Ensure that the specified League exists
            {

                return false;

            }


            // Check for scheduling conflicts on the same Court at the same Date and Time
            var conflict = await _context.Matches.AnyAsync(m => (m.CourtId == match.CourtId) && (m.MatchDate == match.MatchDate) && (m.StartTime == match.StartTime));  

            if (conflict)
            {

                return false;

            }

            _context.Matches.Add(match);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Match match)
        {
            if (!await ExistsAsync(match.Id))  // Ensure that the Match exists before trying to update it
            {

                return false;

            }

            if (match.HomeTeamSeasonLeagueId == match.AwayTeamSeasonLeagueId)  // Ensure that the Home and Away Teams are not the same
            {

                return false;

            }

            if (match.StartTime >= match.EndTime)  // Ensure that the StartTime is before the EndTime
            {

                return false;

            }


            // Check for scheduling conflicts on the same Court at the same Date and Time
            var conflict = await _context.Matches.AnyAsync(m => (m.Id != match.Id) && (m.CourtId == match.CourtId) && (m.MatchDate == match.MatchDate) && (m.StartTime == match.StartTime));  

            if (conflict)
            {

                return false;

            }

            _context.Matches.Update(match);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var match = await _context.Matches.FindAsync(id);  // Find the Match by its Id

            if (match == null)
            {

                return false;

            }

            _context.Matches.Remove(match);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Matches.AnyAsync(m => m.Id == id);  // Check if any Match exists with the specified Id
        }
    }
}
