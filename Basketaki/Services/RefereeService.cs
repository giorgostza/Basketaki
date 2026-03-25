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
                .Include(r => r.MatchReferees)
                .OrderBy(r => r.LastName)         
                .ThenBy(r => r.FirstName)
                .ToListAsync();

        }

        public async Task<Referee?> GetByIdAsync(int id)
        {
            // Retrieve a Referee by their ID, including their associated MatchReferees for comprehensive data retrieval
            return await _context.Referees.Include(r => r.MatchReferees).FirstOrDefaultAsync(r => r.Id == id); 

        }

        public async Task<bool> CreateAsync(Referee referee)
        {
            // Validate that the FirstName and LastName properties are not null empty or whitespace 
            if (string.IsNullOrWhiteSpace(referee.FirstName) || string.IsNullOrWhiteSpace(referee.LastName))
            {

                return false;

            }

            _context.Referees.Add(referee);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> UpdateAsync(Referee referee)
        {

            if (!await ExistsAsync(referee.Id)) // Validate that the Referee exists in the database before attempting to update
            {

                return false;

            }

            // Validate that the FirstName and LastName properties are not null, empty or whitespace 
            if (string.IsNullOrWhiteSpace(referee.FirstName) || string.IsNullOrWhiteSpace(referee.LastName)) 
            {

                return false;

            }

            _context.Referees.Update(referee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {

            var referee = await _context.Referees.FindAsync(id); // Retrieve the Referee to be deleted from the database

            if (referee == null)
            {

                return false;

            }


            // Check if the Referee is assigned to any Matches before allowing deletion to prevent orphaned records and maintain data integrity
            var hasMatches = await _context.MatchReferees.AnyAsync(mr => mr.RefereeId == id); 

            if (hasMatches)
            {

                return false;

            }

            _context.Referees.Remove(referee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id) // Check if a Referee with the specified ID exists in the database
        {
            return await _context.Referees.AnyAsync(r => r.Id == id); 
        }
    }
}