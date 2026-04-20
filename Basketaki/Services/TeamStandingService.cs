using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class TeamStandingService : ITeamStandingService
    {
        private readonly ApplicationDbContext _context;

        public TeamStandingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TeamStanding>> GetAllAsync()
        {
            return await _context.TeamStandings.AsNoTracking().Include(ts => ts.TeamSeasonLeague)
                                                                      .ThenInclude(tsl => tsl.Team)
                                                              .Include(ts => ts.TeamSeasonLeague)
                                                                      .ThenInclude(tsl => tsl.League)
                                                              .OrderByDescending(ts => ts.LeaguePoints)
                                                              .ThenByDescending(ts => ts.Wins)
                                                              .ThenBy(ts => ts.Losses)
                                                              .ToListAsync();

        }


        public async Task<TeamStanding?> GetByIdAsync(int id)
        {
            return await _context.TeamStandings.AsNoTracking().Include(ts => ts.TeamSeasonLeague)
                                                                      .ThenInclude(tsl => tsl.Team)
                                                              .Include(ts => ts.TeamSeasonLeague)
                                                                      .ThenInclude(tsl => tsl.League)
                                                              .FirstOrDefaultAsync(ts => ts.Id == id);

        }


        public async Task<TeamStanding?> GetByTeamSeasonLeagueIdAsync(int teamSeasonLeagueId)
        {
            return await _context.TeamStandings.AsNoTracking().Include(ts => ts.TeamSeasonLeague)
                                                                      .ThenInclude(tsl => tsl.Team)
                                                              .Include(ts => ts.TeamSeasonLeague)
                                                                      .ThenInclude(tsl => tsl.League)
                                                              .FirstOrDefaultAsync(ts => ts.TeamSeasonLeagueId == teamSeasonLeagueId);


        }



        public async Task<SimpleResult> CreateAsync(TeamStanding standing)
        {
            if (standing == null)
            {

                return SimpleResult.Fail("Team standing data is required.");

            }


            var teamSeasonLeagueExists = await _context.TeamSeasonLeagues.AsNoTracking().AnyAsync(tsl => tsl.Id == standing.TeamSeasonLeagueId);

            if (!teamSeasonLeagueExists)
            {

                return SimpleResult.Fail("TeamSeasonLeague does not exist.");

            }


            var exists = await _context.TeamStandings.AsNoTracking().AnyAsync(ts => ts.TeamSeasonLeagueId == standing.TeamSeasonLeagueId);

            if (exists)
            {

                return SimpleResult.Fail("Standing for this TeamSeasonLeague already exists.");

            }


            var validationResult = ValidateStanding(standing);

            if (!validationResult.Success)
            {

                return validationResult;

            }


            _context.TeamStandings.Add(standing);


            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Team standing created successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to create team standing.");

            }

        }



        public async Task<SimpleResult> UpdateAsync(TeamStanding standing)
        {
            if (standing == null)
            {

                return SimpleResult.Fail("Team standing data is required.");

            }


            var existing = await _context.TeamStandings.FirstOrDefaultAsync(ts => ts.Id == standing.Id);

            if (existing == null)
            {

                return SimpleResult.Fail("TeamStanding not found.");

            }


            var validationResult = ValidateStanding(standing);

            if (!validationResult.Success)
            {

                return validationResult;

            }


            existing.Played = standing.Played;
            existing.Wins = standing.Wins;
            existing.Losses = standing.Losses;
            existing.PointsFor = standing.PointsFor;
            existing.PointsAgainst = standing.PointsAgainst;
            existing.LeaguePoints = standing.LeaguePoints;
            existing.NoShow = standing.NoShow;
            existing.CurrentStreak = standing.CurrentStreak;



            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Team standing updated successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to update team standing.");

            }

        }



        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var standing = await _context.TeamStandings.FindAsync(id);

            if (standing == null)
            {

                return SimpleResult.Fail("TeamStanding not found.");

            }



            try
            {

                _context.TeamStandings.Remove(standing);
                await _context.SaveChangesAsync();

                return SimpleResult.Ok("Team standing deleted successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to delete team standing.");

            }

        }



        private static SimpleResult ValidateStanding(TeamStanding standing)
        {
            if (standing.Played < 0 || standing.Wins < 0 || standing.Losses < 0 || standing.PointsFor < 0 ||
                standing.PointsAgainst < 0 || standing.LeaguePoints < 0 || standing.NoShow < 0)
            {

                return SimpleResult.Fail("Standing values cannot be negative.");

            }


            if (standing.Played < standing.Wins + standing.Losses)
            {

                return SimpleResult.Fail("Played cannot be less than wins plus losses.");

            }


            if (standing.NoShow > standing.Losses)
            {

                return SimpleResult.Fail("No-show count cannot be greater than losses.");

            }


            return SimpleResult.Ok();

        }


    }
}