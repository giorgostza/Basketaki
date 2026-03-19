using Basketaki.Models;

namespace Basketaki.Services
{
    public interface IMatchService
    {
        Task<List<Match>> GetAllAsync();         // Get all Matches
        Task<Match?> GetByIdAsync(int id);       // Get a Match by its ID

        Task<bool> CreateAsync(Match match);     // Create a new Match
        Task<bool> UpdateAsync(Match match);     // Update an existing Match
        Task<bool> DeleteAsync(int id);          // Delete a Match by its ID

        Task<bool> ExistsAsync(int id);          // Check if a Match exists by its ID
    }
}
