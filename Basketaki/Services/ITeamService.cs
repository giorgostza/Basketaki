using Basketaki.Models;

namespace Basketaki.Services
{
    public interface ITeamService
    {
        Task<List<Team>> GetAllAsync();           // Get all Teams
        Task<Team?> GetByIdAsync(int id);         // Get a Team by its ID
        Task<SimpleResult> CreateAsync(Team team);        // Create a new Team
        Task<SimpleResult> UpdateAsync(Team team);        // Update an existing Team
        Task<SimpleResult> DeleteAsync(int id);           // Delete a Team by its ID
    
    }
}