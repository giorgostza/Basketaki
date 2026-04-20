using Basketaki.Models;

namespace Basketaki.Services
{
    public interface IMatchRefereeService
    {
        Task<List<MatchReferee>> GetByMatchIdAsync(int matchId);

        Task<SimpleResult> CreateAsync(int matchId, int refereeId);

        Task<SimpleResult> DeleteAsync(int matchId, int refereeId);

       
    }
}