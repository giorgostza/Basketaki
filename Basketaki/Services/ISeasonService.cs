using Basketaki.Models;

namespace Basketaki.Services
{
    public interface ISeasonService
    {

        Task<List<Season>> GetAllAsync();                   // Get all Seasons
        Task<Season?> GetByIdAsync(int id);                 // Get a Season by its ID, returns null if not found
        Task<SimpleResult> CreateAsync(Season season);      // Create a new Season, returns true if successful, false if not 
        Task<SimpleResult> UpdateAsync(Season season);      // Update an existing Season, returns true if successful, false if not
        Task<SimpleResult> DeleteAsync(int id);             // Delete a Season by its ID, returns true if successful, false if not 
        
       
    }
}
