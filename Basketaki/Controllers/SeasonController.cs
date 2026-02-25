using Basketaki.Data;
using Basketaki.Models;
using Basketaki.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basketaki.Controllers
{
    public class SeasonController : Controller
    {
        private readonly ISeasonService _seasonService;

        public SeasonController(ISeasonService seasonService)
        {
            _seasonService = seasonService;
        }

        // GET: Season
        public async Task<IActionResult> Index()
        {
            var seasons = await _seasonService.GetAllAsync();

            return View(seasons);
        }

        // GET: Season/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _seasonService.GetByIdAsync(id.Value);

            if (season == null)
            {
                return NotFound();
            }

            return View(season);
        }

        // GET: Season/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Season/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Season season)
        {

            if (season.EndDate<= season.StartDate)
            {

                ModelState.AddModelError("", "End date must be after start date.");

            }

            if (await _seasonService.NameExistsAsync(season.Name))
            {

                ModelState.AddModelError("Name", "Season with this name already exists.");

            }
               



            if (!ModelState.IsValid)
            {
                return View(season);
            }

            await _seasonService.CreateAsync(season);

            return RedirectToAction(nameof(Index));
        
        }

        // GET: Season/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _seasonService.GetByIdAsync(id.Value);

            if (season == null)
            {
                return NotFound();
            }

            return View(season);
        }

        // POST: Season/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Season season)
        {


            if (id != season.Id)
            {
                return NotFound();
            }

            if (season.EndDate <= season.StartDate)
            {
                ModelState.AddModelError("", "End date must be after start date.");
            }

            var existingSeason = await _seasonService.GetByIdAsync(id);

            if (existingSeason == null)
            {

                return NotFound();

            }

            if (existingSeason.Name != season.Name && await _seasonService.NameExistsAsync(season.Name))
            {

                ModelState.AddModelError("Name", "Season with this name already exists.");

            }


            if (!ModelState.IsValid)
            {

                return View(season);

            }

            await _seasonService.UpdateAsync(season);

            return RedirectToAction(nameof(Index));
       
        }

        // GET: Season/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _seasonService.GetByIdAsync(id.Value);

            if (season == null)
            {
                return NotFound();
            }

            return View(season);
        }

        // POST: Season/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _seasonService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return RedirectToAction(nameof(Index));

        }

      
    }
}
