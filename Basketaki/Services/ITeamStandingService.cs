using Basketaki.Models;

namespace Basketaki.Services
{
    public interface ITeamStandingService
    {
        Task<List<TeamStanding>> GetAllAsync();

        Task<TeamStanding?> GetByIdAsync(int id);

        Task<TeamStanding?> GetByTeamSeasonLeagueIdAsync(int teamSeasonLeagueId);

        Task<SimpleResult> CreateAsync(TeamStanding standing);

        Task<SimpleResult> UpdateAsync(TeamStanding standing);

        Task<SimpleResult> DeleteAsync(int id);

        
    }
}