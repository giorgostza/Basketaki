using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class SeasonService : ISeasonService
    {
        private readonly ApplicationDbContext _context;

        public SeasonService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Season>> GetAllAsync()
        {
            return await _context.Seasons
                .AsNoTracking()
                .OrderByDescending(s => s.StartDate)
                .ToListAsync();
        }

        public async Task<Season?> GetByIdAsync(int id)
        {
            return await _context.Seasons
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<SimpleResult> CreateAsync(Season season)
        {
            var name = season.Name?.Trim().ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(name))
            {
                return new SimpleResult { Success = false, Message = "Name is required" };
            }

            if (season.StartDate >= season.EndDate)
            {
                return new SimpleResult { Success = false, Message = "StartDate must be before EndDate" };
            }

            var exists = await _context.Seasons.AnyAsync(s => s.Name.ToLower() == name);

            if (exists)
            {
                return new SimpleResult { Success = false, Message = "Season already exists" };
            }

            season.Name = name;

            _context.Seasons.Add(season);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<SimpleResult> UpdateAsync(Season season)
        {
            var existing = await _context.Seasons.FindAsync(season.Id);

            if (existing == null)
            {
                return new SimpleResult { Success = false, Message = "Season not found" };
            }

            var name = season.Name?.Trim().ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(name))
            {
                return new SimpleResult { Success = false, Message = "Name is required" };
            }

            if (season.StartDate >= season.EndDate)
            {
                return new SimpleResult { Success = false, Message = "StartDate must be before EndDate" };
            }

            var duplicate = await _context.Seasons.AnyAsync(s => s.Id != season.Id && s.Name.ToLower() == name);

            if (duplicate)
            {
                return new SimpleResult { Success = false, Message = "Season already exists" };
            }

            // Controlled update
            existing.Name = name;
            existing.StartDate = season.StartDate;
            existing.EndDate = season.EndDate;

            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var season = await _context.Seasons.FindAsync(id);

            if (season == null)
            {
                return new SimpleResult { Success = false, Message = "Season not found" };
            }

            var hasLeagues = await _context.Leagues.AnyAsync(l => l.SeasonId == id);

            if (hasLeagues)
            {
                return new SimpleResult { Success = false, Message = "Cannot delete season with leagues" };
            }

            _context.Seasons.Remove(season);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            var normalized = name.Trim().ToLower();

            return await _context.Seasons
                .AnyAsync(s => s.Name.ToLower() == normalized);
        }
    }
}