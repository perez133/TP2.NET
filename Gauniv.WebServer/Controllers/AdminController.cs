// File: Controllers/AdminController.cs
using Gauniv.WebServer.Data;
using Gauniv.WebServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Gauniv.WebServer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var games = await _context.Games.Include(g => g.Categories).ToListAsync();
            return View(games);
        }

        // GET: /Admin/Index - List all games.
        public async Task<IActionResult> Index()
        {
            var games = await _context.Games.Include(g => g.Categories).ToListAsync();
            return View(games);
        }

        // GET: /Admin/CreateGame
        public async Task<IActionResult> CreateGame()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }

        // POST: /Admin/CreateGame
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGame(CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(model);
            }

            byte[] payload = new byte[0];
            if (model.File != null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    await model.File.CopyToAsync(ms);
                    payload = ms.ToArray();
                }
            }

            var game = new Game
            {
                Nom = model.Name,
                Description = model.Description,
                Prix = model.Price,
                Payload = payload
            };

            if (model.CategoryIds != null)
            {
                var categories = await _context.Categories
                    .Where(c => model.CategoryIds.Contains(c.Id))
                    .ToListAsync();
                game.Categories = categories;
            }

            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Game created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/EditGame/{id}
        public async Task<IActionResult> EditGame(int id)
        {
            var game = await _context.Games.Include(g => g.Categories)
                                           .FirstOrDefaultAsync(g => g.Id == id);
            if (game == null)
                return NotFound();

            var model = new EditViewModel
            {
                Id = game.Id,
                Name = game.Nom,
                Description = game.Description,
                Price = game.Prix,
                CategoryIds = game.Categories.Select(c => c.Id).ToArray()
            };

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(model);
        }

        // POST: /Admin/EditGame/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGame(int id, EditViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories.ToListAsync();
                return View(model);
            }

            var game = await _context.Games.Include(g => g.Categories)
                                           .FirstOrDefaultAsync(g => g.Id == id);
            if (game == null)
                return NotFound();

            game.Nom = model.Name;
            game.Description = model.Description;
            game.Prix = model.Price;

            if (model.File != null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    await model.File.CopyToAsync(ms);
                    game.Payload = ms.ToArray();
                }
            }

            game.Categories.Clear();
            if (model.CategoryIds != null)
            {
                var categories = await _context.Categories
                    .Where(c => model.CategoryIds.Contains(c.Id))
                    .ToListAsync();
                game.Categories = categories;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Game updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/DeleteGame/{id}
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.Include(g => g.Categories)
                                           .FirstOrDefaultAsync(g => g.Id == id);
            if (game == null)
                return NotFound();
            return View(game);
        }

        // POST: /Admin/DeleteGame/{id}
        [HttpPost, ActionName("DeleteGame")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGameConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return NotFound();
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Game deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
