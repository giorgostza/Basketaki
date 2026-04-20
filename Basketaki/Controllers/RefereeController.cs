using Basketaki.Models;
using Basketaki.Services;
using Microsoft.AspNetCore.Mvc;

namespace Basketaki.Controllers
{
    public class RefereeController : Controller
    {
        private readonly IRefereeService _refereeService;

        public RefereeController(IRefereeService refereeService)
        {
            _refereeService = refereeService;
        }

        public async Task<IActionResult> Index()
        {
            var referees = await _refereeService.GetAllAsync();

            return View(referees);
        }


        public async Task<IActionResult> Details(int id)
        {
            var referee = await _refereeService.GetByIdAsync(id);

            if (referee == null)
            {

                return NotFound();

            }

            return View(referee);
        }



        [HttpGet]
        public IActionResult Create()
        {

            return View();

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Referee referee)
        {
            if (!ModelState.IsValid)
            {

                return View(referee);

            }



            var result = await _refereeService.CreateAsync(referee);

            if (!result.Success)
            {

                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to create referee.");

                return View(referee);
            }



            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }





        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var referee = await _refereeService.GetByIdAsync(id);

            if (referee == null)
            {

                return NotFound();

            }

            return View(referee);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Referee referee)
        {
            if (id != referee.Id)
            {

                return BadRequest();

            }


            if (!ModelState.IsValid)
            {

                return View(referee);

            }


            var result = await _refereeService.UpdateAsync(referee);

            if (!result.Success)
            {

                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to update referee.");

                return View(referee);

            }



            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Index));
        }




        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var referee = await _refereeService.GetByIdAsync(id);

            if (referee == null)
            {

                return NotFound();

            }

            return View(referee);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _refereeService.DeleteAsync(id);

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
