using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _context;

        public TeamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Team>> GetAllAsync()
        {
            return await _context.Teams
                .AsNoTracking()
                .Include(t => t.TeamSeasonLeagues)
                .Include(t => t.PlayerSeasonTeams)
                .ToListAsync();
        }

        public async Task<Team?> GetByIdAsync(int id)
        {
            return await _context.Teams
                .AsNoTracking()
                .Include(t => t.TeamSeasonLeagues)
                .Include(t => t.PlayerSeasonTeams)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<SimpleResult> CreateAsync(Team team)
        {
            var name = team.Name?.Trim().ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(name))
            {
                return new SimpleResult { Success = false, Message = "Name is required" };
            }

            var exists = await _context.Teams
                .AnyAsync(t => t.Name.ToLower() == name);

            if (exists)
            {
                return new SimpleResult { Success = false, Message = "Team already exists" };
            }

            team.Name = name;
            team.CoachName = team.CoachName?.Trim();
            team.LogoPhotoUrl = team.LogoPhotoUrl?.Trim();

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<SimpleResult> UpdateAsync(Team team)
        {
            var existing = await _context.Teams.FindAsync(team.Id);

            if (existing == null)
            {
                return new SimpleResult { Success = false, Message = "Team not found" };
            }

            var name = team.Name?.Trim().ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(name))
            {
                return new SimpleResult { Success = false, Message = "Name is required" };
            }

            var duplicate = await _context.Teams
                .AnyAsync(t => t.Id != team.Id && t.Name.ToLower() == name);

            if (duplicate)
            {
                return new SimpleResult { Success = false, Message = "Team already exists" };
            }

            // Controlled update
            existing.Name = name;
            existing.CoachName = team.CoachName?.Trim();
            existing.LogoPhotoUrl = team.LogoPhotoUrl?.Trim();

            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);

            if (team == null)
            {
                return new SimpleResult { Success = false, Message = "Team not found" };
            }

            var hasLeagues = await _context.TeamSeasonLeagues.AnyAsync(tsl => tsl.TeamId == id);

            if (hasLeagues)
            {
                return new SimpleResult { Success = false, Message = "Cannot delete team assigned to leagues" };
            }

            var hasPlayers = await _context.PlayerSeasonTeams.AnyAsync(pst => pst.TeamId == id);

            if (hasPlayers)
            {
                return new SimpleResult { Success = false, Message = "Cannot delete team with players" };
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }
    }
}