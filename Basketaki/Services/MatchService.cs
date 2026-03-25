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

        public async Task<bool> CreateAsync(Match match)
        {
            
            if (match.HomeTeamSeasonLeagueId == match.AwayTeamSeasonLeagueId) // check if Home and Away teams are the same
            {

                return false;

            }

            
            if (match.StartTime >= match.EndTime) // check if StartTime is before EndTime
            {

                return false;

            }

            
            var courtExists = await _context.Courts.AnyAsync(c => c.Id == match.CourtId);
            var leagueExists = await _context.Leagues.AnyAsync(l => l.Id == match.LeagueId);
            var homeTeamExists = await _context.TeamSeasonLeagues.AnyAsync(t => t.Id == match.HomeTeamSeasonLeagueId);
            var awayTeamExists = await _context.TeamSeasonLeagues.AnyAsync(t => t.Id == match.AwayTeamSeasonLeagueId);

            if (!courtExists || !leagueExists || !homeTeamExists || !awayTeamExists)  // check if Court, League, HomeTeam and AwayTeam exist
            {

                return false;

            }

            
            var courtConflict = await _context.Matches.AnyAsync(m => m.CourtId == match.CourtId && m.MatchDate == match.MatchDate && m.StartTime == match.StartTime );
            // check if there's a Match at the same Court, Date and Time

            if (courtConflict)
            {

                return false;

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

                return false;

            }

            
            _context.Matches.Add(match);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Match match)
        {
            
            if (!await ExistsAsync(match.Id)) // check if Match exists
            {

                return false;

            }

            
            if (match.HomeTeamSeasonLeagueId == match.AwayTeamSeasonLeagueId) // check if Home and Away teams are the same
            {

                return false;

            }

            
            if (match.StartTime >= match.EndTime)  // check if StartTime is before EndTime
            {

                return false;

            }


           
            var courtConflict = await _context.Matches.AnyAsync(m => m.Id != match.Id && m.CourtId == match.CourtId && m.MatchDate == match.MatchDate && m.StartTime == match.StartTime);
            // check if there's another Match at the same Court, Date and Time (excluding the current Match)

            if (courtConflict)
            {

                return false;

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

                return false;

            }

            _context.Matches.Update(match);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var match = await _context.Matches.FindAsync(id); // check if Match exists

            if (match == null)
            {

                return false;

            }

            _context.Matches.Remove(match);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {

            return await _context.Matches.AnyAsync(m => m.Id == id);  // Check if a Match with the specified ID exists in the database

        }

    }
}