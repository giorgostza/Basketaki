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
            return await _context.Courts.ToListAsync();  // Get all Courts from the database
        }

        public async Task<Court?> GetByIdAsync(int id)
        {
            return await _context.Courts.FirstOrDefaultAsync(c => c.Id == id); // Get a specific Court by its ID, or return null if not found

        }

        public async Task<bool> CreateAsync(Court court)
        {
            var exists = await _context.Courts.AnyAsync(c => c.Name == court.Name && c.Location == court.Location);

            if (exists)                     // Check if a Court with the same Name and Location already exists in the database
            {

                return false;

            }
                

            _context.Courts.Add(court);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Court court)
        {
            var exists = await _context.Courts.AnyAsync(c => c.Id == court.Id);

            if (!exists)
            {

                return false;

            }

            _context.Courts.Update(court);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var court = await _context.Courts.FindAsync(id);  // Find the Court by its ID

            if (court == null)
            {
                return false;
            }


            var hasMatches = await _context.Matches.AnyAsync(m => m.CourtId == id);

            if (hasMatches)
            {
                return false;                      // Cannot delete the Court if it has associated Matches
            }

            _context.Courts.Remove(court);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Courts.AnyAsync(c => c.Id == id); // Check if a Court with the specified ID exists in the database
        }
    }
}