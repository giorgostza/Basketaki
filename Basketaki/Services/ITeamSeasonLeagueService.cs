using Basketaki.Models;

namespace Basketaki.Services
{
    public interface ITeamSeasonLeagueService
    {
        Task<List<TeamSeasonLeague>> GetAllAsync();                   // Get all Team-SeasonLeague associations

        Task<TeamSeasonLeague?> GetByIdAsync(int id);                 // Get a specific association by ID

        Task<SimpleResult> CreateAsync(TeamSeasonLeague model);               // Create a new association or returns false if it is invalid
        Task<SimpleResult> DeleteAsync(int id);              // Delete an association by ID or returns false if it doesn't exist or is in use

        Task<bool> CombinationExistsAsync(int teamId, int leagueId);  // Check if a specific Team-League combination already exists
    }
}
