using Basketaki.Models;
using Basketaki.Services;
using Basketaki.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Basketaki.Controllers
{
    public class MatchPhotoController : Controller
    {
        private readonly IMatchPhotoService _matchPhotoService;
        private readonly IMatchService _matchService;

        public MatchPhotoController(IMatchPhotoService matchPhotoService, IMatchService matchService)
        {
            _matchPhotoService = matchPhotoService;
            _matchService = matchService;
        }

        public async Task<IActionResult> Index(int? matchId)
        {
            if (matchId.HasValue)
            {
                var photos = await _matchPhotoService.GetByMatchAsync(matchId.Value);

                ViewBag.MatchId = matchId.Value;

                return View(photos);

            }



            return View(new List<MatchPhoto>());
        }



        public async Task<IActionResult> Details(int id)
        {
            var photo = await _matchPhotoService.GetByIdAsync(id);

            if (photo == null)
            {

                return NotFound();

            }

            return View(photo);
        }



        [HttpGet]
        public async Task<IActionResult> Create(int? matchId)
        {
            var viewModel = new MatchPhotoFormViewModel
            {
                MatchId = matchId ?? 0,
                Matches = await GetMatchSelectListAsync(matchId)

            };


            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MatchPhotoFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Matches = await GetMatchSelectListAsync(viewModel.MatchId);

                return View(viewModel);
            }



            var photo = new MatchPhoto
            {
                ImageUrl = viewModel.ImageUrl,
                MatchId = viewModel.MatchId

            };

            var result = await _matchPhotoService.CreateAsync(photo);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to create photo.");

                viewModel.Matches = await GetMatchSelectListAsync(viewModel.MatchId);

                return View(viewModel);
            }



            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index), new { matchId = viewModel.MatchId });
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var photo = await _matchPhotoService.GetByIdAsync(id);

            if (photo == null)
            {

                return NotFound();

            }


            return View(photo);

        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _matchPhotoService.GetByIdAsync(id);

            if (photo == null)
            {

                return NotFound();

            }



            var matchId = photo.MatchId;
            var result = await _matchPhotoService.DeleteAsync(id);

            if (!result.Success)
            {

                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Delete), new { id });

            }


            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index), new { matchId });
        }




        private async Task<List<SelectListItem>> GetMatchSelectListAsync(int? selectedMatchId = null)
        {
            var matches = await _matchService.GetAllAsync();

            return matches.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = $"{m.MatchDate:dd/MM/yyyy} - {m.HomeTeamSeasonLeague?.Team?.Name} vs {m.AwayTeamSeasonLeague?.Team?.Name}",
                Selected = selectedMatchId.HasValue && m.Id == selectedMatchId.Value

            }).ToList();

        }



    }
}