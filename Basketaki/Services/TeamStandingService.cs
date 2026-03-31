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

        // Read-only methods with AsNoTracking for performance
        public async Task<List<TeamStanding>> GetAllAsync()
        {
            return await _context.TeamStandings
                .AsNoTracking()
                .Include(ts => ts.TeamSeasonLeague)
                    .ThenInclude(tsl => tsl.Team)
                .Include(ts => ts.TeamSeasonLeague)
                    .ThenInclude(tsl => tsl.League)
                .OrderByDescending(ts => ts.LeaguePoints)
                .ThenByDescending(ts => ts.Wins)
                .ThenBy(ts => ts.Losses)
                .ToListAsync();
        }

        public async Task<TeamStanding?> GetByIdAsync(int id)
        {
            return await _context.TeamStandings
                .AsNoTracking()
                .Include(ts => ts.TeamSeasonLeague)
                    .ThenInclude(tsl => tsl.Team)
                .Include(ts => ts.TeamSeasonLeague)
                    .ThenInclude(tsl => tsl.League)
                .FirstOrDefaultAsync(ts => ts.Id == id);
        }

        public async Task<TeamStanding?> GetByTeamSeasonLeagueIdAsync(int teamSeasonLeagueId)
        {
            return await _context.TeamStandings
                .AsNoTracking()
                .Include(ts => ts.TeamSeasonLeague)
                    .ThenInclude(tsl => tsl.Team)
                .Include(ts => ts.TeamSeasonLeague)
                    .ThenInclude(tsl => tsl.League)
                .FirstOrDefaultAsync(ts => ts.TeamSeasonLeagueId == teamSeasonLeagueId);
        }

        // Create a new TeamStanding
        public async Task<SimpleResult> CreateAsync(TeamStanding standing)
        {
            if (!await _context.TeamSeasonLeagues.AnyAsync(tsl => tsl.Id == standing.TeamSeasonLeagueId))
            {
                return new SimpleResult { Success = false, Message = "TeamSeasonLeague does not exist." };
            }

            if (await _context.TeamStandings.AnyAsync(ts => ts.TeamSeasonLeagueId == standing.TeamSeasonLeagueId))
            {
                return new SimpleResult { Success = false, Message = "Standing for this TeamSeasonLeague already exists." };
            }

            if (standing.LeaguePoints < 0 || standing.Wins < 0 || standing.Losses < 0)
            {
                return new SimpleResult { Success = false, Message = "LeaguePoints, Wins, or Losses cannot be negative." };
            }

            _context.TeamStandings.Add(standing);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        // Update an existing TeamStanding
        public async Task<SimpleResult> UpdateAsync(TeamStanding standing)
        {
            var existing = await _context.TeamStandings.FirstOrDefaultAsync(ts => ts.Id == standing.Id);
            if (existing == null)
            {
                return new SimpleResult { Success = false, Message = "TeamStanding not found." };
            }

            if (standing.LeaguePoints < 0 || standing.Wins < 0 || standing.Losses < 0)
            {
                return new SimpleResult { Success = false, Message = "LeaguePoints, Wins, or Losses cannot be negative." };
            }

            _context.TeamStandings.Update(standing);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        // Delete a TeamStanding
        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var standing = await _context.TeamStandings.FindAsync(id);
            if (standing == null)
            {
                return new SimpleResult { Success = false, Message = "TeamStanding not found." };
            }

            _context.TeamStandings.Remove(standing);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }
    }
}