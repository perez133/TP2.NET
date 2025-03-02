// File: Controllers/ShopController.cs
using Gauniv.WebServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gauniv.WebServer.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ShopController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Shop
        // Display a list of all games available for purchase.
        public async Task<IActionResult> Index()
        {
            var games = await _context.Games.Include(g => g.Categories).ToListAsync();
            return View(games);
        }

        // GET: /Shop/Buy/{id}
        public async Task<IActionResult> Buy(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return NotFound();

            return View(game);
        }

        // POST: /Shop/Buy/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // Add the game to the player's owned games list if not already owned.
            if (!user.JeuxAchetes.Any(g => g.Id == game.Id))
            {
                user.JeuxAchetes.Add(game);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Game purchased successfully.";
            }
            else
            {
                TempData["Error"] = "You already own this game.";
            }
            return RedirectToAction("Index", "Library");
        }
    }
}
