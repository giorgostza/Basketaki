using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext _context;

        public MatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Match>> GetAllAsync()
        {
            return await _context.Matches.AsNoTracking().Include(m => m.Court)
                                                        .Include(m => m.League)
                                                        .Include(m => m.HomeTeamSeasonLeague)
                                                            .ThenInclude(tsl => tsl.Team)
                                                        .Include(m => m.AwayTeamSeasonLeague)
                                                            .ThenInclude(tsl => tsl.Team)
                                                        .OrderByDescending(m => m.MatchDate)
                                                        .ThenBy(m => m.StartTime)
                                                        .ToListAsync();

        }


        public async Task<Match?> GetByIdAsync(int id)
        {
            return await _context.Matches.AsNoTracking().Include(m => m.Court)
                                                        .Include(m => m.League)
                                                        .Include(m => m.HomeTeamSeasonLeague)
                                                            .ThenInclude(tsl => tsl.Team)
                                                        .Include(m => m.AwayTeamSeasonLeague)
                                                            .ThenInclude(tsl => tsl.Team)
                                                        .Include(m => m.PlayerStats)
                                                            .ThenInclude(ps => ps.PlayerSeasonTeam)
                                                            .ThenInclude(pst => pst.Player)
                                                        .Include(m => m.Photos)
                                                        .Include(m => m.MatchReferees)
                                                        .FirstOrDefaultAsync(m => m.Id == id);

        }



        public async Task<SimpleResult> CreateAsync(Match match)
        {
            if (match == null)
            {

                return SimpleResult.Fail("Match data is required.");

            }

            if (match.HomeTeamSeasonLeagueId == match.AwayTeamSeasonLeagueId)
            {

                return SimpleResult.Fail("Home and Away teams cannot be the same.");

            }

            if (match.StartTime >= match.EndTime)
            {

                return SimpleResult.Fail("Start time must be before end time.");

            }

            if (!match.IsPlayed && (match.HomeScore.HasValue || match.AwayScore.HasValue))
            {

                return SimpleResult.Fail("Scores cannot be set if the match has not been played.");

            }

            if (match.IsPlayed && (!match.HomeScore.HasValue || !match.AwayScore.HasValue))
            {

                return SimpleResult.Fail("Scores are required when the match is marked as played.");

            }



            var courtExists = await _context.Courts.AnyAsync(c => c.Id == match.CourtId);

            if (!courtExists)
            {

                return SimpleResult.Fail("Court not found.");

            }


            var league = await _context.Leagues.AsNoTracking().FirstOrDefaultAsync(l => l.Id == match.LeagueId);

            if (league == null)
            {

                return SimpleResult.Fail("League not found.");

            }



            var homeTeamSeasonLeague = await _context.TeamSeasonLeagues.AsNoTracking().FirstOrDefaultAsync(t => t.Id == match.HomeTeamSeasonLeagueId);
            var awayTeamSeasonLeague = await _context.TeamSeasonLeagues.AsNoTracking().FirstOrDefaultAsync(t => t.Id == match.AwayTeamSeasonLeagueId);

            if (homeTeamSeasonLeague == null || awayTeamSeasonLeague == null)
            {

                return SimpleResult.Fail("Home or away team not found.");

            }


            if (homeTeamSeasonLeague.LeagueId != match.LeagueId || awayTeamSeasonLeague.LeagueId != match.LeagueId)
            {

                return SimpleResult.Fail("Both teams must belong to the selected league.");

            }


            var courtConflict = await _context.Matches.AnyAsync(m => m.CourtId == match.CourtId &&
                                                                m.MatchDate == match.MatchDate &&
                                                                match.StartTime < m.EndTime &&
                                                                match.EndTime > m.StartTime);

            if (courtConflict)
            {

                return SimpleResult.Fail("Court is already booked during this time.");

            }


            var teamConflict = await _context.Matches.AnyAsync(m => m.MatchDate == match.MatchDate &&
                                                               match.StartTime < m.EndTime &&
                                                               match.EndTime > m.StartTime &&
                                                                (
                                                                  m.HomeTeamSeasonLeagueId == match.HomeTeamSeasonLeagueId ||
                                                                  m.AwayTeamSeasonLeagueId == match.HomeTeamSeasonLeagueId ||
                                                                  m.HomeTeamSeasonLeagueId == match.AwayTeamSeasonLeagueId ||
                                                                  m.AwayTeamSeasonLeagueId == match.AwayTeamSeasonLeagueId
                                                                 ));

            if (teamConflict)
            {

                return SimpleResult.Fail("One of the teams already has a match during this time.");

            }


            _context.Matches.Add(match);


            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Match created successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to create match. The selected court or teams may already be booked for this time.");

            }


        }


        public async Task<SimpleResult> UpdateAsync(Match match)
        {
            if (match == null)
            {

                return SimpleResult.Fail("Match data is required.");

            }


            var existing = await _context.Matches.FindAsync(match.Id);

            if (existing == null)
            {

                return SimpleResult.Fail("Match not found.");

            }


            if (match.HomeTeamSeasonLeagueId == match.AwayTeamSeasonLeagueId)
            {

                return SimpleResult.Fail("Home and Away teams cannot be the same.");

            }


            if (match.StartTime >= match.EndTime)
            {

                return SimpleResult.Fail("Start time must be before end time.");

            }


            if (!match.IsPlayed && (match.HomeScore.HasValue || match.AwayScore.HasValue))
            {

                return SimpleResult.Fail("Scores cannot be set if the match has not been played.");

            }


            if (match.IsPlayed && (!match.HomeScore.HasValue || !match.AwayScore.HasValue))
            {

                return SimpleResult.Fail("Scores are required when the match is marked as played.");

            }


            var courtExists = await _context.Courts.AnyAsync(c => c.Id == match.CourtId);

            if (!courtExists)
            {

                return SimpleResult.Fail("Court not found.");

            }


            var league = await _context.Leagues.AsNoTracking().FirstOrDefaultAsync(l => l.Id == match.LeagueId);

            if (league == null)
            {

                return SimpleResult.Fail("League not found.");

            }



            var homeTeamSeasonLeague = await _context.TeamSeasonLeagues.AsNoTracking()
                                                                       .FirstOrDefaultAsync(t => t.Id == match.HomeTeamSeasonLeagueId);

            var awayTeamSeasonLeague = await _context.TeamSeasonLeagues.AsNoTracking()
                                                                       .FirstOrDefaultAsync(t => t.Id == match.AwayTeamSeasonLeagueId);

            if (homeTeamSeasonLeague == null || awayTeamSeasonLeague == null)
            {

                return SimpleResult.Fail("Home or away team not found.");

            }


            if (homeTeamSeasonLeague.LeagueId != match.LeagueId || awayTeamSeasonLeague.LeagueId != match.LeagueId)
            {

                return SimpleResult.Fail("Both teams must belong to the selected league.");

            }



            var courtConflict = await _context.Matches.AnyAsync(m => m.Id != match.Id &&
                                                                m.CourtId == match.CourtId &&
                                                                m.MatchDate == match.MatchDate &&
                                                                match.StartTime < m.EndTime &&
                                                                match.EndTime > m.StartTime);

            if (courtConflict)
            {

                return SimpleResult.Fail("Court is already booked during this time.");

            }



            var teamConflict = await _context.Matches.AnyAsync(m => m.Id != match.Id &&
                                                               m.MatchDate == match.MatchDate &&
                                                               match.StartTime < m.EndTime &&
                                                               match.EndTime > m.StartTime &&
                                                                   (
                                                                    m.HomeTeamSeasonLeagueId == match.HomeTeamSeasonLeagueId ||
                                                                    m.AwayTeamSeasonLeagueId == match.HomeTeamSeasonLeagueId ||
                                                                    m.HomeTeamSeasonLeagueId == match.AwayTeamSeasonLeagueId ||
                                                                    m.AwayTeamSeasonLeagueId == match.AwayTeamSeasonLeagueId
                                                                ));

            if (teamConflict)
            {

                return SimpleResult.Fail("One of the teams already has a match during this time.");

            }




            existing.MatchDate = match.MatchDate;
            existing.StartTime = match.StartTime;
            existing.EndTime = match.EndTime;
            existing.CourtId = match.CourtId;
            existing.LeagueId = match.LeagueId;
            existing.HomeTeamSeasonLeagueId = match.HomeTeamSeasonLeagueId;
            existing.AwayTeamSeasonLeagueId = match.AwayTeamSeasonLeagueId;
            existing.HomeScore = match.HomeScore;
            existing.AwayScore = match.AwayScore;
            existing.IsPlayed = match.IsPlayed;



            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Match updated successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to update match. The selected court or teams may already be booked for this time.");

            }

        }



        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var match = await _context.Matches.FindAsync(id);

            if (match == null)
            {

                return SimpleResult.Fail("Match not found.");

            }


            try
            {

                _context.Matches.Remove(match);
                await _context.SaveChangesAsync();

                return SimpleResult.Ok("Match deleted successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to delete match because it is used by other data.");

            }

        }

    }
}