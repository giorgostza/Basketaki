using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class CourtService : ICourtService
    {
        private readonly ApplicationDbContext _context;

        public CourtService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Court>> GetAllAsync()
        {
            return await _context.Courts.ToListAsync();
        }

        public async Task<Court?> GetByIdAsync(int id)
        {
            return await _context.Courts.FirstOrDefaultAsync(c => c.Id == id);

        }

        public async Task<bool> CreateAsync(Court court)
        {
            _context.Courts.Add(court);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(Court court)
        {
            _context.Courts.Update(court);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var court = await _context.Courts.FindAsync(id);

            if (court == null)
            {
                return false;
            }
               

            _context.Courts.Remove(court);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Courts.AnyAsync(c => c.Id == id);
        }
    }
}