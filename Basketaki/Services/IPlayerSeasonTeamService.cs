using Basketaki.Models;

namespace Basketaki.Services
{
    public interface IPlayerSeasonTeamService
    {
        Task<List<PlayerSeasonTeam>> GetAllAsync();

        Task<PlayerSeasonTeam?> GetByIdAsync(int id);

        Task<SimpleResult> CreateAsync(PlayerSeasonTeam model);

        Task<SimpleResult> UpdateAsync(PlayerSeasonTeam model);

        Task<SimpleResult> DeleteAsync(int id);

        
    }
}
