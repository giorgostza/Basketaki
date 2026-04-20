using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class PlayerStatService : IPlayerStatService
    {
        private readonly ApplicationDbContext _context;

        public PlayerStatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PlayerStat>> GetAllAsync()
        {
            return await _context.PlayerStats.AsNoTracking().Include(ps => ps.PlayerSeasonTeam)
                                                                    .ThenInclude(pst => pst.Player)
                                                            .Include(ps => ps.PlayerSeasonTeam)
                                                                    .ThenInclude(pst => pst.Team)
                                                            .Include(ps => ps.Match)
                                                            .OrderByDescending(ps => ps.Match.MatchDate)
                                                            .ThenBy(ps => ps.PlayerSeasonTeam.Team.Name)
                                                            .ThenBy(ps => ps.PlayerSeasonTeam.Player.LastName)
                                                            .ThenBy(ps => ps.PlayerSeasonTeam.Player.FirstName)
                                                            .ToListAsync();

        }


        public async Task<PlayerStat?> GetByIdAsync(int id)
        {
            return await _context.PlayerStats.AsNoTracking().Include(ps => ps.PlayerSeasonTeam)
                                                                    .ThenInclude(pst => pst.Player)
                                                            .Include(ps => ps.PlayerSeasonTeam)
                                                                    .ThenInclude(pst => pst.Team)
                                                            .Include(ps => ps.Match)
                                                            .FirstOrDefaultAsync(ps => ps.Id == id);

        }


        public async Task<List<PlayerStat>> GetByMatchIdAsync(int matchId)
        {
            return await _context.PlayerStats.AsNoTracking().Where(ps => ps.MatchId == matchId)
                                                            .Include(ps => ps.PlayerSeasonTeam)
                                                                    .ThenInclude(pst => pst.Player)
                                                            .Include(ps => ps.PlayerSeasonTeam)
                                                                    .ThenInclude(pst => pst.Team)
                                                            .OrderBy(ps => ps.PlayerSeasonTeam.Team.Name)
                                                            .ThenBy(ps => ps.PlayerSeasonTeam.Player.LastName)
                                                            .ThenBy(ps => ps.PlayerSeasonTeam.Player.FirstName)
                                                            .ToListAsync();

        }


        public async Task<SimpleResult> CreateAsync(PlayerStat playerStat)
        {
            if (playerStat == null)
            {

                return SimpleResult.Fail("Player stat data is required.");

            }


            var validationResult = ValidateStatValues(playerStat);

            if (!validationResult.Success)
            {

                return validationResult;

            }


            var playerSeasonTeam = await _context.PlayerSeasonTeams.AsNoTracking().FirstOrDefaultAsync(pst => pst.Id == playerStat.PlayerSeasonTeamId);
            var match = await _context.Matches.AsNoTracking().FirstOrDefaultAsync(m => m.Id == playerStat.MatchId);

            if (playerSeasonTeam == null || match == null)
            {

                return SimpleResult.Fail("Invalid player or match.");

            }

            return await CreateWithProperChecksAsync(playerStat, playerSeasonTeam, match);

        }



        public async Task<SimpleResult> UpdateAsync(PlayerStat playerStat)
        {
            if (playerStat == null)
            {

                return SimpleResult.Fail("Player stat data is required.");

            }


            var existing = await _context.PlayerStats.FindAsync(playerStat.Id);

            if (existing == null)
            {

                return SimpleResult.Fail("Stat not found.");

            }


            var validationResult = ValidateStatValues(playerStat);

            if (!validationResult.Success)
            {

                return validationResult;

            }


            var playerSeasonTeam = await _context.PlayerSeasonTeams.AsNoTracking().FirstOrDefaultAsync(pst => pst.Id == playerStat.PlayerSeasonTeamId);
            var match = await _context.Matches.AsNoTracking().FirstOrDefaultAsync(m => m.Id == playerStat.MatchId);

            if (playerSeasonTeam == null || match == null)
            {

                return SimpleResult.Fail("Invalid player or match.");

            }


            var homeTeamSeasonLeague = await _context.TeamSeasonLeagues.AsNoTracking().FirstOrDefaultAsync(tsl => tsl.Id == match.HomeTeamSeasonLeagueId);
            var awayTeamSeasonLeague = await _context.TeamSeasonLeagues.AsNoTracking().FirstOrDefaultAsync(tsl => tsl.Id == match.AwayTeamSeasonLeagueId);

            if (homeTeamSeasonLeague == null || awayTeamSeasonLeague == null)
            {

                return SimpleResult.Fail("Match teams not found.");

            }


            var belongsToMatch = playerSeasonTeam.TeamId == homeTeamSeasonLeague.TeamId ||
                                 playerSeasonTeam.TeamId == awayTeamSeasonLeague.TeamId;

            if (!belongsToMatch)
            {

                return SimpleResult.Fail("Player must belong to one of the teams participating in the match.");

            }



            var duplicate = await _context.PlayerStats.AsNoTracking()
                                                      .AnyAsync(ps =>
                                                          ps.Id != playerStat.Id &&
                                                          ps.PlayerSeasonTeamId == playerStat.PlayerSeasonTeamId &&
                                                          ps.MatchId == playerStat.MatchId);

            if (duplicate)
            {

                return SimpleResult.Fail("Duplicate stat for this player and match.");

            }



            existing.PlayerSeasonTeamId = playerStat.PlayerSeasonTeamId;
            existing.MatchId = playerStat.MatchId;
            existing.Points = playerStat.Points;
            existing.Assists = playerStat.Assists;
            existing.OffensiveRebounds = playerStat.OffensiveRebounds;
            existing.DefensiveRebounds = playerStat.DefensiveRebounds;
            existing.Steals = playerStat.Steals;
            existing.Blocks = playerStat.Blocks;
            existing.Fouls = playerStat.Fouls;
            existing.MinutesPlayed = playerStat.MinutesPlayed;
            existing.FreeThrowsMade = playerStat.FreeThrowsMade;
            existing.FreeThrowsAttempted = playerStat.FreeThrowsAttempted;
            existing.TwoPointsMade = playerStat.TwoPointsMade;
            existing.TwoPointsAttempted = playerStat.TwoPointsAttempted;
            existing.ThreePointsMade = playerStat.ThreePointsMade;
            existing.ThreePointsAttempted = playerStat.ThreePointsAttempted;
            existing.IsMVP = playerStat.IsMVP;
            existing.SuspensionGames = playerStat.SuspensionGames;



            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Player stats updated successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to update player stats.");

            }

        }



        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var stat = await _context.PlayerStats.FindAsync(id);

            if (stat == null)
            {

                return SimpleResult.Fail("Stat not found.");

            }



            try
            {

                _context.PlayerStats.Remove(stat);
                await _context.SaveChangesAsync();

                return SimpleResult.Ok("Player stats deleted successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to delete player stats.");

            }


        }




        private async Task<SimpleResult> CreateWithProperChecksAsync(PlayerStat playerStat, PlayerSeasonTeam playerSeasonTeam, Match match)
        {
            var homeTeamSeasonLeague = await _context.TeamSeasonLeagues.AsNoTracking()
                                                                       .FirstOrDefaultAsync(tsl => tsl.Id == match.HomeTeamSeasonLeagueId);

            var awayTeamSeasonLeague = await _context.TeamSeasonLeagues.AsNoTracking()
                                                                       .FirstOrDefaultAsync(tsl => tsl.Id == match.AwayTeamSeasonLeagueId);

            if (homeTeamSeasonLeague == null || awayTeamSeasonLeague == null)
            {

                return SimpleResult.Fail("Match teams not found.");

            }


            var belongsToMatch = playerSeasonTeam.TeamId == homeTeamSeasonLeague.TeamId ||
                                 playerSeasonTeam.TeamId == awayTeamSeasonLeague.TeamId;

            if (!belongsToMatch)
            {

                return SimpleResult.Fail("Player must belong to one of the teams participating in the match.");

            }


            var exists = await _context.PlayerStats.AsNoTracking()
                                                   .AnyAsync(ps =>
                                                       ps.PlayerSeasonTeamId == playerStat.PlayerSeasonTeamId &&
                                                       ps.MatchId == playerStat.MatchId);

            if (exists)
            {

                return SimpleResult.Fail("Stats already exist for this player in this match.");

            }


            _context.PlayerStats.Add(playerStat);


            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Player stats created successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to create player stats.");

            }

        }




        private static SimpleResult ValidateStatValues(PlayerStat playerStat)
        {
            if (playerStat.Points < 0)
            {

                return SimpleResult.Fail("Points cannot be negative.");

            }


            if (playerStat.Fouls > 5)
            {

                return SimpleResult.Fail("Fouls cannot exceed 5.");

            }


            if (playerStat.FreeThrowsMade > playerStat.FreeThrowsAttempted)
            {

                return SimpleResult.Fail("Free throws made cannot exceed free throws attempted.");

            }


            if (playerStat.TwoPointsMade > playerStat.TwoPointsAttempted)
            {

                return SimpleResult.Fail("Two-points made cannot exceed two-points attempted.");

            }


            if (playerStat.ThreePointsMade > playerStat.ThreePointsAttempted)
            {

                return SimpleResult.Fail("Three-points made cannot exceed three-points attempted.");

            }


            return SimpleResult.Ok();

        }

    }
}