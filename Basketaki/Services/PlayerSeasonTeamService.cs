using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class PlayerSeasonTeamService : IPlayerSeasonTeamService
    {
        private readonly ApplicationDbContext _context;

        public PlayerSeasonTeamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PlayerSeasonTeam>> GetAllAsync()
        {
            return await _context.PlayerSeasonTeams
                .AsNoTracking()
                .Include(pst => pst.Player)
                .Include(pst => pst.Team)
                .Include(pst => pst.Season)
                .OrderBy(pst => pst.Season.StartDate)
                .ThenBy(pst => pst.Team.Name)
                .ThenBy(pst => pst.Player.LastName)
                .ThenBy(pst => pst.Player.FirstName)
                .ToListAsync();
        }

        public async Task<PlayerSeasonTeam?> GetByIdAsync(int id)
        {
            return await _context.PlayerSeasonTeams
                .AsNoTracking()
                .Include(pst => pst.Player)
                .Include(pst => pst.Team)
                .Include(pst => pst.Season)
                .FirstOrDefaultAsync(pst => pst.Id == id);
        }

        public async Task<SimpleResult> CreateAsync(PlayerSeasonTeam model)
        {
            var playerExists = await _context.Players.AnyAsync(p => p.Id == model.PlayerId);
            var teamExists = await _context.Teams.AnyAsync(t => t.Id == model.TeamId);
            var seasonExists = await _context.Seasons.AnyAsync(s => s.Id == model.SeasonId);

            if (!playerExists || !teamExists || !seasonExists)
            {
                return new SimpleResult { Success = false, Message = "Invalid Player, Team or Season" };
            }

            if (await CombinationExistsAsync(model.PlayerId, model.SeasonId))
            {
                return new SimpleResult { Success = false, Message = "Player already assigned in this season" };
            }

            _context.PlayerSeasonTeams.Add(model);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<SimpleResult> DeleteAsync(int id)
        {
            if (!await ExistsAsync(id))
            {
                return new SimpleResult { Success = false, Message = "Record not found" };
            }

            var hasStats = await _context.PlayerStats
                .AnyAsync(ps => ps.PlayerSeasonTeamId == id);

            if (hasStats)
            {
                return new SimpleResult { Success = false, Message = "Cannot delete record with stats" };
            }

            var entity = await _context.PlayerSeasonTeams.FindAsync(id);

            _context.PlayerSeasonTeams.Remove(entity!);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.PlayerSeasonTeams
                .AnyAsync(pst => pst.Id == id);
        }

        public async Task<bool> CombinationExistsAsync(int playerId, int seasonId)
        {
            return await _context.PlayerSeasonTeams
                .AnyAsync(pst => pst.PlayerId == playerId && pst.SeasonId == seasonId);
        }
    }
}