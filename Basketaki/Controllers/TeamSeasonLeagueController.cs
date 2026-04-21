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
        private readonly ITeamService _teamService;
        private readonly ILeagueService _leagueService;

        public TeamSeasonLeagueController(ITeamService teamService, ILeagueService leagueService, ITeamSeasonLeagueService teamSeasonLeagueService)
        {
            _teamSeasonLeagueService = teamSeasonLeagueService;
            _teamService = teamService;
            _leagueService = leagueService;
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
            viewModel.Teams = await GetTeamSelectListAsync(viewModel.TeamId);
            viewModel.Leagues = await GetLeagueSelectListAsync(viewModel.LeagueId);
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

        private async Task<List<SelectListItem>> GetLeagueSelectListAsync(int? selectedLeagueId = null)
        {
            var leagues = await _leagueService.GetAllAsync();

            return leagues.Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = $"{l.Name} - {l.City} ({l.Season.Name})",
                Selected = selectedLeagueId.HasValue && l.Id == selectedLeagueId.Value

            }).ToList();

        }


    }
}