using Basketaki.Models;

namespace Basketaki.Services
{
    public interface IPlayerService
    {
        Task<List<Player>> GetAllAsync();  // Get all players
        Task<Player?> GetByIdAsync(int id); // Get a player by its ID, returns null if not found
        Task<SimpleResult> CreateAsync(Player player);  // Create a new player, returns true if successful
        Task<SimpleResult> UpdateAsync(Player player);  // Update an existing player, returns true if successful
        Task<SimpleResult> DeleteAsync(int id);  // Delete a player by its ID, returns true if successful, false if not 
        Task<bool> ExistsAsync(int id);  // Check if a player with the specified ID exists, returns true if it does, false otherwise
        Task<bool> JerseyNumberExistsAsync(int jerseyNumber, int teamId, int seasonId);  // Check if a jersey number is already taken by a team in a season, returns true if it is, false otherwise


    }
}
