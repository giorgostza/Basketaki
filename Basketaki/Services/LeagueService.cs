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

            return await _context.Leagues.AsNoTracking().Include(l => l.Season)
                                                        .OrderBy(l => l.Name)
                                                        .ThenBy(l => l.City)
                                                        .ToListAsync();

        }



        public async Task<League?> GetByIdAsync(int id)
        {

            return await _context.Leagues.AsNoTracking().Include(l => l.Season)
                                                        .Include(l => l.TeamSeasonLeagues)
                                                                .ThenInclude(tsl => tsl.Team)
                                                        .FirstOrDefaultAsync(l => l.Id == id);

        }



        public async Task<SimpleResult> CreateAsync(League league)
        {
            if (league == null)
            {

                return SimpleResult.Fail("League data is required.");

            }


            if (string.IsNullOrWhiteSpace(league.Name) || string.IsNullOrWhiteSpace(league.City))
            {

                return SimpleResult.Fail("Name and City are required.");

            }


            var seasonExists = await _context.Seasons.AnyAsync(s => s.Id == league.SeasonId);

            if (!seasonExists)
            {

                return SimpleResult.Fail("Associated season not found.");

            }


            var trimmedName = league.Name.Trim();
            var trimmedCity = league.City.Trim();



            var exists = await _context.Leagues.AnyAsync(l => l.Name == trimmedName && l.City == trimmedCity && l.SeasonId == league.SeasonId);

            if (exists)
            {

                return SimpleResult.Fail("League with the same name already exists in the same city for this season.");

            }



            league.Name = trimmedName;
            league.City = trimmedCity;

            _context.Leagues.Add(league);


            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("League created successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to create league. A league with the same name, city, and season may already exist.");

            }

        }



        public async Task<SimpleResult> UpdateAsync(League league)
        {
            if (league == null)
            {

                return SimpleResult.Fail("League data is required.");

            }


            var existing = await _context.Leagues.FindAsync(league.Id);

            if (existing == null)
            {

                return SimpleResult.Fail("League not found.");

            }


            if (string.IsNullOrWhiteSpace(league.Name) || string.IsNullOrWhiteSpace(league.City))
            {

                return SimpleResult.Fail("Name and City are required.");

            }



            var seasonExists = await _context.Seasons.AnyAsync(s => s.Id == league.SeasonId);

            if (!seasonExists)
            {

                return SimpleResult.Fail("Associated season not found.");

            }


            var trimmedName = league.Name.Trim();
            var trimmedCity = league.City.Trim();


            var duplicate = await _context.Leagues.AnyAsync(l => l.Name == trimmedName &&
                                                                 l.City == trimmedCity &&
                                                                 l.SeasonId == league.SeasonId);

            if (duplicate)
            {

                return SimpleResult.Fail("Another league with the same name already exists in the same city for this season.");

            }


            existing.Name = trimmedName;
            existing.City = trimmedCity;
            existing.SeasonId = league.SeasonId;


            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("League updated successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to update league. A league with the same name, city, and season may already exist.");

            }
        }



        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var league = await _context.Leagues.FindAsync(id);

            if (league == null)
            {

                return SimpleResult.Fail("League not found.");

            }


            var hasAssignedTeams = await _context.TeamSeasonLeagues.AnyAsync(tsl => tsl.LeagueId == id);

            if (hasAssignedTeams)
            {

                return SimpleResult.Fail("Cannot delete league with assigned teams.");

            }


            var hasMatches = await _context.Matches.AnyAsync(m => m.LeagueId == id);

            if (hasMatches)
            {

                return SimpleResult.Fail("Cannot delete league with matches.");

            }



            try
            {

                _context.Leagues.Remove(league);
                await _context.SaveChangesAsync();

                return SimpleResult.Ok("League deleted successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to delete league because it is used by other data.");

            }


        }


    }
}