using Basketaki.Data;
using Basketaki.Models;
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

            return await _context.Players.AsNoTracking().Include(p => p.PlayerSeasonTeams)
                                                                .ThenInclude(pst => pst.Team)
                                                        .Include(p => p.PlayerSeasonTeams)
                                                                .ThenInclude(pst => pst.Season)
                                                        .ToListAsync();


        }

        public async Task<Player?> GetByIdAsync(int id) // Retrieve a Player by its ID 
        {                                               // including related Team and Season information through the PlayerSeasonTeams relationship

            return await _context.Players.AsNoTracking().Include(p => p.PlayerSeasonTeams)
                                                                .ThenInclude(pst => pst.Team)
                                                        .Include(p => p.PlayerSeasonTeams)
                                                                .ThenInclude(pst => pst.Season)
                                                        .FirstOrDefaultAsync(p => p.Id == id);

        }

        public async Task<SimpleResult> CreateAsync(Player player)
        {
            var firstName = (player.FirstName != null) ? player.FirstName.Trim().ToLower() : "";
            var lastName = (player.LastName != null) ? player.LastName.Trim().ToLower() : "";
            

            // Check if both FirstName and LastName are null, empty, or consist only of whitespace characters
            if ( string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName)) 
            {

                return new SimpleResult { Success = false, Message = "FirstName or LastName is required" };

            }

            // Check if Height is less than  0
            if (player.JerseyNumber < 0)
            {

                return new SimpleResult { Success = false, Message = "JerseyNumber must be positive" };

            }

            player.FirstName = firstName;
            player.LastName = lastName;

            _context.Players.Add(player);
             await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<SimpleResult> UpdateAsync(Player player)
        {
            var existing = await _context.Players.FindAsync(player.Id);

            if (existing == null)
            {

                return new SimpleResult { Success = false, Message = "Player not found" };

            }


            var firstName = (player.FirstName != null) ? player.FirstName.Trim().ToLower() : "";
            var lastName = (player.LastName != null) ? player.LastName.Trim().ToLower() : "";


            if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
            {

                return new SimpleResult { Success = false, Message = "FirstName or LastName is required" };

            }
                

            // Check jersey conflicts in PlayerSeasonTeams
            foreach (var pst in player.PlayerSeasonTeams)
            {
                bool jerseyConflict = await _context.PlayerSeasonTeams.Include(x => x.Player).AnyAsync(x => x.TeamId == pst.TeamId &&
                                                                                                       x.SeasonId == pst.SeasonId &&
                                                                                                       x.Player.JerseyNumber == player.JerseyNumber &&
                                                                                                       x.PlayerId != player.Id);


                if (jerseyConflict) 
                {

                    return new SimpleResult { Success = false, Message = "Jersey number already taken in this team/season" };

                }
                   
            }

            // Update these fields
            existing.FirstName = firstName;
            existing.LastName = lastName;
            existing.Height = player.Height;
            existing.Weight = player.Weight;
            existing.JerseyNumber = player.JerseyNumber;
            existing.Position = player.Position;
            existing.PlayerPhotoUrl = player.PlayerPhotoUrl?.Trim();

            await _context.SaveChangesAsync();
            return new SimpleResult { Success = true };
        }


        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var player = await _context.Players.FindAsync(id); // Find the Player entity by its ID

            if (player == null)
            {

                return new SimpleResult { Success = false, Message = "Player not found" };

            }


            // Check if there are any PlayerSeasonTeam records associated with the Player
            if (await _context.PlayerSeasonTeams.AnyAsync(pst => pst.PlayerId == id)) 
            {

                return new SimpleResult { Success = false, Message = "Cannot delete player assigned to teams/seasons" };

            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };

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