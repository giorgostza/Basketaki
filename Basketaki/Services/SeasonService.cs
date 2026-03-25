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


        public async Task<List<Season>> GetAllAsync()  // Return Seasons ordered by StartDate in descending order
        {

            return await _context.Seasons.OrderByDescending(s => s.StartDate).ToListAsync(); 

        }

        public async Task<bool> CreateAsync(Season season)
        {

            if (await NameExistsAsync(season.Name)) // Validate that the Season Name is unique
            {

                return false;

            }


            if (season.StartDate >= season.EndDate) // Validate that the StartDate is before the EndDate
            {

                return false;

            }
               
            
            _context.Seasons.Add(season);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Season season)
        {

            var exists = await _context.Seasons.AnyAsync(s => s.Name == season.Name && s.Id != season.Id);
            // Validate that the Season Name is unique (excluding the current Season being updated)
            if (exists)
            {

                return false;

            }

            if (season.StartDate >= season.EndDate) // Validate that the StartDate is before the EndDate
            {

                return false;

            }

            _context.Seasons.Update(season);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var season = await _context.Seasons.FindAsync(id); // Find the Season by its ID to ensure it exists before trying to delete

            if (season == null)
            {

                return false;

            }

            // Validate that there are no Leagues associated with the Season before allowing deletion
            if (await _context.Leagues.AnyAsync(l => l.SeasonId == id)) 
            {

                return false;

            }
                

            _context.Seasons.Remove(season);

            return await _context.SaveChangesAsync() > 0;
        }




        public async Task<bool> ExistsAsync(int id) // Check if a Season with the specified ID exists in the database
        {

            return await _context.Seasons.AnyAsync(s => s.Id == id);

        }
        
        public async Task<Season?> GetByIdAsync(int id) // Retrieve a Season by its ID, returning null if it does not exist
        {

            return await _context.Seasons.FirstOrDefaultAsync(s => s.Id == id);

        }

        public async Task<bool> NameExistsAsync(string name) // Check if a Season with the specified Name already exists in the database
        {

            return await _context.Seasons.AnyAsync(s => s.Name == name); 

        }

        
    }
}
