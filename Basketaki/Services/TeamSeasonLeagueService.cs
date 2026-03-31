using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class TeamSeasonLeagueService : ITeamSeasonLeagueService
    {
        private readonly ApplicationDbContext _context;

        public TeamSeasonLeagueService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TeamSeasonLeague>> GetAllAsync()
        {
            return await _context.TeamSeasonLeagues
                                 .AsNoTracking()
                                 .Include(tsl => tsl.Team)
                                 .Include(tsl => tsl.League)
                                        .ThenInclude(l => l.Season)
                                 .OrderBy(tsl => tsl.Team.Name)
                                 .ThenBy(tsl => tsl.League.Name)
                                 .ToListAsync();
        }

        public async Task<TeamSeasonLeague?> GetByIdAsync(int id)
        {
            return await _context.TeamSeasonLeagues
                                 .AsNoTracking()
                                 .Include(tsl => tsl.Team)
                                 .Include(tsl => tsl.League)
                                        .ThenInclude(l => l.Season)
                                 .FirstOrDefaultAsync(tsl => tsl.Id == id);
        }

        public async Task<SimpleResult> CreateAsync(TeamSeasonLeague model)
        {
            if (!await _context.Teams.AnyAsync(t => t.Id == model.TeamId))
                return new SimpleResult { Success = false, Message = "Team not found" };

            if (!await _context.Leagues.AnyAsync(l => l.Id == model.LeagueId))
                return new SimpleResult { Success = false, Message = "League not found" };

            if (await CombinationExistsAsync(model.TeamId, model.LeagueId))
                return new SimpleResult { Success = false, Message = "Team already participates in this League" };

            _context.TeamSeasonLeagues.Add(model);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var entity = await _context.TeamSeasonLeagues.FindAsync(id);

            if (entity == null)
                return new SimpleResult { Success = false, Message = "Association not found" };

            var hasMatches = await _context.Matches.AnyAsync(m => m.HomeTeamSeasonLeagueId == id || m.AwayTeamSeasonLeagueId == id);
            if (hasMatches)
                return new SimpleResult { Success = false, Message = "Cannot delete: Matches exist for this Team-League" };

            var league = await _context.Leagues.FirstOrDefaultAsync(l => l.Id == entity.LeagueId);
            if (league == null)
                return new SimpleResult { Success = false, Message = "League not found" };

            var hasPlayers = await _context.PlayerSeasonTeams.AnyAsync(pst => pst.TeamId == entity.TeamId && pst.SeasonId == league.SeasonId);
            if (hasPlayers)
                return new SimpleResult { Success = false, Message = "Cannot delete: Players assigned to this Team in the Season" };

            _context.TeamSeasonLeagues.Remove(entity);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

       

        public async Task<bool> CombinationExistsAsync(int teamId, int leagueId)
        {
            return await _context.TeamSeasonLeagues.AnyAsync(tsl => tsl.TeamId == teamId && tsl.LeagueId == leagueId);
        }
    }
}