using Basketaki.Constants;
using Basketaki.Models;
using Basketaki.Services;
using Basketaki.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Basketaki.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public async Task<IActionResult> Index()
        {
            var players = await _playerService.GetAllAsync();

            return View(players);
        }


        public async Task<IActionResult> Details(int id)
        {
            var player = await _playerService.GetByIdAsync(id);

            if (player == null)
            {

                return NotFound();

            }

            return View(player);

        }



        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new PlayerFormViewModel
            {

                Positions = GetPositionSelectList()

            };


            return View(viewModel);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {

                viewModel.Positions = GetPositionSelectList(viewModel.Position);

                return View(viewModel);

            }



            var player = new Player
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                DateOfBirth = viewModel.DateOfBirth,
                Height = viewModel.Height,
                Weight = viewModel.Weight,
                Position = viewModel.Position,
                PhotoUrl = viewModel.PhotoUrl
            };

            var result = await _playerService.CreateAsync(player);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to create player.");

                viewModel.Positions = GetPositionSelectList(viewModel.Position);

                return View(viewModel);

            }



            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));

        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var player = await _playerService.GetByIdAsync(id);

            if (player == null)
            {

                return NotFound();

            }



            var viewModel = new PlayerFormViewModel
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                DateOfBirth = player.DateOfBirth,
                Height = player.Height,
                Weight = player.Weight,
                Position = player.Position,
                PhotoUrl = player.PhotoUrl,
                Positions = GetPositionSelectList(player.Position)
            };


            return View(viewModel);

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlayerFormViewModel viewModel)
        {
            if (id != viewModel.Id)
            {

                return BadRequest();

            }

            if (!ModelState.IsValid)
            {
                viewModel.Positions = GetPositionSelectList(viewModel.Position);

                return View(viewModel);

            }


            var player = new Player
            {
                Id = viewModel.Id,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                DateOfBirth = viewModel.DateOfBirth,
                Height = viewModel.Height,
                Weight = viewModel.Weight,
                Position = viewModel.Position,
                PhotoUrl = viewModel.PhotoUrl
            };

            var result = await _playerService.UpdateAsync(player);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to update player.");

                viewModel.Positions = GetPositionSelectList(viewModel.Position);

                return View(viewModel);

            }



            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));

        }




        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var player = await _playerService.GetByIdAsync(id);

            if (player == null)
            {

                return NotFound();

            }



            return View(player);

        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _playerService.DeleteAsync(id);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;

                return RedirectToAction(nameof(Delete), new { id });

            }


            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }




        private List<SelectListItem> GetPositionSelectList(PlayerPosition? selectedPosition = null)
        {
            return Enum.GetValues(typeof(PlayerPosition)).Cast<PlayerPosition>().Select(position => new SelectListItem
            {
                    Value = position.ToString(),
                    Text = position.ToString(),
                    Selected = selectedPosition.HasValue && position == selectedPosition.Value

            }).ToList();

        }


    }
}