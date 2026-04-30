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
        private readonly ILookupService _lookupService;

        public PlayerSeasonTeamController(IPlayerSeasonTeamService playerSeasonTeamService, ILookupService lookupService)
        {
            _playerSeasonTeamService = playerSeasonTeamService;
            _lookupService = lookupService;
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

            viewModel.Players = await _lookupService.GetPlayerSelectListAsync(viewModel.PlayerId);
            viewModel.Teams = await _lookupService.GetTeamSelectListAsync(viewModel.TeamId);
            viewModel.Seasons = await _lookupService.GetSeasonSelectListAsync(viewModel.SeasonId);

        }

        

    }
}