using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class TeamStandingService : ITeamStandingService
    {
        private readonly ApplicationDbContext _context;

        public TeamStandingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TeamStanding>> GetAllAsync() // This method retrieves all team standings from the database
        {                                                   // including related team and league information, and orders them by league points, wins, and losses.
            return await _context.TeamStandings
                .Include(ts => ts.TeamSeasonLeague)
                    .ThenInclude(tsl => tsl.Team)
                .Include(ts => ts.TeamSeasonLeague)
                    .ThenInclude(tsl => tsl.League)
                .OrderByDescending(ts => ts.LeaguePoints)     
                .ThenByDescending(ts => ts.Wins)
                .ThenBy(ts => ts.Losses)
                .ToListAsync();
        }

        public async Task<TeamStanding?> GetByIdAsync(int id) // This method retrieves a specific team standing by its ID, including related team and league information.
        {                                                     // It returns null if the standing is not found.

            return await _context.TeamStandings
                .Include(ts => ts.TeamSeasonLeague)
                    .ThenInclude(tsl => tsl.Team)
                .Include(ts => ts.TeamSeasonLeague)
                    .ThenInclude(tsl => tsl.League)
                .FirstOrDefaultAsync(ts => ts.Id == id);

        }

        public async Task<TeamStanding?> GetByTeamSeasonLeagueIdAsync(int teamSeasonLeagueId) // This method retrieves a team standing based on the TeamSeasonLeagueId
        {                                                                                     // including related team and league information
                                                                                              // It returns null if not found.
            return await _context.TeamStandings
                .Include(ts => ts.TeamSeasonLeague)
                    .ThenInclude(tsl => tsl.Team)
                .Include(ts => ts.TeamSeasonLeague)
                    .ThenInclude(tsl => tsl.League)
                .FirstOrDefaultAsync(ts => ts.TeamSeasonLeagueId == teamSeasonLeagueId);

        }

        public async Task<bool> CreateAsync(TeamStanding standing)
        {
            // Validate that the specified TeamSeasonLeagueId exists 
            if (!await _context.TeamSeasonLeagues.AnyAsync(tsl => tsl.Id == standing.TeamSeasonLeagueId))
            {

                return false;

            }

            // Ensure that there isn't already a standing for the same TeamSeasonLeagueId
            if (await _context.TeamStandings.AnyAsync(ts => ts.TeamSeasonLeagueId == standing.TeamSeasonLeagueId))
            {

                return false;

            }

            // Validate that LeaguePoints, Wins, and Losses are not negative
            if (standing.LeaguePoints < 0 || standing.Wins < 0 || standing.Losses < 0)
            {

                return false;

            }

            _context.TeamStandings.Add(standing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(TeamStanding standing)
        {
            if (!await ExistsAsync(standing.Id)) // Validate that the standing exists before trying to update
            {

                return false;

            }

            // Validate that the specified TeamSeasonLeagueId exists
            if (standing.LeaguePoints < 0 || standing.Wins < 0 || standing.Losses < 0)
            {

                return false;

            }

            _context.TeamStandings.Update(standing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var standing = await _context.TeamStandings.FindAsync(id);

            if (standing == null)
            {

                return false;

            }

            _context.TeamStandings.Remove(standing);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id) // This method checks if a team standing with the specified ID exists 
        {

            return await _context.TeamStandings.AnyAsync(ts => ts.Id == id);

        }
    }
}