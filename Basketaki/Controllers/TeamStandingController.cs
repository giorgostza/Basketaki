using Basketaki.Models;
using Basketaki.Services;
using Basketaki.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Basketaki.Controllers
{
    public class TeamStandingController : Controller
    {
        private readonly ITeamStandingService _teamStandingService;
        private readonly ITeamSeasonLeagueService _teamSeasonLeagueService;

        public TeamStandingController(ITeamStandingService teamStandingService, ITeamSeasonLeagueService teamSeasonLeagueService)
        {
            _teamStandingService = teamStandingService;
            _teamSeasonLeagueService = teamSeasonLeagueService;
        }

        public async Task<IActionResult> Index()
        {
            var standings = await _teamStandingService.GetAllAsync();

            return View(standings);
        }


        public async Task<IActionResult> Details(int id)
        {
            var standing = await _teamStandingService.GetByIdAsync(id);

            if (standing == null)
            {

                return NotFound();

            }

            return View(standing);

        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new TeamStandingFormViewModel();

            await LoadTeamSeasonLeaguesAsync(viewModel);

            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamStandingFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LoadTeamSeasonLeaguesAsync(viewModel);

                return View(viewModel);
            }


            var standing = new TeamStanding
            {
                TeamSeasonLeagueId = viewModel.TeamSeasonLeagueId,
                Played = viewModel.Played,
                Wins = viewModel.Wins,
                Losses = viewModel.Losses,
                PointsFor = viewModel.PointsFor,
                PointsAgainst = viewModel.PointsAgainst,
                LeaguePoints = viewModel.LeaguePoints,
                NoShow = viewModel.NoShow,
                CurrentStreak = viewModel.CurrentStreak
            };

            var result = await _teamStandingService.CreateAsync(standing);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to create standing.");

                await LoadTeamSeasonLeaguesAsync(viewModel);

                return View(viewModel);
            }


            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var standing = await _teamStandingService.GetByIdAsync(id);

            if (standing == null)
            {

                return NotFound();

            }


            var viewModel = new TeamStandingFormViewModel
            {
                Id = standing.Id,
                TeamSeasonLeagueId = standing.TeamSeasonLeagueId,
                Played = standing.Played,
                Wins = standing.Wins,
                Losses = standing.Losses,
                PointsFor = standing.PointsFor,
                PointsAgainst = standing.PointsAgainst,
                LeaguePoints = standing.LeaguePoints,
                NoShow = standing.NoShow,
                CurrentStreak = standing.CurrentStreak
            };

            await LoadTeamSeasonLeaguesAsync(viewModel);

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TeamStandingFormViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await LoadTeamSeasonLeaguesAsync(viewModel);

                return View(viewModel);
            }



            var standing = new TeamStanding
            {
                Id = viewModel.Id,
                TeamSeasonLeagueId = viewModel.TeamSeasonLeagueId,
                Played = viewModel.Played,
                Wins = viewModel.Wins,
                Losses = viewModel.Losses,
                PointsFor = viewModel.PointsFor,
                PointsAgainst = viewModel.PointsAgainst,
                LeaguePoints = viewModel.LeaguePoints,
                NoShow = viewModel.NoShow,
                CurrentStreak = viewModel.CurrentStreak
            };

            var result = await _teamStandingService.UpdateAsync(standing);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to update standing.");

                await LoadTeamSeasonLeaguesAsync(viewModel);

                return View(viewModel);
            }



            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var standing = await _teamStandingService.GetByIdAsync(id);

            if (standing == null)
            {

                return NotFound();

            }

            return View(standing);

        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _teamStandingService.DeleteAsync(id);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;

                return RedirectToAction(nameof(Delete), new { id });
            }



            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));

        }




        private async Task LoadTeamSeasonLeaguesAsync(TeamStandingFormViewModel viewModel)
        {
            var teamSeasonLeagues = await _teamSeasonLeagueService.GetAllAsync();

            viewModel.TeamSeasonLeagues = teamSeasonLeagues.Select(tsl => new SelectListItem
            {
                Value = tsl.Id.ToString(),
                Text = $"{tsl.Team.Name} - {tsl.League.Name} ({tsl.League.Season.Name})",
                Selected = tsl.Id == viewModel.TeamSeasonLeagueId

            }).ToList();

        }


    }
}