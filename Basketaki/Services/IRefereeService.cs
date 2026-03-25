using Basketaki.Models;

namespace Basketaki.Services
{
    public interface IRefereeService
    {
        Task<List<Referee>> GetAllAsync();

        Task<Referee?> GetByIdAsync(int id);

        Task<bool> CreateAsync(Referee referee);

        Task<bool> UpdateAsync(Referee referee);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}
