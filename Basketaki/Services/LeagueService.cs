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

            return await _context.Leagues.AsNoTracking().Include(l => l.Season).ToListAsync();  // brings Season data with each League

        }

        public async Task<League?> GetByIdAsync(int id)   
        {

            return await _context.Leagues.AsNoTracking().Include(l => l.Season)
                                                        .Include(l => l.TeamSeasonLeagues)
                                                        .FirstOrDefaultAsync(l => l.Id == id);  // brings Season and TeamSeasonLeagues data for the specific League


        }

        public async Task<SimpleResult> CreateAsync(League league)
        {

            if (string.IsNullOrWhiteSpace(league.Name)) // Check if the Name property is null, empty, or consists only of whitespace characters
            {

                return new SimpleResult { Success = false, Message = "Name is required" };

            }

            if (!await _context.Seasons.AnyAsync(s => s.Id == league.SeasonId)) // Check if the associated Season exists with the provided SeasonId
            {

                return new SimpleResult { Success = false, Message = "Associated season not found" };

            }



            var name = league.Name.Trim().ToLower();
            var exists = await _context.Leagues.AnyAsync(l => l.Name.ToLower() == name && l.SeasonId == league.SeasonId);

            if (exists)  // Check if a League with the same Name already exists for the same Season
            {

                return new SimpleResult { Success = false, Message = "League with same name already exists in this season" };

            }

            league.Name = league.Name.Trim();

            _context.Leagues.Add(league);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };

        }

        public async Task<SimpleResult> UpdateAsync(League league)
        {

            var existing = await _context.Leagues.FindAsync(league.Id);

            if (existing == null)
            {

                return new SimpleResult { Success = false, Message = "League not found" };

            }

            if (string.IsNullOrWhiteSpace(league.Name))
            {

                return new SimpleResult { Success = false, Message = "Name is required" };

            }



            var name = league.Name.Trim().ToLower();
            var duplicate = await _context.Leagues.AnyAsync(l => l.Id != league.Id && l.Name.ToLower() == name && l.SeasonId == league.SeasonId);

            if (duplicate)
            {

                return new SimpleResult { Success = false, Message = "Another league with same name exists in this season" };

            }

            existing.Name = league.Name.Trim();
            existing.SeasonId = league.SeasonId;

            
            await _context.SaveChangesAsync();
            return new SimpleResult { Success = true };
        }

        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var league = await _context.Leagues.FindAsync(id);  // Find the League by its ID

            if (league == null)
            {

                return new SimpleResult { Success = false, Message = "League not found" };

            }

            
            if (await _context.TeamSeasonLeagues.AnyAsync(tsl => tsl.LeagueId == id))  // Check if there are any TeamSeasonLeagues associated with the League
            {

                return new SimpleResult { Success = false, Message = "Cannot delete league with assigned teams" };

            }

            
            if (await _context.Matches.AnyAsync(m => m.LeagueId == id)) // Check if there are any Matches associated with the League
            {

                return new SimpleResult { Success = false, Message = "Cannot delete league with matches" };

            }

            _context.Leagues.Remove(league);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<bool> ExistsAsync(int id)
        {

            return await _context.Leagues.AnyAsync(l => l.Id == id);  // Check if a League with the specified ID exists in the database

        }

        public async Task<bool> NameExistsAsync(string name ,int seasonId )
        {
            var normalized = name.Trim().ToLower();
            return await _context.Leagues.AnyAsync(l => l.Name == normalized && l.SeasonId == seasonId);  // Check if a League with the specified Name already exists for the same Season

        }
    }
}
