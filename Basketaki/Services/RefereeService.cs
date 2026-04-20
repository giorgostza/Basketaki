using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class RefereeService : IRefereeService
    {
        private readonly ApplicationDbContext _context;

        public RefereeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Referee>> GetAllAsync()
        {

            return await _context.Referees.AsNoTracking().OrderBy(r => r.LastName).ThenBy(r => r.FirstName).ToListAsync();

        }


        public async Task<Referee?> GetByIdAsync(int id)
        {
            return await _context.Referees.AsNoTracking().Include(r => r.MatchReferees)
                                                                 .ThenInclude(mr => mr.Match)
                                                         .FirstOrDefaultAsync(r => r.Id == id);

        }



        public async Task<SimpleResult> CreateAsync(Referee referee)
        {
            if (referee == null)
            {

                return SimpleResult.Fail("Referee data is required.");

            }


            if (string.IsNullOrWhiteSpace(referee.FirstName) || string.IsNullOrWhiteSpace(referee.LastName))
            {

                return SimpleResult.Fail("First name and last name are required.");

            }


            var today = DateOnly.FromDateTime(DateTime.Today);

            if (referee.DateOfBirth > today)
            {

                return SimpleResult.Fail("Date of birth cannot be in the future.");

            }


            var age = CalculateAge(referee.DateOfBirth);

            if (age < 18 || age > 60)
            {

                return SimpleResult.Fail("Referee age must be between 18 and 60.");

            }



            var trimmedFirstName = referee.FirstName.Trim();
            var trimmedLastName = referee.LastName.Trim();
            var trimmedPhotoUrl = string.IsNullOrWhiteSpace(referee.PhotoUrl) ? null : referee.PhotoUrl.Trim();


            referee.FirstName = trimmedFirstName;
            referee.LastName = trimmedLastName;
            referee.PhotoUrl = trimmedPhotoUrl;


            _context.Referees.Add(referee);


            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Referee created successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to create referee.");

            }

        }



        public async Task<SimpleResult> UpdateAsync(Referee referee)
        {
            if (referee == null)
            {

                return SimpleResult.Fail("Referee data is required.");

            }


            var existing = await _context.Referees.FindAsync(referee.Id);

            if (existing == null)
            {

                return SimpleResult.Fail("Referee not found.");

            }


            if (string.IsNullOrWhiteSpace(referee.FirstName) || string.IsNullOrWhiteSpace(referee.LastName))
            {

                return SimpleResult.Fail("First name and last name are required.");

            }


            var today = DateOnly.FromDateTime(DateTime.Today);

            if (referee.DateOfBirth > today)
            {

                return SimpleResult.Fail("Date of birth cannot be in the future.");

            }


            var age = CalculateAge(referee.DateOfBirth);

            if (age < 18 || age > 60)
            {

                return SimpleResult.Fail("Referee age must be between 18 and 60.");

            }


            var trimmedFirstName = referee.FirstName.Trim();
            var trimmedLastName = referee.LastName.Trim();
            var trimmedPhotoUrl = string.IsNullOrWhiteSpace(referee.PhotoUrl) ? null : referee.PhotoUrl.Trim();


            existing.FirstName = trimmedFirstName;
            existing.LastName = trimmedLastName;
            existing.DateOfBirth = referee.DateOfBirth;
            existing.Height = referee.Height;
            existing.PhotoUrl = trimmedPhotoUrl;



            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Referee updated successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to update referee.");

            }

        }


        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var referee = await _context.Referees.FindAsync(id);

            if (referee == null)
            {

                return SimpleResult.Fail("Referee not found.");

            }


            var hasMatches = await _context.MatchReferees.AnyAsync(mr => mr.RefereeId == id);

            if (hasMatches)
            {

                return SimpleResult.Fail("Cannot delete referee assigned to matches.");

            }



            try
            {

                _context.Referees.Remove(referee);
                await _context.SaveChangesAsync();

                return SimpleResult.Ok("Referee deleted successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to delete referee because it is used by other data.");

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