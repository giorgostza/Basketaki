using Basketaki.Models;

namespace Basketaki.Services
{
    public interface IPlayerStatService
    {
        Task<List<PlayerStat>> GetAllAsync();

        Task<PlayerStat?> GetByIdAsync(int id);

        Task<List<PlayerStat>> GetByMatchIdAsync(int matchId);

        Task<bool> CreateAsync(PlayerStat playerStat);

        Task<bool> UpdateAsync(PlayerStat playerStat);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);


    }
}

