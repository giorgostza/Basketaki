using Basketaki.Models;

namespace Basketaki.Services
{
    public interface ICoachService
    {
        Task<List<Coach>> GetAllAsync();
        Task<Coach?> GetByIdAsync(int id);
        Task<SimpleResult> CreateAsync(Coach coach);
        Task<SimpleResult> UpdateAsync(Coach coach);
        Task<SimpleResult> DeleteAsync(int id);
    }
}
