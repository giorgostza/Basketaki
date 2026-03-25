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

        public async Task<List<Player>> GetAllAsync() // Retrieve all Players from the database
        {                                             // including related Team and Season information through the PlayerSeasonTeams relationship

            return await _context.Players  
                .Include(p => p.PlayerSeasonTeams)      
                        .ThenInclude(pst => pst.Team)
                .Include(p => p.PlayerSeasonTeams)
                        .ThenInclude(pst => pst.Season)
                .ToListAsync();

        }

        public async Task<Player?> GetByIdAsync(int id) // Retrieve a Player by its ID 
        {                                               // including related Team and Season information through the PlayerSeasonTeams relationship

            return await _context.Players
                .Include(p => p.PlayerSeasonTeams)
                        .ThenInclude(pst => pst.Team)
                .Include(p => p.PlayerSeasonTeams)
                        .ThenInclude(pst => pst.Season)
                .FirstOrDefaultAsync(p => p.Id == id);

        }

        public async Task<bool> CreateAsync(Player player)
        {
            // Check if both FirstName and LastName are null, empty, or consist only of whitespace characters
            if ( string.IsNullOrWhiteSpace(player.FirstName) && string.IsNullOrWhiteSpace(player.LastName)) 
            {

                return false;

            }

            // Check if Height is less than or equal to 0
            if (player.JerseyNumber <= 0)
            {

                return false;

            }


            _context.Players.Add(player);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Player player)
        {
            if (!await ExistsAsync(player.Id)) // Check if the Player exists in the database before attempting to update
            {

                return false;

            }


            
            foreach (var pst in player.PlayerSeasonTeams) // Loop through each PlayerSeasonTeam associated with the Player to check for jersey number conflicts
            {
                bool jerseyConflict = await _context.PlayerSeasonTeams.Include(x => x.Player)
                                                    .AnyAsync(x =>
                                                        x.TeamId == pst.TeamId &&
                                                        x.SeasonId == pst.SeasonId &&
                                                        x.Player.JerseyNumber == player.JerseyNumber &&
                                                        x.PlayerId != player.Id);


                if (jerseyConflict)
                {

                    return false;

                }
            }


            _context.Players.Update(player);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var player = await _context.Players.FindAsync(id); // Find the Player entity by its ID

            if (player == null)
            {

                return false;

            }


            // Check if there are any PlayerSeasonTeam records associated with the Player
            if (await _context.PlayerSeasonTeams.AnyAsync(pst => pst.PlayerId == id)) 
            {

                return false;

            }  

            _context.Players.Remove(player);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {

            return await _context.Players.AnyAsync(p => p.Id == id);  // Check if a Player with the specified ID exists in the database

        }

        
        public async Task<bool> JerseyNumberExistsAsync(int jerseyNumber, int teamId, int seasonId)
        {

            return await _context.PlayerSeasonTeams
                        .Include(pst => pst.Player).AnyAsync(pst => pst.TeamId == teamId && pst.SeasonId == seasonId && pst.Player.JerseyNumber == jerseyNumber);
            // Check if any PlayerSeasonTeam exists with the same TeamId, SeasonId and Player's JerseyNumber
            // ensuring that Jersey Numbers are unique within a Team for a given Season.

        }

    }
}