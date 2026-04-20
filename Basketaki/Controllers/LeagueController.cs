using Basketaki.Models;
using Basketaki.Services;
using Basketaki.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Basketaki.Controllers
{
    public class LeagueController : Controller
    {
        private readonly ILeagueService _leagueService;
        private readonly ISeasonService _seasonService;

        public LeagueController(ILeagueService leagueService, ISeasonService seasonService)
        {
            _leagueService = leagueService;
            _seasonService = seasonService;
        }

        public async Task<IActionResult> Index()
        {
            var leagues = await _leagueService.GetAllAsync();

            return View(leagues);
        }


        public async Task<IActionResult> Details(int id)
        {
            var league = await _leagueService.GetByIdAsync(id);

            if (league == null)
            {

                return NotFound();

            }

            return View(league);

        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new LeagueFormViewModel
            {

                Seasons = await GetSeasonSelectListAsync()

            };

            return View(viewModel);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeagueFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {

                viewModel.Seasons = await GetSeasonSelectListAsync(viewModel.SeasonId);

                return View(viewModel);
            }



            var league = new League
            {
                Name = viewModel.Name,
                City = viewModel.City,
                SeasonId = viewModel.SeasonId
            };

            var result = await _leagueService.CreateAsync(league);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to create league.");

                viewModel.Seasons = await GetSeasonSelectListAsync(viewModel.SeasonId);

                return View(viewModel);
            }



            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }




        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var league = await _leagueService.GetByIdAsync(id);

            if (league == null)
            {

                return NotFound();

            }



            var viewModel = new LeagueFormViewModel
            {
                Id = league.Id,
                Name = league.Name,
                City = league.City,
                SeasonId = league.SeasonId,
                Seasons = await GetSeasonSelectListAsync(league.SeasonId)
            };


            return View(viewModel);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeagueFormViewModel viewModel)
        {
            if (id != viewModel.Id)
            {

                return BadRequest();

            }


            if (!ModelState.IsValid)
            {

                viewModel.Seasons = await GetSeasonSelectListAsync(viewModel.SeasonId);

                return View(viewModel);
            }


            var league = new League
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                City = viewModel.City,
                SeasonId = viewModel.SeasonId
            };

            var result = await _leagueService.UpdateAsync(league);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to update league.");

                viewModel.Seasons = await GetSeasonSelectListAsync(viewModel.SeasonId);

                return View(viewModel);
            }



            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }




        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var league = await _leagueService.GetByIdAsync(id);

            if (league == null)
            {

                return NotFound();

            }

            return View(league);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _leagueService.DeleteAsync(id);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;

                return RedirectToAction(nameof(Delete), new { id });
            }


            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
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