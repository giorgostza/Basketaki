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

        public async Task<List<PlayerStat>> GetAllAsync() // Get all player stats with related Player and Team information 
        {                                                 // ordered by Match date, then Team name, then Player last name

            return await _context.PlayerStats
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

        public async Task<PlayerStat?> GetByIdAsync(int id) // Get a single Player stat by ID with related Player and Team information
        {

            return await _context.PlayerStats
                .Include(ps => ps.PlayerSeasonTeam)
                        .ThenInclude(pst => pst.Player)
                .Include(ps => ps.PlayerSeasonTeam)
                        .ThenInclude(pst => pst.Team)
                .Include(ps => ps.Match)
                .FirstOrDefaultAsync(ps => ps.Id == id);

        }

        public async Task<List<PlayerStat>> GetByMatchIdAsync(int matchId) // Get all player stats for a specific Match ID with related Player and Team information
        {

            return await _context.PlayerStats
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

        public async Task<bool> CreateAsync(PlayerStat playerStat)
        {

            if (playerStat.Points < 0)
            {

                return false;

            }

            //Check if a stat for the same Player in the same Match already exists to prevent duplicates
            var exists = await _context.PlayerStats.AnyAsync(ps => ps.PlayerSeasonTeamId == playerStat.PlayerSeasonTeamId && ps.MatchId == playerStat.MatchId);

            if (exists)
            {

                return false;

            }

            

            var pstExists = await _context.PlayerSeasonTeams.AnyAsync(p => p.Id == playerStat.PlayerSeasonTeamId);

            var matchExists = await _context.Matches.AnyAsync(m => m.Id == playerStat.MatchId);

            
            if (!pstExists || !matchExists) // Ensure the referenced PlayerSeasonTeam and Match exist before creating the stat
            {

                return false;

            }

            _context.PlayerStats.Add(playerStat);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(PlayerStat playerStat)
        {
            if (!await ExistsAsync(playerStat.Id)) // Ensure the stat exists before trying to update
            {

                return false;

            }


            // Check if another stat for the same Player in the same Match already exists to prevent duplicates (excluding the current stat)
            var duplicate = await _context.PlayerStats.AnyAsync(ps => ps.Id != playerStat.Id 
                                                                && ps.PlayerSeasonTeamId == playerStat.PlayerSeasonTeamId
                                                                && ps.MatchId == playerStat.MatchId);

            if (duplicate)
            {

                return false;

            }

            _context.PlayerStats.Update(playerStat);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var stat = await _context.PlayerStats.FindAsync(id); // Find the stat by ID to ensure it exists before trying to delete

            if (stat == null)
            {

                return false;

            }

            _context.PlayerStats.Remove(stat);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id) // Check if a PlayerStat with the specified ID exists in the database
        {

            return await _context.PlayerStats.AnyAsync(ps => ps.Id == id); 

        }
    }
}