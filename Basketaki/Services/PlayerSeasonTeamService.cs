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


        public async Task<List<PlayerSeasonTeam>> GetAllAsync()
        {
            return await _context.PlayerSeasonTeams.AsNoTracking().Include(pst => pst.Player)
                                                                  .Include(pst => pst.Team)
                                                                  .Include(pst => pst.Season)
                                                                  .OrderByDescending(pst => pst.Season.StartDate)
                                                                  .ThenBy(pst => pst.Team.Name)
                                                                  .ThenBy(pst => pst.Player.LastName)
                                                                  .ThenBy(pst => pst.Player.FirstName)
                                                                  .ToListAsync();

        }


        public async Task<PlayerSeasonTeam?> GetByIdAsync(int id)
        {
            return await _context.PlayerSeasonTeams.AsNoTracking().Include(pst => pst.Player)
                                                                  .Include(pst => pst.Team)
                                                                  .Include(pst => pst.Season)
                                                                  .FirstOrDefaultAsync(pst => pst.Id == id);

        }


        public async Task<SimpleResult> CreateAsync(PlayerSeasonTeam model)
        {
            if (model == null)
            {

                return SimpleResult.Fail("Player assignment data is required.");

            }


            var playerExists = await _context.Players.AsNoTracking().AnyAsync(p => p.Id == model.PlayerId);
            var teamExists = await _context.Teams.AsNoTracking().AnyAsync(t => t.Id == model.TeamId);
            var season = await _context.Seasons.AsNoTracking().FirstOrDefaultAsync(s => s.Id == model.SeasonId);

            if (!playerExists || !teamExists || season == null)
            {

                return SimpleResult.Fail("Invalid player, team or season.");

            }


            if (model.LeaveDate.HasValue && model.LeaveDate.Value < model.JoinDate)
            {

                return SimpleResult.Fail("Leave date cannot be earlier than join date.");

            }


            if (model.JoinDate < season.StartDate || model.JoinDate > season.EndDate)
            {

                return SimpleResult.Fail("Join date must be within the selected season.");

            }

            if (model.LeaveDate.HasValue && (model.LeaveDate.Value < season.StartDate || model.LeaveDate.Value > season.EndDate))
            {

                return SimpleResult.Fail("Leave date must be within the selected season.");

            }


            var overlappingTeamAssignment = await _context.PlayerSeasonTeams.AsNoTracking()
                                                                            .AnyAsync(pst =>
                                                                                pst.PlayerId == model.PlayerId &&
                                                                                pst.SeasonId == model.SeasonId &&
                                                                                DatesOverlap(
                                                                                    pst.JoinDate,
                                                                                    pst.LeaveDate,
                                                                                    model.JoinDate,
                                                                                    model.LeaveDate));

            if (overlappingTeamAssignment)
            {

                return SimpleResult.Fail("Player already has another active team assignment in the same period.");

            } 


            var jerseyConflict = await _context.PlayerSeasonTeams.AsNoTracking()
                                                                 .AnyAsync(pst =>
                                                                     pst.TeamId == model.TeamId &&
                                                                     pst.SeasonId == model.SeasonId &&
                                                                     pst.JerseyNumber == model.JerseyNumber &&
                                                                     DatesOverlap(
                                                                         pst.JoinDate,
                                                                         pst.LeaveDate,
                                                                         model.JoinDate,
                                                                         model.LeaveDate));

            if (jerseyConflict)
            {

                return SimpleResult.Fail("Jersey number is already taken for this team during the same period.");

            }

            _context.PlayerSeasonTeams.Add(model);

            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Player assignment created successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to create player assignment.");

            }
        }


        public async Task<SimpleResult> UpdateAsync(PlayerSeasonTeam model)
        {
            if (model == null)
            {

                return SimpleResult.Fail("Player assignment data is required.");

            }


            var existing = await _context.PlayerSeasonTeams.FindAsync(model.Id);

            if (existing == null)
            {

                return SimpleResult.Fail("Record not found.");

            }


            var playerExists = await _context.Players.AsNoTracking().AnyAsync(p => p.Id == model.PlayerId);
            var teamExists = await _context.Teams.AsNoTracking().AnyAsync(t => t.Id == model.TeamId);
            var season = await _context.Seasons.AsNoTracking().FirstOrDefaultAsync(s => s.Id == model.SeasonId);

            if (!playerExists || !teamExists || season == null)
            {

                return SimpleResult.Fail("Invalid player, team or season.");

            }


            if (model.LeaveDate.HasValue && model.LeaveDate.Value < model.JoinDate)
            {

                return SimpleResult.Fail("Leave date cannot be earlier than join date.");

            }


            if (model.JoinDate < season.StartDate || model.JoinDate > season.EndDate)
            {

                return SimpleResult.Fail("Join date must be within the selected season.");

            }


            if (model.LeaveDate.HasValue && (model.LeaveDate.Value < season.StartDate || model.LeaveDate.Value > season.EndDate))
            {

                return SimpleResult.Fail("Leave date must be within the selected season.");

            }



            var overlappingTeamAssignment = await _context.PlayerSeasonTeams.AsNoTracking()
                                                                            .AnyAsync(pst =>
                                                                                pst.Id != model.Id &&
                                                                                pst.PlayerId == model.PlayerId &&
                                                                                pst.SeasonId == model.SeasonId &&
                                                                                DatesOverlap(
                                                                                    pst.JoinDate,
                                                                                    pst.LeaveDate,
                                                                                    model.JoinDate,
                                                                                    model.LeaveDate));

            if (overlappingTeamAssignment)
            {

                return SimpleResult.Fail("Player already has another active team assignment in the same period.");

            }



            var jerseyConflict = await _context.PlayerSeasonTeams.AsNoTracking()
                                                                 .AnyAsync(pst =>
                                                                     pst.Id != model.Id &&
                                                                     pst.TeamId == model.TeamId &&
                                                                     pst.SeasonId == model.SeasonId &&
                                                                     pst.JerseyNumber == model.JerseyNumber &&
                                                                     DatesOverlap(
                                                                        pst.JoinDate,
                                                                        pst.LeaveDate,
                                                                        model.JoinDate,
                                                                        model.LeaveDate));

            if (jerseyConflict)
            {

                return SimpleResult.Fail("Jersey number is already taken for this team during the same period.");

            }



            existing.PlayerId = model.PlayerId;
            existing.TeamId = model.TeamId;
            existing.SeasonId = model.SeasonId;
            existing.JerseyNumber = model.JerseyNumber;
            existing.JoinDate = model.JoinDate;
            existing.LeaveDate = model.LeaveDate;

            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Player assignment updated successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to update player assignment.");

            }

        }



        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var entity = await _context.PlayerSeasonTeams.FindAsync(id);

            if (entity == null)
            {

                return SimpleResult.Fail("Record not found.");

            }


            var hasStats = await _context.PlayerStats.AsNoTracking().AnyAsync(ps => ps.PlayerSeasonTeamId == id);

            if (hasStats)
            {

                return SimpleResult.Fail("Cannot delete record with stats.");

            }


            try
            {

                _context.PlayerSeasonTeams.Remove(entity);
                await _context.SaveChangesAsync();

                return SimpleResult.Ok("Player assignment deleted successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to delete player assignment because it is used by other data.");

            }

        }




        private static bool DatesOverlap(DateOnly start1, DateOnly? end1, DateOnly start2, DateOnly? end2)
        {

            var actualEnd1 = end1 ?? DateOnly.MaxValue;
            var actualEnd2 = end2 ?? DateOnly.MaxValue;

            return start1 <= actualEnd2 && start2 <= actualEnd1;

        }


    }
}