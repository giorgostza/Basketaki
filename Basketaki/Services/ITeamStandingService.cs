using Basketaki.Models;

namespace Basketaki.Services
{
    public interface ITeamStandingService
    {
        Task<List<TeamStanding>> GetAllAsync();

        Task<TeamStanding?> GetByIdAsync(int id);

        Task<TeamStanding?> GetByTeamSeasonLeagueIdAsync(int teamSeasonLeagueId);

        Task<bool> CreateAsync(TeamStanding standing);

        Task<bool> UpdateAsync(TeamStanding standing);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}