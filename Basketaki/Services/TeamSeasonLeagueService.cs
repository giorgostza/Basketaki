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
            return await _context.TeamSeasonLeagues.AsNoTracking().Include(tsl => tsl.Team)
                                                                  .Include(tsl => tsl.League)
                                                                          .ThenInclude(l => l.Season)
                                                                  .OrderByDescending(tsl => tsl.League.Season.StartDate)
                                                                  .ThenBy(tsl => tsl.League.Name)
                                                                  .ThenBy(tsl => tsl.Team.Name)
                                                                  .ToListAsync();

        }


        public async Task<TeamSeasonLeague?> GetByIdAsync(int id)
        {
            return await _context.TeamSeasonLeagues.AsNoTracking().Include(tsl => tsl.Team)
                                                                  .Include(tsl => tsl.League)
                                                                          .ThenInclude(l => l.Season)
                                                                  .FirstOrDefaultAsync(tsl => tsl.Id == id);

        }



        public async Task<SimpleResult> CreateAsync(TeamSeasonLeague model)
        {
            if (model == null)
            {

                return SimpleResult.Fail("Team league assignment data is required.");

            }


            var teamExists = await _context.Teams.AsNoTracking().AnyAsync(t => t.Id == model.TeamId);

            if (!teamExists)
            {

                return SimpleResult.Fail("Team not found.");

            }


            var leagueExists = await _context.Leagues.AsNoTracking().AnyAsync(l => l.Id == model.LeagueId);

            if (!leagueExists)
            {

                return SimpleResult.Fail("League not found.");

            }


            var exists = await _context.TeamSeasonLeagues.AsNoTracking().AnyAsync(tsl => tsl.TeamId == model.TeamId &&
                                                                                         tsl.LeagueId == model.LeagueId);

            if (exists)
            {

                return SimpleResult.Fail("Team already participates in this league.");

            }


            _context.TeamSeasonLeagues.Add(model);



            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Team league assignment created successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to create team league assignment. This team may already participate in the selected league.");

            }

        }


        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var entity = await _context.TeamSeasonLeagues.Include(tsl => tsl.League).FirstOrDefaultAsync(tsl => tsl.Id == id);

            if (entity == null)
            {

                return SimpleResult.Fail("Association not found.");

            }


            var hasMatches = await _context.Matches.AsNoTracking().AnyAsync(m => m.HomeTeamSeasonLeagueId == id || m.AwayTeamSeasonLeagueId == id);

            if (hasMatches)
            {

                return SimpleResult.Fail("Cannot delete association because matches exist for this team in the league.");

            }


            var hasPlayers = await _context.PlayerSeasonTeams.AsNoTracking().AnyAsync(pst => pst.TeamId == entity.TeamId &&
                                                                                             pst.SeasonId == entity.League.SeasonId);

            if (hasPlayers)
            {

                return SimpleResult.Fail("Cannot delete association because players are assigned to this team in the season.");

            }


            var hasStanding = await _context.TeamStandings.AsNoTracking().AnyAsync(ts => ts.TeamSeasonLeagueId == id);

            if (hasStanding)
            {

                return SimpleResult.Fail("Cannot delete association because a standing record exists.");

            }



            try
            {

                _context.TeamSeasonLeagues.Remove(entity);
                await _context.SaveChangesAsync();

                return SimpleResult.Ok("Team league assignment deleted successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to delete team league assignment because it is used by other data.");

            }


        }

    }
}