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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

                

            var court = await _courtService.GetByIdAsync(id.Value);

            if (court == null)
            {
                return NotFound();
            }

            return View(court);
        }

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
                

            await _courtService.CreateAsync(court);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
                

            var court = await _courtService.GetByIdAsync(id.Value);

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
                return NotFound();
            }


            if (!ModelState.IsValid)
            {
                return View(court);
            }
                                

            await _courtService.UpdateAsync(court);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
               

            var court = await _courtService.GetByIdAsync(id.Value);

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
            await _courtService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}