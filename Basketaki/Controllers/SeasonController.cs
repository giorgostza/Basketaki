using Basketaki.Models;
using Basketaki.Services;
using Microsoft.AspNetCore.Mvc;

namespace Basketaki.Controllers
{
    public class SeasonController : Controller
    {
        private readonly ISeasonService _seasonService;

        public SeasonController(ISeasonService seasonService)
        {

            _seasonService = seasonService;

        }

        public async Task<IActionResult> Index()
        {

            var seasons = await _seasonService.GetAllAsync();

            return View(seasons);

        }

        public async Task<IActionResult> Details(int id)
        {
            var season = await _seasonService.GetByIdAsync(id);

            if (season == null)
            {

                return NotFound();

            }

            return View(season);

        }


        [HttpGet]
        public IActionResult Create()
        {

            return View();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Season season)
        {
            if (!ModelState.IsValid)
            {

                return View(season);

            }

            var result = await _seasonService.CreateAsync(season);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to create season.");

                return View(season);

            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var season = await _seasonService.GetByIdAsync(id);

            if (season == null)
            {

                return NotFound();

            }

            return View(season);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Season season)
        {
            if (id != season.Id)
            {

                return BadRequest();

            }

            if (!ModelState.IsValid)
            {

                return View(season);

            }


            var result = await _seasonService.UpdateAsync(season);

            if (!result.Success)
            {

                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to update season.");
                return View(season);

            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));

        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var season = await _seasonService.GetByIdAsync(id);

            if (season == null)
            {

                return NotFound();

            }

            return View(season);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _seasonService.DeleteAsync(id);

            if (!result.Success)
            {

                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Delete), new { id });

            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));

        }

    }
}