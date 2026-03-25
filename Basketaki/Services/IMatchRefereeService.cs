using Basketaki.Models;

namespace Basketaki.Services
{
    public interface IMatchRefereeService
    {
        Task<List<MatchReferee>> GetByMatchIdAsync(int matchId);

        Task<bool> AssignRefereeAsync(int matchId, int refereeId);

        Task<bool> RemoveRefereeAsync(int matchId, int refereeId);

        Task<bool> ExistsAsync(int matchId, int refereeId);
    }
}
