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

        public async Task<List<Match>> GetAllAsync() // Retrieve all Matches, including related entities such as Court, League, Home and Away teams.
        {                                            // PlayerStats and Photos are not included in this method for performance reasons.

            return await _context.Matches
                        .AsNoTracking()
                        .Include(m => m.Court)
                        .Include(m => m.League)
                        .Include(m => m.HomeTeamSeasonLeague)
                                .ThenInclude(tsl => tsl.Team)
                        .Include(m => m.AwayTeamSeasonLeague)
                                .ThenInclude(tsl => tsl.Team)
                        .ToListAsync();


        }

        public async Task<Match?> GetByIdAsync(int id) // Retrieve a Match by its ID, including related entities such as Court, League, Home and Away teams, PlayerStats and Photos.
        {                                              // Returns null if not found.

            return await _context.Matches
                        .AsNoTracking()
                        .Include(m => m.Court)
                        .Include(m => m.League)
                        .Include(m => m.HomeTeamSeasonLeague)
                                .ThenInclude(tsl => tsl.Team)
                        .Include(m => m.AwayTeamSeasonLeague)
                                .ThenInclude(tsl => tsl.Team)
                        .Include(m => m.PlayerStats)
                        .Include(m => m.Photos)
                        .FirstOrDefaultAsync(m => m.Id == id); 

        }

        public async Task<SimpleResult> CreateAsync(Match match)
        {
            
            if (match.HomeTeamSeasonLeagueId == match.AwayTeamSeasonLeagueId) // check if Home and Away teams are the same
            {

                return new SimpleResult { Success = false, Message = "Home and Away teams cannot be the same" };

            }

            
            if (match.StartTime >= match.EndTime) // check if StartTime is before EndTime
            {

                return new SimpleResult { Success = false, Message = "Start Time must be before End Time" };

            }

            
            var courtExists = await _context.Courts.AnyAsync(c => c.Id == match.CourtId);
            var leagueExists = await _context.Leagues.AnyAsync(l => l.Id == match.LeagueId);
            var homeTeamExists = await _context.TeamSeasonLeagues.AnyAsync(t => t.Id == match.HomeTeamSeasonLeagueId);
            var awayTeamExists = await _context.TeamSeasonLeagues.AnyAsync(t => t.Id == match.AwayTeamSeasonLeagueId);

            if (!courtExists || !leagueExists || !homeTeamExists || !awayTeamExists)  // check if Court, League, HomeTeam and AwayTeam exist
            {

                return new SimpleResult { Success = false, Message = "Court, League, or Teams not found" };

            }

            
            var courtConflict = await _context.Matches.AnyAsync(m => m.CourtId == match.CourtId && m.MatchDate == match.MatchDate && m.StartTime == match.StartTime );
            // check if there's a Match at the same Court, Date and Time

            if (courtConflict)
            {

                return new SimpleResult { Success = false, Message = "Court is already booked at this time" };


            }


            



            var teamConflict = await _context.Matches.AnyAsync(m => m.MatchDate == match.MatchDate && m.StartTime == match.StartTime &&
                                                                    (m.HomeTeamSeasonLeagueId == match.HomeTeamSeasonLeagueId ||
                                                                         m.AwayTeamSeasonLeagueId == match.HomeTeamSeasonLeagueId ||
                                                                         m.HomeTeamSeasonLeagueId == match.AwayTeamSeasonLeagueId ||
                                                                         m.AwayTeamSeasonLeagueId == match.AwayTeamSeasonLeagueId)
                                                               );
            // check if either Home or Away team has another Match at the same Date and Time


            if (teamConflict)
            {

                return new SimpleResult { Success = false, Message = "One of the teams already has a match at this time" };

            }


            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };

        }

        public async Task<SimpleResult> UpdateAsync(Match match)
        {
            var existing = await _context.Matches.FindAsync(match.Id);

            if (existing == null)
            {

                return new SimpleResult { Success = false, Message = "Match not found" };

            }
                


            

            
            if (match.HomeTeamSeasonLeagueId == match.AwayTeamSeasonLeagueId) // check if Home and Away teams are the same
            {

                return new SimpleResult { Success = false, Message = "Home and Away teams cannot be the same" };

            }

            
            if (match.StartTime >= match.EndTime)  // check if StartTime is before EndTime
            {

                return new SimpleResult { Success = false, Message = "StartTime must be before EndTime" };

            }


           
            var courtConflict = await _context.Matches.AnyAsync(m => m.Id != match.Id && m.CourtId == match.CourtId && m.MatchDate == match.MatchDate && m.StartTime == match.StartTime);
            // check if there's another Match at the same Court, Date and Time (excluding the current Match)

            if (courtConflict)
            {

                return new SimpleResult { Success = false, Message = "Court is already booked at this time" };

            }

           
            var teamConflict = await _context.Matches.AnyAsync(m => m.Id != match.Id && m.MatchDate == match.MatchDate && m.StartTime == match.StartTime &&
                                                                   (m.HomeTeamSeasonLeagueId == match.HomeTeamSeasonLeagueId ||
                                                                    m.AwayTeamSeasonLeagueId == match.HomeTeamSeasonLeagueId ||
                                                                    m.HomeTeamSeasonLeagueId == match.AwayTeamSeasonLeagueId ||
                                                                    m.AwayTeamSeasonLeagueId == match.AwayTeamSeasonLeagueId)
                                                               );
            // check if either Home or Away team has another Match at the same Date and Time (excluding the current Match)

            if (teamConflict)
            {

                return new SimpleResult { Success = false, Message = "One of the teams already has a match at this time" };

            }


            // Update only essential fields

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

            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var match = await _context.Matches.FindAsync(id); // check if Match exists

            if (match == null)
            {

                return new SimpleResult { Success = false, Message = "Match not found" };

            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

       

    }
}