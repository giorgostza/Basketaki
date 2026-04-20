using Basketaki.Models;

namespace Basketaki.Services
{
    public interface ITeamSeasonLeagueService
    {
        Task<List<TeamSeasonLeague>> GetAllAsync();
        Task<TeamSeasonLeague?> GetByIdAsync(int id);
        Task<SimpleResult> CreateAsync(TeamSeasonLeague model);
        Task<SimpleResult> DeleteAsync(int id);
    }
}