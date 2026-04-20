 using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class CoachService : ICoachService
    {
        private readonly ApplicationDbContext _context;

        public CoachService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Coach>> GetAllAsync()
        {

            return await _context.Coaches.AsNoTracking().OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToListAsync();

        }



        public async Task<Coach?> GetByIdAsync(int id)
        {

            return await _context.Coaches.AsNoTracking().Include(c => c.Teams).FirstOrDefaultAsync(c => c.Id == id);

        }



        public async Task<SimpleResult> CreateAsync(Coach coach)
        {
            if (coach == null)
            {

                return SimpleResult.Fail("Coach data is required.");

            }


            if (string.IsNullOrWhiteSpace(coach.FirstName) || string.IsNullOrWhiteSpace(coach.LastName))
            {

                return SimpleResult.Fail("First name and last name are required.");

            }



            var today = DateOnly.FromDateTime(DateTime.Today);

            if (coach.DateOfBirth > today)
            {

                return SimpleResult.Fail("Date of birth cannot be in the future.");

            }



            var age = CalculateAge(coach.DateOfBirth);

            if (age < 18 || age > 80)
            {

                return SimpleResult.Fail("Coach age must be between 18 and 80.");

            }



            var trimmedFirstName = coach.FirstName.Trim();
            var trimmedLastName = coach.LastName.Trim();
            var trimmedPhotoUrl = string.IsNullOrWhiteSpace(coach.PhotoUrl) ? null : coach.PhotoUrl.Trim();

            coach.FirstName = trimmedFirstName;
            coach.LastName = trimmedLastName;
            coach.PhotoUrl = trimmedPhotoUrl;

            _context.Coaches.Add(coach);



            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Coach created successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to create coach.");

            }


        }



        public async Task<SimpleResult> UpdateAsync(Coach coach)
        {
            if (coach == null)
            {

                return SimpleResult.Fail("Coach data is required.");

            }


            var existing = await _context.Coaches.FindAsync(coach.Id);

            if (existing == null)
            {

                return SimpleResult.Fail("Coach not found.");

            }


            if (string.IsNullOrWhiteSpace(coach.FirstName) || string.IsNullOrWhiteSpace(coach.LastName))
            {

                return SimpleResult.Fail("First name and last name are required.");

            }


            var today = DateOnly.FromDateTime(DateTime.Today);

            if (coach.DateOfBirth > today)
            {

                return SimpleResult.Fail("Date of birth cannot be in the future.");

            }


            var age = CalculateAge(coach.DateOfBirth);

            if (age < 18 || age > 80)
            {

                return SimpleResult.Fail("Coach age must be between 18 and 80.");

            }


            var trimmedFirstName = coach.FirstName.Trim();
            var trimmedLastName = coach.LastName.Trim();
            var trimmedPhotoUrl = string.IsNullOrWhiteSpace(coach.PhotoUrl) ? null : coach.PhotoUrl.Trim();


            existing.FirstName = trimmedFirstName;
            existing.LastName = trimmedLastName;
            existing.DateOfBirth = coach.DateOfBirth;
            existing.Height = coach.Height;
            existing.PhotoUrl = trimmedPhotoUrl;

            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Coach updated successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to update coach.");

            }

        }



        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var coach = await _context.Coaches.FindAsync(id);

            if (coach == null)
            {

                return SimpleResult.Fail("Coach not found.");

            }


            var isAssignedToTeam = await _context.Teams.AnyAsync(t => t.CoachId == id);

            if (isAssignedToTeam)
            {

                return SimpleResult.Fail("Cannot delete coach assigned to a team.");

            }


            try
            {

                _context.Coaches.Remove(coach);
                await _context.SaveChangesAsync();

                return SimpleResult.Ok("Coach deleted successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to delete coach because it is used by other data.");

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