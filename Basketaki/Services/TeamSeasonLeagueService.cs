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
                                 .OrderBy(tsl => tsl.Team.Name)
                                 .ThenBy(tsl => tsl.League.Name)
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
            // Check if the specified TeamId exists in the database to ensure referential integrity
            if (!await _context.Teams.AnyAsync(t => t.Id == model.TeamId)) 
            {

                return false;

            }


            // Check if the specified LeagueId exists in the database to ensure referential integrity
            if (!await _context.Leagues.AnyAsync(l => l.Id == model.LeagueId))
            {

                return false;

            }


            // Check if a TeamSeasonLeague with the same TeamId and LeagueId already exists to prevent duplicate entries
            if (await CombinationExistsAsync(model.TeamId, model.LeagueId)) 
            {

                return false;

            }

            _context.TeamSeasonLeagues.Add(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Retrieve the TeamSeasonLeague entity to be deleted from the database using its ID
            var entity = await _context.TeamSeasonLeagues.FindAsync(id); 

            if (entity == null)
            {

                return false;

            }


            // Check if there are any Matches associated with the TeamSeasonLeague before allowing deletion
            var hasMatches = await _context.Matches.AnyAsync(m => m.HomeTeamSeasonLeagueId == id || m.AwayTeamSeasonLeagueId == id);

            if (hasMatches)
            {

                return false;

            }


            // Retrieve the League associated with the TeamSeasonLeague to check its SeasonId for the player association check
            var league = await _context.Leagues.FirstOrDefaultAsync(l => l.Id == entity.LeagueId);

            if (league == null)
            {

                return false;

            }


            // Check if there are any PlayerSeasonTeams associated with the Team and Season of the TeamSeasonLeague before allowing deletion
            var hasPlayers = await _context.PlayerSeasonTeams.AnyAsync(pst => pst.TeamId == entity.TeamId && pst.SeasonId == league.SeasonId);

            if (hasPlayers)
            {

                return false;

            }

            _context.TeamSeasonLeagues.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id) // Check if a TeamSeasonLeague with the specified ID exists in the database
        {

            return await _context.TeamSeasonLeagues.AnyAsync(tsl => tsl.Id == id); 

        }

        public async Task<bool> CombinationExistsAsync(int teamId, int leagueId)
        {
            // Check if a TeamSeasonLeague with the specified TeamId and LeagueId already exists in the database to prevent duplicate entries
            return await _context.TeamSeasonLeagues.AnyAsync(tsl => tsl.TeamId == teamId && tsl.LeagueId == leagueId);

        }
    }
}