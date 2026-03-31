using Basketaki.Models;

namespace Basketaki.Services
{
    public interface IMatchService
    {
        Task<List<Match>> GetAllAsync();         // Get all Matches
        Task<Match?> GetByIdAsync(int id);       // Get a Match by its ID

        Task<SimpleResult> CreateAsync(Match match);     // Create a new Match
        Task<SimpleResult> UpdateAsync(Match match);     // Update an existing Match
        Task<SimpleResult> DeleteAsync(int id);          // Delete a Match by its ID

       
    }
}
