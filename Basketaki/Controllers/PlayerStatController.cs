using Basketaki.Models;
using Basketaki.Services;
using Basketaki.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Basketaki.Controllers
{
    public class PlayerStatController : Controller
    {
        private readonly IPlayerStatService _playerStatService;
        private readonly IMatchService _matchService;
        private readonly ILookupService _lookupService;

        public PlayerStatController(IPlayerStatService playerStatService, IMatchService matchService, ILookupService lookupService)
        {
            _playerStatService = playerStatService;
            _matchService = matchService;
            _lookupService = lookupService;
        }

        public async Task<IActionResult> Index(int? matchId)
        {
            if (matchId.HasValue)
            {
                var stats = await _playerStatService.GetByMatchIdAsync(matchId.Value);
                var match = await _matchService.GetByIdAsync(matchId.Value);

                ViewBag.Match = match;
                ViewBag.MatchId = matchId.Value;

                return View(stats);
            }

            var allStats = await _playerStatService.GetAllAsync();

            return View(allStats);
        }


        public async Task<IActionResult> Details(int id)
        {
            var stat = await _playerStatService.GetByIdAsync(id);

            if (stat == null)
            {

                return NotFound();

            }

            return View(stat);

        }



        [HttpGet]
        public async Task<IActionResult> Create(int? matchId)
        {
            var viewModel = new PlayerStatFormViewModel
            {
                MatchId = matchId ?? 0
            };

            await LoadDropdownsAsync(viewModel);

            return View(viewModel);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerStatFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync(viewModel);

                return View(viewModel);
            }


            var playerStat = new PlayerStat
            {
                PlayerSeasonTeamId = viewModel.PlayerSeasonTeamId,
                MatchId = viewModel.MatchId,
                Points = viewModel.Points,
                OffensiveRebounds = viewModel.OffensiveRebounds,
                DefensiveRebounds = viewModel.DefensiveRebounds,
                Assists = viewModel.Assists,
                Steals = viewModel.Steals,
                Blocks = viewModel.Blocks,
                Fouls = viewModel.Fouls,
                MinutesPlayed = viewModel.MinutesPlayed,
                FreeThrowsMade = viewModel.FreeThrowsMade,
                FreeThrowsAttempted = viewModel.FreeThrowsAttempted,
                TwoPointsMade = viewModel.TwoPointsMade,
                TwoPointsAttempted = viewModel.TwoPointsAttempted,
                ThreePointsMade = viewModel.ThreePointsMade,
                ThreePointsAttempted = viewModel.ThreePointsAttempted,
                IsMVP = viewModel.IsMVP,
                SuspensionGames = viewModel.SuspensionGames
            };

            var result = await _playerStatService.CreateAsync(playerStat);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to create player stat.");

                await LoadDropdownsAsync(viewModel);

                return View(viewModel);
            }


            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index), new { matchId = viewModel.MatchId });

        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var stat = await _playerStatService.GetByIdAsync(id);

            if (stat == null)
            {

                return NotFound();

            }



            var viewModel = new PlayerStatFormViewModel
            {
                Id = stat.Id,
                PlayerSeasonTeamId = stat.PlayerSeasonTeamId,
                MatchId = stat.MatchId,
                Points = stat.Points,
                OffensiveRebounds = stat.OffensiveRebounds,
                DefensiveRebounds = stat.DefensiveRebounds,
                Assists = stat.Assists,
                Steals = stat.Steals,
                Blocks = stat.Blocks,
                Fouls = stat.Fouls,
                MinutesPlayed = stat.MinutesPlayed,
                FreeThrowsMade = stat.FreeThrowsMade,
                FreeThrowsAttempted = stat.FreeThrowsAttempted,
                TwoPointsMade = stat.TwoPointsMade,
                TwoPointsAttempted = stat.TwoPointsAttempted,
                ThreePointsMade = stat.ThreePointsMade,
                ThreePointsAttempted = stat.ThreePointsAttempted,
                IsMVP = stat.IsMVP,
                SuspensionGames = stat.SuspensionGames
            };

            await LoadDropdownsAsync(viewModel);

            return View(viewModel);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlayerStatFormViewModel viewModel)
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



            var playerStat = new PlayerStat
            {
                Id = viewModel.Id,
                PlayerSeasonTeamId = viewModel.PlayerSeasonTeamId,
                MatchId = viewModel.MatchId,
                Points = viewModel.Points,
                OffensiveRebounds = viewModel.OffensiveRebounds,
                DefensiveRebounds = viewModel.DefensiveRebounds,
                Assists = viewModel.Assists,
                Steals = viewModel.Steals,
                Blocks = viewModel.Blocks,
                Fouls = viewModel.Fouls,
                MinutesPlayed = viewModel.MinutesPlayed,
                FreeThrowsMade = viewModel.FreeThrowsMade,
                FreeThrowsAttempted = viewModel.FreeThrowsAttempted,
                TwoPointsMade = viewModel.TwoPointsMade,
                TwoPointsAttempted = viewModel.TwoPointsAttempted,
                ThreePointsMade = viewModel.ThreePointsMade,
                ThreePointsAttempted = viewModel.ThreePointsAttempted,
                IsMVP = viewModel.IsMVP,
                SuspensionGames = viewModel.SuspensionGames
            };

            var result = await _playerStatService.UpdateAsync(playerStat);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to update player stat.");

                await LoadDropdownsAsync(viewModel);

                return View(viewModel);
            }


            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index), new { matchId = viewModel.MatchId });

        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var stat = await _playerStatService.GetByIdAsync(id);

            if (stat == null)
            {

                return NotFound();

            }

            return View(stat);

        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stat = await _playerStatService.GetByIdAsync(id);

            if (stat == null)
            {

                return NotFound();

            }

            var matchId = stat.MatchId;
            var result = await _playerStatService.DeleteAsync(id);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;

                return RedirectToAction(nameof(Delete), new { id });
            }


            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index), new { matchId });

        }




        private async Task LoadDropdownsAsync(PlayerStatFormViewModel viewModel)
        {

            viewModel.Matches = await _lookupService.GetMatchSelectListAsync(viewModel.MatchId);
            viewModel.PlayerSeasonTeams = await _lookupService.GetPlayerSeasonTeamSelectListAsync(viewModel.PlayerSeasonTeamId);


        }


    }
}