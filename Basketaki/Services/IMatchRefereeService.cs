using Basketaki.Models;

namespace Basketaki.Services
{
    public interface IMatchRefereeService
    {
        Task<List<MatchReferee>> GetByMatchIdAsync(int matchId);

        Task<SimpleResult> AssignRefereeAsync(int matchId, int refereeId);

        Task<SimpleResult> RemoveRefereeAsync(int matchId, int refereeId);

        Task<bool> ExistsAsync(int matchId, int refereeId);
    }
}