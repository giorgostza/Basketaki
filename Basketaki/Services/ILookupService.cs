using Microsoft.AspNetCore.Mvc.Rendering;

namespace Basketaki.Services
{
    public interface ILookupService
    {
        Task<List<SelectListItem>> GetCoachSelectListAsync(int? selectedId = null);
        Task<List<SelectListItem>> GetCourtSelectListAsync(int? selectedId = null);
        Task<List<SelectListItem>> GetSeasonSelectListAsync(int? selectedId = null);
        Task<List<SelectListItem>> GetLeagueSelectListAsync(int? selectedId = null);
        Task<List<SelectListItem>> GetTeamSelectListAsync(int? selectedId = null);
        Task<List<SelectListItem>> GetTeamSeasonLeagueSelectListAsync(int? selectedId = null);
        Task<List<SelectListItem>> GetPlayerSelectListAsync(int? selectedId = null);
        Task<List<SelectListItem>> GetPlayerSeasonTeamSelectListAsync(int? selectedId = null);
        Task<List<SelectListItem>> GetMatchSelectListAsync(int? selectedId = null);
        Task<List<SelectListItem>> GetRefereeSelectListAsync(int? selectedId = null);
    }
}
