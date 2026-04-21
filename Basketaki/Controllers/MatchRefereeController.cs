using Basketaki.Services;
using Basketaki.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Basketaki.Controllers
{
    public class MatchRefereeController : Controller
    {
        private readonly IMatchRefereeService _matchRefereeService;
        private readonly IMatchService _matchService;
        private readonly IRefereeService _refereeService;

        public MatchRefereeController(IMatchRefereeService matchRefereeService, IMatchService matchService, IRefereeService refereeService)
        {

            _matchRefereeService = matchRefereeService;
            _matchService = matchService;
            _refereeService = refereeService;

        }


        public async Task<IActionResult> Index(int matchId)
        {
            var match = await _matchService.GetByIdAsync(matchId);

            if (match == null)
            {

                return NotFound();

            }

            var assignments = await _matchRefereeService.GetByMatchIdAsync(matchId);

            ViewBag.Match = match;
            ViewBag.MatchId = matchId;

            return View(assignments);

        }




        [HttpGet]
        public async Task<IActionResult> Create(int matchId)
        {
            var match = await _matchService.GetByIdAsync(matchId);

            if (match == null)
            {

                return NotFound();

            }



            var viewModel = new MatchRefereeFormViewModel
            {
                MatchId = matchId,
                MatchDisplayName = $"{match.MatchDate:yyyy-MM-dd} - {match.HomeTeamSeasonLeague?.Team?.Name} vs {match.AwayTeamSeasonLeague?.Team?.Name}",
                Referees = await GetRefereeSelectListAsync()

            };

            return View(viewModel);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MatchRefereeFormViewModel viewModel)
        {
            var match = await _matchService.GetByIdAsync(viewModel.MatchId);

            if (match == null)
            {

                return NotFound();

            }


            if (!ModelState.IsValid)
            {
                viewModel.MatchDisplayName = $"{match.MatchDate:yyyy-MM-dd} - {match.HomeTeamSeasonLeague?.Team?.Name} vs {match.AwayTeamSeasonLeague?.Team?.Name}";
                viewModel.Referees = await GetRefereeSelectListAsync(viewModel.RefereeId);
                return View(viewModel);

            }



            var result = await _matchRefereeService.CreateAsync(viewModel.MatchId, viewModel.RefereeId);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to assign referee.");

                viewModel.MatchDisplayName = $"{match.MatchDate:dd/MM/yyyy} - {match.HomeTeamSeasonLeague?.Team?.Name} vs {match.AwayTeamSeasonLeague?.Team?.Name}";
                viewModel.Referees = await GetRefereeSelectListAsync(viewModel.RefereeId);

                return View(viewModel);
            }



            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index), new { matchId = viewModel.MatchId });
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int matchId, int refereeId)
        {
            var match = await _matchService.GetByIdAsync(matchId);

            if (match == null)
            {

                return NotFound();

            }



            var assignments = await _matchRefereeService.GetByMatchIdAsync(matchId);
            var assignment = assignments.FirstOrDefault(a => a.RefereeId == refereeId);

            if (assignment == null)
            {

                return NotFound();

            }



            ViewBag.Match = match;

            return View(assignment);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int matchId, int refereeId)
        {
            var result = await _matchRefereeService.DeleteAsync(matchId, refereeId);

            if (!result.Success)
            {

                TempData["ErrorMessage"] = result.Message;

                return RedirectToAction(nameof(Delete), new { matchId, refereeId });

            }



            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index), new { matchId });

        }




        private async Task<List<SelectListItem>> GetRefereeSelectListAsync(int? selectedRefereeId = null)
        {
            var referees = await _refereeService.GetAllAsync();

            return referees.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.FullName,
                Selected = selectedRefereeId.HasValue && r.Id == selectedRefereeId.Value

            }).ToList();

        }


    }
}