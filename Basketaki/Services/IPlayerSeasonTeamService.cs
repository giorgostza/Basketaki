using Basketaki.Models;

namespace Basketaki.Services
{
    public interface IPlayerSeasonTeamService
    {
        Task<List<PlayerSeasonTeam>> GetAllAsync();

        Task<PlayerSeasonTeam?> GetByIdAsync(int id);

        Task<bool> CreateAsync(PlayerSeasonTeam model);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<bool> CombinationExistsAsync(int playerId, int seasonId);
    }
}
