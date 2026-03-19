using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class TeamSeasonLeagueService : ITeamSeasonLeagueService
    {
        private readonly ApplicationDbContext _context;

        public TeamSeasonLeagueService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TeamSeasonLeague>> GetAllAsync() 
        {
            return await _context.TeamSeasonLeagues   
                .Include(tsl => tsl.Team)                      
                .Include(tsl => tsl.League)
                        .ThenInclude(l => l.Season)
                .ToListAsync();
        }

        public async Task<TeamSeasonLeague?> GetByIdAsync(int id) 
        {
            return await _context.TeamSeasonLeagues
                .Include(tsl => tsl.Team)
                .Include(tsl => tsl.League)
                        .ThenInclude(l => l.Season)
                .FirstOrDefaultAsync(tsl => tsl.Id == id);
        }

        public async Task<bool> CreateAsync(TeamSeasonLeague model)
        {
            
            var teamExists = await _context.Teams.AnyAsync(t => t.Id == model.TeamId);  // Check if the team exists

            if (!teamExists)
            {

                return false;

            }
                

            
            var leagueExists = await _context.Leagues.AnyAsync(l => l.Id == model.LeagueId);  // Check if the league exists

            if (!leagueExists)
            {

                return false;

            }


            if (await CombinationExistsAsync(model.TeamId, model.LeagueId))  // Check if the combination of TeamId and LeagueId already exists
            {                                                                   

                return false;

            }

            _context.TeamSeasonLeagues.Add(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.TeamSeasonLeagues.FindAsync(id);

            if (entity == null)   // Check if the entity exists
            {

                return false;

            }


            if (await _context.Matches.AnyAsync(m => m.HomeTeamSeasonLeagueId == id || m.AwayTeamSeasonLeagueId == id))  // Check if there are any Matches associated with this TeamSeasonLeague
            {

                return false;

            }



            _context.TeamSeasonLeagues.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.TeamSeasonLeagues.AnyAsync(tsl => tsl.Id == id);  // Check if an entity with the given ID exists
        }

        public async Task<bool> CombinationExistsAsync(int teamId, int leagueId)
        {
            return await _context.TeamSeasonLeagues.AnyAsync(tsl => (tsl.TeamId == teamId) && (tsl.LeagueId == leagueId));  // Check if a specific combination of TeamId and LeagueId already exists

        }
    }
}
