using Basketaki.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class LookupService : ILookupService
    {
        private readonly ApplicationDbContext _context;

        public LookupService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SelectListItem>> GetCoachSelectListAsync(int? selectedId = null)
        {
            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "",
                    Text = "-- No Coach --",
                    Selected = !selectedId.HasValue
                }
            };

            var coaches = await _context.Coaches
                .AsNoTracking()
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync();

            items.AddRange(coaches.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.FullName,
                Selected = selectedId.HasValue && c.Id == selectedId.Value
            }));

            return items;
        }

        public async Task<List<SelectListItem>> GetCourtSelectListAsync(int? selectedId = null)
        {
            return await _context.Courts
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Location)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Name} ({c.Location})",
                    Selected = selectedId.HasValue && c.Id == selectedId.Value
                })
                .ToListAsync();
        }

        public async Task<List<SelectListItem>> GetSeasonSelectListAsync(int? selectedId = null)
        {
            return await _context.Seasons
                .AsNoTracking()
                .OrderByDescending(s => s.StartDate)
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name,
                    Selected = selectedId.HasValue && s.Id == selectedId.Value
                })
                .ToListAsync();
        }

        public async Task<List<SelectListItem>> GetLeagueSelectListAsync(int? selectedId = null)
        {
            return await _context.Leagues
                .AsNoTracking()
                .Include(l => l.Season)
                .OrderByDescending(l => l.Season.StartDate)
                .ThenBy(l => l.Name)
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = $"{l.Name} - {l.City} ({l.Season.Name})",
                    Selected = selectedId.HasValue && l.Id == selectedId.Value
                })
                .ToListAsync();
        }

        public async Task<List<SelectListItem>> GetTeamSelectListAsync(int? selectedId = null)
        {
            return await _context.Teams
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .ThenBy(t => t.City)
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = $"{t.Name} ({t.City})",
                    Selected = selectedId.HasValue && t.Id == selectedId.Value
                })
                .ToListAsync();
        }

        public async Task<List<SelectListItem>> GetTeamSeasonLeagueSelectListAsync(int? selectedId = null)
        {
            return await _context.TeamSeasonLeagues
                .AsNoTracking()
                .Include(tsl => tsl.Team)
                .Include(tsl => tsl.League)
                    .ThenInclude(l => l.Season)
                .OrderByDescending(tsl => tsl.League.Season.StartDate)
                .ThenBy(tsl => tsl.League.Name)
                .ThenBy(tsl => tsl.Team.Name)
                .Select(tsl => new SelectListItem
                {
                    Value = tsl.Id.ToString(),
                    Text = $"{tsl.Team.Name} - {tsl.League.Name} ({tsl.League.Season.Name})",
                    Selected = selectedId.HasValue && tsl.Id == selectedId.Value
                })
                .ToListAsync();
        }

        public async Task<List<SelectListItem>> GetPlayerSelectListAsync(int? selectedId = null)
        {
            return await _context.Players
                .AsNoTracking()
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.FullName,
                    Selected = selectedId.HasValue && p.Id == selectedId.Value
                })
                .ToListAsync();
        }

        public async Task<List<SelectListItem>> GetPlayerSeasonTeamSelectListAsync(int? selectedId = null)
        {
            return await _context.PlayerSeasonTeams
                .AsNoTracking()
                .Include(pst => pst.Player)
                .Include(pst => pst.Team)
                .Include(pst => pst.Season)
                .OrderByDescending(pst => pst.Season.StartDate)
                .ThenBy(pst => pst.Team.Name)
                .ThenBy(pst => pst.Player.LastName)
                .Select(pst => new SelectListItem
                {
                    Value = pst.Id.ToString(),
                    Text = $"{pst.Player.FullName} - {pst.Team.Name} #{pst.JerseyNumber} ({pst.Season.Name})",
                    Selected = selectedId.HasValue && pst.Id == selectedId.Value
                })
                .ToListAsync();
        }

        public async Task<List<SelectListItem>> GetMatchSelectListAsync(int? selectedId = null)
        {
            return await _context.Matches
                .AsNoTracking()
                .Include(m => m.HomeTeamSeasonLeague)
                    .ThenInclude(tsl => tsl.Team)
                .Include(m => m.AwayTeamSeasonLeague)
                    .ThenInclude(tsl => tsl.Team)
                .OrderByDescending(m => m.MatchDate)
                .ThenBy(m => m.StartTime)
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = $"{m.MatchDate:dd/MM/yyyy} - {m.HomeTeamSeasonLeague.Team.Name} vs {m.AwayTeamSeasonLeague.Team.Name}",
                    Selected = selectedId.HasValue && m.Id == selectedId.Value
                })
                .ToListAsync();
        }

        public async Task<List<SelectListItem>> GetRefereeSelectListAsync(int? selectedId = null)
        {
            return await _context.Referees
                .AsNoTracking()
                .OrderBy(r => r.LastName)
                .ThenBy(r => r.FirstName)
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.FullName,
                    Selected = selectedId.HasValue && r.Id == selectedId.Value
                })
                .ToListAsync();
        }
    }
}
