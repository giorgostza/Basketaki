using Basketaki.Models;

namespace Basketaki.Services
{
    public interface IMatchPhotoService
    {
        Task<List<MatchPhoto>> GetByMatchAsync(int matchId);  // photos per match

        Task<MatchPhoto?> GetByIdAsync(int id);

        Task<bool> CreateAsync(MatchPhoto photo);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}
