using Basketaki.Models;

namespace Basketaki.Services
{
    public interface ISeasonService
    {

        Task<List<Season>> GetAllAsync();           // Get all Seasons
        Task<Season?> GetByIdAsync(int id);         // Get a Season by its ID, returns null if not found
        Task<bool> CreateAsync(Season season);      // Create a new Season, returns true if successful, false if not 
        Task<bool> UpdateAsync(Season season);      // Update an existing Season, returns true if successful, false if not
        Task<bool> DeleteAsync(int id);             // Delete a Season by its ID, returns true if successful, false if not 
        Task<bool> ExistsAsync(int id);             // Check if a Season with the specified ID exists, returns true if it does, false otherwise
        Task<bool> NameExistsAsync(string name);    // Check if a Season with the specified name exists, returns true if it does, false otherwise

    }
}
