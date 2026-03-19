using Basketaki.Data;

using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly ApplicationDbContext _context;

        public PlayerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Player>> GetAllAsync()
        {
            return await _context.Players  
                .Include(p => p.PlayerSeasonTeams)      
                    .ThenInclude(pst => pst.Team)
                    .Include(p => p.PlayerSeasonTeams)
                        .ThenInclude(pst => pst.Season)
                .ToListAsync();
        }

        public async Task<Player?> GetByIdAsync(int id)
        {
            return await _context.Players
                .Include(p => p.PlayerSeasonTeams)
                    .ThenInclude(pst => pst.Team)
                    .Include(p => p.PlayerSeasonTeams)
                        .ThenInclude(pst => pst.Season)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> CreateAsync(Player player)
        {
            _context.Players.Add(player);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Player player)
        {
            _context.Players.Update(player);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {

                return false;

            }



            if (await _context.PlayerSeasonTeams.AnyAsync(pst => pst.PlayerId == id))
            {

                return false;

            }  

            _context.Players.Remove(player);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Players.AnyAsync(p => p.Id == id);
        }

        
        public async Task<bool> JerseyNumberExistsAsync(int jerseyNumber, int teamId, int seasonId)
        {
            return await _context.PlayerSeasonTeams
                .Include(pst => pst.Player)
                .AnyAsync(pst =>
                    pst.TeamId == teamId &&
                    pst.SeasonId == seasonId &&
                    pst.Player.JerseyNumber == jerseyNumber);
        }
    }
}