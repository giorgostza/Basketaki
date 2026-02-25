using Basketaki.Models;

namespace Basketaki.Services
{
    public interface ISeasonService
    {


        Task<List<Season>> GetAllAsync();
        Task<Season?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Season season);
        Task<bool> UpdateAsync(Season season);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> NameExistsAsync(string name);

    }
}
