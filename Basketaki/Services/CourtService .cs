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

            return await _context.Courts.AsNoTracking().ToListAsync();  // Get all Courts from the database without tracking them for changes (Read Only)

        }

        public async Task<Court?> GetByIdAsync(int id)
        {

            return await _context.Courts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id); // Get a specific Court by its ID, or return null if not found

        }

        public async Task<SimpleResult> CreateAsync(Court court)
        {

            if (string.IsNullOrWhiteSpace(court.Name) || string.IsNullOrWhiteSpace(court.Location))
            {

                return new SimpleResult { Success = false, Message = "Name and Location are required" };

            }

            var name = court.Name.Trim().ToLower();
            var location = court.Location.Trim().ToLower();

            var exists = await _context.Courts.AnyAsync(c => c.Name.ToLower() == name && c.Location.ToLower() == location);  

            if (exists)                     // Check if a Court with the same Name and Location already exists in the database
            {

                return new SimpleResult { Success = false, Message = "Court with same name and location already exists" };

            }

            court.Name = court.Name.Trim();
            court.Location = court.Location.Trim();

            _context.Courts.Add(court);
            await _context.SaveChangesAsync();
            return new SimpleResult { Success = true };

        }

        public async Task<SimpleResult> UpdateAsync(Court court)  
        {
            var existing = await _context.Courts.FindAsync(court.Id);

            if (existing == null)
            {

                return new SimpleResult { Success = false, Message = "Court not found" };

            }

            if (string.IsNullOrWhiteSpace(court.Name) || string.IsNullOrWhiteSpace(court.Location))
            {

                return new SimpleResult { Success = false, Message = "Name and Location are required" };

            }

            var name = court.Name.Trim().ToLower();
            var location = court.Location.Trim().ToLower();



            var duplicate = await _context.Courts.AnyAsync(c => c.Name.ToLower() == name && c.Location.ToLower() == location && c.Id != court.Id);  

            if (duplicate)    
            {

                return new SimpleResult { Success = false, Message = "Court with same name and location already exists" };

            }

            existing.Name = court.Name.Trim();
            existing.Location = court.Location.Trim();
            existing.Description = court.Description;



            await _context.SaveChangesAsync();
            return new SimpleResult { Success = true };

        }

        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var court = await _context.Courts.FindAsync(id);  // Find the Court by its ID

            if (court == null)
            {

                return new SimpleResult { Success = false, Message = "Court not found" };

            }



            var hasMatches = await _context.Matches.AnyAsync(m => m.CourtId == id);

            if (hasMatches)
            {

                return new SimpleResult { Success = false, Message = "Cannot delete court because it has matches" };       // Cannot delete the Court if it has associated Matches

            }


            _context.Courts.Remove(court);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };

        }

        public async Task<bool> ExistsAsync(int id)
        {

            return await _context.Courts.AnyAsync(c => c.Id == id); // Check if a Court with the specified ID exists in the database

        }

    }
}