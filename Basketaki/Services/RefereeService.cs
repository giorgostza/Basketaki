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

        public async Task<List<Referee>> GetAllAsync() // Retrieve all Referees from the database, including their associated MatchReferees  
        {                                              // and order them by LastName and then by FirstName for better organization

            return await _context.Referees
                        .AsNoTracking()
                        .Include(r => r.MatchReferees)
                        .OrderBy(r => r.LastName)         
                        .ThenBy(r => r.FirstName)
                        .ToListAsync();

        }

        public async Task<Referee?> GetByIdAsync(int id)
        {
            // Retrieve a Referee by their ID, including their associated MatchReferees for comprehensive data retrieval
            return await _context.Referees.AsNoTracking().Include(r => r.MatchReferees).FirstOrDefaultAsync(r => r.Id == id); 

        }

        public async Task<SimpleResult> CreateAsync(Referee referee)
        {
            var firstName = referee.FirstName?.Trim().ToLower() ?? "";
            var lastName = referee.LastName?.Trim().ToLower() ?? "";

            // Validate that the FirstName and LastName properties are not null empty or whitespace 
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {

                return new SimpleResult { Success = false, Message = "FirstName and LastName are required" };

            }

            referee.FirstName = firstName;
            referee.LastName = lastName;

            _context.Referees.Add(referee);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };

        }

        public async Task<SimpleResult> UpdateAsync(Referee referee)
        {

            var existing = await _context.Referees.FindAsync(referee.Id);

            if (existing == null)
            {
                return new SimpleResult { Success = false, Message = "Referee not found" };
            }


            var firstName = referee.FirstName?.Trim().ToLower() ?? "";
            var lastName = referee.LastName?.Trim().ToLower() ?? "";

            // Validate that the FirstName and LastName properties are not null, empty or whitespace 
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName)) 
            {

                return new SimpleResult { Success = false, Message = "FirstName and LastName are required" };

            }

            // Controlled update (όπως Court / Player)

            existing.FirstName = firstName;
            existing.LastName = lastName;
            existing.Age = referee.Age;
            existing.Height = referee.Height;
            existing.PhotoUrl = referee.PhotoUrl?.Trim();

            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<SimpleResult> DeleteAsync(int id)
        {

            var referee = await _context.Referees.FindAsync(id); // Retrieve the Referee to be deleted from the database

            if (referee == null)
            {

                return new SimpleResult { Success = false, Message = "Referee not found" };

            }


            // Check if the Referee is assigned to any Matches before allowing deletion to prevent orphaned records and maintain data integrity
            var hasMatches = await _context.MatchReferees.AnyAsync(mr => mr.RefereeId == id); 

            if (hasMatches)
            {

                return new SimpleResult { Success = false, Message = "Referee not found" };

            }

            _context.Referees.Remove(referee);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

       
    }
}