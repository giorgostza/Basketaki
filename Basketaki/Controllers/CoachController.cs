using Basketaki.Models;
using Basketaki.Services;
using Microsoft.AspNetCore.Mvc;

namespace Basketaki.Controllers
{
    public class CoachController : Controller
    {
        private readonly ICoachService _coachService;

        public CoachController(ICoachService coachService)
        {
            _coachService = coachService;
        }

        public async Task<IActionResult> Index()
        {

            var coaches = await _coachService.GetAllAsync();
            return View(coaches);

        }

        public async Task<IActionResult> Details(int id)
        {
            var coach = await _coachService.GetByIdAsync(id);

            if (coach == null)
            {

                return NotFound();

            }


            return View(coach);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coach coach)
        {
            if (!ModelState.IsValid)
            {

                return View(coach);

            }



            var result = await _coachService.CreateAsync(coach);

            if (!result.Success)
            {

                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to create coach.");

                return View(coach);
            }



            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Index));

        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var coach = await _coachService.GetByIdAsync(id);

            if (coach == null)
            {

                return NotFound();

            }

            return View(coach);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Coach coach)
        {
            if (id != coach.Id)
            {

                return BadRequest();

            }


            if (!ModelState.IsValid)
            {

                return View(coach);

            }



            var result = await _coachService.UpdateAsync(coach);

            if (!result.Success)
            {

                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to update coach.");

                return View(coach);
            }


            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var coach = await _coachService.GetByIdAsync(id);

            if (coach == null)
            {

                return NotFound();

            }

            return View(coach);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _coachService.DeleteAsync(id);

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