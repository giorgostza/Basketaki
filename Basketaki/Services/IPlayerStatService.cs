using Basketaki.Models;

namespace Basketaki.Services
{
    public interface IPlayerStatService
    {
        Task<List<PlayerStat>> GetAllAsync();

        Task<PlayerStat?> GetByIdAsync(int id);

        Task<List<PlayerStat>> GetByMatchIdAsync(int matchId);

        Task<SimpleResult> CreateAsync(PlayerStat playerStat);

        Task<SimpleResult> UpdateAsync(PlayerStat playerStat);

        Task<SimpleResult> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);


    }
}

