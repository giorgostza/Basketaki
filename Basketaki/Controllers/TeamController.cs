using Basketaki.Models;
using Basketaki.Services;
using Basketaki.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Basketaki.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly ICoachService _coachService;

        public TeamController(ITeamService teamService, ICoachService coachService)
        {
            _teamService = teamService;
            _coachService = coachService;
        }

        public async Task<IActionResult> Index()
        {
            var teams = await _teamService.GetAllAsync();

            return View(teams);
        }

        public async Task<IActionResult> Details(int id)
        {
            var team = await _teamService.GetByIdAsync(id);

            if (team == null)
            {

                return NotFound();

            }


            return View(team);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new TeamFormViewModel
            {

                Coaches = await GetCoachSelectListAsync()

            };


            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Coaches = await GetCoachSelectListAsync(viewModel.CoachId);

                return View(viewModel);
            }


            var team = new Team
            {
                Name = viewModel.Name,
                City = viewModel.City,
                PhotoUrl = viewModel.PhotoUrl,
                CoachId = viewModel.CoachId
            };

            var result = await _teamService.CreateAsync(team);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to create team.");
                viewModel.Coaches = await GetCoachSelectListAsync(viewModel.CoachId);

                return View(viewModel);
            }


            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var team = await _teamService.GetByIdAsync(id);

            if (team == null)
            {

                return NotFound();

            }

            var viewModel = new TeamFormViewModel
            {
                Id = team.Id,
                Name = team.Name,
                City = team.City,
                PhotoUrl = team.PhotoUrl,
                CoachId = team.CoachId,
                Coaches = await GetCoachSelectListAsync(team.CoachId)
            };


            return View(viewModel);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TeamFormViewModel viewModel)
        {
            if (id != viewModel.Id)
            {

                return BadRequest();

            }

            if (!ModelState.IsValid)
            {
                viewModel.Coaches = await GetCoachSelectListAsync(viewModel.CoachId);

                return View(viewModel);
            }



            var team = new Team
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                City = viewModel.City,
                PhotoUrl = viewModel.PhotoUrl,
                CoachId = viewModel.CoachId
            };

            var result = await _teamService.UpdateAsync(team);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to update team.");
                viewModel.Coaches = await GetCoachSelectListAsync(viewModel.CoachId);

                return View(viewModel);
            }


            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var team = await _teamService.GetByIdAsync(id);

            if (team == null)
            {

                return NotFound();

            }

            return View(team);

        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _teamService.DeleteAsync(id);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;

                return RedirectToAction(nameof(Delete), new { id });
            }


            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }





        private async Task<List<SelectListItem>> GetCoachSelectListAsync(int? selectedCoachId = null)
        {
            var coaches = await _coachService.GetAllAsync();

            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "",
                    Text = "-- No Coach --",
                    Selected = !selectedCoachId.HasValue
                }

            };

            items.AddRange(coaches.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.FullName,
                Selected = selectedCoachId.HasValue && c.Id == selectedCoachId.Value

            }));

            return items;

        }


    }
}