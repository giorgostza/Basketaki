using Basketaki.Models;

namespace Basketaki.Services
{
    public interface IRefereeService
    {
        Task<List<Referee>> GetAllAsync();

        Task<Referee?> GetByIdAsync(int id);

        Task<SimpleResult> CreateAsync(Referee referee);

        Task<SimpleResult> UpdateAsync(Referee referee);

        Task<SimpleResult> DeleteAsync(int id);

       
    }
}
