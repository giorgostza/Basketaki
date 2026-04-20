using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly ApplicationDbContext _context;

        public PlayerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Player>> GetAllAsync()
        {

            return await _context.Players.AsNoTracking().OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToListAsync();

        }


        public async Task<Player?> GetByIdAsync(int id)
        {
            return await _context.Players.AsNoTracking().Include(p => p.PlayerSeasonTeams)
                                                                .ThenInclude(pst => pst.Team)
                                                        .Include(p => p.PlayerSeasonTeams)
                                                                .ThenInclude(pst => pst.Season)
                                                        .FirstOrDefaultAsync(p => p.Id == id);

        }



        public async Task<SimpleResult> CreateAsync(Player player)
        {
            if (player == null)
            {

                return SimpleResult.Fail("Player data is required.");

            }


            if (string.IsNullOrWhiteSpace(player.FirstName) || string.IsNullOrWhiteSpace(player.LastName))
            {

                return SimpleResult.Fail("First name and last name are required.");

            }


            var today = DateOnly.FromDateTime(DateTime.Today);

            if (player.DateOfBirth > today)
            {

                return SimpleResult.Fail("Date of birth cannot be in the future.");

            }


            var age = CalculateAge(player.DateOfBirth);

            if (age < 15 || age > 50)
            {

                return SimpleResult.Fail("Player age must be between 15 and 50.");

            }



            var trimmedFirstName = player.FirstName.Trim();
            var trimmedLastName = player.LastName.Trim();
            var trimmedPhotoUrl = string.IsNullOrWhiteSpace(player.PhotoUrl) ? null : player.PhotoUrl.Trim();


            player.FirstName = trimmedFirstName;
            player.LastName = trimmedLastName;
            player.PhotoUrl = trimmedPhotoUrl;

            _context.Players.Add(player);

            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Player created successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to create player.");

            }


        }



        public async Task<SimpleResult> UpdateAsync(Player player)
        {
            if (player == null)
            {

                return SimpleResult.Fail("Player data is required.");

            }


            var existing = await _context.Players.FindAsync(player.Id);

            if (existing == null)
            {

                return SimpleResult.Fail("Player not found.");

            }


            if (string.IsNullOrWhiteSpace(player.FirstName) || string.IsNullOrWhiteSpace(player.LastName))
            {

                return SimpleResult.Fail("First name and last name are required.");

            }


            var today = DateOnly.FromDateTime(DateTime.Today);

            if (player.DateOfBirth > today)
            {

                return SimpleResult.Fail("Date of birth cannot be in the future.");

            }


            var age = CalculateAge(player.DateOfBirth);

            if (age < 15 || age > 50)
            {

                return SimpleResult.Fail("Player age must be between 15 and 50.");

            }


            var trimmedFirstName = player.FirstName.Trim();
            var trimmedLastName = player.LastName.Trim();
            var trimmedPhotoUrl = string.IsNullOrWhiteSpace(player.PhotoUrl) ? null : player.PhotoUrl.Trim();


            existing.FirstName = trimmedFirstName;
            existing.LastName = trimmedLastName;
            existing.DateOfBirth = player.DateOfBirth;
            existing.Height = player.Height;
            existing.Weight = player.Weight;
            existing.Position = player.Position;
            existing.PhotoUrl = trimmedPhotoUrl;



            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Player updated successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to update player.");

            }

        }



        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {

                return SimpleResult.Fail("Player not found.");

            }


            var hasPlayerSeasonTeams = await _context.PlayerSeasonTeams.AnyAsync(pst => pst.PlayerId == id);

            if (hasPlayerSeasonTeams)
            {

                return SimpleResult.Fail("Cannot delete player assigned to teams/seasons.");

            }

            try
            {

                _context.Players.Remove(player);
                await _context.SaveChangesAsync();

                return SimpleResult.Ok("Player deleted successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to delete player because it is used by other data.");

            }

        }



        private int CalculateAge(DateOnly birthDate)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age))
            {

                age--;

            }

            return age;

        }


    }
}