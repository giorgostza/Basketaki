using Basketaki.Models;
using Basketaki.Services;
using Basketaki.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Basketaki.Controllers
{
    public class PlayerSeasonTeamController : Controller
    {
        private readonly IPlayerSeasonTeamService _playerSeasonTeamService;
        private readonly IPlayerService _playerService;
        private readonly ITeamService _teamService;
        private readonly ISeasonService _seasonService;

        public PlayerSeasonTeamController(IPlayerSeasonTeamService playerSeasonTeamService, IPlayerService playerService,
                                          ITeamService teamService, ISeasonService seasonService)
        {
            _playerSeasonTeamService = playerSeasonTeamService;
            _playerService = playerService;
            _teamService = teamService;
            _seasonService = seasonService;
        }

        public async Task<IActionResult> Index()
        {
            var assignments = await _playerSeasonTeamService.GetAllAsync();

            return View(assignments);
        }



        public async Task<IActionResult> Details(int id)
        {
            var assignment = await _playerSeasonTeamService.GetByIdAsync(id);

            if (assignment == null)
            {

                return NotFound();

            }



            return View(assignment);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new PlayerSeasonTeamFormViewModel
            {
                JoinDate = DateOnly.FromDateTime(DateTime.Today)
            };


            await LoadDropdownsAsync(viewModel);

            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerSeasonTeamFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(viewModel);

                return View(viewModel);
            }



            var model = new PlayerSeasonTeam
            {
                PlayerId = viewModel.PlayerId,
                TeamId = viewModel.TeamId,
                SeasonId = viewModel.SeasonId,
                JerseyNumber = viewModel.JerseyNumber,
                JoinDate = viewModel.JoinDate,
                LeaveDate = viewModel.LeaveDate
            };

            var result = await _playerSeasonTeamService.CreateAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to create player assignment.");

                await LoadDropdownsAsync(viewModel);

                return View(viewModel);
            }



            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));

        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var assignment = await _playerSeasonTeamService.GetByIdAsync(id);

            if (assignment == null)
            {

                return NotFound();

            }



            var viewModel = new PlayerSeasonTeamFormViewModel
            {
                Id = assignment.Id,
                PlayerId = assignment.PlayerId,
                TeamId = assignment.TeamId,
                SeasonId = assignment.SeasonId,
                JerseyNumber = assignment.JerseyNumber,
                JoinDate = assignment.JoinDate,
                LeaveDate = assignment.LeaveDate
            };


            await LoadDropdownsAsync(viewModel);

            return View(viewModel);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlayerSeasonTeamFormViewModel viewModel)
        {
            if (id != viewModel.Id)
            {

                return BadRequest();

            }

            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(viewModel);

                return View(viewModel);
            }


            var model = new PlayerSeasonTeam
            {
                Id = viewModel.Id,
                PlayerId = viewModel.PlayerId,
                TeamId = viewModel.TeamId,
                SeasonId = viewModel.SeasonId,
                JerseyNumber = viewModel.JerseyNumber,
                JoinDate = viewModel.JoinDate,
                LeaveDate = viewModel.LeaveDate
            };

            var result = await _playerSeasonTeamService.UpdateAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to update player assignment.");

                await LoadDropdownsAsync(viewModel);

                return View(viewModel);
            }


            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var assignment = await _playerSeasonTeamService.GetByIdAsync(id);

            if (assignment == null)
            {

                return NotFound();

            }



            return View(assignment);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _playerSeasonTeamService.DeleteAsync(id);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;

                return RedirectToAction(nameof(Delete), new { id });
            }



            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }




        private async Task LoadDropdownsAsync(PlayerSeasonTeamFormViewModel viewModel)
        {
            viewModel.Players = await GetPlayerSelectListAsync(viewModel.PlayerId);
            viewModel.Teams = await GetTeamSelectListAsync(viewModel.TeamId);
            viewModel.Seasons = await GetSeasonSelectListAsync(viewModel.SeasonId);
        }

        private async Task<List<SelectListItem>> GetPlayerSelectListAsync(int? selectedPlayerId = null)
        {
            var players = await _playerService.GetAllAsync();

            return players.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.FullName,
                Selected = selectedPlayerId.HasValue && p.Id == selectedPlayerId.Value

            }).ToList();

        }


        private async Task<List<SelectListItem>> GetTeamSelectListAsync(int? selectedTeamId = null)
        {
            var teams = await _teamService.GetAllAsync();

            return teams.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = $"{t.Name} ({t.City})",
                Selected = selectedTeamId.HasValue && t.Id == selectedTeamId.Value

            }).ToList();

        }



        private async Task<List<SelectListItem>> GetSeasonSelectListAsync(int? selectedSeasonId = null)
        {
            var seasons = await _seasonService.GetAllAsync();

            return seasons.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name,
                Selected = selectedSeasonId.HasValue && s.Id == selectedSeasonId.Value

            }).ToList();

        }


    }
}