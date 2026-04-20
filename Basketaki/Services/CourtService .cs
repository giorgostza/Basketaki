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

            return await _context.Courts.AsNoTracking().OrderBy(c => c.Name).ThenBy(c => c.Location).ToListAsync();

        }


        public async Task<Court?> GetByIdAsync(int id)
        {

            return await _context.Courts.AsNoTracking().Include(c => c.Matches).FirstOrDefaultAsync(c => c.Id == id);

        }


        public async Task<SimpleResult> CreateAsync(Court court)
        {
            if (court == null)
            {

                return SimpleResult.Fail("Court data is required.");

            }


            if (string.IsNullOrWhiteSpace(court.Name) || string.IsNullOrWhiteSpace(court.Location))
            {

                return SimpleResult.Fail("Name and Location are required.");

            }


            var trimmedName = court.Name.Trim();
            var trimmedLocation = court.Location.Trim();
            var trimmedDescription = string.IsNullOrWhiteSpace(court.Description) ? null : court.Description.Trim();



            var exists = await _context.Courts.AnyAsync(c => c.Name == trimmedName && c.Location == trimmedLocation);

            if (exists)
            {

                return SimpleResult.Fail("Court with the same name and location already exists.");

            }


            court.Name = trimmedName;
            court.Location = trimmedLocation;
            court.Description = trimmedDescription;

            _context.Courts.Add(court);

            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Court created successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to create court. A court with the same name and location may already exist.");

            }

        }



        public async Task<SimpleResult> UpdateAsync(Court court)
        {
            if (court == null)
            {

                return SimpleResult.Fail("Court data is required.");

            }


            var existing = await _context.Courts.FindAsync(court.Id);

            if (existing == null)
            {

                return SimpleResult.Fail("Court not found.");

            }


            if (string.IsNullOrWhiteSpace(court.Name) || string.IsNullOrWhiteSpace(court.Location))
            {

                return SimpleResult.Fail("Name and Location are required.");

            }


            var trimmedName = court.Name.Trim();
            var trimmedLocation = court.Location.Trim();
            var trimmedDescription = string.IsNullOrWhiteSpace(court.Description) ? null : court.Description.Trim();



            var duplicate = await _context.Courts.AnyAsync(c => c.Name == trimmedName && c.Location == trimmedLocation && c.Id != court.Id);

            if (duplicate)
            {

                return SimpleResult.Fail("Court with the same name and location already exists.");

            }


            existing.Name = trimmedName;
            existing.Location = trimmedLocation;
            existing.Description = trimmedDescription;

            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Court updated successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to update court. A court with the same name and location may already exist.");

            }

        }


        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var court = await _context.Courts.FindAsync(id);

            if (court == null)
            {

                return SimpleResult.Fail("Court not found.");

            }


            var hasMatches = await _context.Matches.AnyAsync(m => m.CourtId == id);

            if (hasMatches)
            {

                return SimpleResult.Fail("Cannot delete court because it has matches.");

            }



            try
            {

                _context.Courts.Remove(court);
                await _context.SaveChangesAsync();

                return SimpleResult.Ok("Court deleted successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to delete court because it is used by other data.");

            }


        }

    }
}