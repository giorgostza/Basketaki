using Basketaki.Models;

namespace Basketaki.Services
{
    public interface ICourtService
    {
        Task<List<Court>> GetAllAsync();

        Task<Court?> GetByIdAsync(int id);

        Task<bool> CreateAsync(Court court);

        Task<bool> UpdateAsync(Court court);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}