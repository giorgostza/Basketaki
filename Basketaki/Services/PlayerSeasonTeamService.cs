using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class PlayerSeasonTeamService : IPlayerSeasonTeamService
    {
        private readonly ApplicationDbContext _context;

        public PlayerSeasonTeamService(ApplicationDbContext context)
        {

            _context = context;

        }

        public async Task<List<PlayerSeasonTeam>> GetAllAsync() // Retrieves all PlayerSeasonTeam records, including related Player, Team, and Season data 
        {                                                       // ordered by Season start date, Team name, and Player name

            return await _context.PlayerSeasonTeams
                .Include(pst => pst.Player)
                .Include(pst => pst.Team)
                .Include(pst => pst.Season)
                        .OrderBy(pst => pst.Season.StartDate)
                        .ThenBy(pst => pst.Team.Name)
                        .ThenBy(pst => pst.Player.LastName)
                        .ThenBy(pst => pst.Player.FirstName)
                .ToListAsync(); 

        }

        public async Task<PlayerSeasonTeam?> GetByIdAsync(int id) // Retrieves a PlayerSeasonTeam by its ID, including related Player, Team, and Season data
        {

            return await _context.PlayerSeasonTeams
                .Include(pst => pst.Player)
                .Include(pst => pst.Team)
                .Include(pst => pst.Season)
                .FirstOrDefaultAsync(pst => pst.Id == id); // FindAsync doesn't support Include, so we use FirstOrDefaultAsync with a filter
        
        }

        public async Task<bool> CreateAsync(PlayerSeasonTeam model)
        {
            
            var playerExists = await _context.Players.AnyAsync(p => p.Id == model.PlayerId);
            var teamExists = await _context.Teams.AnyAsync(t => t.Id == model.TeamId);
            var seasonExists = await _context.Seasons.AnyAsync(s => s.Id == model.SeasonId);

            if (!playerExists || !teamExists || !seasonExists) // check if related entities exist before creating the PlayerSeasonTeam
            {

                return false;

            }

            
            if (await CombinationExistsAsync(model.PlayerId, model.SeasonId)) 
            {

                return false;

            }

            _context.PlayerSeasonTeams.Add(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.PlayerSeasonTeams.FindAsync(id); // Find the PlayerSeasonTeam by ID 
                                                                         //without Include since we only need to check for related PlayerStats
            if (entity == null)
            {

                return false;

            }


            // Check if there are any PlayerStats associated with this PlayerSeasonTeam before allowing deletion
            if (await _context.PlayerStats.AnyAsync(ps => ps.PlayerSeasonTeamId == id)) 
            {

                return false;

            }

            _context.PlayerSeasonTeams.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id)  // Check if a PlayerSeasonTeam with the specified ID exists in the database
        {

            return await _context.PlayerSeasonTeams.AnyAsync(pst => pst.Id == id); 

        }

        public async Task<bool> CombinationExistsAsync(int playerId, int seasonId)
        {

            return await _context.PlayerSeasonTeams.AnyAsync(pst => pst.PlayerId == playerId && pst.SeasonId == seasonId);
            // Check if a PlayerSeasonTeam with the same PlayerId and SeasonId already exists 
            // preventing duplicate assignments of a Player to multiple Teams in the same Season

        }
    }
}