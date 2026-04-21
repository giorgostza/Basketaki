using Basketaki.Models;
using Basketaki.Services;
using Basketaki.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Basketaki.Controllers
{
    public class MatchController : Controller
    {
        private readonly IMatchService _matchService;
        private readonly ICourtService _courtService;
        private readonly ILeagueService _leagueService;
        private readonly ITeamSeasonLeagueService _teamSeasonLeagueService;

        public MatchController(IMatchService matchService, ICourtService courtService,
                               ILeagueService leagueService, ITeamSeasonLeagueService teamSeasonLeagueService)
        {

            _matchService = matchService;
            _courtService = courtService;
            _leagueService = leagueService;
            _teamSeasonLeagueService = teamSeasonLeagueService;

        }


        public async Task<IActionResult> Index()
        {
            var matches = await _matchService.GetAllAsync();

            return View(matches);
        }


        public async Task<IActionResult> Details(int id)
        {
            var match = await _matchService.GetByIdAsync(id);

            if (match == null)
            {

                return NotFound();

            }

            return View(match);

        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new MatchFormViewModel
            {
                MatchDate = DateOnly.FromDateTime(DateTime.Today),
                Courts = await GetCourtSelectListAsync(),
                Leagues = await GetLeagueSelectListAsync(),
                HomeTeams = await GetTeamSeasonLeagueSelectListAsync(),
                AwayTeams = await GetTeamSeasonLeagueSelectListAsync()

            };


            return View(viewModel);

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MatchFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {

                await LoadDropdownsAsync(viewModel);

                return View(viewModel);
            }



            var match = new Match
            {
                MatchDate = viewModel.MatchDate,
                StartTime = viewModel.StartTime,
                EndTime = viewModel.EndTime,
                CourtId = viewModel.CourtId,
                LeagueId = viewModel.LeagueId,
                HomeTeamSeasonLeagueId = viewModel.HomeTeamSeasonLeagueId,
                AwayTeamSeasonLeagueId = viewModel.AwayTeamSeasonLeagueId,
                HomeScore = viewModel.HomeScore,
                AwayScore = viewModel.AwayScore,
                IsPlayed = viewModel.IsPlayed

            };

            var result = await _matchService.CreateAsync(match);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to create match.");

                await LoadDropdownsAsync(viewModel);

                return View(viewModel);
            }



            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var match = await _matchService.GetByIdAsync(id);

            if (match == null)
            {

                return NotFound();

            }

            var viewModel = new MatchFormViewModel
            {
                Id = match.Id,
                MatchDate = match.MatchDate,
                StartTime = match.StartTime,
                EndTime = match.EndTime,
                CourtId = match.CourtId,
                LeagueId = match.LeagueId,
                HomeTeamSeasonLeagueId = match.HomeTeamSeasonLeagueId,
                AwayTeamSeasonLeagueId = match.AwayTeamSeasonLeagueId,
                HomeScore = match.HomeScore,
                AwayScore = match.AwayScore,
                IsPlayed = match.IsPlayed

            };


            await LoadDropdownsAsync(viewModel);

            return View(viewModel);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MatchFormViewModel viewModel)
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



            var match = new Match
            {
                Id = viewModel.Id,
                MatchDate = viewModel.MatchDate,
                StartTime = viewModel.StartTime,
                EndTime = viewModel.EndTime,
                CourtId = viewModel.CourtId,
                LeagueId = viewModel.LeagueId,
                HomeTeamSeasonLeagueId = viewModel.HomeTeamSeasonLeagueId,
                AwayTeamSeasonLeagueId = viewModel.AwayTeamSeasonLeagueId,
                HomeScore = viewModel.HomeScore,
                AwayScore = viewModel.AwayScore,
                IsPlayed = viewModel.IsPlayed

            };




            var result = await _matchService.UpdateAsync(match);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to update match.");

                await LoadDropdownsAsync(viewModel);

                return View(viewModel);
            }



            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var match = await _matchService.GetByIdAsync(id);

            if (match == null)
            {

                return NotFound();

            }



            return View(match);
        }




        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _matchService.DeleteAsync(id);

            if (!result.Success)
            {

                TempData["ErrorMessage"] = result.Message;

                return RedirectToAction(nameof(Delete), new { id });
            }



            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }





        private async Task LoadDropdownsAsync(MatchFormViewModel viewModel)
        {
            viewModel.Courts = await GetCourtSelectListAsync(viewModel.CourtId);
            viewModel.Leagues = await GetLeagueSelectListAsync(viewModel.LeagueId);
            viewModel.HomeTeams = await GetTeamSeasonLeagueSelectListAsync(viewModel.HomeTeamSeasonLeagueId);
            viewModel.AwayTeams = await GetTeamSeasonLeagueSelectListAsync(viewModel.AwayTeamSeasonLeagueId);
        }

        private async Task<List<SelectListItem>> GetCourtSelectListAsync(int? selectedCourtId = null)
        {
            var courts = await _courtService.GetAllAsync();

            return courts.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = $"{c.Name} ({c.Location})",
                Selected = selectedCourtId.HasValue && c.Id == selectedCourtId.Value

            }).ToList();

        }



        private async Task<List<SelectListItem>> GetLeagueSelectListAsync(int? selectedLeagueId = null)
        {
            var leagues = await _leagueService.GetAllAsync();

            return leagues.Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = $"{l.Name} ({l.Season.Name})",
                Selected = selectedLeagueId.HasValue && l.Id == selectedLeagueId.Value

            }).ToList();

        }




        private async Task<List<SelectListItem>> GetTeamSeasonLeagueSelectListAsync(int? selectedTeamSeasonLeagueId = null)
        {
            var teamSeasonLeagues = await _teamSeasonLeagueService.GetAllAsync();

            return teamSeasonLeagues.Select(tsl => new SelectListItem
            {
                Value = tsl.Id.ToString(),
                Text = $"{tsl.Team.Name} - {tsl.League.Name} ({tsl.League.Season.Name})",
                Selected = selectedTeamSeasonLeagueId.HasValue && tsl.Id == selectedTeamSeasonLeagueId.Value

            }).ToList();

        }



    }
}