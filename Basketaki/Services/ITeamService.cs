using Basketaki.Models;

namespace Basketaki.Services
{
    public interface ITeamService
    {
        Task<List<Team>> GetAllAsync();           // Get all Teams
        Task<Team?> GetByIdAsync(int id);         // Get a Team by its ID
        Task<bool> CreateAsync(Team team);        // Create a new Team
        Task<bool> UpdateAsync(Team team);        // Update an existing Team
        Task<bool> DeleteAsync(int id);           // Delete a Team by its ID
        Task<bool> ExistsAsync(int id);           // Check if a Team with the specified ID exists
        Task<bool> NameExistsAsync(string name);  // Check if a Team with the specified Name exists
    }
}