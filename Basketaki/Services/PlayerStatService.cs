using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class PlayerStatService : IPlayerStatService
    {
        private readonly ApplicationDbContext _context;

        public PlayerStatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PlayerStat>> GetAllAsync()
        {
            return await _context.PlayerStats
                .AsNoTracking()
                .Include(ps => ps.PlayerSeasonTeam)
                        .ThenInclude(pst => pst.Player)
                .Include(ps => ps.PlayerSeasonTeam)
                        .ThenInclude(pst => pst.Team)
                .Include(ps => ps.Match)
                .OrderBy(ps => ps.Match.MatchDate)
                .ThenBy(ps => ps.PlayerSeasonTeam.Team.Name)
                .ThenBy(ps => ps.PlayerSeasonTeam.Player.LastName)
                .ToListAsync();
        }

        public async Task<PlayerStat?> GetByIdAsync(int id)
        {
            return await _context.PlayerStats
                .AsNoTracking()
                .Include(ps => ps.PlayerSeasonTeam)
                        .ThenInclude(pst => pst.Player)
                .Include(ps => ps.PlayerSeasonTeam)
                        .ThenInclude(pst => pst.Team)
                .Include(ps => ps.Match)
                .FirstOrDefaultAsync(ps => ps.Id == id);
        }

        public async Task<List<PlayerStat>> GetByMatchIdAsync(int matchId)
        {
            return await _context.PlayerStats
                .AsNoTracking()
                .Where(ps => ps.MatchId == matchId)
                .Include(ps => ps.PlayerSeasonTeam)
                       .ThenInclude(pst => pst.Player)
                .Include(ps => ps.PlayerSeasonTeam)
                       .ThenInclude(pst => pst.Team)
                .OrderBy(ps => ps.PlayerSeasonTeam.Team.Name)
                .ThenBy(ps => ps.PlayerSeasonTeam.Player.LastName)
                .ThenBy(ps => ps.PlayerSeasonTeam.Player.FirstName)
                .ToListAsync();
        }

        public async Task<SimpleResult> CreateAsync(PlayerStat playerStat)
        {
            if (playerStat.Points < 0)
            {
                return new SimpleResult { Success = false, Message = "Points cannot be negative" };
            }

            var pstExists = await _context.PlayerSeasonTeams
                .AnyAsync(p => p.Id == playerStat.PlayerSeasonTeamId);

            var matchExists = await _context.Matches
                .AnyAsync(m => m.Id == playerStat.MatchId);

            if (!pstExists || !matchExists)
            {
                return new SimpleResult { Success = false, Message = "Invalid Player or Match" };
            }

            var exists = await _context.PlayerStats
                .AnyAsync(ps => ps.PlayerSeasonTeamId == playerStat.PlayerSeasonTeamId &&
                                ps.MatchId == playerStat.MatchId);

            if (exists)
            {
                return new SimpleResult { Success = false, Message = "Stats already exist for this player in this match" };
            }

            _context.PlayerStats.Add(playerStat);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<SimpleResult> UpdateAsync(PlayerStat playerStat)
        {
            if (!await ExistsAsync(playerStat.Id))
            {
                return new SimpleResult { Success = false, Message = "Stat not found" };
            }

            var duplicate = await _context.PlayerStats
                .AnyAsync(ps => ps.Id != playerStat.Id &&
                                ps.PlayerSeasonTeamId == playerStat.PlayerSeasonTeamId &&
                                ps.MatchId == playerStat.MatchId);

            if (duplicate)
            {
                return new SimpleResult { Success = false, Message = "Duplicate stat for this player and match" };
            }

            var existing = await _context.PlayerStats.FindAsync(playerStat.Id);

            existing!.Points = playerStat.Points;
            existing.Assists = playerStat.Assists;
            existing.OffensiveRebounds = playerStat.OffensiveRebounds;
            existing.DefensiveRebounds = playerStat.DefensiveRebounds;
            existing.Steals = playerStat.Steals;
            existing.Blocks = playerStat.Blocks;
            existing.Fouls = playerStat.Fouls;
            existing.MinutesPlayed = playerStat.MinutesPlayed;
            existing.FreeThrowsMade = playerStat.FreeThrowsMade;
            existing.FreeThrowsAttempted = playerStat.FreeThrowsAttempted;
            existing.TwoPointsMade = playerStat.TwoPointsMade;
            existing.TwoPointsAttempted = playerStat.TwoPointsAttempted;
            existing.ThreePointsMade = playerStat.ThreePointsMade;
            existing.ThreePointsAttempted = playerStat.ThreePointsAttempted;
            existing.IsMVP = playerStat.IsMVP;
            existing.SuspensionGames = playerStat.SuspensionGames;

            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<SimpleResult> DeleteAsync(int id)
        {
            if (!await ExistsAsync(id))
            {
                return new SimpleResult { Success = false, Message = "Stat not found" };
            }

            var stat = await _context.PlayerStats.FindAsync(id);

            _context.PlayerStats.Remove(stat!);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.PlayerStats
                .AnyAsync(ps => ps.Id == id);
        }
    }
}