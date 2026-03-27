using Basketaki.Models;

namespace Basketaki.Services
{
    public interface ILeagueService
    {
        Task<List<League>> GetAllAsync();            // Get all leagues
        Task<League?> GetByIdAsync(int id);          // Get league by id

        Task<SimpleResult> CreateAsync(League league);       // Create league
        Task<SimpleResult> UpdateAsync(League league);       // Update league
        Task<SimpleResult> DeleteAsync(int id);              // Delete league

        Task<bool> ExistsAsync(int id);              // Check if exists
        Task<bool> NameExistsAsync(string name , int seasonId);     // Check duplicate name 

    }
}