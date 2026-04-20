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
            return await _context.Seasons.AsNoTracking().OrderByDescending(s => s.StartDate).ToListAsync();

        }


        public async Task<Season?> GetByIdAsync(int id)
        {
            return await _context.Seasons.AsNoTracking().Include(s => s.Leagues)
                                                        .Include(s => s.PlayerSeasonTeams)
                                                                .ThenInclude(pst => pst.Player)
                                                        .Include(s => s.PlayerSeasonTeams)
                                                                .ThenInclude(pst => pst.Team)
                                                        .FirstOrDefaultAsync(s => s.Id == id);

        }



        public async Task<SimpleResult> CreateAsync(Season season)
        {
            if (season == null)
            {

                return SimpleResult.Fail("Season data is required.");

            }


            if (string.IsNullOrWhiteSpace(season.Name))
            {

                return SimpleResult.Fail("Name is required.");

            }


            if (season.StartDate >= season.EndDate)
            {

                return SimpleResult.Fail("Start date must be before end date.");

            }


            var trimmedName = season.Name.Trim();
            var exists = await _context.Seasons.AnyAsync(s => s.Name == trimmedName);

            if (exists)
            {

                return SimpleResult.Fail("Season already exists.");

            }


            season.Name = trimmedName;


            _context.Seasons.Add(season);


            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Season created successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to create season. A season with the same name may already exist.");

            }

        }



        public async Task<SimpleResult> UpdateAsync(Season season)
        {
            if (season == null)
            {

                return SimpleResult.Fail("Season data is required.");

            }


            var existing = await _context.Seasons.FindAsync(season.Id);

            if (existing == null)
            {

                return SimpleResult.Fail("Season not found.");

            }


            if (string.IsNullOrWhiteSpace(season.Name))
            {

                return SimpleResult.Fail("Name is required.");

            }


            if (season.StartDate >= season.EndDate)
            {

                return SimpleResult.Fail("Start date must be before end date.");

            }


            var trimmedName = season.Name.Trim();
            var duplicate = await _context.Seasons.AnyAsync(s => s.Id != season.Id && s.Name == trimmedName);

            if (duplicate)
            {

                return SimpleResult.Fail("Season already exists.");

            }


            existing.Name = trimmedName;
            existing.StartDate = season.StartDate;
            existing.EndDate = season.EndDate;


            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Season updated successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to update season. A season with the same name may already exist.");

            }

        }



        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var season = await _context.Seasons.FindAsync(id);

            if (season == null)
            {

                return SimpleResult.Fail("Season not found.");

            }


            var hasLeagues = await _context.Leagues.AnyAsync(l => l.SeasonId == id);

            if (hasLeagues)
            {

                return SimpleResult.Fail("Cannot delete season with leagues.");

            }


            var hasPlayerSeasonTeams = await _context.PlayerSeasonTeams.AnyAsync(pst => pst.SeasonId == id);

            if (hasPlayerSeasonTeams)
            {

                return SimpleResult.Fail("Cannot delete season with player registrations.");

            }



            try
            {

                _context.Seasons.Remove(season);
                await _context.SaveChangesAsync();

                return SimpleResult.Ok("Season deleted successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to delete season because it is used by other data.");

            }

        }

    }
}