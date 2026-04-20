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

            return await _context.Teams.AsNoTracking().Include(t => t.Coach).OrderBy(t => t.Name).ThenBy(t => t.City).ToListAsync();

        }


        public async Task<Team?> GetByIdAsync(int id)
        {
            return await _context.Teams.AsNoTracking().Include(t => t.Coach)
                                                      .Include(t => t.TeamSeasonLeagues)
                                                              .ThenInclude(tsl => tsl.League)
                                                              .ThenInclude(l => l.Season)
                                                      .Include(t => t.PlayerSeasonTeams)
                                                              .ThenInclude(pst => pst.Player)
                                                      .FirstOrDefaultAsync(t => t.Id == id);

        }



        public async Task<SimpleResult> CreateAsync(Team team)
        {
            if (team == null)
            {

                return SimpleResult.Fail("Team data is required.");

            }


            if (string.IsNullOrWhiteSpace(team.Name) || string.IsNullOrWhiteSpace(team.City))
            {

                return SimpleResult.Fail("Name and City are required.");

            }


            if (team.CoachId.HasValue)
            {

                var coachExists = await _context.Coaches.AnyAsync(c => c.Id == team.CoachId.Value);

                if (!coachExists)
                {

                    return SimpleResult.Fail("Coach not found.");

                }

            }


            var trimmedName = team.Name.Trim();
            var trimmedCity = team.City.Trim();
            var trimmedPhotoUrl = string.IsNullOrWhiteSpace(team.PhotoUrl) ? null : team.PhotoUrl.Trim();

            var exists = await _context.Teams.AnyAsync(t => t.Name == trimmedName && t.City == trimmedCity);

            if (exists)
            {

                return SimpleResult.Fail("Team with the same name already exists in the same city.");

            }



            team.Name = trimmedName;
            team.City = trimmedCity;
            team.PhotoUrl = trimmedPhotoUrl;


            _context.Teams.Add(team);



            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Team created successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to create team. A team with the same name and city may already exist.");

            }
        }



        public async Task<SimpleResult> UpdateAsync(Team team)
        {
            if (team == null)
            {

                return SimpleResult.Fail("Team data is required.");

            }


            var existing = await _context.Teams.FindAsync(team.Id);

            if (existing == null)
            {

                return SimpleResult.Fail("Team not found.");

            }

            if (string.IsNullOrWhiteSpace(team.Name) || string.IsNullOrWhiteSpace(team.City))
            {

                return SimpleResult.Fail("Name and City are required.");

            }

            if (team.CoachId.HasValue)
            {

                var coachExists = await _context.Coaches.AnyAsync(c => c.Id == team.CoachId.Value);

                if (!coachExists)
                {

                    return SimpleResult.Fail("Coach not found.");

                }

            }


            var trimmedName = team.Name.Trim();
            var trimmedCity = team.City.Trim();
            var trimmedPhotoUrl = string.IsNullOrWhiteSpace(team.PhotoUrl) ? null : team.PhotoUrl.Trim();

            var duplicate = await _context.Teams.AnyAsync(t => t.Id != team.Id && t.Name == trimmedName && t.City == trimmedCity);

            if (duplicate)
            {

                return SimpleResult.Fail("Team with the same name already exists in the same city.");

            }



            existing.Name = trimmedName;
            existing.City = trimmedCity;
            existing.PhotoUrl = trimmedPhotoUrl;
            existing.CoachId = team.CoachId;



            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Team updated successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to update team. A team with the same name and city may already exist.");

            }

        }



        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);

            if (team == null)
            {

                return SimpleResult.Fail("Team not found.");

            }


            var hasLeagues = await _context.TeamSeasonLeagues.AnyAsync(tsl => tsl.TeamId == id);

            if (hasLeagues)
            {

                return SimpleResult.Fail("Cannot delete team assigned to leagues.");

            }


            var hasPlayers = await _context.PlayerSeasonTeams.AnyAsync(pst => pst.TeamId == id);

            if (hasPlayers)
            {

                return SimpleResult.Fail("Cannot delete team with players.");

            }



            try
            {

                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();

                return SimpleResult.Ok("Team deleted successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to delete team because it is used by other data.");

            }


        }

    }
}