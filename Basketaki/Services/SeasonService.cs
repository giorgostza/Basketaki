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

        public async Task<bool> CreateAsync(Season season)
        {
            _context.Seasons.Add(season);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var season = await _context.Seasons.FindAsync(id);

            if (season == null)
                return false;

            _context.Seasons.Remove(season);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Seasons.AnyAsync(s => s.Id == id);
        }
        

        public async Task<List<Season>> GetAllAsync()
        {

            return await _context.Seasons.OrderByDescending(s => s.StartDate).ToListAsync();

        }

        public async Task<Season?> GetByIdAsync(int id)
        {

            return await _context.Seasons.FirstOrDefaultAsync(s => s.Id == id);

        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await _context.Seasons.AnyAsync(s => s.Name == name);
        }

        public async Task<bool> UpdateAsync(Season season)
        {
            _context.Seasons.Update(season);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
