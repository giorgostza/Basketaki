using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _context;

        public TeamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Team>> GetAllAsync()
        {
            return await _context.Teams
                .Include(t => t.TeamSeasonLeagues)  // Show which Leagues the Team participates
                .Include(t => t.PlayerSeasonTeams)  // Show Players by Season
                .ToListAsync();  
        }

        public async Task<Team?> GetByIdAsync(int id)   
        {
            return await _context.Teams
                .Include(t => t.TeamSeasonLeagues)      // Show which Leagues the Team participates
                .Include(t => t.PlayerSeasonTeams)      // Show Players by Season
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> CreateAsync(Team team)
        {
            
            if (await NameExistsAsync(team.Name))     // Check if the Name is UNIQUE before creating the Team
            {

                return false;

            }
               

            _context.Teams.Add(team);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Team team)
        {
            
            var exists = await _context.Teams.AnyAsync(t => t.Name == team.Name && t.Id != team.Id);

            if (exists)                  // Check if the Name is UNIQUE before updating the Team
            {
                return false;
            }
                

            _context.Teams.Update(team);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);  // Find the Team by Id

            if (team == null)                                     
            {
                return false;
            }

            
            if (await _context.TeamSeasonLeagues.AnyAsync(tsl => tsl.TeamId == id))  // Check if the Team is assigned to any League before DELETE
            {
                return false;                                                           
            }


            if (await _context.PlayerSeasonTeams.AnyAsync(pst => pst.TeamId == id))  // Check if the Team has Players assigned before DELETE
            {
                return false;                                                           
            }
                

            _context.Teams.Remove(team);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id) 
        {
            return await _context.Teams.AnyAsync(t => t.Id == id);
        }

        public async Task<bool> NameExistsAsync(string name)    
        {
            return await _context.Teams.AnyAsync(t => t.Name == name);
        }
    }
}