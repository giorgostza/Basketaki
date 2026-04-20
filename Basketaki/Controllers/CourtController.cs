using Basketaki.Models;
using Basketaki.Services;
using Microsoft.AspNetCore.Mvc;

namespace Basketaki.Controllers
{
    public class CourtController : Controller
    {
        private readonly ICourtService _courtService;

        public CourtController(ICourtService courtService)
        {
            _courtService = courtService;
        }

        public async Task<IActionResult> Index()
        {

            var courts = await _courtService.GetAllAsync();

            return View(courts);
        }


        public async Task<IActionResult> Details(int id)
        {
            var court = await _courtService.GetByIdAsync(id);

            if (court == null)
            {

                return NotFound();

            }

            return View(court);
        }



        [HttpGet]
        public IActionResult Create()
        {

            return View();

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Court court)
        {
            if (!ModelState.IsValid)
            {

                return View(court);

            }



            var result = await _courtService.CreateAsync(court);

            if (!result.Success)
            {

                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to create court.");

                return View(court);
            }



            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var court = await _courtService.GetByIdAsync(id);

            if (court == null)
            {

                return NotFound();

            }



            return View(court);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Court court)
        {
            if (id != court.Id)
            {

                return BadRequest();

            }


            if (!ModelState.IsValid)
            {

                return View(court);

            }



            var result = await _courtService.UpdateAsync(court);

            if (!result.Success)
            {

                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to update court.");

                return View(court);
            }


            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var court = await _courtService.GetByIdAsync(id);

            if (court == null)
            {

                return NotFound();

            }

            return View(court);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _courtService.DeleteAsync(id);

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