using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly ApplicationDbContext _context;

        public LeagueService(ApplicationDbContext context)  
        {

            _context = context;

        }

        public async Task<List<League>> GetAllAsync()  
        {

            return await _context.Leagues.Include(l => l.Season).ToListAsync();  // brings Season data with each League

        }

        public async Task<League?> GetByIdAsync(int id)   
        {

            return await _context.Leagues
                                        .Include(l => l.Season)
                                        .Include(l => l.TeamSeasonLeagues)
                                        .FirstOrDefaultAsync(l => l.Id == id);  // brings Season and TeamSeasonLeagues data for the specific League

        }

        public async Task<bool> CreateAsync(League league)
        {
            
            if (!await _context.Seasons.AnyAsync(s => s.Id == league.SeasonId)) // Check if the associated Season exists with the provided SeasonId
            {

                return false;      

            }

            
            var exists = await _context.Leagues.AnyAsync(l => l.Name == league.Name && l.SeasonId == league.SeasonId);


            if (exists)  // Check if a League with the same Name already exists for the same Season
            {

                return false;

            }

            _context.Leagues.Add(league);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(League league)
        {
            if (!await ExistsAsync(league.Id))
            {

                return false;

            }

           
            var exists = await _context.Leagues.AnyAsync(l => l.Name == league.Name && l.SeasonId == league.SeasonId && l.Id != league.Id);
            // Check if a League with the same Name already exists for the same Season, excluding the current League being updated

            if (exists)
            {

                return false;

            }

            _context.Leagues.Update(league);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var league = await _context.Leagues.FindAsync(id);  // Find the League by its ID

            if (league == null)
            {

                return false;

            }

            
            if (await _context.TeamSeasonLeagues.AnyAsync(tsl => tsl.LeagueId == id))  // Check if there are any TeamSeasonLeagues associated with the League
            {

                return false;

            }

            
            if (await _context.Matches.AnyAsync(m => m.LeagueId == id)) // Check if there are any Matches associated with the League
            {

                return false;

            }

            _context.Leagues.Remove(league);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {

            return await _context.Leagues.AnyAsync(l => l.Id == id);  // Check if a League with the specified ID exists in the database

        }

        public async Task<bool> NameExistsAsync(string name ,int seasonId )
        {

            return await _context.Leagues.AnyAsync(l => l.Name == name && l.SeasonId == seasonId);  // Check if a League with the specified Name already exists for the same Season

        }
    }
}
