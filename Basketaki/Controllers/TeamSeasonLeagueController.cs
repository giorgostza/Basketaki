using Basketaki.Models;
using Basketaki.Services;
using Basketaki.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Basketaki.Controllers
{
    public class TeamSeasonLeagueController : Controller
    {
        private readonly ITeamSeasonLeagueService _teamSeasonLeagueService;
        private readonly ILookupService _lookupService;

        public TeamSeasonLeagueController(ITeamSeasonLeagueService teamSeasonLeagueService, ILookupService lookupService)
        {
            _teamSeasonLeagueService = teamSeasonLeagueService;
            _lookupService = lookupService;
        }

        public async Task<IActionResult> Index()
        {
            var assignments = await _teamSeasonLeagueService.GetAllAsync();

            return View(assignments);
        }


        public async Task<IActionResult> Details(int id)
        {
            var assignment = await _teamSeasonLeagueService.GetByIdAsync(id);

            if (assignment == null)
            {

                return NotFound();

            }

            return View(assignment);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new TeamSeasonLeagueFormViewModel();

            await LoadDropdownsAsync(viewModel);

            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamSeasonLeagueFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(viewModel);

                return View(viewModel);
            }


            var model = new TeamSeasonLeague
            {
                TeamId = viewModel.TeamId,
                LeagueId = viewModel.LeagueId
            };

            var result = await _teamSeasonLeagueService.CreateAsync(model);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to create team league assignment.");

                await LoadDropdownsAsync(viewModel);

                return View(viewModel);
            }


            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var assignment = await _teamSeasonLeagueService.GetByIdAsync(id);

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
            var result = await _teamSeasonLeagueService.DeleteAsync(id);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;

                return RedirectToAction(nameof(Delete), new { id });
            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }




        private async Task LoadDropdownsAsync(TeamSeasonLeagueFormViewModel viewModel)
        {
            viewModel.Teams = await _lookupService.GetTeamSelectListAsync(viewModel.TeamId);
            viewModel.Leagues = await _lookupService.GetLeagueSelectListAsync(viewModel.LeagueId);
        }

       
    }
}